using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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

        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(EnumerationFullyQualifiedName,
            (node, _) => node is BaseTypeDeclarationSyntax declarationSyntax &&
                         declarationSyntax.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PartialKeyword)) &&
                         !string.IsNullOrEmpty(declarationSyntax.Identifier.Text),
            (nodeContext, _) =>
            {
                var attribute = nodeContext.Attributes.Single();
                var fileName = (string)attribute.ConstructorArguments[0].Value!;

                var declaration = (INamedTypeSymbol)nodeContext.TargetSymbol;
                var typeNamespace = declaration.ContainingNamespace.ToDisplayString(
                    SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle
                        .Omitted));
                var typeName = nodeContext.TargetSymbol.Name;

                var syntax = (BaseTypeDeclarationSyntax)nodeContext.TargetNode;
                var kind = syntax.Kind();

                return new EnumerationData(typeNamespace, typeName, kind, fileName);
            });

        context.RegisterSourceOutput(pipeline, (productionContext, data) =>
        {
            (string fullyQualifiedNamespace, string typeName, SyntaxKind kind, string fileName) = data;

            var kindSyntax = kind switch
            {
                SyntaxKind.ClassDeclaration => "class",
                SyntaxKind.StructDeclaration => "struct",
                SyntaxKind.RecordDeclaration => "record",
                SyntaxKind.RecordStructDeclaration => "record struct",
                _ => null
            };

            if (kindSyntax == null)
            {
                return;
            }

            var hintName = $"{fullyQualifiedNamespace}.{typeName}_EnumerationClass.g.cs";
            var classText = SourceText.From(
                $$"""
                namespace {{fullyQualifiedNamespace}}
                {
                    partial {{kindSyntax}} {{typeName}}
                    {
                        // Enumeration for file '{{fileName}}'
                    }
                }
                """, Encoding.UTF8);

            productionContext.AddSource(hintName, classText);
        });
    }

    private record EnumerationData(string FullyQualifiedNamespace, string TypeName, SyntaxKind Kind, string FileName);
}