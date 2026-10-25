"""Exercise the compiler diagnostics demonstrated in the scope/method lessons."""
from concurrent.futures import ThreadPoolExecutor
from pathlib import Path
import subprocess
import sys
import tempfile

dotnet = sys.argv[1]
scratch = Path(__file__).resolve().parent / 'obj' / 'compiler-checks'
scratch.mkdir(parents=True, exist_ok=True)
examples = [
    ('try-without-handler', 'try { Console.WriteLine(3000); } Console.WriteLine("Exit program");', 'CS1524'),
    ('missing-return', 'double GetDiscountedPrice(int itemIndex) { } Console.WriteLine(GetDiscountedPrice(0));', 'CS0161'),
    ('narrow-return-type', 'int UsdToVnd(double usd) { return 23500 * usd; } Console.WriteLine(UsdToVnd(23.73));', 'CS0266'),
    ('method-local-scope', 'PrintCircleArea(12); double circumference = 2 * pi * radius; void PrintCircleArea(int radius) { double pi = 3.14159; Console.WriteLine(pi * radius * radius); }', 'CS0103'),
    ('named-argument-order', 'RSVP(allergies: "none", inviteOnly: false, "Linh", 2); void RSVP(string name, int partySize, string allergies, bool inviteOnly) {}', 'CS8323'),
    ('string-addition-to-int', 'int first = 2; string second = "4"; int result = first + second; Console.WriteLine(result);', 'CS0029'),
    ('foreach-assignment', 'string[] names = { "Alex", "David" }; foreach (var name in names) { if (name == "David") name = "Sammy"; }', 'CS1656'),
    ('outside-block', 'bool flag = true; if (flag) { int value = 10; Console.WriteLine(value); } Console.WriteLine(value);', 'CS0103'),
    ('unassigned-local', 'bool flag = true; int value; if (flag) { Console.WriteLine(value); } value = 10; Console.WriteLine(value);', 'CS0165'),
    ('conditional-assignment', 'bool flag = true; int value; if (flag) { value = 10; } Console.WriteLine(value);', 'CS0165'),
    ('void-result', 'void Add(int a, int b) { Console.WriteLine(a + b); } int result = Add(3, 4); Console.WriteLine(result);', 'CS0029'),
    ('missing-namespace', 'Console.WriteLine(Calculator.Add(2, 3)); namespace MathUtilities { public static class Calculator { public static int Add(int a, int b) => a + b; } }', 'CS0103'),
    ('constant-true', 'int value; if (true) { value = 10; } Console.WriteLine(value);', None),
]

def check(example):
    name, source, diagnostic = example
    with tempfile.TemporaryDirectory(prefix='csharp-compiler-', dir=scratch) as folder:
        root = Path(folder)
        (root / 'Example.csproj').write_text('<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net10.0</TargetFramework><ImplicitUsings>enable</ImplicitUsings></PropertyGroup></Project>', encoding='utf-8')
        (root / 'NuGet.Config').write_text('<configuration><packageSources><clear /></packageSources></configuration>', encoding='utf-8')
        (root / 'Program.cs').write_text(source, encoding='utf-8')
        result = subprocess.run([dotnet, 'build', '--nologo', '-v', 'quiet'], cwd=root, capture_output=True, text=True, encoding='utf-8', timeout=120)
        if diagnostic:
            assert result.returncode != 0 and diagnostic in result.stdout, (name, result.stdout, result.stderr)
        else:
            assert result.returncode == 0, (name, result.stdout, result.stderr)
        return f'PASS {name}: {diagnostic or "build succeeds"}'

with ThreadPoolExecutor(max_workers=3) as executor:
    for result in executor.map(check, examples):
        print(result, flush=True)
print(f'{len(examples)} compiler diagnostic scenarios passed')
