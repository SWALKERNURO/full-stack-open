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
    ('logic equality', '', ['True\nFalse\nFalse\nTrue']),
    ('logic normalize', '', ['True']),
    ('logic inequality', '', ['False\nTrue\nTrue\nFalse']),
    ('logic comparison', '', ['False\nTrue\nTrue\nTrue']),
    ('logic contains', '', ['True\nFalse']),
    ('logic negation', '', ['False\nTrue']),
    ('logic discount 1001', '', ['Discount: 100\nDiscount: 100']),
    ('logic discount 1000', '', ['Discount: 50\nDiscount: 50']),
    ('logic discount 0', '', ['Discount: 50\nDiscount: 50']),
    ('logic coin 0', '', ['heads']),
    ('logic coin 1', '', ['tails']),
    ('logic coin', '', []),
    ('logic permissions Admin|Manager 55', '', ['Welcome, Admin user.']),
    ('logic permissions Admin 56', '', ['Welcome, Super Admin user.']),
    ('logic permissions Admin 0', '', ['Welcome, Admin user.']),
    ('logic permissions Manager 20', '', ['Contact an Admin for access.']),
    ('logic permissions Manager 19', '', ['You do not have sufficient privileges.']),
    ('logic permissions Visitor 100', '', ['You do not have sufficient privileges.']),
    ('scope inside', '', ['Inside the code block: 10']),
    ('scope initialized', '', ['Inside the code block: 0\nOutside the code block: 10']),
    ('scope constant', '', ['Inside the code block: 10\nOutside the code block: 10']),
    ('scope styles', '', ['True\nTrue\nTrue']),
    ('scope names bob', '', ['Found Bob']),
    ('scope names steve', '', ['Found Steve']),
    ('scope names alice', '', ['Found Chuck']),
    ('scope challenge', '', ['Set contains 42\nTotal: 108']),
    ('scope challenge 4 8', '', ['Total: 12']),
    ('switch employee 100', '', ['John Smith, Junior Associate']),
    ('switch employee 200', '', ['John Smith, Senior Associate']),
    ('switch employee 201', '', ['John Smith, Associate']),
    ('switch employee 300', '', ['John Smith, Manager']),
    ('switch employee 400', '', ['John Smith, Senior Manager']),
    ('switch shared 100', '', ['John Smith, Senior Associate']),
    ('switch shared 200', '', ['John Smith, Senior Associate']),
    ('switch sku', '', ['Product: Large Maroon Sweat shirt']),
    ('switch sku 02-BL-S', '', ['Product: Small Black T-Shirt']),
    ('switch sku 03-WH-M', '', ['Product: Medium White Sweat pants']),
    ('switch sku 99-XX-X', '', ['Product: One Size Fits All White Other']),
    ('for up', '', ['\n'.join(map(str, range(10)))]),
    ('for down', '', ['\n'.join(map(str, range(10, -1, -1)))]),
    ('for step', '', ['0\n3\n6\n9']),
    ('for break', '', ['0\n1\n2\n3\n4\n5\n6\n7']),
    ('for reverse', '', ['Michael\nDavid\nEddie\nAlex']),
    ('for rename', '', ['Alex\nEddie\nSammy\nMichael']),
    ('for fizzbuzz', '', ['3 - Fizz', '5 - Buzz', '15 - FizzBuzz', '100 - Buzz']),
    ('while do 2 5 8 2 7', '', ['2\n5\n8\n2\n7']),
    ('while do 7', '', ['7']),
    ('while do', '', []),
    ('while pretest 9 7 5 1', '', ['9\n7\n5\nLast number: 1']),
    ('while pretest 2', '', ['Last number: 2']),
    ('while continue 9 8 5 1 6 7', '', ['5\n1\n6\n7']),
    ('while battle 1 1 7 6 9', '', ['Monster was damaged and lost 9 health and now has -7 health.', 'Hero wins!']),
    ('while battle 1 2 1 4 7 6', '', ['Hero was damaged and lost 6 health and now has -2 health.', 'Monster wins!']),
    ('while battle 10', '', ['Monster was damaged and lost 10 health and now has 0 health.', 'Hero wins!']),
    ('while integer', 'two\n2\n7\n', ['invalid number', 'You entered 2.', 'Your input value (7) has been accepted.']),
    ('while integer', '5\n10\n6\n', ['You entered 5.', 'You entered 10.', 'Your input value (6) has been accepted.']),
    ('while integer', '9\n', ['Your input value (9) has been accepted.']),
    ('while integer', '', []),
    ('while role', 'Admin\n   Administrator   \n', ['"Admin" is not valid.', 'Your input value (Administrator) has been accepted.']),
    ('while role', 'MANAGER\n', ['Your input value (MANAGER) has been accepted.']),
    ('while role', 'user\n', ['Your input value (user) has been accepted.']),
    ('while sentences', '', ['I like pizza\nI like roast chicken\nI like salad\nI like all three of the menu choices']),
    ('@10-contoso-pets-guided', '1\n\nExit\n', ['ID #: d1', 'Nickname: lola', 'ID #: d2', 'ID #: c3', 'ID #: c4']),
    ('@10-contoso-pets-guided', '2\ndog\n?\n\n\n\nn\n1\n\nexit\n', ['We currently have 4 pets', 'ID #: d5\nSpecies: dog\nAge: ?\nNickname: tbd\nPhysical description: tbd\nPersonality: tbd']),
    ('@10-contoso-pets-guided', '2\nbird\nCAT\nwrong\n-1\n2\nWhite\nFriendly\nSnow\nmaybe\nn\n1\n\nexit\n', ['ID #: c5\nSpecies: cat\nAge: 2\nNickname: snow\nPhysical description: white\nPersonality: friendly']),
    ('@10-contoso-pets-guided', '2\ndog\n1\nsmall\nplayful\nrex\ny\ncat\n0\nblack\nquiet\nmoon\nn\n1\n\nexit\n', ['ID #: d5', 'Nickname: rex', 'ID #: c6', 'Nickname: moon']),
    ('@10-contoso-pets-guided', '2\n' + ('dog\n?\n\n\n\ny\n' * 3) + 'cat\n1\nsmall\nquiet\nlast\n\n2\n\n1\n\nexit\n', ['We have reached our limit', 'ID #: c8', 'Nickname: last']),
    ('@10-contoso-pets-guided', '3\n\n8\n\nbad\nexit\n', ['Challenge Project', 'UNDER CONSTRUCTION', 'Please select a menu option']),
    ('@10-contoso-pets-guided', '2\ndog\n', []),
    ('@11-contoso-pets-challenge', '3\none\n-1\n1\n\n   \nsmall white Siamese cat weighing about 8 pounds. litter box trained.\n\n1\n\nexit\n', ['Age and physical description fields are complete', 'ID #: c4\nSpecies: cat\nAge: 1', 'Physical description: small white Siamese cat']),
    ('@11-contoso-pets-challenge', '4\n\n   \nsnowflake\n\nloves to curl up in a warm spot\n\n1\n\nexit\n', ['Nickname and personality description fields are complete', 'Nickname: snowflake', 'Personality: loves to curl up in a warm spot']),
    ('@11-contoso-pets-challenge', '3\n2\nwhite cat\n\n3\n\n4\nsnow\nfriendly\n\n4\n\nexit\n', ['Age and physical description fields are complete', 'Nickname and personality description fields are complete']),
    ('@11-contoso-pets-challenge', '2\ndog\n?\n\n\n\nn\n3\n1\nwhite cat\n0\nblack dog\n\n4\nsnow\nquiet\nrex\nplayful\n\n1\n\nexit\n', ['ID #: d5\nSpecies: dog\nAge: 0\nNickname: rex\nPhysical description: black dog\nPersonality: playful']),
    ('@11-contoso-pets-challenge', '3\n', []),
    ('types integral', '', ['sbyte  : -128 to 127', 'short  : -32768 to 32767', 'int    : -2147483648 to 2147483647', 'long   : -9223372036854775808 to 9223372036854775807', 'byte   : 0 to 255', 'ushort : 0 to 65535', 'uint   : 0 to 4294967295', 'ulong  : 0 to 18446744073709551615']),
    ('types floating', '', ['float  : -3.4028235E+38 to 3.4028235E+38', 'double : -1.7976931348623157E+308 to 1.7976931348623157E+308', 'decimal: -79228162514264337593543950335 to 79228162514264337593543950335']),
    ('types reference', '', ['Default array: 0, 0, 0', 'Hello World!', 'val_A: 2\nval_B: 5', 'ref_A[0]: 5\nref_B[0]: 5']),
    ('convert demos', '', ['Concatenation: 24', 'int: 3\ndecimal: 3', 'decimal: 3.14\nint: 3', 'Decimal: 1.23456789\nFloat  : 1.2345679', 'ToString: 57', 'Parsed sum: 12', 'Converted product: 35', 'Truncated: 1\nRounded: 2']),
    ('convert round 1.999', '', ['Truncated: 1\nRounded: 2']),
    ('convert round 1.499', '', ['Truncated: 1\nRounded: 1']),
    ('convert round 2.5', '', ['Truncated: 2\nRounded: 2']),
    ('convert round -1.5', '', ['Truncated: -1\nRounded: -2']),
    ('convert parse 102', '', ['Measurement: 102\nMeasurement (w/ offset): 152']),
    ('convert parse bad', '', ['Unable to report the measurement.']),
    ('convert parse 9999999999999999999999', '', ['Unable to report the measurement.']),
    ('convert parse 0', '', ['Measurement: 0']),
    ('convert format-error', '', ['Expected FormatException when parsing Bob.']),
    ('convert mixed', '', ['Message: ABCDEF\nTotal: 68.3']),
    ('convert mixed 1.5 A -2 B 0.5', '', ['Message: AB\nTotal: 0.0']),
    ('convert math', '', ['as an int: 2', 'as a decimal: 1.4418604651162790697674418605', 'as a float: 0.3909091']),
    ('arrays sort', '', ['Sorted...\nA11\nA13\nB12\nB14\nReversed...\nB14\nB12\nA13\nA11']),
    ('arrays resize', '', ['Before: B14\nAfter: \nCleared value is null: True', 'Clearing 2 ... count: 4\n||B12|A13', 'Expected NullReferenceException', 'Resizing 6 ... count: 6\n||B12|A13|C01|C02', 'Resizing 3 ... count: 3\n||B12']),
    ('arrays split', '', ['321cba\n3,2,1,c,b,a\n3\n2\n1\nc\nb\na']),
    ('arrays words', '', ['ehT kciuq nworb xof spmuj revo eht yzal god']),
    ('arrays words one a three', '', ['eno a eerht']),
    ('arrays orders', '', ['A345\nB123\nB177\nB179\nC15 - Error\nC234\nC235\nG3003 - Error']),
    ('arrays orders X123,X12,X1234', '', ['X12 - Error\nX123\nX1234 - Error']),
    ('format basics', '', ['Hello World!\nWorld Hello!\nHello Hello Hello!', 'Price: $123.45 (Save $50.00)', 'Measurement: 123,456.79 units', 'Measurement: 123,456.7891 units', 'Tax rate: 36.79%', 'You saved $7.56 off the regular $67.55 price. A discount of 11.19%!']),
    ('format receipt', '', ['Invoice Number: 1201', 'Shares: 25.457 Product', 'Sub Total: $2,750.00', 'Tax: 15.83%', 'Total Billed: $3,185.19']),
    ('format padding', '', ['[    Pad this]', '[Pad this    ]', '----Pad this', 'Pad this----', '00C110']),
    ('format letter', '', ['Dear Ms. Barros,', '2,975,000.00 shares at a return of 12.75%.', 'Glorious Future offers a return of 13.13%.', 'potential profit would be $63,000,000.00.', "Here's a quick comparison:\n\n"]),
    ('strings substring', '', ['13\n36\n(inside the parentheses\ninside the parentheses\nbetween the tags']),
    ('strings last', '', ['First h: 0; last h: 7', 'set of parentheses\nWhat if\nmore than\nset of parentheses']),
    ('strings any', '', ["Found 'e' in 'Hello, world!' at index: 1.", '(find) the {opening symbols}\n(find) the {opening symbols}', 'What if\ndifferent symbols\nopen symbol\nmatching closing symbol']),
    ('strings replace', '', ['123455000  3', 'This is example data']),
    ('strings html', '', ['Quantity: 5000\nOutput: <h2>Widgets &reg;</h2><span>5000</span>']),
    ('@12-pet-search-guided', '1\n\nexit\n', ['Suggested Donation: $85.00', 'Suggested Donation: $49.99', 'Suggested Donation: $40.00', 'Suggested Donation: $45.00']),
    ('@12-pet-search-guided', '2\n\n   \n GOLDEN \n\nexit\n', ['Our dog Nickname: lola is a match!', 'Our dog Nickname: gus is a match!']),
    ('@12-pet-search-guided', '2\nmedium\n\nexit\n', ['Our dog Nickname: lola is a match!']),
    ('@12-pet-search-guided', '2\nscuba\n\nexit\n', ['None of our dogs are a match found for: scuba']),
    ('@12-pet-search-guided', '2\nkisses\n\nexit\n', ['Our dog Nickname: lola is a match!']),
    ('@12-pet-search-guided', '2\nfriendly\n\nexit\n', ['None of our dogs are a match found for: friendly']),
    ('@12-pet-search-guided', '2\n', []),
    ('@13-pet-search-challenge', '2\nlarge, cream, golden\n\nexit\n', ['Our dog Nickname: lola matches your search for cream', 'Our dog Nickname: lola matches your search for golden', 'Our dog Nickname: gus matches your search for golden', 'Our dog Nickname: gus matches your search for large']),
    ('@13-pet-search-challenge', '2\nbig, grey, stripes\n\nexit\n', ['No matches found for any available dogs: big, grey, stripes']),
    ('@13-pet-search-challenge', '2\n\n , , \n GOLDEN , big ,\n\nexit\n', ['Our dog Nickname: lola matches your search for golden', 'Our dog Nickname: gus matches your search for golden']),
    ('@13-pet-search-challenge', '2\nkisses, friendly\n\nexit\n', ['Our dog Nickname: lola matches your search for kisses']),
    ('@13-pet-search-challenge', '2\n', []),
    ('methods syntax', '', ['Before calling a method\nHello World!\nAfter calling a method\n1 2 3 4 5']),
    ('methods random', '', ['Generating random numbers:']),
    ('methods times', '-6\n6\n', ['8:00 12:00 16:00 20:00', 'New Medicine Schedule:\n20:00 0:00 4:00 8:00']),
    ('methods times', '6\n-6\n', ['New Medicine Schedule:\n20:00 0:00 4:00 8:00']),
    ('methods times', '-6\n-8\n', ['New Medicine Schedule:\n6:00 10:00 14:00 18:00']),
    ('methods times', '13\n0\n', ['Invalid GMT', 'New Medicine Schedule:\n8:00 12:00 16:00 20:00']),
    ('methods ipv4', '', ['107.31.1.5 is a valid IPv4 address', '255.0.0.255 is a valid IPv4 address', '555..0.555 is an invalid IPv4 address', '255...255 is an invalid IPv4 address']),
    ('methods ipv4 0.0.0.0 255.255.255.255 1.02.3.4 1..2.3.4 256.2.3.4 999999999999.1.1.1 1.a.2.3', '', ['0.0.0.0 is a valid', '255.255.255.255 is a valid', '1.02.3.4 is an invalid', '1..2.3.4 is an invalid', '256.2.3.4 is an invalid', '999999999999.1.1.1 is an invalid', '1.a.2.3 is an invalid']),
    ('methods fortune 24', '', ['You have much to fear.', 'may have unexpected outcomes.']),
    ('methods fortune 25', '', ['You have much to appreciate.', 'should align with your values.']),
    ('methods fortune 75', '', ['You have much to appreciate.', 'should align with your values.']),
    ('methods fortune 76', '', ['You have much to look forward to.', 'is likely to succeed.']),
    ('parameters times', '', ['800 -> 2000\n1200 -> 0\n1600 -> 400\n2000 -> 800']),
    ('parameters scope', '', ['Jenna, Ayesha, Carlos, Viktor,', 'Robert, Vanya,', 'Circle with radius 12', 'Area = 452.38896', 'Circle with radius 24', 'Area = 1809.55584']),
    ('parameters value', '', ['inside Multiply method: 3 x 4 = 12\nglobal statement: 3 x 4 = 0']),
    ('parameters array', '', ['1 2 3 4 5\n0 0 0 0 0']),
    ('parameters string', '', ['Start: Healthy\nMiddle: Unhealthy\nEnd: Healthy']),
    ('parameters captured-string', '', ['Start: Healthy\nMiddle: Unhealthy\nEnd: Unhealthy']),
    ('parameters rsvp', '', ['Sorry, Tony is not on the guest list', 'Name: Rebecca, \tParty Size: 1, \tAllergies: none', 'Name: Nadia, \tParty Size: 2, \tAllergies: Nuts', 'Name: Linh, \tParty Size: 2, \tAllergies: none', 'Name: Noor, \tParty Size: 4, \tAllergies: none', 'Name: Jonte, \tParty Size: 2, \tAllergies: Stone fruit']),
    ('parameters email', '', ['robavin@contoso.com\nsibright@contoso.com\nkisinclair@contoso.com\naakamath@contoso.com\nsadelucchi@contoso.com\nsiali@contoso.com\nviashton@hayworth.com\ncodysart@hayworth.com\nshlawrence@hayworth.com\ndavaldes@hayworth.com']),
    ('returns checkout', '', ['Total: $44.58']),
    ('returns currency', '', ['$23.73 USD = $557655 VND\n$557655 VND = $23.73 USD']),
    ('returns words', '', ['snake\nekans', 'there are snakes at the zoo\nereht era sekans ta eht ooz']),
    ('returns palindrome', '', ['racecar: True', 'talented: False', 'deified: True', 'tent: False', 'tenet: True']),
    ('returns palindrome a ab aa abba abcba', '', ['a: True', 'ab: False', 'aa: True', 'abba: True', 'abcba: True']),
    ('returns coins-one 60', '', ['Change found at positions 2 and 5']),
    ('returns coins-one 80', '', ['No two coins make change']),
    ('returns coins 30', '', ['Change found at positions:\n0,3\n0,4\n1,3\n1,4\n3,6']),
    ('returns coins 80', '', ['No two coins make change']),
    ('returns coins 2 1 1 1 1 1 1', '', ['0,1\n0,2\n0,3\n0,4\n0,5']),
    ('returns game', 'n\n', ['Would you like to play?']),
    ('returns game 1 2 5 6 1 1', 'Y\ny\ny\nn\n', ['Roll a number greater than 1 to win!', 'You rolled a 2', 'Roll a number greater than 5 to win!', 'You rolled a 6', 'You rolled a 1', 'You lose!']),
    ('returns game', 'y\nn\n', ['Play again?']),
    ('zoo', '', ['School A', 'School B', 'School C']),
    ('zoo 42', '', ['School A', 'School B', 'School C']),
    ('@14-mini-game --verify', '', ['PASS food overlap detection', 'PASS optional horizontal speed', 'PASS temporary freeze and normal-state recovery', 'PASS food regeneration within bounds', 'PASS terminal resize detection', 'PASS optional unsupported-input termination', 'PASS escape termination']),
]
prefix = sys.argv[2] if len(sys.argv) > 2 else ''
if prefix:
    cases = [case for case in cases if case[0].startswith(prefix)]
    assert cases, f'No scenarios matched {prefix!r}'
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
    if mode == 'logic coin':
        assert result.stdout.strip() in ['heads', 'tails'], result.stdout
    if mode == 'methods random':
        values = list(map(int, result.stdout.splitlines()[1].split()))
        assert len(values) == 5 and all(1 <= x <= 99 for x in values), result.stdout
    if mode == 'parameters rsvp':
        assert result.stdout.count('Name: ') == 5 and 'Name: Tony' not in result.stdout, result.stdout
    if mode == 'parameters email':
        assert len(result.stdout.splitlines()) == 10, result.stdout
    if mode.startswith('zoo'):
        animals = 'alpacas capybaras chickens ducks emus geese goats iguanas kangaroos lemurs llamas macaws ostriches pigs ponies rabbits sheep tortoises'.split()
        sections = re.split(r'School [ABC]\n', result.stdout)[1:]
        orders = []
        for section, groups in zip(sections, [6, 3, 2]):
            rows = section.strip().splitlines()
            assert len(rows) == groups, (groups, rows)
            found = []
            for i, row in enumerate(rows, 1):
                label, names = row.split(': ', 1)
                assert label == f'Group {i}', row
                assigned = names.split()
                assert len(assigned) == 18 // groups, assigned
                found.extend(assigned)
            assert sorted(found) == animals, found
            orders.append(found)
        assert len(sections) == 3 and orders[0] != orders[1] and orders[1] != orders[2], orders
    if mode == 'returns coins 2 1 1 1 1 1 1':
        assert len(result.stdout.strip().splitlines()) == 6, result.stdout
    if mode == 'returns game' and stdin == 'n\n':
        assert 'You rolled' not in result.stdout, result.stdout
    if mode == 'returns game' and stdin == 'y\nn\n':
        target = int(re.search(r'greater than (\d+)', result.stdout)[1])
        roll = int(re.search(r'You rolled a (\d+)', result.stdout)[1])
        assert 1 <= target <= 5 and 1 <= roll <= 6, result.stdout
        assert ('You win!' in result.stdout) == (roll > target), result.stdout
    if mode == 'returns game 1 2 5 6 1 1':
        assert result.stdout.count('You win!') == 2 and result.stdout.count('You lose!') == 1, result.stdout
    if mode == 'scope challenge 4 8':
        assert 'Set contains 42' not in result.stdout, result.stdout
    if mode == 'for break':
        assert result.stdout.strip().splitlines() == list(map(str, range(8))), result.stdout
    if mode == 'while do':
        values = list(map(int, result.stdout.split()))
        assert values and values[-1] == 7 and all(1 <= x <= 10 for x in values), values
        assert 7 not in values[:-1], values
    if mode == 'while battle 10':
        assert 'Hero was damaged' not in result.stdout, result.stdout
    if mode == 'while integer' and not stdin:
        assert 'accepted' not in result.stdout, result.stdout
    if mode in ['convert parse bad', 'convert parse 0', 'convert parse 9999999999999999999999']:
        assert 'offset' not in result.stdout, result.stdout
    if mode == 'format padding':
        line = next(x for x in result.stdout.splitlines() if x.startswith('769C'))
        assert len(line) == 40 and line[:6] == '769C  ' and line[6:30].rstrip() == 'Mr. Stephen Ortega' and line[30:] == ' $5,000.00', repr(line)
    if mode == 'format letter':
        rows = result.stdout.strip().splitlines()[-2:]
        assert rows[0][:20] == 'Magic Yield'.ljust(20) and rows[1][:20] == 'Glorious Future'.ljust(20), rows
        assert rows[0][20:30] == '12.75%'.ljust(10) and rows[1][20:30] == '13.13%'.ljust(10), rows
    if mode == '@12-pet-search-guided':
        if stdin.startswith('1\n'):
            assert result.stdout.count('Suggested Donation:') == 4, result.stdout
        if 'medium' in stdin or 'kisses' in stdin:
            assert result.stdout.count('is a match!') == 1 and 'Nickname: gus' not in result.stdout, result.stdout
        if 'GOLDEN' in stdin:
            assert result.stdout.count('is a match!') == 2 and result.stdout.count('Enter one desired') == 3, result.stdout
    if mode == '@13-pet-search-challenge' and stdin != '2\n':
        matches = [x for x in result.stdout.splitlines() if x.startswith('Our dog')]
        if 'cream' in stdin:
            assert len(matches) == 4 and 'cream' in matches[0] and 'golden' in matches[1] and 'golden' in matches[2] and 'large' in matches[3], matches
            assert result.stdout.count('Nickname: lola (ID #: d1)') == 1 and result.stdout.count('Nickname: gus (ID #: d2)') == 1, result.stdout
        if 'GOLDEN' in stdin:
            assert result.stdout.count('Enter dog characteristics') == 3, result.stdout
        if 'friendly' in stdin:
            assert len(matches) == 1 and 'Nickname: gus (ID' not in result.stdout, result.stdout
        # Captured output retains animation frames; an interactive terminal overwrites them.
        for count in [2, 1, 0]:
            assert re.search(r'searching our dog .*? [|/\\-] ' + str(count), result.stdout), result.stdout
    if mode == '@10-contoso-pets-guided':
        if stdin.startswith('1\n'):
            assert result.stdout.count('ID #: ') == 4, result.stdout
        if 'Nickname: last' in result.stdout:
            assert result.stdout.count('ID #: ') == 8 and 'ID #: d9' not in result.stdout, result.stdout
        if 'bird' in stdin:
            assert result.stdout.count("Enter 'dog' or 'cat'") == 2, result.stdout
            assert result.stdout.count("Enter the pet's age") == 3, result.stdout
    if mode == '@11-contoso-pets-challenge':
        assert 'for ID #: d1' not in result.stdout and 'for ID #: d2' not in result.stdout and 'for ID #: c3' not in result.stdout, result.stdout
        if stdin.startswith('3\none'):
            assert result.stdout.count('Enter an age for ID #: c4') == 3, result.stdout
            assert result.stdout.count('Enter a physical description for ID #: c4') == 3, result.stdout
        if stdin.startswith('3\n2'):
            assert result.stdout.count('Enter an age for ID #: c4') == 1, result.stdout
            assert result.stdout.count('Enter a nickname for ID #: c4') == 1, result.stdout
        if stdin == '3\n':
            assert 'fields are complete' not in result.stdout, result.stdout
    if mode == 'for fizzbuzz':
        lines = result.stdout.strip().splitlines()
        assert len(lines) == 100, len(lines)
        for n, line in enumerate(lines, 1):
            words = ('Fizz' if n % 3 == 0 else '') + ('Buzz' if n % 5 == 0 else '')
            assert line == str(n) + (' - ' + words if words else ''), (n, line)
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
if prefix:
    print(f'{len(cases)} selected program scenarios passed')
    sys.exit(0)
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
