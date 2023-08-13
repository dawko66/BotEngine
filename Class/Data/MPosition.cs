using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

[Serializable]
public class MPosition : BotElement
{
	public static readonly string name = "Position"; // Name this class

	public Point Pos;

	public MPosition(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Integer, "X"));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Integer, "Y"));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Position));
	}
}
