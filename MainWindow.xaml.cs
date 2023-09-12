using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Microsoft.Win32;
using AutoBot_2._0.Class.Data;

namespace AutoBot_v1._1
{
	/// <summary>
	/// Logika interakcji dla klasy Window.xaml
	/// </summary>
	/// 



	public partial class Window2 : Window
	{
		bool IsMouseDown;

		protected override void OnSourceInitialized(EventArgs e)
		{
			//new ScanColorWindow().Show();

		}

		public Window2()
		{
			InitializeComponent();

			#region Shortcut's
			KeyBinding kb = new KeyBinding();
			//kb.Gesture = new KeyBinding(OpenButton_Click, Key.S, );


			// open
			RoutedCommand newCmd = new RoutedCommand();
			newCmd.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
			CommandBindings.Add(new CommandBinding(newCmd, NewButton_Click));

			// open
			RoutedCommand openCmd = new RoutedCommand();
			openCmd.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
			CommandBindings.Add(new CommandBinding(openCmd, OpenButton_Click));

			// save as
			RoutedCommand saveAsCmd = new RoutedCommand();
			saveAsCmd.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
			CommandBindings.Add(new CommandBinding(saveAsCmd, SaveAsButton_Click));

			// save
			RoutedCommand saveCmd = new RoutedCommand();
			saveCmd.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
			CommandBindings.Add(new CommandBinding(saveCmd, SaveButton_Click));
			#endregion

			Container.Children.Add(AutoBot_2._0.Class.Graph.Console.Instance);
			Grid.SetRow(AutoBot_2._0.Class.Graph.Console.Instance, 3);

			tree.window = this;
/*
			tabPanel.tabGrid.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
			{
				*//*if (((TabControl)tabPanel.Children[0]).Items.Count > 0)
					tree.Graph = tabPanel.GetCurrentGraph();*//*
			};*/
			tabPanel.NewTab += (object sender, EventArgs e) =>
			{
				((Graph)sender).Selected += (object s1, EventArgs e1) =>
				{
					void ClearTree()
					{
						if (tree.listener != null)
							tree.listener.UnHookKeyboard();

						if (tree.hotKeys != null)
							tree.hotKeys.stop();

						tree.Items.Clear();
					}
					ClearTree();
					UpdateLayout();

					if (s1 != null)
					{
						Block b = (Block)s1;
						BotElement be = ((Graph)sender).BotData.BotElements.Find(x => x.Id == b.Id);
						tree.Graph = (Graph)sender;
						tree.CurrentBlock = b;
						tree.CreateTree(tree, tree.BObject(be));
					}
                    else
                    {
						tree.CreateTree(tree, tree.BObject(((Graph)sender).BotData));
					}

					tree.Tree_SizeChanged(tree, null);
					Button_Click(null, null);
				};
			};
			tabPanel.AddNewProject();

			objectList.MouseUp += (object sender, MouseButtonEventArgs e) =>
			{
				if (IsMouseDown == true)
				{
					//!!!~~ BotElements
					string d = (string)objectList.SelectedItem;
					int id = BotElement.GiveID(tabPanel.GetCurrentGraph().BotData.BotElements);

					Point position = new Point(tabPanel.GetCurrentGraph().HorizontalOffset + tabPanel.GetCurrentGraph().ViewportWidth / 2 - Block.BlockWidth / 2,
						tabPanel.GetCurrentGraph().VerticalOffset + tabPanel.GetCurrentGraph().ViewportHeight / 2 - Block.BlockWidth / 2);

					if (d == MText.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MText(id) { Poisition = position });
					else if (d == MCursor.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MCursor(id) { Poisition = position, Point = new Point(), KeyState = 3 });
					else if (d == MKey.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MKey(id) { Poisition = position, KeyCode = 20, KeyState = 2 });
					else if (d == MScanColor.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MScanColor(id) { Poisition = position, ColorsAndPositions = new List<ColorAndPosition>(), ColorPositions = new List<ColorPosition>() });
					else if (d == MOpenBOT.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MOpenBOT(id) { Poisition = position });
					else if (d == MIf.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MIf(id) { Poisition = position });
					else if (d == MCallToAPI.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MCallToAPI(id) { Poisition = position });
					else if (d == MMultiplexer.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MMultiplexer(id) { Poisition = position });
					else if (d == MColorPositionSplitter.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MColorPositionSplitter(id) { Poisition = position });
					else if (d == MArray.name) tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MArray(id) { Poisition = position });

                    tabPanel.GetCurrentGraph().AddBlocks(tabPanel.GetCurrentGraph().CreateBlock(tabPanel.GetCurrentGraph().BotData.BotElements.Last()));
					UpdateLayout();

					IsMouseDown = false;
					objectList.UnselectAll();
				}
			};
			objectList.SelectionChanged += (object s, SelectionChangedEventArgs e) =>
			{
				IsMouseDown = true;
			};
		}

