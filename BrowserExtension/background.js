chrome.runtime.onMessage.addListener((message) => {
  // send the message back to our local program
  fetch('http://localhost:30012', {
    method: 'POST',
    body: JSON.stringify(message),
  })
})
