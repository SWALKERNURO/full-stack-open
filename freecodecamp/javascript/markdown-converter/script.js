const markdownInput = document.getElementById('markdown-input');
const htmlOutput = document.getElementById('html-output');
const preview = document.getElementById('preview');

function escapeHTML(value) {
  return value.replace(/&/g, '&amp;').replace(/</g, '&lt;')
    .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
}
function safeURL(value) {
  return /^(?:javascript|vbscript|data)\s*:/i.test(value.trim()) ? '#' : value;
}
function convertMarkdown() {
  return markdownInput.value.split(/\r?\n/).map((line) => {
    let html = escapeHTML(line);
    html = html.replace(/!\[([^\]]*)\]\(([^)]+)\)/g,
      (_, alt, src) => '<img alt="' + alt + '" src="' + safeURL(src) + '">');
    html = html.replace(/\[([^\]]+)\]\(([^)]+)\)/g,
      (_, text, url) => '<a href="' + safeURL(url) + '">' + text + '</a>');
    html = html.replace(/\*\*(.+?)\*\*(?!\*)/g, '<strong>$1</strong>')
      .replace(/__(.+?)__(?!_)/g, '<strong>$1</strong>')
      .replace(/\*([^*]+)\*/g, '<em>$1</em>')
      .replace(/_([^_]+)_/g, '<em>$1</em>');
    html = html.replace(/^[ \t]*(#{1,3})[ \t]+(.+)$/,
      (_, hashes, text) => '<h' + hashes.length + '>' + text + '</h' + hashes.length + '>');
    html = html.replace(/^[ \t]*&gt;[ \t]+(.*)$/, '<blockquote>$1</blockquote>');
    return html;
  }).join('');
}
function updatePreview() {
  const html = convertMarkdown();
  htmlOutput.textContent = html;
  preview.innerHTML = html;
}
markdownInput.addEventListener('input', updatePreview);
updatePreview();
