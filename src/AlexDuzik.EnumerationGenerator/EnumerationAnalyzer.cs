using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using static AlexDuzik.EnumerationGenerator.DiagnosticDescriptors;

namespace AlexDuzik.EnumerationGenerator;

/// <summary>
/// A Roslyn analyzer that enforces correct usage of the Enumeration attribute.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EnumerationAnalyzer : DiagnosticAnalyzer
{
    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(symbolContext =>
        {
            var enumerationAttributeSymbol = symbolContext.Compilation.GetTypeByMetadataName(Constants.EnumerationFullyQualifiedName) ??
                throw new InvalidOperationException("Expected to find enumeration attribute type");
            var namedType = (INamedTypeSymbol)symbolContext.Symbol;

            var attributes = namedType.GetAttributes();
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeClass == null ||
                    !SymbolEqualityComparer.IncludeNullability.Equals(attribute.AttributeClass,
                        enumerationAttributeSymbol))
                {
                    continue;
                }

                var attributeSyntax = (AttributeSyntax?)attribute.ApplicationSyntaxReference?.GetSyntax();
                if (attributeSyntax == null)
                {
                    continue;
                }

                var attributeList = (AttributeListSyntax?)attributeSyntax?.Parent;
                var syntaxNode = (TypeDeclarationSyntax?)attributeList?.Parent;
                if (syntaxNode == null)
                {
                    continue;
                }

                if (syntaxNode.Modifiers.All(modifier => !modifier.IsKind(SyntaxKind.PartialKeyword)))
                {
                    symbolContext.ReportDiagnostic(
                        Diagnostic.Create(
                            TypesMustBePartial,
                            syntaxNode.Identifier.GetLocation(),
                            syntaxNode.Identifier.ValueText));
                }

                var fileNameArgument = attribute.ConstructorArguments[0];
                var fileNameSyntax = attributeSyntax!.ArgumentList!.Arguments[0];
                var fileName = (string?)fileNameArgument.Value;
                var additionalFiles = symbolContext.Options.AdditionalFiles;

                if (!additionalFiles.Any(file => file.Path == fileName))
                {
                    symbolContext.ReportDiagnostic(
                        Diagnostic.Create(
                            FileNotFound,
                            fileNameSyntax.GetLocation(),
                            fileName));
                }
            }
        }, SymbolKind.NamedType);
    }

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
    [
        TypesMustBePartial,
        FileNotFound
    ];
}