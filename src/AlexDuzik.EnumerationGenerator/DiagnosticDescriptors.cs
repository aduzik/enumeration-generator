using Microsoft.CodeAnalysis;

namespace AlexDuzik.EnumerationGenerator;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor TypesMustBePartial = new(
        DiagnosticIds.TypeMustBePartial,
        "Enumeration type class must be partial",
        "The type '{0}' must be a partial",
        DiagnosticCategories.Usage,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor FileNotFound = new(
        DiagnosticIds.FileNotFound,
        "Enumeration CSV file not found",
        "The file '{0}' was not found",
        DiagnosticCategories.Usage,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}