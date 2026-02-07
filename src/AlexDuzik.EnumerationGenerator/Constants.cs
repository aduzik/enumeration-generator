using System.Diagnostics.CodeAnalysis;

namespace AlexDuzik.EnumerationGenerator;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
internal static class Constants
{
    public const string EmbeddedAttributeFileName = "Microsoft.CodeAnalysis.EmbeddedAttribute.cs";
    public const string EnumerationAttributeFileName = "EnumerationAttribute.g.cs";

    public const string EnumerationAttributeClassName = "EnumerationAttribute";
    public const string EnumerationAttributeNamespace = "AlexDuzik.EnumerationGenerator";
    public const string EnumerationFullyQualifiedName = EnumerationAttributeNamespace + "." + EnumerationAttributeClassName;
}