using System.Reflection;

namespace Fias.Api.Extensions
{
    public static class Extensions
    {
        public static void SetValueType(this PropertyInfo property, object? obj, string? value)
        {
            var propertyType = property.PropertyType.Name == typeof(Nullable).Name
                ? Nullable.GetUnderlyingType(property.PropertyType)
                : property.PropertyType;
            if (string.IsNullOrWhiteSpace(value))
            {
                if (property.PropertyType.Name == typeof(Nullable).FullName || property.PropertyType.Name == typeof(string).Name)
                {
                    property.SetValue(obj, null);
                    return;
                }
                else
                {
                    return;
                }
            }

            if (propertyType is null)
                return;

            switch (propertyType.Name)
            {
                case var v when v == typeof(ushort).Name:
                    property.SetValue(obj, ushort.Parse(value));
                    break;
                case var v when v == typeof(uint).Name:
                    property.SetValue(obj, uint.Parse(value));
                    break;
                case var v when v == typeof(ulong).Name:
                    property.SetValue(obj, ulong.Parse(value));
                    break;
                case var v when v == typeof(UInt128).Name:
                    property.SetValue(obj, UInt128.Parse(value));
                    break;
                case var v when v == typeof(UIntPtr).Name:
                    property.SetValue(obj, UIntPtr.Parse(value));
                    break;
                case var v when v == typeof(short).Name:
                    property.SetValue(obj, short.Parse(value));
                    break;
                case var v when v == typeof(int).Name:
                    property.SetValue(obj, int.Parse(value));
                    break;
                case var v when v == typeof(long).Name:
                    property.SetValue(obj, long.Parse(value));
                    break;
                case var v when v == typeof(Int128).Name:
                    property.SetValue(obj, Int128.Parse(value));
                    break;
                case var v when v == typeof(float).Name:
                    property.SetValue(obj, float.Parse(value));
                    break;
                case var v when v == typeof(double).Name:
                    property.SetValue(obj, double.Parse(value));
                    break;
                case var v when v == typeof(decimal).Name:
                    property.SetValue(obj, decimal.Parse(value));
                    break;
                case var v when v == typeof(byte).Name:
                    property.SetValue(obj, byte.Parse(value));
                    break;
                case var v when v == typeof(DateTime).Name:
                    property.SetValue(obj, DateTime.Parse(value));
                    break;
                case var v when v == typeof(Guid).Name:
                    property.SetValue(obj, Guid.Parse(value));
                    break;
                case var v when v == typeof(bool).Name:
                    property.SetValue(obj, bool.Parse(value));
                    break;
                case var v when v == typeof(string).Name:
                default:
                    property.SetValue(obj, value);
                    break;
            }
        }
    }
}
