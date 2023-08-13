using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace AutoBot_2._0.Class.Graph
{
	public sealed class Console : TextBox
	{
		private string bufMessage;
		private static string linePrefix = "> ";
		private bool IsControlDown;

		private static readonly object locked = new object();
		private static Console instance = null;
		public static Console Instance
		{
			get
			{
				if (instance == null)
				{
					lock (locked)
					{
						if (instance == null)
						{
							instance = new Console();
						}
					}
				}
				return instance;
			}
		}

		Console()
		{
			TextWrapping = TextWrapping.Wrap;
			AcceptsReturn = true;
			VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			Height = 200;
			Text = linePrefix;
			ContextMenu = null;

			PreviewKeyDown += ExecCommand;
			KeyUp += (object sender, KeyEventArgs e) =>
			{
				IsControlDown = false;
			};
		}

		private void ExecCommand(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var command = ((Console)sender).GetLineText(((Console)sender).LineCount - 1).Remove(0, linePrefix.Length);

				Text += "\n" + linePrefix;
				SelectionStart = Text.Length;

				CommandInterpreter(command);

				e.Handled = true;
				Instance.ScrollToEnd();
			}

			var l = ((Console)sender).SelectionStart;
			var d = ((Console)sender).GetCharacterIndexFromLineIndex(((Console)sender).LineCount - 1) + linePrefix.Length;

			if (!(l >= d))
				e.Handled = true;

			if (e.Key == Key.Back && l < d + 1)
				e.Handled = true;

			if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
				e.Handled = false;

			#region shortcut
			if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
			{
				IsControlDown = true;
				e.Handled = false;
			}

			if (IsControlDown && e.Key == Key.C)
			{
				// copy
				Clipboard.SetText(SelectedText);
			}
			if (IsControlDown && e.Key == Key.A)
			{
				// select all text
				SelectAll();
			}
			#endregion

			if (e.Key == Key.Up ||
				e.Key == Key.Down ||
				e.Key == Key.Left ||
				e.Key == Key.Right)
				e.Handled = false;

			if (!(l >= d) && e.Handled == true)
			{
				((Console)sender).SelectionStart = ((Console)sender).Text.Length;
				
				if (e.Key != Key.Back)
					e.Handled = false;

			}
		}

		private void CommandInterpreter(string command)
		{
			switch (command)
			{
				case "": break;
				case "clear":
					Text = linePrefix;
					SelectionStart = Text.Length;
					break;
				default: AddMessage("Unknow command \"" + command + "\"."); break;
			}
		}

		public static void AddMessage(string message)
		{
			try
			{
				var localInst = Instance;
				var poz2 = localInst.SelectionStart;
				var poz = localInst.SelectionStart - localInst.GetCharacterIndexFromLineIndex(localInst.LineCount - 1);

				Instance.Text = Instance.Text.Insert(
					Instance.GetCharacterIndexFromLineIndex(Instance.LineCount - 1),
					linePrefix + "(" +DateTime.Now.ToString("HH:mm:ss") + ") " + message + "\n"
					);
				var pos = Instance.GetCharacterIndexFromLineIndex(Instance.LineCount - 1);

				if (poz >= linePrefix.Length)
					Instance.SelectionStart = pos + poz;
				else Instance.SelectionStart = poz2;

				Instance.ScrollToEnd();
            }
			catch
			{
				

            }
        }

        public static void BufforedMessage(string message)
		{
			Instance.bufMessage += message;
		}
		public static void AddBuffMessage(string prefix = "", string surfix = "")
		{
			if (Instance.bufMessage != "")
			{
				AddMessage(prefix + Instance.bufMessage + surfix);
				ClearBuffMessage();
			}
		}
		public static void ClearBuffMessage()
		{
			Instance.bufMessage = "";
		}

	}
}
