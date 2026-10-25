const { useState } = React;
const WIN_LINES = [[0,1,2],[3,4,5],[6,7,8],[0,3,6],[1,4,7],[2,5,8],[0,4,8],[2,4,6]];
export function Board() {
  const [squares, setSquares] = useState(Array(9).fill(null));
  const [xNext, setXNext] = useState(true);
  const winningLine = WIN_LINES.find(([a,b,c]) => squares[a] && squares[a] === squares[b] && squares[a] === squares[c]);
  const winner = winningLine ? squares[winningLine[0]] : null;
  const draw = !winner && squares.every(Boolean);
  function play(index) {
    if (winner || squares[index] || draw) return;
    setSquares(previous => previous.map((value, position) => position === index ? (xNext ? 'X' : 'O') : value));
    setXNext(!xNext);
  }
  function reset() { setSquares(Array(9).fill(null)); setXNext(true); }
  return <main className="game"><p className="eyebrow">A LITTLE FRIENDLY COMPETITION</p><h1>Tic-Tac-Toe</h1><p className="status" role="status" aria-live="polite">{winner ? 'Winner: ' + winner : draw ? "It's a draw!" : 'Next player: ' + (xNext ? 'X' : 'O')}</p><div className="board" aria-label="Game board">{squares.map((value,index) => <button key={index} className={'square ' + (value || '') + (winningLine?.includes(index) ? ' winning' : '')} onClick={() => play(index)} aria-label={'Row ' + (Math.floor(index/3)+1) + ', column ' + (index%3+1) + ': ' + (value || 'empty')}>{value}</button>)}</div><button id="reset" onClick={reset}>New game</button><p className="hint">Take turns. Three in a row wins.</p></main>;
}