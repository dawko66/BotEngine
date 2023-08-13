using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

[Serializable]
public class MTest : BotElement
{
	public MTest(int Id)
	{
		this.Id = Id;
		Type = "Test";
		Name = Type + Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next, "INP"));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, new TextBox()));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.Yes, CType.ButtonAction));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.Yes, CType.Var, "", true));

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Char));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, null));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.ButtonAction, "", true));

	}
}
