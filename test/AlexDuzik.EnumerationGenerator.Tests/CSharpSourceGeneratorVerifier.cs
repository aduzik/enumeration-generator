using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace AlexDuzik.EnumerationGenerator.Tests;

public static class CSharpSourceGeneratorVerifier<TSourceGenerator>
    where TSourceGenerator : IIncrementalGenerator, new()
{
    public class Test : CSharpSourceGeneratorTest<TSourceGenerator, DefaultVerifier>;
}