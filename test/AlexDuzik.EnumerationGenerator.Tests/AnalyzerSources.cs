using Microsoft.CodeAnalysis.Text;

namespace AlexDuzik.EnumerationGenerator.Tests;

public static partial class AnalyzerSources
{
    public static readonly (Type type, string fileName, SourceText source) EmbeddedAttribute = (
        typeof(EnumerationSourceGenerator),
        Constants.EmbeddedAttributeFileName,
        AttributeSourceTexts.EmbeddedAttribute);

    
    public static readonly (Type type, string fileName, SourceText source) EnumerationAttribute = (
        typeof(EnumerationSourceGenerator), Constants.EnumerationAttributeFileName,
        AttributeSourceTexts.EnumerationAttribute);
}