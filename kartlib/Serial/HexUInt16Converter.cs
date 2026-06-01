using System.ComponentModel;

namespace kartlib.Serial
{
    public class HexUInt16Converter : UInt16Converter
    {
        public override object? ConvertTo(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is ushort ushortValue)
                return "0x" + ushortValue.ToString("X4");

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        {
            if (value is string stringValue)
            {
                stringValue = stringValue.Trim();
                if (stringValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    return Convert.ToUInt16(stringValue.Substring(2), 16);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