        List<CData> CData;
		private void cursorButton_Click(object sender, RoutedEventArgs e)
		{
			//tabPanel.GetGraph().BotElements.Clear();
			tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MText(BotElement.GiveID(tabPanel.GetCurrentGraph().BotData.BotElements)) { Poisition = new Point(50, 50) });
			tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MCursor(BotElement.GiveID(tabPanel.GetCurrentGraph().BotData.BotElements)) { Poisition = new Point(50, 200), Point = new Point(), KeyState = 3 });
			tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MKey(BotElement.GiveID(tabPanel.GetCurrentGraph().BotData.BotElements)) { Poisition = new Point(350, 50) });
			tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MScanColor(BotElement.GiveID(tabPanel.GetCurrentGraph().BotData.BotElements)) { Poisition = new Point(350, 200), ColorsAndPositions = new List<ColorAndPosition>(), ColorPositions = new List<ColorPosition>() });
			tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MOpenBOT(BotElement.GiveID(tabPanel.GetCurrentGraph().BotData.BotElements)) { Poisition = new Point(50, 350), Source = "WeatherForecast.json" });
			tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MPosition(BotElement.GiveID(tabPanel.GetCurrentGraph().BotData.BotElements)) { Poisition = new Point(350, 350) });
			tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MArray(BotElement.GiveID(tabPanel.GetCurrentGraph().BotData.BotElements)) { Poisition = new Point(50, 500) });
			tabPanel.GetCurrentGraph().BotData.BotElements.Add(new MTest(BotElement.GiveID(tabPanel.GetCurrentGraph().BotData.BotElements)) { Poisition = new Point(350, 500) });

			CData = new List<CData>()
			{
				/*new CData(1, 2, 2, 1),
				new CData(5, 2, 1, 3),
				new CData(8, 2, 1, 3),
				new CData(7, 2, 4, 3),*/
			};
			tabPanel.GetCurrentGraph().BotData.CDatas = CData;

			foreach (BotElement be in tabPanel.GetCurrentGraph().BotData.BotElements)
			{
				if (be.Id != 0) tabPanel.GetCurrentGraph().AddBlocks(tabPanel.GetCurrentGraph().CreateBlock(be));
			}
			UpdateLayout();
			//tabPanel.GetGraph().RenderBlocks();
			tabPanel.GetCurrentGraph().RenderCLines();

			/*			tabPanel.GetGraph().RemoveAllBlocks();
						tabPanel.GetGraph().CDatas.Clear();

						BotData botData = Serializer.DeserializeObject("daw.xml");
						tabPanel.GetGraph().BotElements = botData.BotElements;
						tabPanel.GetGraph().CDatas = botData.CDatas;

						tabPanel.GetGraph().RenderBlocks();
						tabPanel.GetGraph().RenderCLines();*/
		}

		private void keyboardButton_Click(object sender, RoutedEventArgs e)
		{
			/*            #region Serialize
						string fileName = "WeatherForecast.json";
						var obj = new BotData() { BotElements = tabPanel.GetGraph().BotElements, CDatas = tabPanel.GetGraph().CDatas };
						var t = JsonConvert.SerializeObject(obj, Formatting.Indented);
						//File.WriteAllText(fileName, t);
						#endregion

						#region Deserialize
						tabPanel.GetGraph().RemoveAllBlocks();
						tabPanel.GetGraph().CDatas.Clear();

						string load = File.ReadAllText("WeatherForecast.json");
						var s = JsonConvert.DeserializeObject<BotData>(load);
						tabPanel.GetGraph().BotElements = s.BotElements;
						tabPanel.GetGraph().CDatas = s.CDatas;

						tabPanel.GetGraph().RenderBlocks();
						tabPanel.GetGraph().RenderCLines();
						#endregion*/


			//tree.tree
			//new ColorLoupe().Show();
			/*			tabPanel.GetGraph().RemoveAllBlocks();
						tabPanel.GetGraph().CDatas.Clear();

						BotData botData = Serializer.DeserializeObject("dziad.xml");
						tabPanel.GetGraph().BotElements = botData.BotElements;
						tabPanel.GetGraph().CDatas = botData.CDatas;

						tabPanel.GetGraph().RenderBlocks();
						tabPanel.GetGraph().RenderCLines();*/

			var d = new Compiler(new BotData() { BotElements = tabPanel.GetCurrentGraph().BotData.BotElements, CDatas = tabPanel.GetCurrentGraph().BotData.CDatas });
			d.Show();
			//d.Close();
		}


		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			tabPanel.AddNewProject();
		}

		private void OpenButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "bot files (*.bot)|*.xml|All files (*.*)|*.*";
			openFileDialog.FilterIndex = 1;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.FileName = tabPanel.GetCurrentGraph().Source;

			if (openFileDialog.ShowDialog() == true)
			{
				tabPanel.openProject(openFileDialog.FileName);
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (tabPanel.GetCurrentGraph().Source != null && tabPanel.GetCurrentGraph().Source != "")
			{
				Serializer.SerializeObject(new BotData()
				{
					BotElements = tabPanel.GetCurrentGraph().BotData.BotElements,
					CDatas = tabPanel.GetCurrentGraph().BotData.CDatas
				}, tabPanel.GetCurrentGraph().Source);
				AutoBot_2._0.Class.Graph.Console.AddMessage("Saving complete. " + tabPanel.GetCurrentGraph().Source);
			}
			else
				SaveAsButton_Click(sender, e);
		}

		private void SaveAsButton_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "bot files (*.bot)|*.xml|All files (*.*)|*.*";
			saveFileDialog.FilterIndex = 1;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.FileName = tabPanel.GetCurrentGraph().Source;

			if (saveFileDialog.ShowDialog() == true)
			{
				tabPanel.SaveAs(saveFileDialog.FileName);
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			tree.Visibility = Visibility.Visible;
			objectList.Visibility = Visibility.Hidden;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			tree.Visibility = Visibility.Hidden;
			objectList.Visibility = Visibility.Visible;
		}


		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			// 1 : D1 : 2
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}
	}
}
