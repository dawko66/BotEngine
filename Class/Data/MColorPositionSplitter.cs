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

[Serializable]
public class MColorPositionSplitter : BotElement
{
	public const string name = "ColorPositionSplitter";    // Name this class

    public ColorPosition InColorPosition;
    public Point OutPosition;
    public Color OutColor;
    public bool OutIsActive;

    public MColorPositionSplitter() { }
	public MColorPositionSplitter(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next));
        CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.ColorPosition));
        CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Color));
        CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Position));
        CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Bool));
    }


    public static void BotAction(MColorPositionSplitter o)
	{
        if (o.InColorPosition != null)
        {
            o.Variables.Add(new VariableData() { Id = 4, Variable = o.InColorPosition.Color });         // Color
            o.Variables.Add(new VariableData() { Id = 5, Variable = o.InColorPosition.Position });      // Position
            o.Variables.Add(new VariableData() { Id = 6, Variable = o.InColorPosition.IsActive });      // Position
        }
    }
}
