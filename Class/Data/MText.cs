using AutoBot_2._0.Class.Data;
using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

[Serializable]
public class MText : BotElement
{
	public const string name = "Text";    // Name this class

	public string text; // Text to write
	public MText() { }
	public MText(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.String));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.String));
	}

	/* Verry old worked code now is not use *

	private static void pressKey(char sign)
	{
		byte pressKey = 0;
		bool shift = false, alt = false;

		switch (sign)
		{
			case ')': pressKey = 0x30; shift = true; break;
			case '!': pressKey = 0x31; shift = true; break;
			case '@': pressKey = 0x32; shift = true; break;
			case '#': pressKey = 0x33; shift = true; break;
			case '$': pressKey = 0x34; shift = true; break;
			case '%': pressKey = 0x35; shift = true; break;
			case '^': pressKey = 0x36; shift = true; break;
			case '&': pressKey = 0x37; shift = true; break;
			case '*': pressKey = 0x38; shift = true; break;
			case '(': pressKey = 0x39; shift = true; break;
				
			case 'A': pressKey = 0x41; shift = true; break;
			case 'B': pressKey = 0x42; shift = true; break;
			case 'C': pressKey = 0x43; shift = true; break;
			case 'D': pressKey = 0x44; shift = true; break;
			case 'E': pressKey = 0x45; shift = true; break;
			case 'F': pressKey = 0x46; shift = true; break;
			case 'G': pressKey = 0x47; shift = true; break;
			case 'H': pressKey = 0x48; shift = true; break;
			case 'I': pressKey = 0x49; shift = true; break;
			case 'J': pressKey = 0x4A; shift = true; break;
			case 'K': pressKey = 0x4B; shift = true; break;
			case 'L': pressKey = 0x4C; shift = true; break;
			case 'M': pressKey = 0x4D; shift = true; break;
			case 'N': pressKey = 0x4E; shift = true; break;
			case 'O': pressKey = 0x4F; shift = true; break;
			case 'P': pressKey = 0x50; shift = true; break;
			case 'Q': pressKey = 0x51; shift = true; break;
			case 'R': pressKey = 0x52; shift = true; break;
			case 'S': pressKey = 0x53; shift = true; break;
			case 'T': pressKey = 0x54; shift = true; break;
			case 'U': pressKey = 0x55; shift = true; break;
			case 'V': pressKey = 0x56; shift = true; break;
			case 'W': pressKey = 0x57; shift = true; break;
			case 'X': pressKey = 0x58; shift = true; break;
			case 'Y': pressKey = 0x59; shift = true; break;
			case 'Z': pressKey = 0x5A; shift = true; break;

			case '<': pressKey = 0xBC; shift = true; break;
			case '>': pressKey = 0xBE; shift = true; break;
			case '?': pressKey = 0xBF; shift = true; break;

			case ':': pressKey = 0xBA; shift = true; break;
			case '"': pressKey = 0xDE; shift = true; break;
			case '|': pressKey = 0xE2; shift = true; break;

			case '{': pressKey = 0xDB; shift = true; break;
			case '}': pressKey = 0xDD; shift = true; break;

			case '+': pressKey = 0xBB; shift = true; break;
			case '_': pressKey = 0xBD; shift = true; break;

			case '~': pressKey = 0xC0; break;

			case '0': pressKey = 0x30; break;
			case '1': pressKey = 0x31; break;
			case '2': pressKey = 0x32; break;
			case '3': pressKey = 0x33; break;
			case '4': pressKey = 0x34; break;
			case '5': pressKey = 0x35; break;
			case '6': pressKey = 0x36; break;
			case '7': pressKey = 0x37; break;
			case '8': pressKey = 0x38; break;
			case '9': pressKey = 0x39; break;

			case 'a': pressKey = 0x41; break;
			case 'b': pressKey = 0x42; break;
			case 'c': pressKey = 0x43; break;
			case 'd': pressKey = 0x44; break;
			case 'e': pressKey = 0x45; break;
			case 'f': pressKey = 0x46; break;
			case 'g': pressKey = 0x47; break;
			case 'h': pressKey = 0x48; break;
			case 'i': pressKey = 0x49; break;
			case 'j': pressKey = 0x4A; break;
			case 'k': pressKey = 0x4B; break;
			case 'l': pressKey = 0x4C; break;
			case 'm': pressKey = 0x4D; break;
			case 'n': pressKey = 0x4E; break;
			case 'o': pressKey = 0x4F; break;
			case 'p': pressKey = 0x50; break;
			case 'q': pressKey = 0x51; break;
			case 'r': pressKey = 0x52; break;
			case 's': pressKey = 0x53; break;
			case 't': pressKey = 0x54; break;
			case 'u': pressKey = 0x55; break;
			case 'v': pressKey = 0x56; break;
			case 'w': pressKey = 0x57; break;
			case 'x': pressKey = 0x58; break;
			case 'y': pressKey = 0x59; break;
			case 'z': pressKey = 0x5A; break;

			case ' ': pressKey = 0x20; break;

			case ',': pressKey = 0xBC; break;
			case '.': pressKey = 0xBE; break;
			case '/': pressKey = 0xBF; break;
				
			case ';': pressKey = 0xBA; break;
			case '\'': pressKey = 0xDE; break;
			case '\\': pressKey = 0xE2; break;

			case '[': pressKey = 0xDB; break;
			case ']': pressKey = 0xDD; break;
				
			case '=': pressKey = 0xBB; break;
			case '-': pressKey = 0xBD; break;

			case '`': pressKey = 0xC0; break;

			case 'ą': pressKey = 0x41; alt = true; break;
			case 'ć': pressKey = 0x43; alt = true; break;
			case 'ę': pressKey = 0x45; alt = true; break;
			case 'ł': pressKey = 0x4C; alt = true; break;
			case 'ń': pressKey = 0x4E; alt = true; break;
			case 'ó': pressKey = 0x4F; alt = true; break;
			case 'ś': pressKey = 0x53; alt = true; break;
			case 'ż': pressKey = 0x5A; alt = true; break;
			case 'ź': pressKey = 0x58; alt = true; break;

			case 'Ą': pressKey = 0x41; alt = true; shift = true; break;
			case 'Ć': pressKey = 0x43; alt = true; shift = true; break;
			case 'Ę': pressKey = 0x45; alt = true; shift = true; break;
			case 'Ł': pressKey = 0x4C; alt = true; shift = true; break;
			case 'Ń': pressKey = 0x4E; alt = true; shift = true; break;
			case 'Ó': pressKey = 0x4F; alt = true; shift = true; break;
			case 'Ś': pressKey = 0x53; alt = true; shift = true; break;
			case 'Ż': pressKey = 0x5A; alt = true; shift = true; break;
			case 'Ź': pressKey = 0x58; alt = true; shift = true; break;

			default: pressKey = 0; break;
		}
		if (pressKey != 0)
		{
			if (alt == false && shift == false)
			{
				MKey.keybd_event(pressKey, 0x45, 0x1 | 0, 0);
				MKey.keybd_event(pressKey, 0x45, 0x1 | 0x2, 0);
			}
			if (alt == false && shift == true)
			{
				MKey.keybd_event(0xA0, 0x45, 0x1 | 0, 0);
				MKey.keybd_event(pressKey, 0x45, 0x1 | 0, 0);
				MKey.keybd_event(pressKey, 0x45, 0x1 | 0x2, 0);
				MKey.keybd_event(0xA0, 0x45, 0x1 | 0x2, 0);
			}
			if (alt == true && shift == false)
			{
				MKey.keybd_event(0xA5, 0x45, 0x1 | 0, 0);
				MKey.keybd_event(pressKey, 0x45, 0x1 | 0, 0);
				MKey.keybd_event(pressKey, 0x45, 0x1 | 0x2, 0);
				MKey.keybd_event(0xA5, 0x45, 0x1 | 0x2, 0);
			}
			if (alt == true && shift == true)
			{
				MKey.keybd_event(0xA0, 0x45, 0x1 | 0, 0);
				MKey.keybd_event(0xA5, 0x45, 0x1 | 0, 0);
				MKey.keybd_event(pressKey, 0x45, 0x1 | 0, 0);
				MKey.keybd_event(pressKey, 0x45, 0x1 | 0x2, 0);
				MKey.keybd_event(0xA5, 0x45, 0x1 | 0x2, 0);
				MKey.keybd_event(0xA0, 0x45, 0x1 | 0x2, 0);
			}
		}
	}
	/********************************************************************/

	public static void BotAction(MText o)
	{
		/* Function make Copy and Paste (works for all charracter) */

		if (o.text == null) o.text = "";

		Clipboard.SetText(o.text); // Save text in clipboard (Copy)
		for (int i = 0; i < o.Times; i++)
		{
			/* Paste text */
			MKey.keybd_event(17, 0x45, 0x1 | 0, 0);     // Control Down

			MKey.keybd_event(86, 0x45, 0x1 | 0, 0);     // V Down
			MKey.keybd_event(86, 0x45, 0x1 | 0x2, 0);   // V Up

			MKey.keybd_event(17, 0x45, 0x1 | 0x2, 0);   // Control Up
		}

		o.Variables.Add(new VariableData() { Id = 4, Variable = o.text });
	}
}
