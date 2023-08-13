using AutoBot_2._0.Class.Data;
using AutoBot_v1._1.NewClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace AutoBot_v1._1.Class.Data
{
	[Serializable]
	public class BotElement
	{
		public int Id;
		public Point Poisition = new Point();

		public string Type;
		public string Name;
		public string Description;
		public int Times;
		public bool IsDelay = true;
		
		[XmlIgnore]
		public List<VariableData> Variables = new List<VariableData>();

		[XmlIgnore]
		public TimeSpan DelayAt = new TimeSpan(0, 0, 0, 0, 50);
		[XmlIgnore]
		public TimeSpan DelayTo = new TimeSpan(0, 0, 0, 0, 120);

		[XmlElement("DelayAt")]
		public long _DelayAt
		{
			get { return DelayAt.Ticks; }
			set { DelayAt = new TimeSpan(value); }
		}
		[XmlElement("DelayTo")]
		public long _DelayTo
		{
			get { return DelayTo.Ticks; }
			set { DelayTo = new TimeSpan(value); }
		}

		public List<CPData> CPDatas = new List<CPData>();
		
		
		public static BotElement FindById(List<BotElement> e, int Id)
		{
			return e.Find(x => x.Id == Id);
		}
		
		public static int GiveID(List<BotElement> botElements) 
		{
			int highestID = 0;
			foreach (BotElement be in botElements)
			{
				if (be.Id > highestID) highestID = be.Id;
			}
			return highestID + 1;
		}

		//public abstract void BotAction(object obj);
	}
}
