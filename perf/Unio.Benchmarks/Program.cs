// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved
// BenchmarkDotNet entry point – discovers and runs all benchmark classes in this assembly.

using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
