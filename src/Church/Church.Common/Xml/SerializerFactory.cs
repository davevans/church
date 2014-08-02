using System.Xml.Serialization;

namespace Church.Common.Xml
{
    public static class SerializerFactory<T>
    {
        public static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(T));
    }
}
