using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.NewClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace AutoBot_v1._1.Class.Data
{
	[Serializable]
	public class CPData
	{
		public CIO CIO { get; set; }
		public CArray CArray { get; set; }
		public CType CType { get; set; }

		public int Id { get; set; }			//con point
		public int BlockId { get; set; }    //parent

		public string Title { get; set; }

		[XmlIgnore]
		private UIElement UIElement { get; set; }
		public bool IsInfinite { get; set; }

		public CPData() { }
		public CPData(int BlockId, int Id, CIO CIO, CArray CArray, CType CType, string Title = "", bool IsInfinite = false)
        {
			this.BlockId = BlockId;
			this.Id = Id;

			this.CIO = CIO;
            this.CArray = CArray;
            this.CType = CType;

			this.Title = Title;
			this.IsInfinite = IsInfinite;
        }
		
		public CPData(int BlockId, int Id, CIO CIO, UIElement UIElement)
		{
			this.BlockId = BlockId;
			this.Id = Id;

			this.CIO = CIO;

			this.UIElement = UIElement;
		}

		public override bool Equals(object o)
        {
			return (o as CPData)?.BlockId == BlockId &&
				(o as CPData)?.CArray == CArray &&
				(o as CPData)?.CIO == CIO &&
				(o as CPData)?.CType == CType &&
				(o as CPData)?.Id == Id &&
				(o as CPData)?.IsInfinite == IsInfinite &&
				(o as CPData)?.Title == Title &&
				(o as CPData)?.UIElement == UIElement;
		}


		public static int GiveID(List<CPData> cDatas)
		{
			int highestID = 0;
			foreach (CPData cd in cDatas)
			{
				if (cd.Id > highestID) highestID = cd.Id;
			}
			return highestID + 1;
		}
	}
}
