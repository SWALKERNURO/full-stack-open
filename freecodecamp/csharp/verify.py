"""Run completed course programs against known input/output examples."""
from pathlib import Path
import subprocess
import sys

dotnet = sys.argv[1]
root = Path(__file__).resolve().parent
assembly = root / 'bin' / 'Debug' / 'net9.0' / 'CourseLabs.dll'
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
]
for mode, stdin, expected in cases:
    result = subprocess.run([dotnet, str(assembly), *mode.split()], input=stdin, capture_output=True, text=True, encoding='utf-8', timeout=20, check=True)
    for value in expected:
        assert value in result.stdout, (mode, stdin, value, result.stdout)
    if mode == 'subscription 11':
        assert result.stdout.strip() == '', result.stdout
    if mode in ['subscription 0', 'subscription 6', 'subscription 10']:
        assert 'save' not in result.stdout, result.stdout
    if mode.startswith('dice') and mode != 'dice 5 5 3':
        assert 'doubles' not in result.stdout, result.stdout
    print(f'PASS {mode}: {stdin.strip()!r}')
print(f'{len(cases)} program scenarios passed')
