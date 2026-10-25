# Final cash-register challenge

Derived from MicrosoftLearning/Challenge-project-debugging-CSharp at commit 020d9c383e1049f78a9d564c879e9c75eb45b085; license retained. Starter.original.cs.txt preserves the supplied source.

The baseline reproduces both failure messages and a discrepancy across 100 attempted transactions. With seed 42 it ends at actual 3100 versus expected 1250. The corrected run ends at 1321 for both figures.

MakeChange now rejects underpayment before changing the till. All remaining payment/change updates are staged in a copy; the original bill counts are committed only if exact change is possible. Failure after preparing some change therefore leaves every original count unchanged. Change output is emitted after success. This fixes both corruption paths without erasing or hiding failed transactions.

The standard run simulates 100 random item costs in 2..49. Optional seeds reproduce runs. Every transaction checks that denomination counts are nonnegative and that the actual balance equals the independently maintained expected balance. Failure messages remain visible as required. Both required messages occur with seed 42.

`--verify` checks 17 state scenarios, including underpayment, empty tills, failure after partial change, newly received bills, denomination selection, invalid arguments, and overflow without mutation. Twelve focused program scenarios passed, covering the baseline, sample, eight 100-transaction simulations, and state checks.

The final project assessment passed at 100%; the freeCodeCamp project challenge was saved. As recorded in preceding labs, the later VS Code debug adapter stalled; code was isolated and verified through explicit .NET runs and assertions. No later GUI debugger steps are claimed.