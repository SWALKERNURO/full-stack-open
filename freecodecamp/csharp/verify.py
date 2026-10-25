"""Run completed course programs against known input/output examples."""
from pathlib import Path
import subprocess
import sys
import re
import tempfile

dotnet = sys.argv[1]
root = Path(__file__).resolve().parent
assembly = root / 'bin' / 'Debug' / 'net10.0' / 'CourseLabs.dll'
cases = [
    ('greeting', 'Spencer\n', ["Hello, Spencer! It's great to meet you.", 'Uppercase: SPENCER', 'Lowercase: spencer']),
    ('profile', 'Sam\n25\n', ['Hello, Sam! You are 25 years old.', 'Name: Sam\nAge: 25']),
    ('variables', '', ['Favorite food: pizza', 'Slices eaten: 3', 'Is hungry: False', 'Favorite color: blue', 'Slices eaten after dessert: 5']),
    ('tip', '45.50\n18\n', ['Tip amount:   $8.190', 'Total amount: $53.690', 'Formatted total: $53.69']),
    ('tip', '100\n0\n', ['Rounded tip: $0.00', 'Formatted total: $100.00']),
    ('temperature', '94\n', ['94�F is 34.444444', 'Rounded: 34.4�C', 'Converted back: 94.0�F']),
    ('temperature', '32\n', ['32�F is 0�C', 'Converted back: 32.0�F']),
    ('temperature', '212\n', ['212�F is 100�C', 'Converted back: 212.0�F']),
    ('temperature', '-40\n', ['-40�F is -40�C', 'Converted back: -40.0�F']),
    ('dice 6 6 6', '', ['You rolled triples!', 'Final total: 24', 'You win a new car!']),
    ('dice 3 3 3', '', ['You rolled triples!', 'Final total: 15', 'You win a new laptop!']),
    ('dice 1 1 1', '', ['Final total: 9', 'You win a kitten!']),
    ('dice 5 5 3', '', ['You rolled doubles!', 'Final total: 15', 'You win a new laptop!']),
    ('dice 1 2 4', '', ['Final total: 7', 'You win a trip for two!']),
    ('dice 1 2 3', '', ['Final total: 6', 'You win a kitten!']),
    ('subscription 0', '', ['Your subscription has expired.']),
    ('subscription 1', '', ['Your subscription expires within a day!', 'Renew now and save 20%.']),
    ('subscription 2', '', ['Your subscription expires in 2 days.', 'Renew now and save 10%.']),
    ('subscription 5', '', ['Your subscription expires in 5 days.', 'Renew now and save 10%.']),
    ('subscription 6', '', ['Your subscription will expire soon. Renew now!']),
    ('subscription 10', '', ['Your subscription will expire soon. Renew now!']),
    ('subscription 11', '', []),
    ('@06-create-guess-the-number-game 10', '10\n', ['Correct! You got it in 1 guesses.']),
    ('@06-create-guess-the-number-game 10', '5\n15\n10\n', ['Too low.', 'Too high.', 'Correct! You got it in 3 guesses.']),
    ('@06-create-guess-the-number-game 20', '1\n2\n3\n4\n5\n', ['Guess #5:', 'Out of guesses! The number was 20.']),
    ('@06-create-guess-the-number-game 10', 'abc\n0\n21\n9\n10\n', ['Please enter a whole number between 1 and 20.', "You're very close!", 'Correct! You got it in 2 guesses.']),
    ('@07-create-a-leaderboard 42 88 15 100 67 73 29 91', '', ['Total rounds logged: 8', '1. 100 points', '2. 91 points', '3. 88 points', '4. 73 points', '5. 67 points']),
    ('@07-create-a-leaderboard 20 70 40', '', ['Total rounds logged: 3', '1. 70 points', '2. 40 points', '3. 20 points', '4. 0 points', '5. 0 points']),
    ('@07-create-a-leaderboard', '', ['Total rounds logged: 8']),
    ('@08-create-a-billing-calculator', '', ['Subtotal: $49.99', 'Tax:      $4.00', 'Total:    $53.99', 'Out-of-state subtotal: $80.00', 'Out-of-state tax:      $4.00', 'Out-of-state total:    $84.00']),
    ('@08-create-a-billing-calculator 0', '', ['Subtotal: $0.00', 'Tax:      $0.00', 'Total:    $0.00']),
]
for mode, stdin, expected in cases:
    mode_parts = mode.split()
    target = assembly
    if mode.startswith('@'):
        lab = mode_parts.pop(0)[1:]
        target = root / 'labs' / lab / 'bin' / 'Debug' / 'net10.0' / (lab + '.dll')
    result = subprocess.run([dotnet, str(target), *mode_parts], input=stdin, capture_output=True, text=True, encoding='utf-8', timeout=20, check=True)
    for value in expected:
        assert value in result.stdout, (mode, stdin, value, result.stdout)
    if mode == 'subscription 11':
        assert result.stdout.strip() == '', result.stdout
    if mode in ['subscription 0', 'subscription 6', 'subscription 10']:
        assert 'save' not in result.stdout, result.stdout
    if mode.startswith('dice') and mode != 'dice 5 5 3':
        assert 'doubles' not in result.stdout, result.stdout
    if mode.startswith('@') and 'Correct!' in result.stdout:
        assert 'Out of guesses!' not in result.stdout, result.stdout
    if mode == '@07-create-a-leaderboard':
        scores = [int(x) for x in re.findall(r'Round \d+: scored (\d+) points', result.stdout)]
        leaders = [int(x) for x in re.findall(r'^\d\. (\d+) points', result.stdout, re.M)]
        assert len(scores) == 8 and all(0 <= x <= 100 for x in scores), scores
        assert leaders == sorted(scores, reverse=True)[:5], (scores, leaders)
    print(f'PASS {mode}: {stdin.strip()!r}')
log_assembly = root / 'labs' / '09-create-a-log-file-viewer' / 'bin' / 'Debug' / 'net10.0' / '09-create-a-log-file-viewer.dll'
with tempfile.TemporaryDirectory(prefix='csharp-log-') as folder:
    log_path = Path(folder) / 'log.txt'
    previous = ''
    for count in range(1, 4):
        result = subprocess.run([dotnet, str(log_assembly)], cwd=folder, capture_output=True, text=True, encoding='utf-8', check=True, timeout=20)
        content = log_path.read_text(encoding='utf-8')
        assert content.startswith(previous) and len(content.splitlines()) == count, content
        assert f'Total events logged: {count}' in result.stdout, result.stdout
        assert len(re.findall(r'\[\d{2}:\d{2}:\d{2}\] Application started', result.stdout)) == count, result.stdout
        previous = content
        print(f'PASS log append and history: run {count}')
print(f'{len(cases) + 3} program scenarios passed')
