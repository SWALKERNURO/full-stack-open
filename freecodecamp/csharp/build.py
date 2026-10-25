"""Build the main course examples and every completed lab project."""
from pathlib import Path
import subprocess
import sys

root = Path(__file__).resolve().parent
projects = sorted(p for p in root.rglob('*.csproj') if not {'bin', 'obj'}.intersection(p.relative_to(root).parts))
for project in projects:
    subprocess.run([sys.argv[1], 'build', str(project), '--nologo', '-v', 'quiet'], cwd=root, check=True)
print(f'{len(projects)} C# projects built')
