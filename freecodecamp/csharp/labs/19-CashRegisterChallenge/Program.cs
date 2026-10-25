Console.OutputEncoding = System.Text.Encoding.UTF8;
string mode = args.FirstOrDefault() ?? "random";
int seed = args.Length > 1 ? int.Parse(args[1]) : Random.Shared.Next();
if (mode == "--verify") CashChecks.Run();
else if (mode == "baseline-sample") ChallengeStarterReplay.Run(false, seed);
else if (mode == "baseline-random") ChallengeStarterReplay.Run(true, seed);
else CashRegisterSimulation.Run(mode != "sample", seed);