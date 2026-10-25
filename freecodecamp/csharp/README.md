# Foundational C# course work

Source and verification for Microsoft Learn exercises completed alongside the
freeCodeCamp Foundational C# with Microsoft curriculum.

## Recorded progress

- Microsoft Learn: all thirty-four modules and assessments are complete;
  all thirty-four assessments scored 100%. All six learning-path trophies are earned.
- freeCodeCamp: all six sections and their trophies are verified: 40 of 40
  course challenges complete.
- The Microsoft account is linked through the approved transcript-sharing flow.
- Final certification details are managed in the freeCodeCamp account; this
  repository records the completed coursework and code verification.

## Programs

`Program.cs` includes greeting, profile, variable, tip-calculator, temperature,
dice-game, subscription-renewal, Boolean logic, scope, switch, for, and while
exercises, plus numeric types, conversions, array methods, formatting, and
string extraction/replacement. Normal dice and subscription runs
use random values. Optional arguments allow reproducible branch verification.

```text
dotnet build CourseLabs.csproj
python verify.py /path/to/dotnet
dotnet run --project CourseLabs.csproj -- dice
dotnet run --project CourseLabs.csproj -- subscription
```

The current verification checks 234 program scenarios, including zero tips,
freezing/boiling/negative temperatures, mutually exclusive doubles/triples
bonuses, subscription notification/discount boundaries, SKU defaults, all 100
FizzBuzz lines, battle outcomes, input validation, pet-data workflows, numeric
precision and conversion, report column positions, and single/multiple term
dog-description searches.

The standalone labs also include the VS Code console project, a five-guess
number game, a Top 5 leaderboard, a billing calculator, and an append-only log
viewer. Two Contoso Pets projects implement sample records, a repeating menu,
validated entry, eight-pet capacity, and completion of missing age, description,
nickname and personality fields. Two further pet-search projects add suggested
donations formatted as currency and sorted, comma-separated search terms. The
final search reports each matching term and displays each dog's description
once. A rotating countdown runs from 2 to 0 and clears its status line; this
was also verified in an interactive terminal. Complete records are preserved; unknown and
blank fields prompt for valid input. The development setup uses Visual Studio Code with C# Dev Kit and .NET
10. The starter lab files come from
https://github.com/MicrosoftLearning/mslearn-csharp-programming at commit
`3419c4256ca1c8d6c225e3132ee359a981f30f63`; their license is retained in
`MICROSOFT-STARTER-LICENSE.txt`.

Build all completed projects with `python build.py /path/to/dotnet`, then run
`python verify.py /path/to/dotnet`. There are currently sixteen projects and 234
passing program scenarios. Log checks use an isolated temporary directory and
verify that repeated runs preserve existing entries.

`python verify_compiler.py /path/to/dotnet` verifies thirteen compiler examples,
including out-of-scope variables, definite assignment, and read-only foreach
iteration variables. Expected diagnostics are checked in temporary projects.
The Contoso Pets data is derived from Microsoft's official
`Guided-project-branching-looping-CSharp` and
`Challenge-project-branching-looping-CSharp`,
`Guided-project-Work-with-variable-data-CSharp`, and
`Challenge-project-work-variable-data-CSharp` starter projects; each lab retains
the applicable license.

Method exercises cover random output, time-zone adjustment, IPv4 validation,
fortune branches, optional arguments, generated sample addresses, string and
Boolean results, and bounded one- and two-dimensional array results. The zoo
planner shuffles all 18 animals and assigns 6, 3, and 2 groups to the three schools.
The mini-game, developed from `Challenge-project-Create-methods-in-CSharp`,
supports food consumption, temporary freezing, optional speed and key handling,
and resize termination. Live VS Code terminal checks verified food cleanup,
appearance changes, freeze recovery, normal movement with speed disabled,
unsupported-key behavior, and the resized-terminal exit message. Its `--verify`
mode also checks fourteen game-state and boundary conditions.

For focused local checks, pass a mode prefix after the dotnet path, for example
`python verify.py /path/to/dotnet returns`. The default verification still runs
the full suite. Numeric report precision is explicit to keep output consistent
between Windows and Linux culture data.

Code was prepared and tested with AI assistance under the account owner's
direction. Completion claims reflect observed platform results, not estimated
personal study time. Microsoft Learn's listed training durations are curriculum
estimates rather than measured hours worked by the account owner.

The debugging and exception labs retain exercise stages separately from the
corrected final cash-register project. The final project simulates 100 transactions,
checks the independent balance after each attempt, and commits bill changes only
after exact change is available. Its 17 state checks cover successful payments,
underpayment, partial-change failure, input validation, and overflow without mutation.

Live VS Code debugger work verified breakpoints, stepping, call stacks, conditional
breakpoints, watches, runtime variable edits, and the first exception exercises.
Later debug-adapter startup hung in this environment. Those later paths were
verified with explicit .NET runs and assertions; the per-lab notes record that
limitation and do not claim successful later GUI debug sessions.