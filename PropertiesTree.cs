using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace AutoBot_v1._1
{
	/// <summary>
	/// Logika interakcji dla klasy Tree.xaml
	/// </summary>
	class PropertiesTree : UserControl
	{
		public LowLevelKeyboardListener listener;
		public HotKeys hotKeys;
		public Window window;

		public PropertiesTree() { }

		#region Objects
		public List<TreeViewItem> CursorObject(MCursor b)
		{
			Label action = new Label() { Content = "Action" };

			Label position = new Label()
			{
				Content = MCursor.GetMousePosition().ToString(),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalContentAlignment = VerticalAlignment.Center,
				FontSize = 30
			};
			DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(10) };
			timer.Tick += (object sender, EventArgs e) =>
			{
				Point p = MCursor.GetMousePosition();
				position.Content = p.X + ":" + p.Y;
			};
			timer.Start();

			List<TreeViewItem> cursor = new List<TreeViewItem>()
			{
				ItemType(b),
				ItemName(b),
				TreeViewElement(ContentRow(action, CursorComboBox(b)), null),
				TreeViewElement(ContentRow(position), null),
				CursorData(b),
				ItemDescription(b),
				ItemTimes(b),
				ItemDelay(b)
			};

			return cursor;
		}
		public List<TreeViewItem> KeyObject(MKey b)
		{
			Label key = new Label()
			{
				Content = "Key"
			};

			Label keyScanner = new Label()
			{
				Content = "Key scanner"
			};
			CheckBox keyScannerValue = new CheckBox()
			{
				VerticalAlignment = VerticalAlignment.Center,
				IsChecked = b.ActiveScanner
			};
			keyScannerValue.Click += (object sender, RoutedEventArgs e) =>
			{
				b.ActiveScanner = (bool)keyScannerValue.IsChecked;
			};

			Label keyAction = new Label()
			{
				Content = "Key action"
			};

			List<TreeViewItem> treeViewItems = new List<TreeViewItem>()
			{
				ItemType(b),
				ItemName(b),
				TreeViewElement(ContentRow(key, KeyComboBox(b)), null),
				TreeViewElement(ContentRow(keyScanner, keyScannerValue), null),
				TreeViewElement(ContentRow(keyAction, KeyActionComboBox(b)), null),
				ItemDescription(b),
				ItemTimes(b),
				ItemDelay(b)
			};

			return treeViewItems;
		}
		public List<TreeViewItem> TextObject(MText b)
		{
			Label data = new Label()
			{
				Content = "Text"
			};
			Watermark dataValue = new Watermark()
			{

			};
			dataValue.placeholder.Text = "Text ...";
			dataValue.SearchTermTextBox.VerticalContentAlignment = VerticalAlignment.Center;
			dataValue.placeholder.VerticalAlignment = VerticalAlignment.Center;
			dataValue.SearchTermTextBox.Text = b.text;
			dataValue.SearchTermTextBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.text = dataValue.SearchTermTextBox.Text;
			};

			Watermark dataExpand = new Watermark()
			{
				Height = 100
			};
			dataExpand.placeholder.Text = "Text ...";
			dataExpand.SearchTermTextBox.TextWrapping = TextWrapping.Wrap;
			dataExpand.SearchTermTextBox.AcceptsReturn = true;
			dataExpand.SearchTermTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			dataExpand.SearchTermTextBox.Text = b.text;

			dataValue.SearchTermTextBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				dataExpand.SearchTermTextBox.Text = dataValue.SearchTermTextBox.Text;
			};
			dataExpand.SearchTermTextBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				dataValue.SearchTermTextBox.Text = dataExpand.SearchTermTextBox.Text;
			};

			List<TreeViewItem> treeViewItems = new List<TreeViewItem>()
			{
				ItemType(b),
				ItemName(b),
				TreeViewElement(ContentRow(data, dataValue),
				new List<TreeViewItem>() { TreeViewElement(ContentRow(dataExpand), null) }),
				ItemDescription(b),
				ItemTimes(b),
				ItemDelay(b)
			};

			treeViewItems[2].MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				if (treeViewItems[2].IsExpanded == true)
					dataValue.Visibility = Visibility.Hidden;
				else
					dataValue.Visibility = Visibility.Visible;
			};
			treeViewItems[2].LayoutUpdated += (object sender, EventArgs e) =>
			{
				if (treeViewItems[2].IsExpanded == true)
					dataValue.Visibility = Visibility.Hidden;
				else
					dataValue.Visibility = Visibility.Visible;
			};

			return treeViewItems;
		}
		public List<TreeViewItem> ScanColorObject(BotElement botElement)
		{
			Label label = new Label()
			{
				Content = "Data"
			};
			Button button = new Button()
			{
				Content = "Edit"
			};

			List<TreeViewItem> treeViewItems = new List<TreeViewItem>()
			{
				ItemType(botElement),
				ItemName(botElement),
				TreeViewElement(ContentRow(label, button), null),
				ItemDescription(botElement),
				ItemCheck(botElement),
				ItemDelay(botElement)
			};

			return treeViewItems;
		}
		public List<TreeViewItem> ScanAreaColorObject(BotElement botElement)
		{
			Label label = new Label()
			{
				Content = "Data"
			};
			Button button = new Button()
			{
				Content = "Edit"
			};

			List<TreeViewItem> treeViewItems = new List<TreeViewItem>()
			{
				ItemType(botElement),
				ItemName(botElement),
				TreeViewElement(ContentRow(label, button), null),
				ItemDescription(botElement),
				ItemCheck(botElement),
				ItemDelay(botElement)
			};

			return treeViewItems;
		}
		#endregion

		#region Controls
		public TreeViewItem ItemType(BotElement b)
		{
			Label type = new Label() { Content = "Type" };
			Label typeValue = new Label() { Content = b.Type, VerticalContentAlignment = VerticalAlignment.Center };

			return TreeViewElement(ContentRow(type, typeValue), null);
		}
		public TreeViewItem ItemName(BotElement b)
		{
			Label name = new Label() { Content = "Name" };
			TextBox nameValue = new TextBox() { Text = b.Name, VerticalContentAlignment = VerticalAlignment.Center };
			nameValue.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Name = nameValue.Text;
			};

			return TreeViewElement(ContentRow(name, nameValue), null);
		}
		public TreeViewItem ItemDelay(BotElement b)
		{
			Label delay = new Label()
			{
				Content = "Delay"
			};
			Label delayValue = new Label()
			{
				Content = "[" + b.Delay[0].Hours + ":" + b.Delay[0].Minutes + ":" + b.Delay[0].Seconds + "." + b.Delay[0].Milliseconds + "]-[" +
				b.Delay[1].Hours + ":" + b.Delay[1].Minutes + ":" + b.Delay[1].Seconds + "." + b.Delay[1].Milliseconds + "]",
				VerticalContentAlignment = VerticalAlignment.Center
			};

			Label randomDelay = new Label()
			{
				Content = "Random delay"
			};
			CheckBox randomDelayValue = new CheckBox()
			{
				IsChecked = b.IsDelay,
				VerticalContentAlignment = VerticalAlignment.Center
			};

			Label at = new Label()
			{
				Content = "At"
			};
			Label atValue = new Label()
			{
				Content = "[" + b.Delay[0].Hours + ":" + b.Delay[0].Minutes + ":" + b.Delay[0].Seconds + "." + b.Delay[0].Milliseconds + "]"
			};
			Label to = new Label()
			{
				Content = "To"
			};
			Label toValue = new Label()
			{
				Content = "[" + b.Delay[1].Hours + ":" + b.Delay[1].Minutes + ":" + b.Delay[1].Seconds + "." + b.Delay[1].Milliseconds + "]"
			};

			#region At To Objects
			Label atMilliseccond = new Label()
			{
				Content = "Milliseccond"
			};
			NumericUpDown atMilliseccondValue = new NumericUpDown()
			{
				MaxValue = 999,
				Value = b.Delay[0].Milliseconds
			};
			Label atSeccond = new Label()
			{
				Content = "Seccond"
			};
			NumericUpDown atSeccondValue = new NumericUpDown()
			{
				MaxValue = 59,
				Value = b.Delay[0].Seconds

			};
			Label atMinute = new Label()
			{
				Content = "Minute"
			};
			NumericUpDown atMinuteValue = new NumericUpDown()
			{
				MaxValue = 59,
				Value = b.Delay[0].Minutes
			};
			Label atHour = new Label()
			{
				Content = "Hour"
			};
			NumericUpDown atHourValue = new NumericUpDown()
			{
				MaxValue = 24,
				Value = b.Delay[0].Hours
			};

			void AtTextChanged(object sender, TextChangedEventArgs e)
			{
				b.Delay[0] = new TimeSpan(0, atHourValue.Value, atMinuteValue.Value, atSeccondValue.Value, atMilliseccondValue.Value);
				atValue.Content = "[" + atHourValue.Value + ":" + atMinuteValue.Value + ":" + atSeccondValue.Value + "." + atMilliseccondValue.Value + "]";
				SetDelayValue();
			}

			atMilliseccondValue.TextChanged += AtTextChanged;
			atSeccondValue.TextChanged += AtTextChanged;
			atMinuteValue.TextChanged += AtTextChanged;
			atHourValue.TextChanged += AtTextChanged;

			List<TreeViewItem> atTime = new List<TreeViewItem>()
					{
						TreeViewElement(ContentRow(atMilliseccond, atMilliseccondValue), null),
						TreeViewElement(ContentRow(atSeccond, atSeccondValue), null),
						TreeViewElement(ContentRow(atMinute, atMinuteValue), null),
						TreeViewElement(ContentRow(atHour, atHourValue), null)
					};

			Label toMilliseccond = new Label()
			{
				Content = "Milliseccond"
			};
			NumericUpDown toMilliseccondValue = new NumericUpDown()
			{
				MaxValue = 999,
				Value = b.Delay[1].Milliseconds
			};
			Label toSeccond = new Label()
			{
				Content = "Seccond"
			};
			NumericUpDown toSeccondValue = new NumericUpDown()
			{
				MaxValue = 59,
				Value = b.Delay[1].Seconds
			};
			Label toMinute = new Label()
			{
				Content = "Minute"
			};
			NumericUpDown toMinuteValue = new NumericUpDown()
			{
				MaxValue = 59,
				Value = b.Delay[1].Minutes
			};
			Label toHour = new Label()
			{
				Content = "Hour"
			};
			NumericUpDown toHourValue = new NumericUpDown()
			{
				MaxValue = 24,
				Value = b.Delay[1].Hours
			};

			void ToTextChanged(object sender, TextChangedEventArgs e)
			{
				b.Delay[0] = new TimeSpan(0, toHourValue.Value, toMinuteValue.Value, toSeccondValue.Value, toMilliseccondValue.Value);
				toValue.Content = "[" + toHourValue.Value + ":" + toMinuteValue.Value + ":" + toSeccondValue.Value + "." + toMilliseccondValue.Value + "]";
				SetDelayValue();
			}

			toMilliseccondValue.TextChanged += ToTextChanged;
			toSeccondValue.TextChanged += ToTextChanged;
			toMinuteValue.TextChanged += ToTextChanged;
			toHourValue.TextChanged += ToTextChanged;

			List<TreeViewItem> toTime = new List<TreeViewItem>()
					{
						TreeViewElement(ContentRow(toMilliseccond, toMilliseccondValue), null),
						TreeViewElement(ContentRow(toSeccond, toSeccondValue), null),
						TreeViewElement(ContentRow(toMinute, toMinuteValue), null),
						TreeViewElement(ContentRow(toHour, toHourValue), null)
					};

			void SetDelayValue()
			{
				if (randomDelayValue.IsChecked == true)
					delayValue.Content = atValue.Content + "-" + toValue.Content;
				else
					delayValue.Content = atValue.Content;
			}
			#endregion

			List<TreeViewItem> delayList = new List<TreeViewItem>();

			randomDelayValue.Click += ClickRandomDelayValue;
			void ClickRandomDelayValue(object sender, RoutedEventArgs e)
			{
				if (((CheckBox)sender).IsChecked == true)
				{
					delayList[2].Visibility = Visibility.Visible;
					at.Content = "At";
					SetDelayValue();
				}
				else
				{
					delayList[2].Visibility = Visibility.Collapsed;
					at.Content = "Sleep";
					SetDelayValue();
				}
			};

			delayList.Add(TreeViewElement(ContentRow(randomDelay, randomDelayValue), null));
			delayList.Add(TreeViewElement(ContentRow(at, atValue), atTime));
			delayList.Add(TreeViewElement(ContentRow(to, toValue), toTime));

			ClickRandomDelayValue(randomDelayValue, null);

			return TreeViewElement(ContentRow(delay, delayValue), delayList);
		}
		public TreeViewItem ItemDescription(BotElement b)
		{
			Label description = new Label()
			{
				Content = "Description"
			};
			Watermark descriptionValue = new Watermark()
			{
				MaxHeight = 30
			};
			descriptionValue.placeholder.Text = "Description ...";
			descriptionValue.SearchTermTextBox.VerticalContentAlignment = VerticalAlignment.Center;
			descriptionValue.placeholder.VerticalAlignment = VerticalAlignment.Center;
			descriptionValue.SearchTermTextBox.Text = b.Description;
			descriptionValue.SearchTermTextBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Description = descriptionValue.SearchTermTextBox.Text;
			};

			Watermark descriptionExpand = new Watermark()
			{
				Height = 100
			};
			descriptionExpand.placeholder.Text = "Description ...";
			descriptionExpand.SearchTermTextBox.TextWrapping = TextWrapping.Wrap;
			descriptionExpand.SearchTermTextBox.AcceptsReturn = true;
			descriptionExpand.SearchTermTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			descriptionExpand.SearchTermTextBox.Text = b.Description;

			descriptionValue.SearchTermTextBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				descriptionExpand.SearchTermTextBox.Text = descriptionValue.SearchTermTextBox.Text;
			};
			descriptionExpand.SearchTermTextBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				descriptionValue.SearchTermTextBox.Text = descriptionExpand.SearchTermTextBox.Text;
			};

			TreeViewItem item = TreeViewElement(ContentRow(description, descriptionValue),
				new List<TreeViewItem>() { TreeViewElement(ContentRow(descriptionExpand), null) }
				);

			item.MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				if (item.IsExpanded == true)
					descriptionValue.Visibility = Visibility.Hidden;
				else
					descriptionValue.Visibility = Visibility.Visible;
			};

			item.LayoutUpdated += (object sender, EventArgs e) =>
			{
				if (item.IsExpanded == true)
					descriptionValue.Visibility = Visibility.Hidden;
				else
					descriptionValue.Visibility = Visibility.Visible;
			};

			return item;
		}
		public TreeViewItem ItemTimes(BotElement b)
		{
			Label times = new Label() { Content = "Times" };
			NumericUpDown timesValue = new NumericUpDown() { FontSize = 14, MinValue = 1, Value = b.Times };
			timesValue.textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Times = timesValue.Value;
			};

			return TreeViewElement(ContentRow(times, timesValue), null);
		}
		public TreeViewItem ItemCheck(BotElement b)
		{
			Label check = new Label()
			{
				Content = "Check"
			};
			Label checkValue = new Label()
			{
				Content = "1:True",
				VerticalContentAlignment = VerticalAlignment.Center
			};

			Label times = new Label()
			{
				Content = "Times"
			};
			NumericUpDown timesValue = new NumericUpDown()
			{
				FontSize = 14,
				Value = 1
			};

			Label checkAllTimes = new Label()
			{
				Content = "Check all times"
			};
			CheckBox checkAllTimesValue = new CheckBox()
			{
				VerticalAlignment = VerticalAlignment.Center
			};

			Label find = new Label()
			{
				Content = "Find"
			};
			ComboBox findValue = FindComboBox();

			findValue.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
			{
				changeCheckValue();
				checkValue.Content = timesValue.textBox.Text + ":" + ((ComboBoxItem)findValue.SelectedItem).Content;

			};
			timesValue.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				if (timesValue.textBox.Text == "0")
					changeCheckValue();
				checkValue.Content = timesValue.textBox.Text + ":" + ((ComboBoxItem)findValue.SelectedItem).Content;

			};
			checkAllTimesValue.Click += (object sender, RoutedEventArgs e) =>
			{
				changeCheckValue();
			};
			void changeCheckValue()
			{
				if (timesValue.Value == 0)
					if (checkAllTimesValue.IsChecked == true)
						timesValue.textBox.Text = "∞";
					else
						timesValue.textBox.Text = "Auto";

			}

			List<TreeViewItem> checkList = new List<TreeViewItem>()
			{
				TreeViewElement(ContentRow(times, timesValue), null),
				TreeViewElement(ContentRow(checkAllTimes, checkAllTimesValue), null),
				TreeViewElement(ContentRow(find, findValue), null),
				ItemDelay(b)
			};

			return TreeViewElement(ContentRow(check, checkValue), checkList);
		}

		public ComboBox CursorComboBox(MCursor b)
		{
			ComboBox actionValue = new ComboBox() { VerticalContentAlignment = VerticalAlignment.Center };
			foreach (string n in MCursor.StateNames)
				actionValue.Items.Add(new ComboBoxItem() { Content = n });
			actionValue.SelectedIndex = b.KeyState;

			actionValue.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
			{
				b.KeyState = actionValue.SelectedIndex;
			};

			return actionValue;
		}
		public ComboBox KeyActionComboBox(MKey b)
		{
			ComboBox actionValue = new ComboBox() { VerticalContentAlignment = VerticalAlignment.Center };
			foreach (string n in MKey.KeyStateName)
				actionValue.Items.Add(new ComboBoxItem() { Content = n });
			actionValue.SelectedIndex = b.KeyState;

			actionValue.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
			{
				b.KeyState = actionValue.SelectedIndex;
			};

			return actionValue;
		}
		public ComboBox FindComboBox()
		{
			ComboBox actionValue = new ComboBox() { VerticalContentAlignment = VerticalAlignment.Center };
			actionValue.Items.Add(new ComboBoxItem() { Content = "True" });
			actionValue.Items.Add(new ComboBoxItem() { Content = "False" });
			actionValue.SelectedIndex = 0;

			return actionValue;
		}
		public ComboBox KeyComboBox(MKey b)
		{
			ComboBox actionValue = new ComboBox() { VerticalContentAlignment = VerticalAlignment.Center };
			for (int i = 0; i < byte.MaxValue; i++)
			{
				string keyText = MKey.keyCodeToString(i);

				actionValue.Items.Add("[" + i.ToString("X2") + "] " + keyText);
			}

			actionValue.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
			{
				b.KeyCode = actionValue.SelectedIndex;
			};
			actionValue.SelectedIndex = b.KeyCode;

			listener = new LowLevelKeyboardListener();
			listener.OnKeyPressed += (object sender, KeyPressedArgs e) =>
			{
				if (b.ActiveScanner == true)
				{
					actionValue.SelectedIndex = (int)e.KeyPressed;
				}
			};

			listener.HookKeyboard();

			return actionValue;
		}

		public TreeViewItem CursorData(MCursor b)
		{
			string hotKeyText = " (Ctrl + F1)";
			Label data = new Label() { Content = "Data" };
			Label dataValue = new Label() { Content = "[" + b.Point.X + ":" + b.Point.Y + "]" + hotKeyText, VerticalContentAlignment = VerticalAlignment.Center };

			Label xPosition = new Label()
			{
				Content = "X"
			};
			NumericUpDown xPositionValue = new NumericUpDown()
			{
				FontSize = 14,
				MaxValue = 99999,
				Value = (int)b.Point.X
			};
			xPositionValue.textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Point.X = xPositionValue.Value;
				dataValue.Content = "[" + b.Point.X + ":" + b.Point.Y + "]" + hotKeyText;
			};
			Label yPosition = new Label()
			{
				Content = "Y"
			};
			NumericUpDown yPositionValue = new NumericUpDown()
			{
				FontSize = 14,
				MaxValue = 99999,
				Value = (int)b.Point.Y
			};
			yPositionValue.textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Point.Y = yPositionValue.Value;
				dataValue.Content = "[" + b.Point.X + ":" + b.Point.Y + "]" + hotKeyText;
			};

			HotKeys.HotKey[] keys = new HotKeys.HotKey[]
			{
				new HotKeys.HotKey(() =>
				{
					Point p = MCursor.GetMousePosition();
					xPositionValue.Value = (int)p.X;
					yPositionValue.Value = (int)p.Y;

				}, ModifierKeys.Control, Key.F1),
			};
			hotKeys = new HotKeys(window, keys);

			List<TreeViewItem> dataList = new List<TreeViewItem>()
			{
				TreeViewElement(ContentRow(xPosition, xPositionValue), null),
				TreeViewElement(ContentRow(yPosition, yPositionValue), null),
			};

			return TreeViewElement(ContentRow(data, dataValue), dataList);
		}
		#endregion

		#region treeView Functions
		public void Tree_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			foreach (TreeViewItem t in ((TreeView)sender).Items)
				TreeViewSetSize(t, (int)((TreeView)sender).ActualWidth);
		}

		private Grid ContentRow(UIElement u1, UIElement u2)
		{
			Grid content = new Grid();

			content.ColumnDefinitions.Add(new ColumnDefinition() { });
			content.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
			content.Margin = new Thickness(0, 1, 0, 1);

			Grid.SetColumn(u1, 0);
			Grid.SetColumn(u2, 1);
			content.Children.Add(u1);
			content.Children.Add(u2);

			return content;
		}
		private Grid ContentRow(UIElement u1)
		{
			Grid content = new Grid();
			content.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
			content.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });

			Grid.SetColumnSpan(u1, 2);
			content.Children.Add(u1);

			return content;
		}

		public TreeViewItem TreeViewElement(UIElement u, List<TreeViewItem> treeViewItems)
		{
			TreeViewItem treeViewItem = new TreeViewItem();
			treeViewItem.Header = u;
			if (null != treeViewItems)
				foreach (var t in treeViewItems)
					treeViewItem.Items.Add(t);

			return treeViewItem;
		}
		public void CreateTree(TreeView tree, List<TreeViewItem> treeViewItems)
		{
			foreach (var t in treeViewItems)
				tree.Items.Add(t);
		}
		public void TreeViewSetSize(TreeViewItem treeViewItem, int treeViewSize, int deep = 1)
		{
			((Grid)treeViewItem.Header).Width = treeViewSize - (19 * deep + 25);
			((Grid)treeViewItem.Header).ColumnDefinitions[1] = new ColumnDefinition() { Width = new GridLength(treeViewSize / 2, GridUnitType.Pixel) };

			if (null != treeViewItem.Items)
			{
				deep++;
				foreach (TreeViewItem t in treeViewItem.Items)
				{
					TreeViewSetSize(t, treeViewSize, deep);
				}
			}
		}
		#endregion
	}
}
