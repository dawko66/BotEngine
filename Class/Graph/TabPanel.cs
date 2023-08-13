using AutoBot_2._0.Class.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoBot_2._0.Class.Graph
{
	class TabPanel : Grid
	{
		public TabControl tabGrid;
		private static int HideItemMargin = 30;

		public event EventHandler NewTab;
		protected virtual void NewTabEvent(object ob, EventArgs e)
		{
			NewTab?.Invoke(ob, e);
		}

		public TabPanel()
		{
			tabGrid = new TabControl()
			{
				MinWidth = 200,
				AllowDrop = true,
			};
			Children.Add(tabGrid);

			tabGrid.SizeChanged += TabGrid_SizeChanged;
            tabGrid.Drop += TabGrid_Drop;

			TabsComboList();
		}

        private void TabGrid_Drop(object sender, DragEventArgs e)
        {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				// Note that you can have more than one file.
				string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach (string i in paths)
					openProject(i);
			}
		}

        private void TabGrid_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			foreach (TabItem i in tabGrid.Items)
			{
				i.Visibility = Visibility.Collapsed;
			}

			double dd = 0;
			foreach (TabItem i in tabGrid.Items)
			{
				i.Visibility = Visibility.Visible;
				tabGrid.UpdateLayout();
				dd += i.ActualWidth;

				if (dd > tabGrid.ActualWidth - HideItemMargin)
				{
					i.Visibility = Visibility.Collapsed;
					tabGrid.UpdateLayout();

					break;
				}
			}
		}

		private Grid TabItemHeader(TabItem ti, string path)
        {
			Label deleteButton = new Label()
			{
				Content = "❌",
				Margin = new Thickness(5, 0, 0, 0),
				Padding = new Thickness(0, -10, 0, 0),
				HorizontalContentAlignment = HorizontalAlignment.Center,
				VerticalContentAlignment = VerticalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				FontSize = 8,
				Height = 15,
				Width = 15,
				BorderThickness = new Thickness(1),
				BorderBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230)),
			};
			deleteButton.MouseMove += (object sender, System.Windows.Input.MouseEventArgs e) =>
			{
				deleteButton.Background = new SolidColorBrush(Color.FromRgb(255, 71, 71));
			};
			deleteButton.MouseLeave += (object sender, System.Windows.Input.MouseEventArgs e) =>
			{
				deleteButton.Background = Brushes.Transparent;
			};
			deleteButton.MouseDown += (object sender, System.Windows.Input.MouseButtonEventArgs e) =>
			{
				// remove current graph

				ti.Template = null;
				tabGrid.Items.Remove(ti);

				if (tabGrid.Items.Count <= 0)
				{
					AddNewProject();
					tabGrid.SelectedIndex = 0;
				}

				TabGrid_SizeChanged(null, null);

				if (tabGrid.SelectedIndex == -1)
					tabGrid.SelectedIndex = 0;
			};

			TextBlock tb = new TextBlock()
			{
				Padding = new Thickness(0, 0, 0, 0),
				Text = path == null ? "New Project" : path.Split('\\').Last(),
				Margin = new Thickness(2),
				FontSize = 12,
				MaxWidth = 100,
				TextTrimming = TextTrimming.CharacterEllipsis,
			};

			Grid sp = new Grid()
			{
			};
			sp.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
			sp.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

			Grid.SetColumn(tb, 0);
			Grid.SetColumn(deleteButton, 1);

			sp.Children.Add(tb);
			sp.Children.Add(deleteButton);

			return sp;
		}

		public void NewTabItem(Graph graph)
		{
			TabItem ti = new TabItem()
			{
				Content = graph,
				IsTabStop = true,
			};

			tabGrid.Items.Add(ti);

			ti.Header = TabItemHeader(ti, graph.Source);
			tabGrid.SelectedIndex = tabGrid.Items.Count - 1;
			TabGrid_SizeChanged(null, null);
			NewTabEvent(graph, new EventArgs());
		}

		public Graph GetCurrentGraph()
		{
			if (tabGrid.SelectedIndex == -1)
				tabGrid.SelectedIndex = 0;

			return (Graph)((TabItem)tabGrid.Items[tabGrid.SelectedIndex]).Content;
		}

		public void AddNewProject()
		{
			NewTabItem(new Graph());
		}
		public void openProject(string path)
		{
			BotData bd;
			bool res = Serializer.DeserializeObject(path, out bd);
			if (res)
			{
				Graph g = new Graph();
				g.BotData.BotElements = bd.BotElements;
				g.BotData.CDatas = bd.CDatas;
				g.Refresh();
				g.Source = path;
				MOpenBOT.CheckSource(g);

				if (!CheckSamePaths(g.Source))
				{
					NewTabItem(g);
					Console.AddMessage("Load success! : " + g.Source);
				}
				else
					Console.AddMessage("Cannot load. This file is alredy opened.");

				g.Refresh();
			}
			else
				Console.AddMessage(path + " open error.");
		}
		public void SaveAs(string path)
		{
			if (!CheckSamePaths(path))
            {
				Serializer.SerializeObject(new BotData()
				{
					BotElements = GetCurrentGraph().BotData.BotElements,
					CDatas = GetCurrentGraph().BotData.CDatas
				}, path);

				GetCurrentGraph().Source = path;
				((TextBlock)((Grid)((TabItem)tabGrid.Items[tabGrid.SelectedIndex]).Header).Children[0]).Text = path.Split('\\').Last();
				Console.AddMessage("Saving file complete: \"" + path + "\"");
			}
			else
				Console.AddMessage("This file is alredy opened: \"" + path + "\"");
		}

		public void TabsComboList()
		{
			ComboBox comboList = new ComboBox()
			{
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Top,
				Width = 15,
				Height = 15,
				Margin = new Thickness(0, 10, 0, 0),
				BorderThickness = new Thickness(0),
			};
			comboList.DropDownOpened += (object sender, EventArgs e) =>
			{
				comboList.Items.Clear();
				foreach (TabItem t in tabGrid.Items)
					comboList.Items.Add(TabItemHeader(t, ((TextBlock)((Grid)t.Header).Children[0]).Text));
			};
			comboList.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
			{
				if (comboList.SelectedIndex != -1)
				{
					tabGrid.SelectedIndex = comboList.SelectedIndex;
					comboList.SelectedIndex = -1;
				}
			};

			this.Children.Add(comboList);
		}

		public bool CheckSamePaths(string path)
		{
			foreach (TabItem i in tabGrid.Items)
				if (((Graph)i.Content).Source == path)
                {
					// focus this TabItem
					tabGrid.SelectedItem = i;
					return true;
				}

			return false;
		}
	}
}
