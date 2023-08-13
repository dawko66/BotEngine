using AutoBot_2._0.Class.Data;
using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1;
using AutoBot_v1._1.Class.Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AutoBot_2._0.Class.Graph
{
	/// <summary>
	/// Logika interakcji dla klasy Tree.xaml
	/// </summary>
	class PropertiesTree : TreeView
	{
		public LowLevelKeyboardListener listener;
		public HotKeys hotKeys;
		public Window window;
		public Graph Graph;
		public Block CurrentBlock;

		public PropertiesTree()
		{

		}

		#region Objects
		public List<TreeViewItem> BObject(BotData bd)
        {
			Label type = new Label() { Content = "Type" };
			Label typeValue = new Label() { Content = "Project", VerticalContentAlignment = VerticalAlignment.Center };

			Label name = new Label()
			{
				Content = "Name"
			};
			Watermark nameValue = new Watermark()
			{

			};
			nameValue.placeholder.Text = "null";
			nameValue.SearchTermTextBox.VerticalContentAlignment = VerticalAlignment.Center;
			nameValue.placeholder.VerticalAlignment = VerticalAlignment.Center;
			nameValue.SearchTermTextBox.Text = bd.Name;
			nameValue.SearchTermTextBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				bd.Name = nameValue.SearchTermTextBox.Text;
			};

			List<TreeViewItem> item = new List<TreeViewItem>()
			{
				TreeViewElement(ContentRow(type, typeValue), null),
				TreeViewElement(ContentRow(name, nameValue), null)
			};
	
			return item;
        }

		public List<TreeViewItem> BObject(BotElement b)
		{
			//!!!~~ BotElements
			if (b.Type == MText.name) return BObject((MText)b);
			else if (b.Type == MCursor.name) return BObject((MCursor)b);
			else if (b.Type == MKey.name) return BObject((MKey)b);
			else if (b.Type == MScanColor.name) return BObject((MScanColor)b);
			else if (b.Type == MScanAreaColor.name) return BObject((MScanAreaColor)b);
			else if (b.Type == MOpenBOT.name) return BObject((MOpenBOT)b);
			else if (b.Type == MIf.name) return BObject((MIf)b);
			else if (b.Type == MCallToAPI.name) return BObject((MCallToAPI)b);
			else if (b.Type == MMultiplexer.name) return BObject((MMultiplexer)b);
			else if (b.Type == MColorPositionSplitter.name) return BObject((MColorPositionSplitter)b);
            else if (b.Type == MArray.name) return BObject((MArray)b);
            else if (b.Type == MPosition.name) return BObject((MPosition)b);

            return new List<TreeViewItem>();
		}
		public List<TreeViewItem> BObject(MCursor b)
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
		public List<TreeViewItem> BObject(MKey b)
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
		public List<TreeViewItem> BObject(MText b)
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

		public List<TreeViewItem> BObject(MScanColor b)
#pragma warning restore CS0246 // Nie można znaleźć nazwy typu lub przestrzeni nazw „MScanColor” (brak dyrektywy using lub odwołania do zestawu?)
		{
			Label label = new Label()
			{
				Content = "Data"
			};
			Button button = new Button()
			{
				Content = "Edit"
			};
			button.Click += (object sender, RoutedEventArgs e) =>
			{
				var window = new ScanColorWindow();
				window.ColorsAndPositions = b.ColorsAndPositions;
				window.ColorPositions = b.ColorPositions;
				window.Show();
				window.Closing += (object s, System.ComponentModel.CancelEventArgs ev) =>
				{
					b.ColorsAndPositions = window.ColorsAndPositions;
					b.ColorPositions = window.ColorPositions;
				};
			};

			List<TreeViewItem> treeViewItems = new List<TreeViewItem>()
			{
				ItemType(b),
				ItemName(b),
				TreeViewElement(ContentRow(label, button), null),
				ItemDescription(b),
				ItemCheck(b),
				ItemDelay(b)
			};

			return treeViewItems;
		}
		public List<TreeViewItem> BObject(MScanAreaColor b)
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
				ItemType(b),
				ItemName(b),
				TreeViewElement(ContentRow(label, button), null),
				ItemDescription(b),
				//ItemCheck((MScanAreaColor)b),
				ItemDelay(b)
			};

			return treeViewItems;
		}
		public List<TreeViewItem> BObject(MOpenBOT b)
		{
			Label patchLabel = new Label()
			{
				Content = "Path"
			};

			Grid patchPanel = new Grid()
			{
				HorizontalAlignment = HorizontalAlignment.Center,
			};
			patchPanel.ColumnDefinitions.Add(new ColumnDefinition() { });
			patchPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

			TextBlock patchContent = new TextBlock()
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left,
				TextTrimming = TextTrimming.CharacterEllipsis,
				Text = b.Source,
			};
			TextBlock fileNameLabel = new TextBlock()
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Left,
			};
			patchPanel.Children.Add(patchContent);
			patchPanel.Children.Add(fileNameLabel);
			Grid.SetColumn(fileNameLabel, 1);


			Label label = new Label()
			{
				Content = "Data"
			};

			Grid buttonGrid = new Grid()
			{
				//HorizontalAlignment = HorizontalAlignment.Center,
			};
			buttonGrid.ColumnDefinitions.Add(new ColumnDefinition() { });
			buttonGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40) });

			Button editBtn = new Button()
			{
				Content = "Edit"
			};
			editBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				if (MOpenBOT.SetPatch(b, Graph))
				{
					if (MOpenBOT.CheckSource(b, Graph))
						Console.AddMessage(b.Name + " set source complete.");

					fileNameLabel.Text = b.Source;
				}
			};

			Button reloadBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				FontSize = 20,
				Content = "⭯",
				Width = 30,
				Height = 30,
			};
			reloadBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				if (MOpenBOT.CheckSource(b, Graph))
					Console.AddMessage(b.Name + " reload complete.");
				
				fileNameLabel.Text = b.Source;
			};

			buttonGrid.Children.Add(editBtn);
			buttonGrid.Children.Add(reloadBtn);
			Grid.SetColumn(reloadBtn, 1);

			List<TreeViewItem> treeViewItems = new List<TreeViewItem>()
			{
				ItemType(b),
				ItemName(b),
				TreeViewElement(ContentRow(patchLabel, patchPanel), null),
				TreeViewElement(ContentRow(label, buttonGrid), null),
				ItemDescription(b),
				ItemTimesAuto(b),
				ItemDelay(b)
			};

			return treeViewItems;
		}
		public List<TreeViewItem> BObject(MIf b)
		{
			List<TreeViewItem> treeViewItems = new List<TreeViewItem>()
			{
				ItemType(b),
				ItemName(b),
				ItemBool(b),
				ItemDescription(b)
			};

			return treeViewItems;
		}

        public List<TreeViewItem> BObject(MCallToAPI b)
        {
            Label name = new Label() { Content = "Call To" };
            TextBox nameValue = new TextBox() { Text = b.IPAddress, VerticalContentAlignment = VerticalAlignment.Center };
            nameValue.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                b.IPAddress = nameValue.Text;
            };

            Label methodName = new Label() { Content = "Method" };
			ComboBox methodValue = new ComboBox();
			methodValue.Items.Add(MCallToAPI.APIMethod.GET);
			methodValue.Items.Add(MCallToAPI.APIMethod.POST);
			methodValue.SelectedValue = b.Method;


			methodValue.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
            {
				b.Method = (MCallToAPI.APIMethod)methodValue.SelectedValue;
            };

            List<TreeViewItem> treeViewItems = new List<TreeViewItem>()
            {
                ItemType(b),
                ItemName(b),
                TreeViewElement(ContentRow(name, nameValue), null),
                TreeViewElement(ContentRow(methodName, methodValue), null),
                TreeViewElement(ContentRow(new Label() { Content = "Input" }), null),
            };

            for (int i = 2; i < b.CPDatas.Count; i++)
			{
				var cp = b.CPDatas[i];
				if (cp.CIO == CIO.Input)
					treeViewItems.Add(CallToApiData(cp));
				else
				{
					treeViewItems.RemoveAt(treeViewItems.Count-1);	//remove last element
					break;
				}
            }
			treeViewItems.Add(TreeViewElement(ContentRow(new Label() { Content = "Output" }), null));

            for (int i = 2; i < b.CPDatas.Count; i++)
            {
                var cp = b.CPDatas[i];
                if (cp.CIO == CIO.Output)
                    treeViewItems.Add(CallToApiData(cp));

                if (i == b.CPDatas.Count -2)
                    break;
            }

            treeViewItems.Add(ItemDescription(b));

            return treeViewItems;
        }

        public List<TreeViewItem> BObject(MMultiplexer b)
        {
            Label methodName = new Label() { Content = "Method" };
            ComboBox methodValue = new ComboBox();
			methodValue.Items.Add(MMultiplexer.SelectType.First);
			methodValue.Items.Add(MMultiplexer.SelectType.Last);
			methodValue.Items.Add(MMultiplexer.SelectType.Random);
			methodValue.Items.Add(MMultiplexer.SelectType.Sequential);
			methodValue.Items.Add(MMultiplexer.SelectType.RandomSequential);
            methodValue.SelectedValue = b.SelectionElementsType;
			
            methodValue.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
            {
                b.SelectionElementsType = (MMultiplexer.SelectType)methodValue.SelectedValue;
            };

            List<TreeViewItem> cursor = new List<TreeViewItem>()
            {
                ItemType(b),
                ItemName(b),
                TreeViewElement(ContentRow(methodName, methodValue), null),
                ItemDescription(b),
                ItemTimesAuto(b),
                ItemDelay(b)
            };

            return cursor;
        }

        public List<TreeViewItem> BObject(MColorPositionSplitter b)
        {
            List<TreeViewItem> cursor = new List<TreeViewItem>()
            {
                ItemType(b),
                ItemName(b),
                ItemDescription(b),
                ItemTimes(b),
                ItemDelay(b)
            };

            return cursor;
        }


        public List<TreeViewItem> BObject(MArray b)
        {
            List<TreeViewItem> cursor = new List<TreeViewItem>()
            {
                ItemType(b),
                ItemName(b),
                ItemDescription(b),
                ItemTimes(b),
                ItemDelay(b)
            };

            return cursor;
        }

        public List<TreeViewItem> BObject(MPosition b)
		{
			Label label = new Label()
			{
				Content = "Data"
			};
			NumericUpDown value = new NumericUpDown() { FontSize = 14, MinValue = 0 };

			Label data = new Label() { Content = "Data" };
			Label dataValue = new Label() { Content = "[" + b.Pos.X + ":" + b.Pos.Y + "]", VerticalContentAlignment = VerticalAlignment.Center };

			Label xPosition = new Label()
			{
				Content = "X"
			};
			NumericUpDown xPositionValue = new NumericUpDown()
			{
				FontSize = 14,
				MaxValue = 99999,
				Value = (int)b.Pos.X
			};
			xPositionValue.textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Pos.X = xPositionValue.Value;
				dataValue.Content = "[" + b.Pos.X + ":" + b.Pos.Y + "]";
			};
			Label yPosition = new Label()
			{
				Content = "Y"
			};
			NumericUpDown yPositionValue = new NumericUpDown()
			{
				FontSize = 14,
				MaxValue = 99999,
				Value = (int)b.Pos.Y
			};
			yPositionValue.textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Pos.Y = yPositionValue.Value;
				dataValue.Content = "[" + b.Pos.X + ":" + b.Pos.Y + "]";
			};
			List<TreeViewItem> dataList = new List<TreeViewItem>()
			{
				TreeViewElement(ContentRow(xPosition, xPositionValue), null),
				TreeViewElement(ContentRow(yPosition, yPositionValue), null),
			};

			var treeData = TreeViewElement(ContentRow(data, dataValue), dataList);
			treeData.IsExpanded = true;

			List<TreeViewItem> treeViewItems = new List<TreeViewItem>()
			{
				ItemType(b),
				ItemName(b),
				treeData,
				ItemDescription(b),
			};

			return treeViewItems;
		}

		#endregion

		#region Controls
		public TreeViewItem ItemType(BotElement b)
		{
			Label type = new Label() { Content = "Type" };
			Label typeValue;
			if (b.Type == "OpenBot")
				typeValue = new Label() { Content = b.Type, VerticalContentAlignment = VerticalAlignment.Center };
			else
				typeValue = new Label() { Content = b.Type, VerticalContentAlignment = VerticalAlignment.Center };

			return TreeViewElement(ContentRow(type, typeValue), null);
		}
		public TreeViewItem ItemName(BotElement b)
		{
			Label name = new Label() { Content = "Name" };
			TextBox nameValue = new TextBox() { Text = b.Name, VerticalContentAlignment = VerticalAlignment.Center };
			nameValue.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Name = nameValue.Text;
				CurrentBlock.TitleLabel.Content = b.Name;
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
				Content = "[" + b.DelayAt.Hours + ":" + b.DelayAt.Minutes + ":" + b.DelayAt.Seconds + "." + b.DelayAt.Milliseconds + "]-[" +
				b.DelayTo.Hours + ":" + b.DelayTo.Minutes + ":" + b.DelayTo.Seconds + "." + b.DelayTo.Milliseconds + "]",
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
				Content = "[" + b.DelayAt.Hours + ":" + b.DelayAt.Minutes + ":" + b.DelayAt.Seconds + "." + b.DelayAt.Milliseconds + "]"
			};
			Label to = new Label()
			{
				Content = "To"
			};
			Label toValue = new Label()
			{
				Content = "[" + b.DelayTo.Hours + ":" + b.DelayTo.Minutes + ":" + b.DelayTo.Seconds + "." + b.DelayTo.Milliseconds + "]"
			};

			#region At To Objects
			Label atMilliseccond = new Label()
			{
				Content = "Milliseccond"
			};
			NumericUpDown atMilliseccondValue = new NumericUpDown()
			{
				MaxValue = 999,
				Value = b.DelayAt.Milliseconds
			};
			Label atSeccond = new Label()
			{
				Content = "Seccond"
			};
			NumericUpDown atSeccondValue = new NumericUpDown()
			{
				MaxValue = 59,
				Value = b.DelayAt.Seconds

			};
			Label atMinute = new Label()
			{
				Content = "Minute"
			};
			NumericUpDown atMinuteValue = new NumericUpDown()
			{
				MaxValue = 59,
				Value = b.DelayAt.Minutes
			};
			Label atHour = new Label()
			{
				Content = "Hour"
			};
			NumericUpDown atHourValue = new NumericUpDown()
			{
				MaxValue = 23,
				Value = b.DelayAt.Hours
			};

			void AtTextChanged(object sender, TextChangedEventArgs e)
			{
				b.DelayAt = new TimeSpan(0, atHourValue.Value, atMinuteValue.Value, atSeccondValue.Value, atMilliseccondValue.Value);
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
				Value = b.DelayTo.Milliseconds
			};
			Label toSeccond = new Label()
			{
				Content = "Seccond"
			};
			NumericUpDown toSeccondValue = new NumericUpDown()
			{
				MaxValue = 59,
				Value = b.DelayTo.Seconds
			};
			Label toMinute = new Label()
			{
				Content = "Minute"
			};
			NumericUpDown toMinuteValue = new NumericUpDown()
			{
				MaxValue = 59,
				Value = b.DelayTo.Minutes
			};
			Label toHour = new Label()
			{
				Content = "Hour"
			};
			NumericUpDown toHourValue = new NumericUpDown()
			{
				MaxValue = 23,
				Value = b.DelayTo.Hours
			};

			void ToTextChanged(object sender, TextChangedEventArgs e)
			{
				b.DelayTo = new TimeSpan(0, toHourValue.Value, toMinuteValue.Value, toSeccondValue.Value, toMilliseccondValue.Value);
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
					//at.Content = "Sleep";
					SetDelayValue();
				}
				b.IsDelay = (bool)randomDelayValue.IsChecked;
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
			b.Times = timesValue.Value;
			timesValue.textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Times = timesValue.Value;
			};

			return TreeViewElement(ContentRow(times, timesValue), null);
		}
		public TreeViewItem ItemTimesAuto(BotElement b)
		{
			Label times = new Label() { Content = "Times" };
			NumericUpDown timesValue = new NumericUpDown() { FontSize = 14, MinValue = 0, Value = b.Times };
			b.Times = timesValue.Value;
			if (timesValue.textBox.Text == "0")
				timesValue.textBox.Text = "∞";
			timesValue.textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Times = timesValue.Value;
			};
			timesValue.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				if (timesValue.textBox.Text == "0")
					timesValue.textBox.Text = "∞";
			};
			return TreeViewElement(ContentRow(times, timesValue), null);
		}

		public TreeViewItem ItemCheck(MScanColor b)
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
				Value = b.Times
			};
			timesValue.textBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				b.Times = timesValue.Value;
			};

			Label checkAllTimes = new Label()
			{
				Content = "Check all times"
			};
			CheckBox checkAllTimesValue = new CheckBox()
			{
				VerticalAlignment = VerticalAlignment.Center,
				IsChecked = b.CheckAllTimes
			};

			Label find = new Label()
			{
				Content = "Find"
			};
			ComboBox findValue = FindComboBox(b);

			findValue.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
			{
				changeCheckValue();
				checkValue.Content = timesValue.textBox.Text + ":" + ((ComboBoxItem)findValue.SelectedItem).Content;
				if (findValue.SelectedIndex == 1)
					b.Find = false;
				else
					b.Find = true;

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
				b.CheckAllTimes = (bool)checkAllTimesValue.IsChecked;
			};
			void changeCheckValue()
			{
				if (timesValue.Value == 0)
					if (checkAllTimesValue.IsChecked == true)
						timesValue.textBox.Text = "∞";
					else
						timesValue.textBox.Text = "Auto";
			}
			changeCheckValue();

			List<TreeViewItem> checkList = new List<TreeViewItem>()
			{
				TreeViewElement(ContentRow(times, timesValue), null),
				TreeViewElement(ContentRow(checkAllTimes, checkAllTimesValue), null),
				TreeViewElement(ContentRow(find, findValue), null),
				//ItemDelay(b)
			};

			return TreeViewElement(ContentRow(check, checkValue), checkList);
		}
		public TreeViewItem ItemBool(MIf b)
		{
			Label times = new Label() { Content = "Condition" };

			ComboBox comboBox = new ComboBox();
			comboBox.Items.Add("False");
			comboBox.Items.Add("True");

			comboBox.SelectedIndex = Convert.ToByte(b.Condition);

			comboBox.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
			{
				b.Condition = Convert.ToBoolean(comboBox.SelectedIndex);
			};

			return TreeViewElement(ContentRow(times, comboBox), null);
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
		public ComboBox FindComboBox(MScanColor b)
#pragma warning restore CS0246 // Nie można znaleźć nazwy typu lub przestrzeni nazw „MScanColor” (brak dyrektywy using lub odwołania do zestawu?)
		{
			ComboBox actionValue = new ComboBox() { VerticalContentAlignment = VerticalAlignment.Center };
			actionValue.Items.Add(new ComboBoxItem() { Content = "True" });
			actionValue.Items.Add(new ComboBoxItem() { Content = "False" });
			if (b.Find == true)
				actionValue.SelectedIndex = 0;
			else
				actionValue.SelectedIndex = 1;


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

				}, (int)ModifierKeys.None, (uint)System.Windows.Forms.Keys.F1),
			};
			hotKeys = new HotKeys(window, keys);

			List<TreeViewItem> dataList = new List<TreeViewItem>()
			{
				TreeViewElement(ContentRow(xPosition, xPositionValue), null),
				TreeViewElement(ContentRow(yPosition, yPositionValue), null),
			};

			return TreeViewElement(ContentRow(data, dataValue), dataList);
		}


        public TreeViewItem CallToApiData(CPData c)
        {
            Label name = new Label()
            {
                Content = c.CType.ToString() + c.Id
            };
			TextBox value = new TextBox()
			{
				Text = c.Title
            };
            value.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                c.Title = value.Text;
            };

			return TreeViewElement(ContentRow(name, value), null);
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
			((Grid)treeViewItem.Header).Width = treeViewSize - (19 * deep + 25); //błąd czasami
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
