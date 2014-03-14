// Type: SerializableDictionary`2
// Assembly: MetroTwitLoop, Version=1.1.0.3134, Culture=neutral, PublicKeyToken=null
// MVID: 489E9933-285D-4E4A-B3E3-8020131273C7
// Assembly location: C:\Users\Joshua\AppData\Local\Apps\2.0\8WK4PAMP.D6Y\V814PCJP.C3C\metr..tion_89233686fad4c081_0001.0001_335d7c2b6b7c57eb\MetroTwitLoop.exe

using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

[XmlRoot("dictionary")]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
  public XmlSchema GetSchema()
  {
    return (XmlSchema) null;
  }

  public void ReadXml(XmlReader reader)
  {
    try
    {
      XmlSerializer xmlSerializer1 = new XmlSerializer(typeof (TKey));
      XmlSerializer xmlSerializer2 = new XmlSerializer(typeof (TValue));
      bool isEmptyElement = reader.IsEmptyElement;
      reader.Read();
      if (isEmptyElement)
        return;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        try
        {
          reader.ReadStartElement("item");
          reader.ReadStartElement("key");
          TKey key = (TKey) xmlSerializer1.Deserialize(reader);
          reader.ReadEndElement();
          reader.ReadStartElement("value");
          TValue obj = (TValue) xmlSerializer2.Deserialize(reader);
          reader.ReadEndElement();
          this.Add(key, obj);
          reader.ReadEndElement();
          int num = (int) reader.MoveToContent();
        }
        catch
        {
          break;
        }
      }
      reader.ReadEndElement();
    }
    catch
    {
    }
  }

  public void WriteXml(XmlWriter writer)
  {
    try
    {
      XmlSerializer xmlSerializer1 = new XmlSerializer(typeof (TKey));
      XmlSerializer xmlSerializer2 = new XmlSerializer(typeof (TValue));
      foreach (TKey index in this.Keys)
      {
        writer.WriteStartElement("item");
        writer.WriteStartElement("key");
        xmlSerializer1.Serialize(writer, (object) index);
        writer.WriteEndElement();
        writer.WriteStartElement("value");
        TValue obj = this[index];
        xmlSerializer2.Serialize(writer, (object) obj);
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
    }
    catch
    {
    }
  }
}
