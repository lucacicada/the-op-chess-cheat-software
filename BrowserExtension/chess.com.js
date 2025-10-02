function getBoardPosition() {
  let pgn = ''

  for (const el of Array.from(document.querySelectorAll('[data-whole-move-number]'))) {
    pgn += `${el.dataset.wholeMoveNumber}. `

    for (const node of Array.from(el.querySelectorAll('[data-node]'))) {
      const figurine = node.querySelector('[data-figurine]')?.dataset.figurine

      pgn += `${figurine || ''}${node.textContent.trim()} `

      if (node.querySelector('.selected')) {
        // return pgn
      }
    }
  }

  return pgn
}

// this is lazy, just use a MutationObserver
setInterval(() => {
  // If the board is flipped, we are playing black
  const black = document.querySelector('wc-chess-board')?.classList.contains('flipped') || false

  // extract the PGN position from the move list
  let position = getBoardPosition()

  if (position) {
    // send everything to the engine for evaluation
    chrome.runtime.sendMessage({ black, position })
  }
}, 75)
