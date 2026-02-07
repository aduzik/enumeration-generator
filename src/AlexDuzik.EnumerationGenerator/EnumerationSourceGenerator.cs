using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AlexDuzik.EnumerationGenerator;

using static Constants;

[Generator]
public class EnumerationSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(output =>
        {
            output.AddEmbeddedAttributeDefinition();
            output.AddSource(
                EnumerationAttributeFileName,
                AttributeSourceTexts.EnumerationAttribute);
        });
    }
}