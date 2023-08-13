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
using System.Linq;

[Serializable]
public class MMultiplexer : BotElement
{
	public const string name = "Multiplexer";    // Name this class

	public List<object> ObjectsArray;
	public List<int> ObjectsIndexes;
	public int DefaultNext; // default first node


	public int LastCPDIndex = 5;
	public int LastIndex = 0;

	public SelectType SelectionElementsType = SelectType.RandomSequential;

	public enum SelectType
	{
		First,
		Last,
		Random,			// Draw with return or draw with repetition (at 0 to n)
        Sequential,     // Draw without returning (without repetition, dependent draw) 
		RandomSequential
    }

    public MMultiplexer() { }
	public MMultiplexer(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + Id;
		this.Times = -1;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next));
        CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.Yes, CType.Var));
        CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Var));
        CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next, IsInfinite: true));
    }

    public static void BotAction(MMultiplexer o)
	{
        var NextCPD = o.CPDatas.Find(x => x.Id > o.LastCPDIndex);
		NextCPD = NextCPD == null && o.ObjectsIndexes.Count > 0 ? o.CPDatas.Find(x => x.Id > 5) : NextCPD;
        o.LastCPDIndex = NextCPD == null || o.ObjectsIndexes.Count <= 0 ? 2 : NextCPD.Id;

        if (o.ObjectsIndexes.Count > 0)
		{
			o.Variables.Add(new VariableData() { Id = 4, Variable = o.ObjectsArray[o.ObjectsIndexes[0]] });
            o.ObjectsIndexes.RemoveAt(0);
		}
    }
}
