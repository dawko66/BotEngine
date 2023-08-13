using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using AutoBot_v1._1;
//using AutoBot_v1._1.Class.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AutoBot_2._0.Class.Data
{
	[Serializable]
    public class BotData
    {
        public List<BotElement> BotElements;
        public List<CData> CDatas;
		public string Name = "";
		public bool IsLocked;
	}

    public class Serializer
    {
		private static XmlAttributes xmlAttributes()
		{
			//!!!~~ BotElements
			XmlAttributes attrs = new XmlAttributes();
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "BotElement", Type = typeof(BotElement) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MText", Type = typeof(MText) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MKey", Type = typeof(MKey) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MCursor", Type = typeof(MCursor) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MScanColor", Type = typeof(MScanColor) });
			//attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MScanAreaColor", Type = typeof(MScanAreaColor) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MOpenBOT", Type = typeof(MOpenBOT) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MIf", Type = typeof(MIf) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MCallToAPI", Type = typeof(MCallToAPI) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MMultiplexer", Type = typeof(MMultiplexer) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MColorPositionSplitter", Type = typeof(MColorPositionSplitter) });
			attrs.XmlElements.Add(new XmlElementAttribute() { ElementName = "MArray", Type = typeof(MArray) });

            return attrs;
		}

		private static XmlAttributeOverrides xmlAttributeOverrides()
		{
			XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();
			attrOverrides.Add(typeof(BotData), "BotElements", xmlAttributes());

			return attrOverrides;
		}

		public static void SerializeObject(BotData myOrders, string filename)
		{
			XmlSerializer s = new XmlSerializer(typeof(BotData), xmlAttributeOverrides());

			TextWriter writer = new StreamWriter(filename);
			s.Serialize(writer, myOrders);
			writer.Close();
		}

		public static bool DeserializeObject(string filename, out BotData bd)
		{
            try
            {
				XmlSerializer s = new XmlSerializer(typeof(BotData), xmlAttributeOverrides());

				TextReader readFile = new StringReader(File.ReadAllText(filename));
				bd = (BotData)s.Deserialize(readFile);
				readFile.Close();

				return true;
			}
			catch
			{
				bd = new BotData()
				{
					BotElements = new List<BotElement>()
					{
						Graph.Graph.GraphBlock(),
					},
					CDatas = new List<CData>()
				};
				return false;
			}
		}

		public static object Clone(object obj)
        {
			XmlSerializer s = new XmlSerializer(typeof(BotData), xmlAttributeOverrides());

			// serialize
			TextWriter writer = new StringWriter();
			s.Serialize(writer, obj);
			writer.Close();

			// deserialize
			TextReader readFile = new StringReader(writer.ToString());
			BotData bd = (BotData)s.Deserialize(readFile);
			readFile.Close();

			return bd;
		}
	}
}
