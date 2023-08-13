using AutoBot_2._0.Class.Data;
using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Linq;
using System.Windows.Controls;

[Serializable]
public class MArray : BotElement
{
	public const string name = "Array";    // Name this class

	public List<object> ListArray = new List<object>();

	public MArray() { }
	public MArray(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Var, IsInfinite: true));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.Yes, CType.Var));
	}


	public static void BotAction(MArray o)
	{
		if (o.ListArray.Count > 0)
		{
			try
			{
                switch (o.ListArray[0].GetType().ToString())
                {
                    case "ColorPosition":
						List<ColorPosition> list = new List<ColorPosition>();
                        foreach (ColorPosition i in o.ListArray)
							list.Add(i);

                        o.Variables.Add(new VariableData() { Id = 4, Variable = list });
						o.ListArray.Clear();
                        break;
                }
			}
			catch
			{
                AutoBot_2._0.Class.Graph.Console.AddMessage(o.Name + " - Not the same data types.");
            }


        }
    }
}
