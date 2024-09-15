namespace Pilot.Contracts.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class HasFileAttribute(string fieldName) : Attribute
{
    public string FieldName { get; set; } = fieldName;
}