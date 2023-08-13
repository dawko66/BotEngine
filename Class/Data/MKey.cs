using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Drawing;
using AutoBot_2._0.Class.Data;

[Serializable]
public class MKey : BotElement
{
	

	#region
	// Key numbers and namemes System.Windows.Forms.Keys or http://myc01.free.fr/mycview/keys.htm

	/* key number => key names */
	/*public static readonly string[] keyNames = {	"EOS",				"LButton",			"RButton",				"Cancel",				"MButton",			"XButton1",			"XButton2",			"#07 Undefined",		"Back",				"Tab",
									"#0A Reserved",		"#0B Reserved",		"Clear",				"Return",				"#0E Undefined",	"#0F Undefined",	"Shift",			"Control",				"Menu",				"Pause",
									"Capital",			"HangulMode",		"#16 Undefined",		"JunjaMode",			"FinalMode",		"HanjaMode",		"#1A Undefined",	"Escape",				"IMEConvert",		"IMENonconvert",
									"IMEAccept",		"IMEModeChange",	"Space",				"Prior",				"Next",				"End",				"Home",				"Left",					"Up",				"Right",
									"Down",				"Select",			"Print",				"Execute",				"Snapshot",			"Insert",			"Delete",			"Help",					"0",				"1",
									"2",				"3",				"4",					"5",					"6",				"7",				"8",				"9",					"#3A Undefined",	"#3B Undefined",
									"#3C Undefined",	"#3D Undefined",	"#3E Undefined",		"#3F Undefined",		"#40 Undefined",	"A",				"B",				"C",					"D",				"E",
									"F",				"G",				"H",					"I",					"J",				"K",				"L",				"M",					"N",				"O",
									"P",				"Q",				"R",					"S",					"T",				"U",				"V",				"W",					"X",				"Y",
									"Z",				"LWin",				"RWin",					"Apps",					"#5E Reserved",		"Sleep",			"NumPad0",			"NumPad1",				"NumPad2",			"NumPad3",
									"NumPad4",			"NumPad5",			"NumPad6",				"NumPad7",				"NumPad8",			"NumPad9",			"Multiply",			"Add",					"Separator",		"Subtract",
									"Decimal",			"Divide",			"F1",					"F2",					"F3",				"F4",				"F5",				"F6",					"F7",				"F8",
									"F9",				"F10",				"F11",					"F12",					"F13",				"F14",				"F15",				"F16",					"F17",				"F18",
									"F19",				"F20",				"F21",					"F22",					"F23",				"F24",				"#88 Unassigned",	"#89 Unassigned",		"#8A Unassigned",	"#8B Unassigned",
									"#8C Unassigned",	"#8D Unassigned",	"#8E Unassigned",		"#8F Unassigned",		"NumLock",			"Scroll",			"#92 OEM specific",	"#93 OEM specific",		"#94 OEM specific",	"#95 OEM specific",
									"#96 OEM specific",	"#97 Unassigned",	"#98 Unassigned",		"#99 Unassigned",		"#9A Unassigned",	"#9B Unassigned",	"#9C Unassigned",	"#9D Unassigned",		"#9E Unassigned",	"#9F Unassigned",
									"LShiftKey",		"RShiftKey",		"LControlKey",			"RControlKey",			"LMenu",			"RMenu",			"BrowserBack",		"BrowserForward",		"BrowserRefresh",	"BrowserStop",
									"BrowserSearch",	"BrowserFavorites",	"BrowserHome",			"VolumeMute",			"VolumeDown",		"VolumeUp",			"MediaNextTrack",	"MediaPreviousTrack",	"MediaStop",		"MediaPlayPause",
									"LaunchMail",		"SelectMedia",		"LaunchApplication1",	"LaunchApplication2",	"#B8 Reserved",		"#B9 Reserved",		"Oem1",				"OemPlus",				"OemComma",			"OemMinus",
									"OemPeriod",		"Oem2",				"Oem3",					"#C1 Reserved",			"#C2 Reserved",		"#C3 Reserved",		"#C4 Reserved",		"#C5 Reserved",			"#C6 Reserved",		"#C7 Reserved",
									"#C8 Reserved",		"#C9 Reserved",		"#CA Reserved",			"#CB Reserved",			"#CC Reserved",		"#CD Reserved",		"#CE Reserved",		"#CF Reserved",			"#D0 Reserved",		"#D1 Reserved",
									"#D2 Reserved",		"#D3 Reserved",		"#D4 Reserved",			"#D5 Reserved",			"#D6 Reserved",		"#D7 Reserved",		"#D8 Unassigned",	"#D9 Unassigned",		"#DA Unassigned",	"Oem4",
									"Oem5",				"Oem6",				"Oem7",					"Oem8",					"#E0 Reserved",		"#E1 OEM specific",	"Oem102",			"#E3 OEM specific",		"#E4 OEM specific",	"ProcessKey",
									"#E6 OEM specific",	"Packet",			"#E8 Unassigned",		"#E9 OEM specific",		"#EA OEM specific",	"#EB OEM specific",	"#EC OEM specific",	"#ED OEM specific",		"#EE OEM specific", "#EF OEM specific",
									"#F0 OEM specific",	"#F1 OEM specific", "#F2 OEM specific",		"#F3 OEM specific",		"#F4 OEM specific", "#F5 OEM specific",	"Attn",				"Crsel",				"Exsel",			"EraseEof",
									"Play",				"Zoom",				"NoName",				"Pa1",					"OemClear",			"EOL" };*/
	#endregion
	
