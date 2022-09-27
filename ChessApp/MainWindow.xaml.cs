namespace ChessApp;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Web.WebView2.Core;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        // put the window in the top-right corner of the screen
        // leave some space on top so we do not cover the browser tabs
        Top = 45;
        Left = SystemParameters.WorkArea.Right - Width + 8;

        // makes the window appears on top of all the others
        Topmost = true;
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        webView.NavigationCompleted += WebView_NavigationCompleted;
        webView.NavigationCompleted += (_, __) => Task.Run(RunListener);

        var env = CoreWebView2Environment.CreateAsync(null, Path.GetFullPath("user-data"), new CoreWebView2EnvironmentOptions("-inprivate")
        {
            Language = "en",
            AllowSingleSignOnUsingOSPrimaryAccount = true,
        }).ConfigureAwait(false).GetAwaiter().GetResult();

        _ = webView.EnsureCoreWebView2Async(env);
    }

    private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        webView.CoreWebView2.CookieManager.DeleteAllCookies();
        webView.CoreWebView2.Navigate("https://lichess.org/analysis");
    }

    private void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        webView.ExecuteScriptAsync(@"
document.querySelector('.cmn-toggle.cmn-toggle--subtle').click();

document.querySelector('[data-act=""menu""]').dispatchEvent( new MouseEvent('mousedown', { 'view': window, 'bubbles': true, 'cancelable': false }) );

document.querySelector('#analyse-multipv').value = 3;
document.querySelector('#analyse-multipv').dispatchEvent(new Event('input'));

document.querySelector('#analyse-threads').value = 1;
document.querySelector('#analyse-threads').dispatchEvent(new Event('input'));

document.querySelector('#analyse-memory').value = 2;
document.querySelector('#analyse-memory').dispatchEvent(new Event('input'));

document.querySelector('[data-act=""menu""]').dispatchEvent( new MouseEvent('mousedown', { 'view': window, 'bubbles': true, 'cancelable': false }) );

document.querySelector('.analyse__underboard').style.display = 'none';
document.querySelector('.analyse__moves').style.display = 'none';
document.querySelector('.analyse__side').style.display = 'none';

document.querySelector('.pv_box').style.display = 'none';
");
    }

    async Task RunListener()
    {
        // listen for a connection
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:30012/");
        listener.Start();

        string lastPos = "";
        while (true)
        {
            var ctx = await listener.GetContextAsync();

            // accept a POST request
            if (ctx.Request.HttpMethod == HttpMethod.Post.Method)
            {
                JsonElement jss;

                using (var sr = new StreamReader(ctx.Request.InputStream))
                {
                    jss = JsonSerializer.Deserialize<JsonElement>(sr.ReadToEnd());
                }

                // we have done, close the connection
                ctx.Response.OutputStream.Close();

                var position = jss.GetProperty("position").GetString()!;

                // TODO: swap the board automatically would be nice btw
                // var black = jss.GetProperty("black").GetBoolean();

                // do not update the same position twice
                if (lastPos == position)
                {
                    continue;
                }

                lastPos = position;

                // invoke and wait, we are on a different thread
                webView.Dispatcher.Invoke(new Action(() =>
                {
                    webView.ExecuteScriptAsync(@$"
document.querySelector('textarea').value = '{position}';
document.querySelector('.button.button-thin.action.text').click();
");
                }));

            }
        }
    }
}
