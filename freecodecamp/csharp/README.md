# Foundational C# course work

Source and verification for Microsoft Learn exercises completed alongside the
freeCodeCamp Foundational C# with Microsoft curriculum.

## Recorded progress

- Microsoft Learn: the first sixteen modules and their assessments are complete;
  all sixteen assessments scored 100%. The Part 1, 2, and 3 trophies are earned.
- freeCodeCamp: the first three sections are fully passed, including their
  verified trophies: 19 of 40 course challenges complete.
- The Microsoft account is linked through the approved transcript-sharing flow.
- The remaining course sections and final certification exam are not complete.

## Programs

`Program.cs` includes greeting, profile, variable, tip-calculator, temperature,
dice-game, subscription-renewal, Boolean logic, scope, switch, for, and while
exercises. Normal dice and subscription runs
use random values. Optional arguments allow reproducible branch verification.

```text
dotnet build CourseLabs.csproj
python verify.py /path/to/dotnet
dotnet run --project CourseLabs.csproj -- dice
dotnet run --project CourseLabs.csproj -- subscription
```

The current verification checks 108 program scenarios, including zero tips,
freezing/boiling/negative temperatures, mutually exclusive doubles/triples
bonuses, subscription notification/discount boundaries, SKU defaults, all 100
FizzBuzz lines, battle outcomes, input validation, and pet-data workflows.

The standalone labs also include the VS Code console project, a five-guess
number game, a Top 5 leaderboard, a billing calculator, and an append-only log
viewer. Two Contoso Pets projects implement sample records, a repeating menu,
validated entry, eight-pet capacity, and completion of missing age, description,
nickname and personality fields. Complete records are preserved; unknown and
blank fields prompt for valid input. The development setup uses Visual Studio Code with C# Dev Kit and .NET
10. The starter lab files come from
https://github.com/MicrosoftLearning/mslearn-csharp-programming at commit
`3419c4256ca1c8d6c225e3132ee359a981f30f63`; their license is retained in
`MICROSOFT-STARTER-LICENSE.txt`.

Build all completed projects with `python build.py /path/to/dotnet`, then run
`python verify.py /path/to/dotnet`. There are currently eight projects and 108
passing program scenarios. Log checks use an isolated temporary directory and
verify that repeated runs preserve existing entries.

`python verify_compiler.py /path/to/dotnet` verifies seven compiler examples,
including out-of-scope variables, definite assignment, and read-only foreach
iteration variables. Expected diagnostics are checked in temporary projects.
The Contoso Pets data is derived from Microsoft's official
`Guided-project-branching-looping-CSharp` and
`Challenge-project-branching-looping-CSharp` starter projects; each lab retains
the applicable license.

Code was prepared and tested with AI assistance under the account owner's
direction. Completion claims reflect observed platform results, not estimated
personal study time. Microsoft Learn's listed training durations are curriculum
estimates rather than measured hours worked by the account owner.