	public enum ScanCode
	{
        Escape = 0x01,
        D1 = 0x02,
        D2 = 0x03,
        D3 = 0x04,
        D4 = 0x05,
        D5 = 0x06,
        D6 = 0x07,
        D7 = 0x08,
        D8 = 0x09,
        D9 = 0x0A,
        D0 = 0x0B,
        OemMinus = 0x0C,
        OemPlus = 0x0D,
        Back = 0x0E,
        Tab = 0x0F,
		Q = 0x10,
		W = 0x11,
		E = 0x12,
		R = 0x13,
		T = 0x14,
		Y = 0x15,
		U = 0x16,
		I = 0x17,
		O = 0x18,
		P = 0x19,
        Oem4 = 0x1A,
        Oem6 = 0x1B,
        Enter = 0x1C,
        Control = 0x1D,
		A = 0x1E,
		S = 0x1F,
		D = 0x20,
		F = 0x21,
		G = 0x22,
		H = 0x23,
		J = 0x24,
		K = 0x25,
		L = 0x26,
        OemSemicolon = 0x27,
        OemQuotes = 0x28,
        Oemtilde = 0x29,
        LShiftKey = 0x2A,
        OemPipe = 0x2B,
        Z = 0x2C,
        X = 0x2D,
        C = 0x2E,
        V = 0x2F,
        B = 0x30,
        N = 0x31,
        M = 0x32,
        Oemcomma = 0x33,
        OemPeriod = 0x34,
        Oem2 = 0x35,
        RShiftKey = 0x36,
        PrintScreen = 0x37,
        Alt = 0x38,
        Space = 0x39,
        CapsLock = 0x3A,
        F1 = 0x3B,
        F2 = 0x3C,
        F3 = 0x3D,
        F4 = 0x3E,
        F5 = 0x3F,
        F6 = 0x40,
        F7 = 0x41,
        F8 = 0x42,
        F9 = 0x43,
        F10 = 0x44,
        NumLock = 0x45,
        Scroll = 0x46,
        Home = 0x47,
        Up = 0x48,
        PageUp = 0x49,
        Subtract = 0x4A,
        Left = 0x4B,
        Clear = 0x4C,
        Right = 0x4D,
        Add = 0x4E,
        End = 0x4F,
        Down = 0x50,
        PageDown = 0x51,
        Insert = 0x52,
        Delete = 0x53,

    }
    public const string name = "Key";	// Name this class
	public static readonly string[] KeyStateName = { "Down", "Up", "Click" };

	public int KeyCode;		// ID keyboard button
	public int KeyState;    // Button state (1-Down, 2-Up, 3-Click)		/* (null-Click ,0-Up, 1-Down) 1 BIT conception */\
	public bool ActiveScanner = true;

	public MKey() { }

	public MKey(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + base.Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Key));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Key));	// 4
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.KeyAction));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.KeyAction));	//6
	}

	#region Keyboard key event
	[StructLayout(LayoutKind.Sequential)]
	public struct KeyboardInput
	{
		public ushort wVk;
		public ushort wScan;
		public uint dwFlags;
		public uint time;
		public IntPtr dwExtraInfo;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	protected internal static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
	
	public static void BotAction(MKey o)
	{
		byte keyCode = Convert.ToByte(o.KeyCode);

		for (int i = 0; i < o.Times; i++)
		{
			switch (o.KeyState)
			{
                case 0: keybd_event(keyCode, 0x9D, 0x1 | 0, 0); break;    // Key Down
                case 1: keybd_event(keyCode, 0x9D, 0x1 | 0x2, 0); break;  // Key Up
                //case 0: keybd_event(keyCode, 0x45, 0x1 | 0, 0); break;    // Key Down
                //case 1: keybd_event(keyCode, 0x45, 0x1 | 0x2, 0); break;  // Key Up
                case 2:                                                     // Key Press
					Thread.Sleep(100);
					keybd_event(keyCode, 0x2c, 0, 0); //90, 0x2c, z
					Thread.Sleep(1000);
					keybd_event(keyCode, 0x2c, 0x2, 0);
					Thread.Sleep(100);
					break;
			}
		}

		// data
		o.Variables.Add(new VariableData() { Id = 4, Variable = o.KeyCode });
		o.Variables.Add(new VariableData() { Id = 6, Variable = o.KeyState });

	}
	#endregion
	
	#region To String
	public static string keyCodeToString(int keyCode)
	{
		return Enum.GetName(typeof(Keys), keyCode);
	}
	
	public string keyCodeToString()
	{
		return keyCodeToString(KeyCode);
	}

	public static string keyStateToString(int keyState)
	{
		return KeyStateName[keyState-1];
	}
	public string keyStateToString()
	{
		return keyStateToString(KeyState);
	}

	public override string ToString()
	{
		return keyCodeToString() + " " + keyStateToString();
	}
	#endregion

	/* Show Keyboard window (Key & Text) */
	public static void showDialog(Window window)
	{
		window.Hide();
		//~~new WindowKeyboard().ShowDialog();
		window.Show();
	}
}
