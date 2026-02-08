namespace CommonSnappableTypes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CompanyInfoAttribute : Attribute
{
    public string CompanyName { get; set; } = null!;
    public string CompanyUrl { get; set; } = null!;
}
