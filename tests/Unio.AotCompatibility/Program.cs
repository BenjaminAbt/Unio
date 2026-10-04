// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

if (RuntimeFeature.IsDynamicCodeSupported)
{
    throw new InvalidOperationException("Run the published native executable.");
}

Console.WriteLine("NativeAOT full-library analysis passed.");
