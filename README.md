# The OP Chess Cheat Software

> For fast loss of friends, cheat at chess

---

## Why it's so good?

- It's better than the paid stuff you find on the internet, PERIOD!
- It have an awesome name!
- It took more time to write this README than make the actual program!
- It uses the power of the radar (you can read it both ways)!
- It's like FOSS (Free Open Source Software)!
- You can lose your pesky friends quickly!
- You can have a chance to make history for being the first to get banned for using this program!
- The slogan is perfect for an Agadmator video, or a Futurama episode!

---

## How does it works

Too hard to read the code yourself? Glad you are here!

It is basically a browser extension that read the HTML from the website, [figure out the board position](./BrowserExtension/chess.com.js#L9), then send a [[POST]](./BrowserExtension/background.js#L3) request to `http://localhost:30012`, the app [listen for a connection](./ChessApp/MainWindow.xaml.cs#L80), [read the board position](./ChessApp/MainWindow.xaml.cs#L101) and boom, it is practically the equivalent of the remote-radar in videogames.

---

## Why it's so op?

Use the program, and you tell me!

## How to use the program

You need to load the extension in your browser manually:

### Firefox

- Navigate to [about:debugging#/runtime/this-firefox](about:debugging#/runtime/this-firefox)
- Click on `Load Temporary Add-on...` in the top center of the page
- Open to the folder `BrowserExtension` and select the file `manifest.json`

### Chrome

- Navigate to [chrome://extensions](chrome://extensions)
- Enable `Developer Mode` on the top right corner
- Click `Load unpacked` on the top left corner
- Select the folder `BrowserExtension`

#### Fix for Manifest v3

The extensions uses the `manifest_version: 2` and it will no longer works in Chrome after the first of January 2023, just edit the `manifest_version` manually on the [manifest.json](./BrowserExtension/manifest.json#L5) file.

See [https://developer.chrome.com/docs/extensions/mv3/mv2-sunset/](https://developer.chrome.com/docs/extensions/mv3/mv2-sunset/) for more.

---

## Build the ChessApp

### Prerequisite

You need the .NET 6.0 SDK [https://dotnet.microsoft.com/en-us/download/dotnet/6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

> `cd ChessApp`

Build the application with:

> `dotnet build ChessApp.csproj -c Release -r win-x64 --no-self-contained /p:DebugType=None /p:DebugSymbols=false`

The binary will be located [bin\Release\net6.0-windows\win-x64](.ChessApp/bin/Release/net6.0-windows/win-x64), by default the file name is `ChessApp.exe`.

You can read more on building a .NET Core application on the official [Microsoft documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build)

### For non Windows users

Well, this program uses [Windows Presentation Foundation (WPF)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview) which is a windows-only technology.

It should be trivial to port this to any other WebView projects, i'm just too lazy to do it.

## A mystery yet to be solved

By reading this you'll have already figured out I'v not told you in which website you can use this program, well...

```text
Hello. We are looking for any sign of intelligence. To find it, we have devised a test.

There is a message hidden in this repo.

Find it, and it will lead you on the road to finding untapped skills. We look forward to meeting the many that will be banned all the way through.

Good luck.
```
