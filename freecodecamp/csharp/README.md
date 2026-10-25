# Foundational C# course work

Source and verification for Microsoft Learn exercises completed alongside the
freeCodeCamp Foundational C# with Microsoft curriculum.

## Recorded progress

- Microsoft Learn: the first nine modules and their assessments are complete;
  all nine assessments scored 100%. The Part 1 and Part 2 trophies are earned.
- freeCodeCamp: both corresponding sections are fully passed, including their
  verified trophies: 11 of 40 course challenges complete.
- The Microsoft account is linked through the approved transcript-sharing flow.
- The remaining course sections and final certification exam are not complete.

## Programs

`Program.cs` includes greeting, profile, variable, tip-calculator, temperature,
dice-game, and subscription-renewal exercises. Normal dice and subscription runs
use random values. Optional arguments allow reproducible branch verification.

```text
dotnet build CourseLabs.csproj
python verify.py /path/to/dotnet
dotnet run --project CourseLabs.csproj -- dice
dotnet run --project CourseLabs.csproj -- subscription
```

The current verification checks 34 program scenarios, including zero tips,
freezing/boiling/negative temperatures, mutually exclusive doubles/triples
bonuses, and subscription notification/discount boundaries.

The standalone labs also include the VS Code console project, a five-guess
number game, a Top 5 leaderboard, a billing calculator, and an append-only log
viewer. The development setup uses Visual Studio Code with C# Dev Kit and .NET
10. The starter lab files come from
https://github.com/MicrosoftLearning/mslearn-csharp-programming at commit
`3419c4256ca1c8d6c225e3132ee359a981f30f63`; their license is retained in
`MICROSOFT-STARTER-LICENSE.txt`.

Build all completed projects with `python build.py /path/to/dotnet`, then run
`python verify.py /path/to/dotnet`. There are currently six projects and 34
passing program scenarios. Log checks use an isolated temporary directory and
verify that repeated runs preserve existing entries.

Code was prepared and tested with AI assistance under the account owner's
direction. Completion claims reflect observed platform results, not estimated
personal study time. Microsoft Learn's listed training durations are curriculum
estimates rather than measured hours worked by the account owner.
