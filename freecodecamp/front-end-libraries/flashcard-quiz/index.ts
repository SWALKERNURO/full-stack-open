interface FlashCard {
  questionText: string;
  questionAnswer: string;
}
class InvalidUserInputError extends Error {
  constructor(message: string) { super(message); this.name = 'InvalidUserInputError'; }
}
const storageKey = 'recall-flashcards-v1';
let currentCards: FlashCard[] = [
  { questionText: 'What does semantic HTML describe?', questionAnswer: 'The meaning and structure of content.' },
  { questionText: 'What does CSS Grid arrange?', questionAnswer: 'Content in rows and columns.' }
];
try {
  const saved = JSON.parse(localStorage.getItem(storageKey) || 'null');
  if (Array.isArray(saved) && saved.every(card => typeof card.questionText === 'string' && typeof card.questionAnswer === 'string')) currentCards = saved;
} catch { /* Keep the starter deck if stored data is unavailable. */ }
let currentIndex = Math.max(0, currentCards.length - 1);
const flashcard = document.getElementById('flashcard') as HTMLButtonElement;
const entryForm = document.getElementById('entry-form') as HTMLFormElement;
const frontText = document.getElementById('front-text') as HTMLTextAreaElement;
const backText = document.getElementById('back-text') as HTMLTextAreaElement;
function renderCard(): void {
  const card = currentCards[currentIndex];
  document.getElementById('question')!.textContent = card?.questionText || 'Your deck is empty. Add a card below.';
  document.getElementById('answer')!.textContent = card?.questionAnswer || 'A new idea is waiting.';
  document.getElementById('card-count')!.textContent = currentCards.length ? (currentIndex + 1) + ' / ' + currentCards.length : '0 cards';
  flashcard.classList.remove('flipped');
}
function saveCards(): void {
  try { localStorage.setItem(storageKey, JSON.stringify(currentCards)); } catch { /* Cards remain available for this session. */ }
}
flashcard.addEventListener('click', () => flashcard.classList.toggle('flipped'));
document.getElementById('previous-btn')!.addEventListener('click', () => {
  if (currentCards.length) currentIndex = (currentIndex - 1 + currentCards.length) % currentCards.length;
  renderCard();
});
document.getElementById('next-btn')!.addEventListener('click', () => {
  if (currentCards.length) currentIndex = (currentIndex + 1) % currentCards.length;
  renderCard();
});
document.getElementById('delete-btn')!.addEventListener('click', () => {
  if (!currentCards.length) return;
  currentCards.splice(currentIndex, 1);
  currentIndex = Math.max(0, currentIndex - 1);
  saveCards();
  renderCard();
});
entryForm.addEventListener('submit', (event) => {
  event.preventDefault();
  const questionText = frontText.value.trim();
  const questionAnswer = backText.value.trim();
  if (!questionText || !questionAnswer) {
    const message = 'Please enter both a question and an answer.';
    document.getElementById('form-error')!.textContent = message;
    throw new InvalidUserInputError(message);
  }
  document.getElementById('form-error')!.textContent = '';
  currentCards.push({ questionText, questionAnswer });
  currentIndex = currentCards.length - 1;
  entryForm.reset();
  saveCards();
  renderCard();
});
renderCard();