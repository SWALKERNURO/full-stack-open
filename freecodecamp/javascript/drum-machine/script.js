const pads = [...document.querySelectorAll('.drum-pad')];
const display = document.getElementById('display');
function playPad(pad) {
  const audio = pad.querySelector('audio');
  audio.currentTime = 0;
  const playback = audio.play();
  if (playback && typeof playback.catch === 'function') playback.catch(() => {});
  display.textContent = pad.id.split('-').map(word => word[0].toUpperCase() + word.slice(1)).join(' ');
  pad.classList.add('active');
  window.setTimeout(() => pad.classList.remove('active'), 130);
}
pads.forEach(pad => pad.addEventListener('click', () => playPad(pad)));
document.addEventListener('keydown', event => {
  if (event.repeat || event.ctrlKey || event.metaKey || event.altKey || /INPUT|TEXTAREA|SELECT/.test(event.target.tagName)) return;
  const audio = document.getElementById(event.key.toUpperCase());
  if (audio && audio.classList.contains('clip')) playPad(audio.parentElement);
});