const { useState, useEffect } = React;
enum Action { EAT = 'EAT', PLAY = 'PLAY', SLEEP = 'SLEEP' }
enum PetMood {
  HAPPY,
  EXCITED,
  CONTENT,
  SAD,
  TIRED,
  SICK,
  HUNGRY
}
const moodImages: Record<PetMood, string> = {
  [PetMood.HAPPY]: '😊', [PetMood.EXCITED]: '🤩', [PetMood.CONTENT]: '🙂',
  [PetMood.SAD]: '🥺', [PetMood.TIRED]: '😴', [PetMood.SICK]: '🤒', [PetMood.HUNGRY]: '😋'
};
interface PetStats { hunger: number; happiness: number; energy: number; }
const clamp = (value: number): number => Math.max(0, Math.min(100, value));
function getMood(stats: PetStats): PetMood {
  if (stats.hunger > 70) return PetMood.HUNGRY;
  if (stats.energy < 30) return PetMood.TIRED;
  if (stats.happiness < 30) return PetMood.SAD;
  if (stats.happiness > 80 && stats.energy > 70) return PetMood.EXCITED;
  if (stats.happiness > 60) return PetMood.HAPPY;
  return PetMood.CONTENT;
}
export const PetGame = () => {
  const [name, setName] = useState('');
  const [started, setStarted] = useState(false);
  const [idleCycle, setIdleCycle] = useState(0);
  const [stats, setStats] = useState<PetStats>({ hunger: 0, happiness: 100, energy: 100 });
  useEffect(() => {
    if (!started) return;
    let remainingTicks = 20;
    const timer = window.setInterval(() => {
      setStats(previous => ({ hunger: clamp(previous.hunger + 5), happiness: clamp(previous.happiness - 5), energy: clamp(previous.energy + 5) }));
      remainingTicks -= 1;
      if (remainingTicks <= 0) window.clearInterval(timer);
    }, 5000);
    return () => window.clearInterval(timer);
  }, [started, idleCycle]);
  function perform(action: Action): void {
    setIdleCycle(previous => previous + 1);
    setStats(previous => {
      switch (action) {
        case Action.EAT: return { ...previous, hunger: clamp(previous.hunger - 20), energy: clamp(previous.energy + 10) };
        case Action.PLAY: return { ...previous, energy: clamp(previous.energy - 15), happiness: clamp(previous.happiness + 15) };
        case Action.SLEEP: return { ...previous, hunger: clamp(previous.hunger + 10), energy: clamp(previous.energy + 25) };
      }
    });
  }
  const mood = getMood(stats);
  const moodLabel = PetMood[mood][0] + PetMood[mood].slice(1).toLowerCase();
  return <main className="pet-game"><p className="eyebrow">LITTLE PAL / A POCKET COMPANION</p>{!started ? <section className="welcome"><div className="welcome-face" aria-hidden="true">🌱</div><h1>A small friend.<br />A little care.</h1><p>Name your companion to begin.</p><form onSubmit={event => { event.preventDefault(); const submittedName = (document.getElementById('pet-name') as HTMLInputElement).value.trim(); if (submittedName) { setName(submittedName); setStarted(true); } }}><label htmlFor="pet-name">What is your pet's name?</label><input id="pet-name" type="text" name="petName" maxLength={40} required placeholder="Choose a name" /><button type="submit">Meet your pal</button></form></section> : <section className="play-view"><h1 className="pet-name">{name}</h1><div className="pet-avatar" role="img" aria-label={moodLabel + ' pet'}>{moodImages[mood]}</div><p className="mood" role="status">{moodLabel}</p><div className="stats">{(['hunger','happiness','energy'] as const).map(key => <div className="stat" key={key}><div className="stat-heading"><span>{key[0].toUpperCase() + key.slice(1)}</span><span className="stat-value">{stats[key]}</span></div><progress value={stats[key]} max="100" aria-label={key} /></div>)}</div><div className="actions"><button id="eat-action" onClick={() => perform(Action.EAT)}>Eat</button><button id="play-action" onClick={() => perform(Action.PLAY)}>Play</button><button id="sleep-action" onClick={() => perform(Action.SLEEP)}>Sleep</button></div><p className="hint">A snack, a game, a nap. Little things make a happy pal.</p></section>}</main>;
};