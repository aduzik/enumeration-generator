using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AlexDuzik.EnumerationGenerator;

[Generator]
public class EnumerationSourceGenerator : IIncrementalGenerator
{
    internal const string EnumerationAttributeText =
        """
        using System;
        using Microsoft.CodeAnalysis;
        
        namespace AlexDuzik.EnumerationGenerator
        {
            [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false), Embedded]
            public class EnumerationAttribute : Attribute
            {
                
            }
        }
        """;
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(output =>
        {
            output.AddEmbeddedAttributeDefinition();
            output.AddSource(
                "EnumerationAttribute.g.cs",
                SourceText.From(EnumerationAttributeText, Encoding.UTF8));
        });
    }
}