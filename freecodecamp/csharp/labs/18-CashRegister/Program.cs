Console.OutputEncoding = System.Text.Encoding.UTF8;
string mode = args.FirstOrDefault() ?? "guided";
int seed = args.Length > 1 ? int.Parse(args[1]) : Random.Shared.Next();
switch (mode)
{
    case "baseline-sample": StarterReplay.Run(false, seed); break;
    case "baseline-random": StarterReplay.Run(true, seed); break;
    case "fixed-sample": FixedReplay.Run(false, seed); break;
    case "fixed-random": FixedReplay.Run(true, seed); break;
    case "guided-sample": GuidedFinalReplay.Run(false, seed); break;
    default: GuidedFinalReplay.Run(true, seed); break;
}