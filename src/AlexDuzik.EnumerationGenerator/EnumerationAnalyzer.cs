using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AlexDuzik.EnumerationGenerator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EnumerationAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor TypesMustBePartial = new(
        "GE1001",
        "Enumeration type class must be partial",
        "The type '{0}' must be a partial",
        "EnumerationGenerator",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
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