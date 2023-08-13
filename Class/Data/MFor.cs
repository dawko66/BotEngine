using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

[Serializable]
public class MFor : BotElement
{
	public static readonly string name = "For";    // Name this class

	public bool Condition;

	public MFor() { }
	public MFor(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next, "True"));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, null));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next, "False"));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Bool));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Bool));
	}

	public static bool BotAction(MIf o)
	{
		return o.Condition;
	}
}
