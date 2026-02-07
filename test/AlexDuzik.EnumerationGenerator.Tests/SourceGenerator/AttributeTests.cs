using System.Text.RegularExpressions;

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
            },
        }.RunAsync(cancellationToken);
    }

    [Test]
    public async Task EnumerationAttribute_CausesPartialClassToBeEmitted(CancellationToken cancellationToken)
    {
        await new GeneratorTest.Test
        {
            TestState =
            {
                Sources =
                {
                    """
                    using AlexDuzik.EnumerationGenerator;

                    namespace TestProject;

                    [Enumeration]
                    public partial class EnumerationType 
                    {
                    }
                    """
                },
                GeneratedSources =
                {
                    (typeof(EnumerationSourceGenerator),
                        "TestProject.EnumerationType_EnumerationClass.g.cs",
                        $$"""
                          namespace TestProject
                          {
                              partial class EnumerationType
                              {
                              }
                          }
                          """)
                }
            },
        }.RunAsync(cancellationToken);
    }
}