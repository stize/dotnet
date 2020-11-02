using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Linq;
using Stize.DotNet.Extensions.Common;

namespace System.Reflection
{
    public static class PrimitiveConverter
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "These are simple conversion function and cannot be split up.")]
        public static object ConvertPrimitiveValue(object value, Type type)
        {
            if (value.GetType() == type || value.GetType() == Nullable.GetUnderlyingType(type))
            {
                return value;
            }

            if (type.IsInstanceOfType(value))
            {
                return value;
            }

            var str = value as string;

            if (type == typeof(char))
            {
                if (str == null || str.Length != 1)
                {
                    throw new InvalidCastException(Error.Format(SRResources.PropertyMustBeStringLengthOne));
                }

                return str[0];
            }
            else if (type == typeof(char?))
            {
                if (str == null || str.Length > 1)
                {
                    throw new InvalidCastException(Error.Format(SRResources.PropertyMustBeStringMaxLengthOne));
                }

                return str.Length > 0 ? str[0] : (char?)null;
            }
            else if (type == typeof(char[]))
            {
                if (str == null)
                {
                    throw new InvalidCastException(Error.Format(SRResources.PropertyMustBeString));
                }

                return str.ToCharArray();
            }

            else if (type == typeof(XElement))
            {
                if (str == null)
                {
                    throw new InvalidCastException(Error.Format(SRResources.PropertyMustBeString));
                }

                return XElement.Parse(str);
            }
            else
            {
                type = Nullable.GetUnderlyingType(type) ?? type;
                if (type.IsEnum())
                {
                    if (str == null)
                    {
                        throw new InvalidCastException(Error.Format(SRResources.PropertyMustBeString));
                    }

                    return Enum.Parse(type, str);
                }
                else if (type == typeof(DateTime))
                {
                    if (value is DateTimeOffset dateTimeOffsetValue)
                    {
                        return dateTimeOffsetValue.DateTime;
                    }

                    //TODO: TODO: move Date to Stize?
                    // https://github.com/OData/odata.net/blob/master/src/Microsoft.OData.Edm/Schema/Date.cs
                    //if (value is Date)
                    //{
                    //    Date dt = (Date)value;
                    //    return (DateTime)dt;
                    //}

                    throw new InvalidCastException(Error.Format(SRResources.PropertyMustBeDateTimeOffsetOrDate));
                }
                else if (type == typeof(TimeSpan))
                {
                    // TODO: move TimeOfDay to Stize?
                    // https://github.com/OData/odata.net/blob/master/src/Microsoft.OData.Edm/Schema/TimeOfDay.cs
                    //if (value is TimeOfDay)
                    //{
                    //    TimeOfDay tod = (TimeOfDay)value;
                    //    return (TimeSpan)tod;
                    //}

                    throw new InvalidCastException(Error.Format(SRResources.PropertyMustBeTimeOfDay));
                }
                else if (type == typeof(bool))
                {
                    if (str != null && bool.TryParse(str, out var result))
                    {
                        return result;
                    }

                    throw new InvalidCastException(Error.Format(SRResources.PropertyMustBeBoolean));
                }
                else
                {
                    // Note that we are not casting the return value to nullable<T> as even if we do it
                    // CLR would unbox it back to T.
                    return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
                }
            }
        }
    }
}