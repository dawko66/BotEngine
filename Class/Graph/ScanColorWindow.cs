using AutoBot_v1._1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AutoBot_2._0.Class.Graph
{
	class ScanColorWindow : Window
	{
		//usunąć new
		public List<ColorAndPosition> ColorsAndPositions = new List<ColorAndPosition>();
		public List<ColorPosition> ColorPositions = new List<ColorPosition>();

		private Grid Container;
		private Grid Panel;
		private Image Image;
		private Rectangle PixelSquare;

		private HotKeys hotKeys;

		private System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(100, 100);
		private double ZoomValue = 8;
		private Point CenterImagePosition;
		private Point MoveVector;

		private bool IsMouseDown;
		private bool IsMouseClickH; //helper to IsMouseClick
		private Point LastMousePosition;
		private Point DownMousePosition;

		StackPanel ColorPanel;
		Label PositionLabel;
		Label RedLabel;
		Label GreenLabel;
		Label BlueLabel;

		CheckBox ActivateAll;
		CheckBox CAPActivateAllColors;
		CheckBox CAPActivateAllPositions;

		ListBox CPListBox;
		ListBox CAndPListBox;

		private int X;
		private int Y;
		private (byte Red, byte Green, byte Blue) ColorValue;
		DispatcherTimer Timer;

		private Label console;

		public ScanColorWindow()
		{
			RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
			Container = new Grid()
			{
				Background = Brushes.Red
			};
			Content = Container;

			Panel = new Grid()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Right,
				Width = 300,
				Height = 500,
			};
			Container.Children.Add(Panel);

			Image = new Image()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
			};
			Container.Children.Add(Image);

			Image.MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				IsMouseDown = true;
				IsMouseClickH = true;
				LastMousePosition = e.GetPosition(this);
				DownMousePosition = e.GetPosition(this);
			};
			Container.MouseUp += (object sender, MouseButtonEventArgs e) =>
			{
				if (IsMouseClickH)
				{
					if (e.ChangedButton == MouseButton.Left)
						CaptureColorPoint();
					else if (e.ChangedButton == MouseButton.Right)
						CaptureColorAndPoint();
				}

				IsMouseDown = false;
				Container.Cursor = Cursors.Arrow;
			};
			Container.MouseMove += (object sender, MouseEventArgs e) =>
			{
				IsMouseClickH = false;
				ZoomMargin(e.GetPosition(Image));
				if (IsMouseDown)
				{
					Container.Cursor = Cursors.SizeAll;
					MoveVector += LastMousePosition - e.GetPosition(this);
					PosImg(bmp, CenterImagePosition, MoveVector);
					LastMousePosition = e.GetPosition(this);
					UpdateLayout();
				}

				#region PixelSquare
				var cursor = e.GetPosition(Image);
				Point pixPos = new Point(cursor.X / ZoomValue, cursor.Y / ZoomValue);

				PixelSquare.Margin = new Thickness(
					(int)pixPos.X * ZoomValue - PixelSquare.StrokeThickness + Image.Margin.Left,
					(int)pixPos.Y * ZoomValue - PixelSquare.StrokeThickness + Image.Margin.Top,
					0,
					0);
				#endregion

				#region PixelSquare position
				Timer.Stop();

				Point movePixels = new Point(MoveVector.X / ZoomValue, MoveVector.Y / ZoomValue);
				var pixelsCount = new Point(Container.ActualWidth / ZoomValue, Container.ActualHeight / ZoomValue);
				var halfPixelCount = new Point(Math.Ceiling(pixelsCount.X / 2) + 1, Math.Ceiling(pixelsCount.Y / 2) + 1);

				var curPixPos = new Point(
					CenterImagePosition.X - halfPixelCount.X + (int)(movePixels.X) + (int)pixPos.X,
					CenterImagePosition.Y - halfPixelCount.Y + (int)(movePixels.Y) + (int)pixPos.Y);

				Color color = MScanColor.GetPixelColorFromBitmap(bmp, new Point(curPixPos.X, curPixPos.Y));
				CursorColorPosition(curPixPos, color);
				#endregion
			};
			Image.MouseWheel += (object sender, MouseWheelEventArgs e) =>
			{
				if (e.Delta > 0)
				{
					if (ZoomValue < 16)
					{
						ZoomMargin(e.GetPosition(this));

						ZoomValue++;
					}
				}
				else if (ZoomValue > 1)
				{
					ZoomMargin(e.GetPosition(this));
					ZoomValue--;

				}
				PosImg(bmp, CenterImagePosition, MoveVector);

			};
			Container.MouseLeave += (object sender, MouseEventArgs e) =>
			{
				Timer.Start();
				var p = e.GetPosition(this);
				IsMouseDown = false;
				Container.Cursor = Cursors.Arrow;
			};
			SizeChanged += (object sender, SizeChangedEventArgs e) =>
			{
				PosImg(bmp, CenterImagePosition, MoveVector);
			};

			console = new Label()
			{
				Background = Brushes.White,
				Height = 25,
				VerticalAlignment = VerticalAlignment.Top,
			};
			Container.Children.Add(console);

			PixelSquare = new Rectangle()
			{
				//Fill = Brushes.Transparent,
				IsHitTestVisible = false,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				StrokeThickness = 2,
				Stroke = Brushes.Red,
			};
			Container.Children.Add(PixelSquare);
			ZoomMargin(CenterImagePosition);

			CreatePanelControl();

            Loaded += (object sender, RoutedEventArgs e) =>
			{
				LoadData();
			};
		}

        private void CreatePanelControl()
		{
			Grid grid = new Grid()
			{
				Background = Brushes.White,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Right,
				Width = 350
			};
			Container.Children.Add(grid);

			grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions.Add(new RowDefinition());

			#region Color panel and text
			ColorPanel = new StackPanel()
			{
				Background = Brushes.Black,
				VerticalAlignment = VerticalAlignment.Top,
				Height = 100
			};
			grid.Children.Add(ColorPanel);

			StackPanel ColorLabelPanel = new StackPanel()
			{
				Orientation = Orientation.Horizontal,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Center
			};
			PositionLabel = new Label()
			{
				FontWeight = FontWeights.Bold,
				FontSize = 28,
				VerticalContentAlignment = VerticalAlignment.Center,
				Padding = new Thickness(0, 5, 0, 5),
				Content = "0 : 0 #"
			};

			RedLabel = new Label()
			{
				FontWeight = FontWeights.Bold,
				FontSize = 28,
				VerticalContentAlignment = VerticalAlignment.Center,
				Padding = new Thickness(0, 5, 0, 5),
				Content = "00"
			};
			GreenLabel = new Label()
			{
				FontWeight = FontWeights.Bold,
				FontSize = 28,
				VerticalContentAlignment = VerticalAlignment.Center,
				Padding = new Thickness(0, 5, 0, 5),
				Content = "00"
			};
			BlueLabel = new Label()
			{
				FontWeight = FontWeights.Bold,
				FontSize = 28,
				VerticalContentAlignment = VerticalAlignment.Center,
				Padding = new Thickness(0, 5, 0, 5),
				Content = "00"
			};

			ColorLabelPanel.Children.Add(PositionLabel);
			ColorLabelPanel.Children.Add(RedLabel);
			ColorLabelPanel.Children.Add(GreenLabel);
			ColorLabelPanel.Children.Add(BlueLabel);

			Grid.SetRow(ColorLabelPanel, 1);
			grid.Children.Add(ColorLabelPanel);

			Timer = new DispatcherTimer();
			Timer.Tick += (object sender, EventArgs e) =>
			{
				Point position = MCursor.GetMousePosition();
				Color color = MScanColor.GetPixelColorFromScreen(new Point(position.X, position.Y));
				CursorColorPosition(position, color);
			};
			Timer.Interval = TimeSpan.Zero;
			Timer.Start();
			#endregion

			#region ColorPosition List
			StackPanel ColorPositionPanel = new StackPanel()
			{
				Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
			};
			Grid.SetRow(ColorPositionPanel, 2);
			grid.Children.Add(ColorPositionPanel);

			Grid CPTittle = new Grid()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Center,
			};

			CPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
			CPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(15) });
			CPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
			CPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
			CPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
			CPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
			CPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });

			ColorPositionPanel.Children.Add(CPTittle);

			Label idLabel = new Label() { Content = "ID", HorizontalAlignment = HorizontalAlignment.Center };
			CPTittle.Children.Add(idLabel);
			Grid.SetColumn(idLabel, 0);

			ActivateAll = new CheckBox()
			{
				VerticalContentAlignment = VerticalAlignment.Center,
				IsChecked = true,
			};
			CPTittle.Children.Add(ActivateAll);
			Grid.SetColumn(ActivateAll, 1);

			Label positionLabel = new Label() { Content = "Position", HorizontalAlignment = HorizontalAlignment.Center };
			CPTittle.Children.Add(positionLabel);
			Grid.SetColumn(positionLabel, 2);
			Grid.SetColumnSpan(positionLabel, 2);

			Label colorLabel = new Label() { Content = "Color", HorizontalAlignment = HorizontalAlignment.Center };
			CPTittle.Children.Add(colorLabel);
			Grid.SetColumn(colorLabel, 4);

			Button moveAllDownBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 20,
				Height = 20,
				Content = "🠗",
				Background = new SolidColorBrush(Color.FromRgb(254, 237, 138)),
			};
			CPTittle.Children.Add(moveAllDownBtn);
			Grid.SetColumn(moveAllDownBtn, 5);

			moveAllDownBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				foreach (var item in CPListBox.Items)
				{
					CAndPListBox.Items.Add(CAPBoxItem(
						((NumericUpDown)((Grid)item).Children[2]).Value,
						((NumericUpDown)((Grid)item).Children[3]).Value,
						((ColorPicker)((Grid)item).Children[4]).ColorValue,
						 (bool)CAPActivateAllPositions.IsChecked,
						 (bool)CAPActivateAllColors.IsChecked
					));
				}
				CPListBox.Items.Clear();
			};

			Button removeAllBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 20,
				Height = 20,
				Content = "🗑",
				Background = new SolidColorBrush(Color.FromRgb(247, 69, 98)),
			};
			CPTittle.Children.Add(removeAllBtn);
			Grid.SetColumn(removeAllBtn, 6);

			removeAllBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				CPListBox.Items.Clear();
			};

			CPListBox = new ListBox()
			{
				Height = 100,
				HorizontalContentAlignment = HorizontalAlignment.Center,
			};
			ColorPositionPanel.Children.Add(CPListBox);
			#endregion

			#region ColorPosition List

			StackPanel ColorAndPositionPanel = new StackPanel()
			{
				Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
			};
			Grid.SetRow(ColorAndPositionPanel, 3);
			grid.Children.Add(ColorAndPositionPanel);

			Grid CAPTittle = new Grid()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Center,
			};

			CAPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
			CAPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(15) });
			CAPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
			CAPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
			CAPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
			CAPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
			CAPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
			CAPTittle.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
			
			ColorAndPositionPanel.Children.Add(CAPTittle);

			Label CAPIdLabel = new Label() { Content = "ID", HorizontalAlignment = HorizontalAlignment.Center };
			CAPTittle.Children.Add(CAPIdLabel);
			Grid.SetColumn(CAPIdLabel, 0);

			CAPActivateAllPositions = new CheckBox()
			{
				VerticalContentAlignment = VerticalAlignment.Center,
				IsChecked = true,
			};
			CAPTittle.Children.Add(CAPActivateAllPositions);
			Grid.SetColumn(CAPActivateAllPositions, 1);

			Label CAPositionLabel = new Label() { Content = "Position", HorizontalAlignment = HorizontalAlignment.Center };
			CAPTittle.Children.Add(CAPositionLabel);
			Grid.SetColumn(CAPositionLabel, 2);
			Grid.SetColumnSpan(CAPositionLabel, 2);

			CAPActivateAllColors = new CheckBox()
			{
				VerticalContentAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				IsChecked = true,
			};
			CAPTittle.Children.Add(CAPActivateAllColors);
			Grid.SetColumn(CAPActivateAllColors, 4);

			Label CAPColorLabel = new Label() { Content = "Color", HorizontalAlignment = HorizontalAlignment.Center };
			CAPTittle.Children.Add(CAPColorLabel);
			Grid.SetColumn(CAPColorLabel, 5);

			Button moveAllUpBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 20,
				Height = 20,
				Content = "🠕",
				Background = new SolidColorBrush(Color.FromRgb(254, 237, 138)),
			};
			CAPTittle.Children.Add(moveAllUpBtn);
			Grid.SetColumn(moveAllUpBtn, 6);

			moveAllUpBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				foreach (var item in CAndPListBox.Items)
				{
					CPListBox.Items.Add(CPBoxItem(
						((NumericUpDown)((Grid)item).Children[2]).Value,
						((NumericUpDown)((Grid)item).Children[3]).Value,
						((ColorPicker)((Grid)item).Children[5]).ColorValue,
						(bool)((CheckBox)((Grid)item).Children[1]).IsChecked
					));
				}
				CAndPListBox.Items.Clear();
			};

			Button CAPRemoveAllBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 20,
				Height = 20,
				Content = "🗑",
				Background = new SolidColorBrush(Color.FromRgb(247, 69, 98)),
			};
			CAPTittle.Children.Add(CAPRemoveAllBtn);
			Grid.SetColumn(CAPRemoveAllBtn, 7);

			CAPRemoveAllBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				CAndPListBox.Items.Clear();
			};

			CAndPListBox = new ListBox()
			{
				Height = 100,
				HorizontalContentAlignment = HorizontalAlignment.Center,
			};
			ColorAndPositionPanel.Children.Add(CAndPListBox);

			Line line = new Line()
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				X1 = 0,
				Y1 = 0,
				X2 = 0,
				Y2 = 20,
				Stroke = Brushes.Red,
				Margin = new Thickness(1, 0, 0, 0),
			};
			ColorAndPositionPanel.Children.Add(line);
			#endregion

			#region Buttons 
			StackPanel ButtonPanel = new StackPanel()
			{
				Orientation = Orientation.Horizontal,
				VerticalAlignment = VerticalAlignment.Top
			};
			grid.Children.Add(ButtonPanel);
			Grid.SetRow(ButtonPanel, 4);

			Button saveBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 80,
				Height = 30,
				Content = "Save 💾",
				Background = new SolidColorBrush(Color.FromRgb(69, 207, 68)),
			};
			ButtonPanel.Children.Add(saveBtn);
			saveBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				SaveData();
				this.Close();
			};

			Button loadBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				IsEnabled = false,
				Width = 80,
				Height = 30,
				Content = "Load BMP 🗁",
				Background = new SolidColorBrush(Color.FromRgb(64, 137, 228)),
			};
			ButtonPanel.Children.Add(loadBtn);

			Button cancelBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 80,
				Height = 30,
				Content = "Cancel",
				Background = new SolidColorBrush(Color.FromRgb(224, 207, 58)),
			};
			ButtonPanel.Children.Add(cancelBtn);
			cancelBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				this.Close();
			};
			#endregion
		}

        private void CursorColorPosition(Point position, Color color)
        {
			PositionLabel.Content = position.X + " : " + position.Y + " #";

			ColorPanel.Background = new SolidColorBrush(color);

			byte redColor = color.R;
			RedLabel.Content = redColor.ToString("X2");
			RedLabel.Foreground = new SolidColorBrush(Color.FromRgb(redColor, 0, 0));

			byte greenColor = color.G;
			GreenLabel.Content = greenColor.ToString("X2");
			GreenLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, greenColor, 0));

			byte blueColor = color.B;
			BlueLabel.Content = blueColor.ToString("X2");
			BlueLabel.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, blueColor));

			X = (int)position.X;
			Y = (int)position.Y;

			var brush = (SolidColorBrush)ColorPanel.Background;
			ColorValue = (
				brush.Color.R,
				brush.Color.G,
				brush.Color.B
			);
		}

        public Grid CPBoxItem(int X, int Y, (byte Red, byte Green, byte Blue) Colorr, bool IsActive)
		{
			Grid grid = new Grid()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Center,
			};

			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(15) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });

			Label idLabel = new Label()
			{
				HorizontalContentAlignment = HorizontalAlignment.Left,
				HorizontalAlignment = HorizontalAlignment.Left,
				Content = "100",
				Margin = new Thickness(0),
			};
			grid.Children.Add(idLabel);
			Grid.SetColumn(idLabel, 0);

			CheckBox IsActiveCheck = new CheckBox()
			{
				VerticalContentAlignment = VerticalAlignment.Center,
				IsChecked = IsActive,
			};
			grid.Children.Add(IsActiveCheck);
			Grid.SetColumn(IsActiveCheck, 1);

			NumericUpDown positionX = new NumericUpDown()
			{
				Margin = new Thickness(3, 0, 0, 0),
				MaxValue = 9999,
				Value = X,
			};
			grid.Children.Add(positionX);
			Grid.SetColumn(positionX, 2);

			NumericUpDown positionY = new NumericUpDown()
			{
				Margin = new Thickness(3, 0, 0, 0),
				MaxValue = 9999,
				Value = Y,
			};
			grid.Children.Add(positionY);
			Grid.SetColumn(positionY, 3);

			ColorPicker colorPicker = new ColorPicker()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				ColorValue = Colorr
			};
			grid.Children.Add(colorPicker);
			Grid.SetColumn(colorPicker, 4);

			Button moveDownBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 20,
				Height = 20,
				Content = "🠗",
				Background = new SolidColorBrush(Color.FromRgb(254, 237, 138)),
			};
			grid.Children.Add(moveDownBtn);
			Grid.SetColumn(moveDownBtn, 5);

			moveDownBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				CAndPListBox.Items.Add(CAPBoxItem(positionX.Value, positionY.Value, colorPicker.ColorValue, (bool)CAPActivateAllPositions.IsChecked, (bool)CAPActivateAllColors.IsChecked));
				CPListBox.Items.Remove(grid);
			};

			Button deleteBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 20,
				Height = 20,
				Content = "🗑",
				Background = new SolidColorBrush(Color.FromRgb(247, 69, 98)),
			};
			grid.Children.Add(deleteBtn);
			Grid.SetColumn(deleteBtn, 6);

			deleteBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				CPListBox.Items.Remove(grid);
			};

			return grid;
		}
		public Grid CAPBoxItem(int X, int Y, (byte Red, byte Green, byte Blue) Colorr, bool IsPositionActive, bool IsColorActive)
		{
			Grid grid = new Grid()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Center,
			};

			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(15) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(60) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });

			Label idLabel = new Label()
			{
				HorizontalContentAlignment = HorizontalAlignment.Left,
				HorizontalAlignment = HorizontalAlignment.Left,
				Content = "100",
				Margin = new Thickness(0),
			};
			grid.Children.Add(idLabel);
			Grid.SetColumn(idLabel, 0);

			CheckBox IsPositionActiveCheck = new CheckBox()
			{
				VerticalContentAlignment = VerticalAlignment.Center,
				IsChecked = IsPositionActive,
			};
			grid.Children.Add(IsPositionActiveCheck);
			Grid.SetColumn(IsPositionActiveCheck, 1);

			NumericUpDown positionX = new NumericUpDown()
			{
				Margin = new Thickness(3, 0, 0, 0),
				MaxValue = 9999,
				Value = X,
			};
			grid.Children.Add(positionX);
			Grid.SetColumn(positionX, 2);

			NumericUpDown positionY = new NumericUpDown()
			{
				Margin = new Thickness(3, 0, 0, 0),
				MaxValue = 9999,
				Value = Y,
			};
			grid.Children.Add(positionY);
			Grid.SetColumn(positionY, 3);

			CheckBox IsColorActiveCheck = new CheckBox()
			{
				VerticalContentAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				IsChecked = IsColorActive,
			};
			grid.Children.Add(IsColorActiveCheck);
			Grid.SetColumn(IsColorActiveCheck, 4);

			ColorPicker colorPicker = new ColorPicker()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				ColorValue = Colorr
			};
			grid.Children.Add(colorPicker);
			Grid.SetColumn(colorPicker, 5);

			Button moveUpBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 20,
				Height = 20,
				Content = "🠕",
				Background = new SolidColorBrush(Color.FromRgb(254, 237, 138)),
			};
			grid.Children.Add(moveUpBtn);
			Grid.SetColumn(moveUpBtn, 6);

			moveUpBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				CPListBox.Items.Add(CPBoxItem(positionX.Value, positionY.Value, colorPicker.ColorValue, (bool)ActivateAll.IsChecked));
				CAndPListBox.Items.Remove(grid);
			};

			Button deleteBtn = new Button()
			{
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				Width = 20,
				Height = 20,
				Content = "🗑",
				Background = new SolidColorBrush(Color.FromRgb(247, 69, 98)),
			};
			grid.Children.Add(deleteBtn);
			Grid.SetColumn(deleteBtn, 7);

			deleteBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				CAndPListBox.Items.Remove(grid);
			};

			return grid;
		}

		public void LoadData()
		{
			foreach (var cp in ColorsAndPositions)
				CAndPListBox.Items.Add(CAPBoxItem(
					(int)cp.Position.X,
					(int)cp.Position.Y, 
					(cp.Color.R, cp.Color.G, cp.Color.B),
					cp.IsPositionActive,
					cp.IsColorActive
				));

			foreach (var cp in ColorPositions)
			{
				CPListBox.Items.Add(CPBoxItem(
					(int)cp.Position.X, (int)cp.Position.Y, (cp.Color.R, cp.Color.G, cp.Color.B), cp.IsActive
				));
			}
		}
		public void SaveData()
		{
			ColorsAndPositions.Clear();
			ColorPositions.Clear();

			foreach (var item in CAndPListBox.Items)
			{
				var color = ((ColorPicker)((Grid)item).Children[5]).ColorValue;
				
				ColorsAndPositions.Add(new ColorAndPosition()
				{
					Position = new Point(
						((NumericUpDown)((Grid)item).Children[2]).Value,
						((NumericUpDown)((Grid)item).Children[3]).Value
					),
					Color = Color.FromRgb(color.Red, color.Green, color.Blue),
					IsPositionActive = (bool)((CheckBox)((Grid)item).Children[1]).IsChecked,
					IsColorActive = (bool)((CheckBox)((Grid)item).Children[4]).IsChecked
				});
			}

			foreach (var item in CPListBox.Items)
			{
				var color = ((ColorPicker)((Grid)item).Children[4]).ColorValue;

				ColorPositions.Add(new ColorPosition()
				{
					Position = new Point(
						((NumericUpDown)((Grid)item).Children[2]).Value,
						((NumericUpDown)((Grid)item).Children[3]).Value
					),
					Color = Color.FromRgb(color.Red, color.Green, color.Blue),
					IsActive = (bool)((CheckBox)((Grid)item).Children[1]).IsChecked
				});
			}
		}

		private void CreateScreenShot()
		{
			bmp = MScanColor.MakeScreenShot();
			CenterImagePosition = MCursor.GetMousePosition();
			MoveVector = new Point();
			PosImg(bmp, CenterImagePosition, MoveVector);
		}
		private void CaptureColorPoint()
		{
			CPListBox.Items.Add(CPBoxItem(X, Y, ColorValue, (bool)ActivateAll.IsChecked));
		}
		private void CaptureColorAndPoint()
		{
			CAndPListBox.Items.Add(CAPBoxItem(X, Y, ColorValue, (bool)CAPActivateAllPositions.IsChecked, (bool)CAPActivateAllColors.IsChecked));
		}

		private void PosImg(System.Drawing.Bitmap bmp, Point curPos, Point move)
		{
			Point m = new Point(move.X / ZoomValue, move.Y / ZoomValue);
			var pixelsCount = new Point(Container.ActualWidth / ZoomValue, Container.ActualHeight / ZoomValue);
			var halfPixelCount = new Point(Math.Ceiling(pixelsCount.X / 2) + 1, Math.Ceiling(pixelsCount.Y / 2) + 1);
			var b = MScanColor.GetImagePart(
				bmp, 
				new Point(curPos.X - halfPixelCount.X + (int)(m.X), curPos.Y - halfPixelCount.Y + (int)(m.Y)),
				new Point(curPos.X + halfPixelCount.X + (int)(m.X), curPos.Y + halfPixelCount.Y + (int)(m.Y))
				);
			Image.Source = Imaging.CreateBitmapSourceFromHBitmap(b.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			Image.Width = Image.Source.Width * ZoomValue;
			Image.Height = Image.Source.Height * ZoomValue;

			Image.Margin = new Thickness(
				(int)((Container.ActualWidth - Image.Width) / 2) - (m.X - (int)(m.X)) * ZoomValue,
				(int)((Container.ActualHeight - Image.Height) / 2) - (m.Y - (int)(m.Y)) * ZoomValue,
				0, 0);

			GC.Collect();
		}

		private void ZoomMargin(Point cursor)
		{
			PixelSquare.Height = ZoomValue + PixelSquare.StrokeThickness * 2;
			PixelSquare.Width = ZoomValue + PixelSquare.StrokeThickness * 2;

			Point pixPos = new Point(cursor.X / ZoomValue, cursor.Y / ZoomValue);

			Point movePixels = new Point(MoveVector.X / ZoomValue, MoveVector.Y / ZoomValue);

			var pixelsCount = new Point(Container.ActualWidth / ZoomValue, Container.ActualHeight / ZoomValue);
			var halfPixelCount = new Point(Math.Ceiling(pixelsCount.X / 2) + 1, Math.Ceiling(pixelsCount.Y / 2) + 1);

			var curPixPos = new Point(
				CenterImagePosition.X - halfPixelCount.X + (int)(movePixels.X) + (int)pixPos.X,
				CenterImagePosition.Y - halfPixelCount.Y + (int)(movePixels.Y) + (int)pixPos.Y);

			Point v = new Point(
				(int)pixPos.X * ZoomValue - PixelSquare.StrokeThickness + Image.Margin.Left,
				(int)pixPos.Y * ZoomValue - PixelSquare.StrokeThickness + Image.Margin.Top);

			console.Content = v.X + " : " + curPixPos.X;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			HotKeys.HotKey[] keys = new HotKeys.HotKey[]
			{
				new HotKeys.HotKey(CreateScreenShot, (uint)ModifierKeys.None, (uint)System.Windows.Forms.Keys.F1),
				new HotKeys.HotKey(CaptureColorPoint, (uint)ModifierKeys.None, (uint)System.Windows.Forms.Keys.F2),
				new HotKeys.HotKey(CaptureColorAndPoint, (uint)ModifierKeys.None, (uint)System.Windows.Forms.Keys.F3)
			};

			hotKeys = new HotKeys(this, keys);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Timer.Stop();
			hotKeys.stop();
		}
	}
}
