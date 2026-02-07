using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.Text;

namespace AlexDuzik.EnumerationGenerator.Tests.SourceGenerator;

using GeneratorTest = CSharpSourceGeneratorVerifier<EnumerationSourceGenerator>;

public class AttributeTests
{
    [Test]
    public async Task EnumerationAttribute_IsPresentInCompilation(CancellationToken cancellationToken)
    {
        Regex.Replace("", "\r\n|\r|\n", "\r\n");
        await new GeneratorTest.Test
        {
            TestState =
            {
                Sources =
                {
                    """
                    public class TestClass() 
                    {
                        // Do nothing
                    }
                    """
                },
                GeneratedSources =
                {
                    AnalyzerSources.EmbeddedAttribute,
                    AnalyzerSources.EnumerationAttribute
                },
            },
        }.RunAsync(cancellationToken);
    }
}