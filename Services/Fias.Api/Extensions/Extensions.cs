using System.Reflection;

namespace Fias.Api.Extensions
{
    public static class Extensions
    {
        public static void SetValueType(this PropertyInfo property, object? obj, string? value)
        {
            var propertyType = property.PropertyType.Name == "Nullable`1"
                ? Nullable.GetUnderlyingType(property.PropertyType)
                : property.PropertyType;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (property.PropertyType.Name != "Nullable`1")
                    return;
                else
                {
                    property.SetValue(obj, null);
                    return;
                }
            }

            if (propertyType is not null)
            {
                switch (propertyType.Name)
                {
                    case "UInt16":
                    case "UInt32":
                    case "UInt64":
                    case "UInt128":
                    case "UIntPtr":
                        property.SetValue(obj, uint.Parse(value));
                        break;
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "Int128":
                        property.SetValue(obj, int.Parse(value));
                        break;
                    case "Single":
                        property.SetValue(obj, float.Parse(value));
                        break;
                    case "Double":
                        property.SetValue(obj, double.Parse(value));
                        break;
                    case "Decimal":
                        property.SetValue(obj, decimal.Parse(value));
                        break;
                    case "Byte":
                        property.SetValue(obj, byte.Parse(value));
                        break;
                    case "DateTime":
                        property.SetValue(obj, DateTime.Parse(value));
                        break;
                    case "Boolean":
                        property.SetValue(obj, bool.Parse(value));
                        break;
                    case "String":
                    default:
                        property.SetValue(obj, value);
                        break;
                }

            }
            else
            {

            }
        }
    }
}
