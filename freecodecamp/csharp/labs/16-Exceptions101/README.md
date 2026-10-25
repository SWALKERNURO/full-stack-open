# Exception handling lab

Modes preserve successive Microsoft Learn exercises in one console project:

- `unhandled`: integer division by zero escapes; floating division produces infinity.
- `handled`: the same exception is caught and execution reaches Exit program.
- `stack`: Process1 calls WriteMessage; an outer catch handles the exception.

The missing catch/finally compiler example is checked by verify_compiler.py (CS1524). All 13 compiler diagnostic scenarios passed.

The module assessment passed at 100%, and the freeCodeCamp challenge was saved.
Observed live VS Code debugger runs:
- The unhandled mode paused with DivideByZeroException. Continuing displayed the unhandled exception and did not reach Exit program.
- The handled mode printed infinity, An exception has been caught, and Exit program.
- The initial stack mode caught the exception in the outer handler.
- Only Process1 was then changed to add an inner catch. The stack mode printed Exception caught in Process1 and Exit program, matching the reviewed solution.

SpecificExamples.cs preserves modes for exception-object messages, typed division/overflow catches, the skipped second operation after the first exception, separate guards, and invalid/overflow/valid parsed input. CatchChallenge.cs uses four separate try/catch blocks and checked addition to handle every requested exception. All nine focused lab scenarios passed. Console output explicitly uses UTF-8 for Windows/Linux consistency.

The later debugger launch hung during startup despite a compatibility restart. Exception type, message, originating frames, typed-catch behavior and all requested output were verified with explicit .NET runs and assertions. The module assessment passed at 100%; this note does not claim those later inspections ran in the VS Code GUI.
