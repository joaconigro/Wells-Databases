using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Wells.Base
{
    public static class Common
    {

        public static List<string> EnumDescriptionsToList(Type aEnum)
        {
            var descriptions = new List<string>();
            var names = Enum.GetNames(aEnum).ToList();

            foreach (var name in names)
            {
                var attr = aEnum.GetField(Enum.GetName(aEnum, names.IndexOf(name))).
                           GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attr != null)
                {
                    descriptions.Add(attr.Description);
                }
                else
                {
                    descriptions.Add(name);
                }
            }
            return descriptions;
        }


        public static string GetEnumDescription(Enum e)
        {
            var type = e.GetType();
            var attr = type.GetField(Enum.GetName(type, e)).
                       GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attr != null)
            {
                return attr.Description;
            }
            return e.ToString();
        }

        public static string GetEnumDescription(Type aEnum, int value)
        {
            return EnumDescriptionsToList(aEnum)[value];
        }

        public static bool IsNumericType(Type type)
        {
            if (type.IsEnum) return false;
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }


        public static bool IsIntegerNumericType(Type type)
        {
            if (type.IsEnum) return false;
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }
    }
}
