# Foundational C# course work

Source and verification for Microsoft Learn exercises completed alongside the
freeCodeCamp Foundational C# with Microsoft curriculum.

## Recorded progress

- Microsoft Learn: the first four modules and their assessments are complete;
  all four assessments scored 100%. The Part 1 learning-path trophy is earned.
- freeCodeCamp: the four matching lesson challenges are submitted and passed.
- Trophy verification is pending: creating a public Microsoft transcript link
  requires explicit approval. No transcript link has been created or submitted.
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

The current verification checks 22 program scenarios, including zero tips,
freezing/boiling/negative temperatures, mutually exclusive doubles/triples
bonuses, and subscription notification/discount boundaries.

Code was prepared and tested with AI assistance under the account owner's
direction. Completion claims reflect observed platform results, not estimated
personal study time. Microsoft Learn's listed training durations are curriculum
estimates rather than measured hours worked by the account owner.
