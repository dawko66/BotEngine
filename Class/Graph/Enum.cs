using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBot_2._0.Class.Graph
{
	#region Connection points enums
	[Serializable]
	public enum CType
	{
		// Connection types
		Next,
		Var,
		Char,
		String,
		Integer,
		Area,
		Position,
		Color,
		ColorPosition,
		ButtonAction,
		KeyAction,
		Key,
		Bool,
	}
	[Serializable]
	public enum CIO
	{
		// Connections input output
		Input, Output
	}
	[Serializable]
	public enum CArray
	{
		// Array connection type
		Yes, No
	}
	#endregion

	#region Block enums
	public enum BlockType
	{
		// Types of block element
		Test,
		Cursor,
		Key, Text, RandomText,
		ScanColor, ScanAreaColor,
		OpenBot,
		Script,
	}
    #endregion
}
