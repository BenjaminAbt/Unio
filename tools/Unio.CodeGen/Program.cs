// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using Unio.CodeGen;

string outputDir = args.Length > 0
    ? args[0]
    : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "src", "Unio"));

Console.WriteLine($"Output directory: {outputDir}");

if (!Directory.Exists(outputDir))
{
    Console.Error.WriteLine($"Directory not found: {outputDir}");
    return 1;
}

for (int arity = CodeGenerator.MinArity; arity <= CodeGenerator.MaxArity; arity++)
{
    string code = CodeGenerator.GenerateUnio(arity);
    string path = Path.Combine(outputDir, string.Create(CultureInfo.InvariantCulture, $"Unio{arity}.Generated.cs"));

    await File.WriteAllTextAsync(path, code)
        .ConfigureAwait(false);

    Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"  Generated: Unio{arity}.Generated.cs"));
}

string unioBase = CodeGenerator.GenerateUnioBase();
await File.WriteAllTextAsync(Path.Combine(outputDir, "UnioBase.Generated.cs"), unioBase)
    .ConfigureAwait(false);

Console.WriteLine("  Generated: UnioBase.Generated.cs");

Console.WriteLine("Done.");
return 0;
