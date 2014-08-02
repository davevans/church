using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Church.Common.Xml
{
    [Flags]
    public enum XmlOptions
    {
        Default = 0,
        ExcludeXmlDecoration = 1,
        ExcludeNameSpace = 2,
        DontIndentNewLine = 4,
        Lean = 7
    }

   public static class XmlUtil
    {
        public static T GetFromXml<T>(string s)
        {
            using (var reader = new StringReader(s))
            {
                return FromXml<T>(reader);
            }
        }

        public static string ToXmlString<T>(this T input, XmlOptions options = XmlOptions.Default, bool absorbExceptions = false)
        {
            try
            {
                using (var writer = new StringWriter())
                {
                    input.ToXml(writer, options);
                    return writer.ToString();
                }
            }
            catch (Exception)
            {
                if(absorbExceptions)
                {
                    return "";
                }

                throw;
            }
        }

        public static void ToXml<T>(this T objectToSerialize, Stream stream, XmlOptions options = XmlOptions.Default)
        {
            var serializer = SerializerFactory<T>.XmlSerializer;

            if (options == XmlOptions.Default)
            {
                serializer.Serialize(stream, objectToSerialize);
            }
            else
            {
                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = options.HasFlag(XmlOptions.ExcludeXmlDecoration),
                    Indent = !options.HasFlag(XmlOptions.DontIndentNewLine)
                };

                

                XmlSerializerNamespaces ns = null;
                if (options.HasFlag(XmlOptions.ExcludeNameSpace))
                {
                    ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                }

                var xmlWriter = XmlWriter.Create(stream, settings);
                serializer.Serialize(xmlWriter, objectToSerialize, ns);
            }
        }

        public static void ToXml<T>(this T objectToSerialize, StringWriter writer, XmlOptions options = XmlOptions.Default)
        {
            var serializer = SerializerFactory<T>.XmlSerializer;

            if(options == XmlOptions.Default)
            {
                serializer.Serialize(writer, objectToSerialize);    
            }
            else
            {
                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration =  options.HasFlag(XmlOptions.ExcludeXmlDecoration),
                    Indent = !options.HasFlag(XmlOptions.DontIndentNewLine),
                };

                XmlSerializerNamespaces ns = null;
                if(options.HasFlag(XmlOptions.ExcludeNameSpace))
                {
                    ns = new XmlSerializerNamespaces();
                    ns.Add("", "");    
                }
               
                var xmlWriter = XmlWriter.Create(writer, settings);
                serializer.Serialize(xmlWriter, objectToSerialize, ns);
            }
        }

        public static T FromXml<T>(Stream stream)
        {
            return (T)SerializerFactory<T>.XmlSerializer.Deserialize(stream);
        }

        public static T FromXml<T>(StringReader reader)
        {
            return (T)SerializerFactory<T>.XmlSerializer.Deserialize(reader);
        }

       
    }
}
