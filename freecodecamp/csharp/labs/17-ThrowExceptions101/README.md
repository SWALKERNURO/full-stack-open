# Creating and throwing exceptions

Microsoft Learn module completed, assessment 100%, freeCodeCamp challenge saved.

`dotnet run --project ThrowExceptions101.csproj` runs the interactive bounds exercise. An ArgumentOutOfRangeException names upperBound when lowerBound >= upperBound; the caller catches it and accepts a replacement upper bound or Exit. Invalid integers and end-of-input are handled. The arithmetic-progression endpoint average matches the lesson's even-number average while avoiding sum/index overflow and multi-billion-iteration loops for integer-limit inputs.

`baseline` preserves the challenge's status-string workflow. `workflow` replaces status returns with void methods and typed exceptions. Workflow1 handles FormatException and continues to subsequent data; the caller handles DivideByZeroException. The default output is checked exactly against the required challenge output. Additional fixtures are workflow-valid, workflow-format, and workflow-zero-first.

Sixteen focused scenarios passed: eleven bounds/input cases and five workflow cases. The first cases verified 3..11 = 7 and recovery from 3..3 to 3..11. Boundary cases cover negative ranges, single even values, and full integer bounds. EOF and Exit terminate cleanly. The final workflow output matches the reviewed solution.

The preceding lab records the VS Code debugger startup limitation. These code paths were compiled and verified through the .NET CLI; this note does not claim a successful GUI debug launch for this module.