using Microsoft.CodeAnalysis.Testing;

namespace AlexDuzik.EnumerationGenerator.Tests;

public class EnumerationAnalyzerTest : Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest<EnumerationAnalyzer, DefaultVerifier>
{
    public EnumerationAnalyzerTest()
    {
        TestState.Sources.Add(AnalyzerSources.EmbeddedAttribute);
        TestState.Sources.Add(AnalyzerSources.EnumerationAttribute);
    }
}