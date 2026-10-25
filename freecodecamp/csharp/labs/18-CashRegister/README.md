# Guided cash-register project

Derived from MicrosoftLearning/Guided-project-debugging-CSharp; license retained. Starter.original.cs.txt preserves the source before changes.

- baseline-sample reproduces the starter's 551-dollar balanced result.
- baseline-random 42 reproduces a discrepancy: actual 375, expected 440.
- FixedReplay changes the five-dollar loop to decrement cashTill[1] instead of cashTill[2]. fixed-random 42 then balances at 440; every intermediate balance is checked. Additional seeds 7 and 200 also balance.
- GuidedFinalReplay converts MakeChange to void, throws InvalidOperationException for underpayment and insufficient change, catches it in the caller, and tests 40 transactions with costs 2..49. Both requested exception messages occur for seed 42.

The guided module explicitly leaves additional transaction-cancellation balance bugs for the following challenge project. That preserved stage ends with actual 1300 versus expected 1068 for seed 42. It is a diagnostic reproducer, not the final corrected transaction implementation.

Eight focused scenarios passed. The initial Console.Clear / interactive pause are omitted or guarded in replay modes so redirected CLI verification can finish cleanly; transaction logic is retained. The VS Code launch configuration selects integratedTerminal. Due to the previously recorded debugger startup hang, later code paths were verified with .NET runs and transaction-by-transaction assertions rather than claimed GUI debugger steps.
Module assessment passed at 100%; freeCodeCamp guided-project challenge saved.
