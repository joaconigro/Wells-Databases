﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace Wells.Base
{
    public static class XmlExtensions
    {
        public static void ForEach(this XmlReader reader, Action<XmlReader> action)
        {
            var name = reader.Name;
            reader.Read();
            if (!reader.IsEmptyElement)
            {
                while (reader.Name != name)
                {
                    action(reader);
                }
                reader.Read();
            }
        }

        public static DateTime ReadContentAsFormatedDateTime(this XmlReader reader)
        {
            var stringDate = reader.ReadElementContentAsString();
            string[] formats = { "dd/MM/yyyy" };
            var ok = DateTime.TryParseExact(stringDate, formats, null, DateTimeStyles.None, out var date);
            if (ok) { return date; }
            return DateTime.Today;
        }

        public static T ReadContentAsEnum<T>(this XmlReader reader)
        {
            var value = reader.ReadElementContentAsString();
            return (T)Enum.Parse(typeof(T), value);
        }

        public static Type ReadContentAsType(this XmlReader reader)
        {
            var values = reader.ReadElementContentAsString().Split("|");
            var typeName = $"{values[0]}, {values[1]}";
            return Type.GetType(typeName);
        }

        public static List<T> ReadElementContentAsList<T>(this XmlReader reader) where T : IXmlSerializable
        {
            var list = new List<T>();
            reader.ForEach(_ =>
            {
                var assemblyName = typeof(T).Assembly.GetName().Name;
                var entity = (T)Activator.CreateInstance(assemblyName, $"{typeof(T).Namespace}.{reader.Name}").Unwrap();
                entity.ReadXml(reader);
                list.Add(entity);
            });
            return list;
        }

        public static List<T> ReadElementContentAsList<T>(this XmlReader reader, Func<string, T> instantiator) where T : IXmlSerializable
        {
            var list = new List<T>();
            reader.ForEach(_ =>
            {
                var entity = instantiator.Invoke(reader.Name);
                entity.ReadXml(reader);
                list.Add(entity);
            });
            return list;
        }

        public static void WriteEmpty(this XmlWriter writer, string startElement)
        {
            writer.WriteStartElement(startElement);
            writer.WriteEndElement();
        }

        public static void Write(this XmlWriter writer, string startElement, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                writer.WriteEmpty(startElement);
            }
            else
            {
                writer.WriteStartElement(startElement);
                writer.WriteValue(value);
                writer.WriteEndElement();
            }
        }

        public static void Write(this XmlWriter writer, string startElement, double value)
        {
            writer.WriteStartElement(startElement);
            writer.WriteValue(value);
            writer.WriteEndElement();
        }

        public static void Write(this XmlWriter writer, string startElement, int value)
        {
            writer.WriteStartElement(startElement);
            writer.WriteValue(value);
            writer.WriteEndElement();
        }

        public static void Write(this XmlWriter writer, string startElement, bool value)
        {
            writer.WriteStartElement(startElement);
            writer.WriteValue(value);
            writer.WriteEndElement();
        }

        public static void Write(this XmlWriter writer, string startElement, DateTime value)
        {
            writer.WriteStartElement(startElement);
            writer.WriteValue(value.ToString("dd/MM/yyyy"));
            writer.WriteEndElement();
        }

        public static void Write(this XmlWriter writer, string startElement, Enum value)
        {
            writer.WriteStartElement(startElement);
            writer.WriteValue(value.ToString(""));
            writer.WriteEndElement();
        }

        public static void Write(this XmlWriter writer, string startElement, Type value)
        {
            writer.WriteStartElement(startElement);
            writer.WriteValue($"{value.FullName}|{value.Assembly.FullName}");
            writer.WriteEndElement();
        }

        public static void Write<T>(this XmlWriter writer, string startElement, IEnumerable<T> list) where T : IXmlSerializable
        {
            writer.WriteStartElement(startElement);
            foreach (var entity in list)
            {
                var values = XmlConvert.EncodeName(entity.GetType().Name).Split("_");
                writer.WriteStartElement(values[0].Trim());
                entity.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
