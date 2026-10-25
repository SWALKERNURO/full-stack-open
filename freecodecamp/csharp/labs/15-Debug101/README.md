# Debugger lab evidence

Microsoft Learn: Implement the Visual Studio Code debugging tools for C#.

- Built Debug101 using .NET 10 with zero warnings and errors.
- Launched the compiled DLL in VS Code's coreclr debugger; also ran without debugging.
- Reproduced the unexpected repeated Andrew greeting.
- Paused at the first loop condition (name = Sophia), stepped into SophiaMessage, examined the two-frame call stack, and stepped back out.
- Observed execution reach the unconditional Andrew append with messageText still containing Sophia's greeting.
- Added braces around the AllGreetings branch. The debugger console then showed the expected single-person greetings and combined greeting.
- For the 2,000-product exercise, paused inside Process1 and Process2 and observed the iteration counter reach 3.

The original samples use a busy pause loop; these snapshots sleep briefly while paused to avoid occupying a CPU core. Local extension project loading encountered a named-pipe access error. An explicit build task and coreclr launch configuration were used; security settings were not changed.

All module exercises were completed. The module assessment passed with 100%, and the corresponding freeCodeCamp challenge was saved.
- Conditional breakpoint products[i,1] == new skipped to indices 204 and 466; Process1 reported a new product. Base C# debugger was enabled independently after disabling the optional Dev Kit for this workspace.

- Internal console reproduced Console.Clear IOException. Switching launch.json to integratedTerminal enabled input. Input 3 produced 9; debugger showed n=3, i=3, and sum changing to 4, isolating the zero-based index mismatch.

- Corrected SumValues call to use startIndex - 1. Live debugger terminal verified input 3 returns 12.

- Watch expression num2 / num1 > 5 evaluated true for 43/5, false for 13/5, then true for 49/7. Changed exit from false to true in VARIABLES; continuing produced exit code 0. An explicit non-incremental rebuild resolved a stale assembly/source mismatch.

- Challenge debugger showed x=5, ChangeValue's local value changing from 5 to 10, and caller x remaining 5 after the method returned. The faulty program printed 5. Fix returns the integer and assigns it to x, matching the reviewed solution.

- Corrected value-return program printed 10 and exited with code 0 in the VS Code debugger.
