using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AutoBot_2._0.Class.Graph
{
	class ColorPicker : StackPanel
	{
		public byte Red
		{
			get
			{
				return Convert.ToByte(RedLabel.Content.ToString(), 16);
			}
			set
			{
				RedLabel.Content = value.ToString("X2");
				ColorBox.Background = new SolidColorBrush(Color.FromRgb(value, Green, Blue));
				ColorTextBox.Text = RedLabel.Content.ToString() + GreenLabel.Content.ToString() + BlueLabel.Content.ToString();
			}
		}
		public byte Green
		{
			get
			{
				return Convert.ToByte(GreenLabel.Content.ToString(), 16);
			}
			set
			{
				GreenLabel.Content = value.ToString("X2");
				ColorBox.Background = new SolidColorBrush(Color.FromRgb(Red, value, Blue));
				ColorTextBox.Text = RedLabel.Content.ToString() + GreenLabel.Content.ToString() + BlueLabel.Content.ToString();
			}
		}
		public byte Blue
		{
			get
			{
				return Convert.ToByte(BlueLabel.Content.ToString(), 16);
			}
			set
			{
				BlueLabel.Content = value.ToString("X2");
				ColorBox.Background = new SolidColorBrush(Color.FromRgb(Red, Green, value));
				ColorTextBox.Text = RedLabel.Content.ToString() + GreenLabel.Content.ToString() + BlueLabel.Content.ToString();
			}
		}

		public (byte Red, byte Green, byte Blue) ColorValue
		{
			get
			{
				return (
					Convert.ToByte(RedLabel.Content.ToString(), 16),
					Convert.ToByte(GreenLabel.Content.ToString(), 16),
					Convert.ToByte(BlueLabel.Content.ToString(), 16)
				);
			}
			set
			{
				RedLabel.Content = value.Red.ToString("X2");
				GreenLabel.Content = value.Green.ToString("X2");
				BlueLabel.Content = value.Blue.ToString("X2");

				ColorBox.Background = new SolidColorBrush(Color.FromRgb(value.Red, value.Green, value.Blue));
				ColorTextBox.Text = RedLabel.Content.ToString() + GreenLabel.Content.ToString() + BlueLabel.Content.ToString();
			}
		}

		private Border ColorBox;
		private TextBox ColorTextBox;
		private Label RedLabel;
		private Label GreenLabel;
		private Label BlueLabel;

		public ColorPicker()
		{
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;
			Orientation = Orientation.Horizontal;
			Height = 20;

			#region TextBox
			Grid TextBoxPanel = new Grid();
			Children.Add(TextBoxPanel);

			ColorTextBox = new TextBox()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,
				Foreground = Brushes.Transparent,
				Height = 20,
				MinWidth = 57,
				FontSize = 12,
				Text = "000000"
			};
			TextBoxPanel.Children.Add(ColorTextBox);

			#region Label
			StackPanel LabelPanel = new StackPanel()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				Margin = new Thickness(3, 1, 1, 1),
				Orientation = Orientation.Horizontal,
			};

			TextBoxPanel.Children.Add(LabelPanel);

			RedLabel = new Label()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,
				Foreground = Brushes.Red,
				Margin = new Thickness(0),
				Padding = new Thickness(0),
				IsHitTestVisible = false,
				FontSize = 12,
				Content = "00"
			};
			LabelPanel.Children.Add(RedLabel);

			GreenLabel = new Label()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,
				Foreground = Brushes.Green,
				Margin = new Thickness(0),
				Padding = new Thickness(0),
				IsHitTestVisible = false,
				FontSize = 12,
				Content = "00"
			};
			LabelPanel.Children.Add(GreenLabel);

			BlueLabel = new Label()
			{
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,
				Foreground = Brushes.Blue,
				Margin = new Thickness(0),
				Padding = new Thickness(0),
				IsHitTestVisible = false,
				FontSize = 12,
				Content = "00"

			};
			LabelPanel.Children.Add(BlueLabel);
			#endregion
			#endregion

			ColorBox = new Border()
			{
				//HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Center,
				Margin = new Thickness(3, 0, 0, 0),
				BorderThickness = new Thickness(1),
				BorderBrush = Brushes.Black,
				Width = 15,
				Height = 15,
				Background = new SolidColorBrush(Color.FromRgb(0, 0, 0))
			};
			Children.Add(ColorBox);

			ColorBox.MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
				cd.Color = System.Drawing.Color.FromArgb(
					RedLabel.Content.ToString() != "" ? Convert.ToByte(RedLabel.Content.ToString().Length == 1 ? RedLabel.Content.ToString() + "0" : RedLabel.Content.ToString(), 16) : (byte)0,
					GreenLabel.Content.ToString() != "" ? Convert.ToByte(GreenLabel.Content.ToString().Length == 1 ? GreenLabel.Content.ToString() + "0" : GreenLabel.Content.ToString(), 16) : (byte)0,
					BlueLabel.Content.ToString() != "" ? Convert.ToByte(BlueLabel.Content.ToString().Length == 1 ? BlueLabel.Content.ToString() + "0" : BlueLabel.Content.ToString(), 16) : (byte)0

					);

				if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					ColorBox.Background = new SolidColorBrush(Color.FromRgb(cd.Color.R, cd.Color.G, cd.Color.B));

					RedLabel.Content = cd.Color.R.ToString("X2");
					GreenLabel.Content = cd.Color.G.ToString("X2");
					BlueLabel.Content = cd.Color.B.ToString("X2");

					ColorTextBox.Text = RedLabel.Content.ToString() + GreenLabel.Content.ToString() + BlueLabel.Content.ToString();
				}
			};

			ColorTextBox.TextChanged += (object sender, TextChangedEventArgs e) =>
			{
				var text = ColorTextBox.Text;

				BlueLabel.Content = text.Substring(text.Length - (text.Length / 3));
				text = text.Remove(text.Length - BlueLabel.Content.ToString().Length, BlueLabel.Content.ToString().Length);
				GreenLabel.Content = text.Substring(text.Length - (text.Length / 2));
				text = text.Remove(text.Length - GreenLabel.Content.ToString().Length, GreenLabel.Content.ToString().Length);
				RedLabel.Content = text.Substring(text.Length - (text.Length / 1));
				text = text.Remove(text.Length - RedLabel.Content.ToString().Length, RedLabel.Content.ToString().Length);

				byte r = RedLabel.Content.ToString() != "" ? Convert.ToByte(RedLabel.Content.ToString().Length == 1 ? RedLabel.Content.ToString() + RedLabel.Content.ToString() : RedLabel.Content.ToString(), 16) : (byte)0;
				byte g = GreenLabel.Content.ToString() != "" ? Convert.ToByte(GreenLabel.Content.ToString().Length == 1 ? GreenLabel.Content.ToString() + GreenLabel.Content.ToString() : GreenLabel.Content.ToString(), 16) : (byte)0;
				byte b = BlueLabel.Content.ToString() != "" ? Convert.ToByte(BlueLabel.Content.ToString().Length == 1 ? BlueLabel.Content.ToString() + BlueLabel.Content.ToString() : BlueLabel.Content.ToString(), 16) : (byte)0;

				ColorBox.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
			};

			ColorTextBox.PreviewKeyDown += (object sender, KeyEventArgs e) =>
			{
				if (!((new Regex("[0-9a-fA-F]")).IsMatch(e.Key.ToString()) ||
				e.Key == Key.Left ||
				e.Key == Key.Right ||
				e.Key == Key.Back ||
				e.Key == Key.Delete ||
				e.Key == Key.Tab) ||
				e.Key == Key.Space)
					e.Handled = true;

				if (((TextBox)sender).Text.Length >= 6 &&
				!(e.Key == Key.Left ||
				e.Key == Key.Right ||
				e.Key == Key.Back ||
				e.Key == Key.Delete ||
				e.Key == Key.Tab))
					e.Handled = true;
			};

			ColorTextBox.LostKeyboardFocus += (object sender, KeyboardFocusChangedEventArgs e) =>
			{
				if (RedLabel.Content.ToString().Length == 1) RedLabel.Content += "0";
				else if (RedLabel.Content.ToString().Length == 0) RedLabel.Content = "00";

				if (GreenLabel.Content.ToString().Length == 1) GreenLabel.Content += "0";
				else if (GreenLabel.Content.ToString().Length == 0) GreenLabel.Content = "00";

				if (BlueLabel.Content.ToString().Length == 1) BlueLabel.Content += "0";
				else if (BlueLabel.Content.ToString().Length == 0) BlueLabel.Content = "00";

				ColorTextBox.Text = RedLabel.Content.ToString() + GreenLabel.Content.ToString() + BlueLabel.Content.ToString();
			};
		}
	}
}
