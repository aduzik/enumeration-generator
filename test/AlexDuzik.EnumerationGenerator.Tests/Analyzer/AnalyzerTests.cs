using System.Text;

using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;

namespace AlexDuzik.EnumerationGenerator.Tests.Analyzer;

public class AnalyzerTests
{
    [Test]
    public async Task Analyzer_ReportsDiagnosticOnNonPartialTypes(CancellationToken cancellationToken)
    {
        await new EnumerationAnalyzerTest
        {
            TestState =
            {
                Sources =
                {
                    ("""
                       using AlexDuzik.EnumerationGenerator;
                       
                       [Enumeration("TestData.csv")]
                       public class NonPartialClass 
                       {
                       }
                       """),
                },
                AdditionalFiles =
                {
                    ("TestData.csv", """
                                     Id,Name
                                     1,Yes
                                     2,No
                                     3,Maybe
                                     """)
                }
            },
            ExpectedDiagnostics =
            {
                new DiagnosticResult(DiagnosticDescriptors.TypesMustBePartial)
                    .WithArguments("NonPartialClass")
                    .WithSpan("/0/Test2.cs", 4, 14, 4, 29),
            }
        }.RunAsync(cancellationToken);
    }

    [Test]
    public async Task Analyzer_ReportsDiagnosticForMissingFiles(CancellationToken cancellationToken)
    {
        await new EnumerationAnalyzerTest
        {
            TestState =
            {
                Sources =
                {
                    SourceText.From("""
                        using AlexDuzik.EnumerationGenerator;
                        
                        namespace AlexDuzik.EnumerationGenerator.Tests;
                        
                        [Enumeration("NonExistentFile.csv")]
                        public partial class ClassWithMissingFile
                        {
                        }
                        """, Encoding.UTF8)
                }
            },
            ExpectedDiagnostics =
            {
                new DiagnosticResult(DiagnosticDescriptors.FileNotFound)
                    .WithArguments("NonExistentFile.csv")
                    .WithSpan("/0/Test2.cs", 5, 14, 5, 35)
            }
        }.RunAsync(cancellationToken);
    }
}