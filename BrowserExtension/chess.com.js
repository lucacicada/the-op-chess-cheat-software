// this is lazy, just use a MutationObserver
setInterval(() => {
  // figure out the board position
  const black = document.querySelector('chess-board')?.classList.contains('flipped') || false

  // extract the PGN position from the move list
  let position = ''

  for (const el of Array.from(document.querySelectorAll('vertical-move-list > .move'))) {
    const number = el.dataset.wholeMoveNumber

    position += `${number}. `

    for (const node of Array.from(el.querySelectorAll('.node'))) {
      const figurine = node.querySelector('span')

      if (figurine) {
        position += `${figurine.dataset.figurine}`
      }

      position += `${node.innerText} `
    }
  }

  if (position) {
    // send everything to the engine for evaluation
    chrome.runtime.sendMessage({ black, position })
  }
}, 75)
