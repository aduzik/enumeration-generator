

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;

namespace AlexDuzik.EnumerationGenerator.Tests;

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
                    AttributeSources.EmbeddedAttribute,
                    (typeof(EnumerationSourceGenerator), 
                        "EnumerationAttribute.g.cs",
                        EnumerationSourceGenerator.EnumerationAttributeText)

                },
            },
            ExpectedDiagnostics =
            {
                new DiagnosticResult(new(
                    "GE1001",
                    "Enumeration type class must be partial",
                    "The type '{0}' must be a partial",
                    "EnumerationGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true)).WithArguments("NonPartialClass").WithSpan(4, 14, 4, 29),
            }
        }.RunAsync(cancellationToken);
    }
}