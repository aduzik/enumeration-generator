using Microsoft.CodeAnalysis.Testing;

namespace AlexDuzik.EnumerationGenerator.Tests.Analyzer;

using Verify = Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest<EnumerationAnalyzer, DefaultVerifier>;

public class AnalyzerTests
{
    [Test]
    public async Task Analyzer_ReportsDiagnosticOnNonPartialTypes(CancellationToken cancellationToken)
    {
        await new Verify
        {
            TestState =
            {
                Sources =
                {
                    ("""
                       using AlexDuzik.EnumerationGenerator;
                       
                       [Enumeration]
                       public class NonPartialClass 
                       {
                       }
                       """),
                    AnalyzerSources.EmbeddedAttribute,
                    AnalyzerSources.EnumerationAttribute,

                },
            },
            ExpectedDiagnostics =
            {
                new DiagnosticResult(DiagnosticDescriptors.TypesMustBePartial)
                    .WithArguments("NonPartialClass")
                    .WithSpan(4, 14, 4, 29),
            }
        }.RunAsync(cancellationToken);
    }
}