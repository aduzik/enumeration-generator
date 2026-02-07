using Microsoft.CodeAnalysis;

namespace AlexDuzik.EnumerationGenerator;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor TypesMustBePartial = new(
        DiagnosticIds.TypeMustBePartial,
        "Enumeration type class must be partial",
        "The type '{0}' must be a partial",
        "EnumerationGenerator",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}