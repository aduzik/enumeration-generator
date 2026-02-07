using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using static AlexDuzik.EnumerationGenerator.DiagnosticDescriptors;

namespace AlexDuzik.EnumerationGenerator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EnumerationAnalyzer : DiagnosticAnalyzer
{
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        context.RegisterSymbolAction(symbolContext =>
        {
            var namedType = (INamedTypeSymbol)symbolContext.Symbol;
            
            var attributes = namedType.GetAttributes();
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeClass?.Name == "EnumerationAttribute")
                {
                    var attributeSyntax = (AttributeSyntax?)attribute.ApplicationSyntaxReference?.GetSyntax();
                    var attributeList = (AttributeListSyntax?)attributeSyntax?.Parent;
                    var syntaxNode  =  (TypeDeclarationSyntax?)attributeList?.Parent;
                    if (syntaxNode != null)
                    {
                        if (syntaxNode.Modifiers.All(modifier => !modifier.IsKind(SyntaxKind.PartialKeyword)))
                        {
                            symbolContext.ReportDiagnostic(
                                Diagnostic.Create(
                                    TypesMustBePartial, 
                                    syntaxNode.Identifier.GetLocation(), 
                                    syntaxNode.Identifier.ValueText));
                        }
                    }
                }
            }
        },  SymbolKind.NamedType);
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
    [
        TypesMustBePartial
    ];
}