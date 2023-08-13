using AutoBot_2._0.Class.Data;
using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

[Serializable]
public class MIf : BotElement
{
	public const string name = "If";    // Name this class

	public bool Condition;

	public MIf() { }
	public MIf(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + Id;

		// Default values
		IsDelay = false;
		DelayAt = new TimeSpan(0, 0, 0, 0, 0);
		DelayTo = new TimeSpan(0, 0, 0, 0, 0);
		Times = 1;
		Condition = true;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next, "True"));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, null));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next, "False"));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Var));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Bool)); // 6
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Var));
	}

	public static void BotAction(MIf o)
	{
		o.Variables.Add(new VariableData() { Id = 6, Variable = o.Condition });
	}
}
