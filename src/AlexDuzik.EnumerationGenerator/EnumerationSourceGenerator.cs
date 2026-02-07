using Microsoft.CodeAnalysis;

namespace AlexDuzik.EnumerationGenerator;

using static Constants;

/// <summary>
/// A source generator for creating Enumeration types from a CSV file
/// </summary>
[Generator]
public class EnumerationSourceGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
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