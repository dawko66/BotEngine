using AutoBot_2._0.Class.Data;
using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

[Serializable]
public class MCursor : BotElement
{
	/////////////////////////////////////////////////////
	/// random cursor position (position +- position) ///
	/////////////////////////////////////////////////////

	public const string name = "Cursor";    // Name this class
	/* Set cursor position and make action */
	public Point Point;  // Cursor move to position

	public int KeyState;    // Mouse button state (0-None, 1-Left Down, 2-Left Up, 3-Left Click, 4-Left Double Click, 5-Right Down, 6-Right Up, 7-Right Click, 8-Right Double Click)
	public static readonly string[] StateNames = { "None", "L Down", "L Up", "L Click", "R Down", "R Up", "R Click" };

	public MCursor() { }

	public MCursor(int Id)
	{
		this.Id = Id;
		Type = "Cursor";
		Name = Type + Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Position));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Position));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.ButtonAction));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.ButtonAction));
	}
	
	#region Get Mouse Position
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetCursorPos(ref Win32Point pt);

	[StructLayout(LayoutKind.Sequential)]
	public struct Win32Point
	{
		public Int32 X;
		public Int32 Y;
	};
	public static Point GetMousePosition()
	{
		Win32Point w32Mouse = new Win32Point();
		GetCursorPos(ref w32Mouse);
		return new Point(w32Mouse.X, w32Mouse.Y);
	}

	public static Win32Point GetMousePosition1()
	{
		Win32Point point = new Win32Point();
		GetCursorPos(ref point);
		return point;
	}
	#endregion

	#region Set Cursor Position
	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool SetCursorPos(int X, int Y);
	#endregion

	# region Mouse Key Event
	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
	#endregion

	public override string ToString()
	{
		return Point + " " + StateNames[KeyState];
	}
	public static void BotAction(MCursor o)
	{
		for (int i = 0; i < o.Times; i++)
		{
			// Thread.Sleep(50);
			SetCursorPos((int)o.Point.X, (int)o.Point.Y);     // Set cursor position

			Thread.Sleep(70);
			switch (o.KeyState)
			{
				/* 0 - none (do nothink only set cursor position) */
				case 1: mouse_event(0x02, 0, 0, 0, 0); break;   // Left Down
				case 2: mouse_event(0x04, 0, 0, 0, 0); break;   // Left Up
				case 3:                                         // Left Click
					mouse_event(0x02, 0, 0, 0, 0);
                    Thread.Sleep(10);

                    mouse_event(0x04, 0, 0, 0, 0);
					break;
				case 4: mouse_event(0x08, 0, 0, 0, 0); break;   // Right Down
				case 5: mouse_event(0x10, 0, 0, 0, 0); break;   // Right Up
				case 6:                                         // Right Click
					mouse_event(0x08, 0, 0, 0, 0);
                    Thread.Sleep(10);

                    mouse_event(0x10, 0, 0, 0, 0);
					break;
			}

			o.Variables.Add(new VariableData() { Id = 4, Variable = o.Point });
			o.Variables.Add(new VariableData() { Id = 6, Variable = o.KeyState });
		}
	}
}
