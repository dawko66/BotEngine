using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

class NumericTextBox : Grid
{
	private TextBox textBox;
	private Button upButton;
	private Button downButton;

	private ColumnDefinition colDef2;
	private RowDefinition rowDef1;

	public enum NumericalSystem
	{
		BINARY,
		OCTAL,
		DECIMAL,
		HEXADECIMAL,
	}
	public NumericalSystem numericalSystem = NumericalSystem.DECIMAL;

	public string MaxValue
	{
		get
		{
			if (numericalSystem == NumericalSystem.DECIMAL) return Convert.ToString(maxValue, 2);
			else if (numericalSystem == NumericalSystem.HEXADECIMAL) return Convert.ToString(maxValue, 2);
			else if (numericalSystem == NumericalSystem.BINARY) return Convert.ToString(maxValue, 2);
			else if (numericalSystem == NumericalSystem.OCTAL) return Convert.ToString(maxValue, 2);
			else return maxValue.ToString();
		}
		set
		{
			if (numericalSystem == NumericalSystem.DECIMAL) maxValue = Convert.ToInt32(value, 10);
			else if (numericalSystem == NumericalSystem.HEXADECIMAL) maxValue = Convert.ToInt32(value, 16);
			else if (numericalSystem == NumericalSystem.BINARY) maxValue = Convert.ToInt32(value, 2);
			else if (numericalSystem == NumericalSystem.OCTAL) maxValue = Convert.ToInt32(value, 8);
		}
	}
	private int maxValue;

	public string MinValue
	{
		get
		{
			if (numericalSystem == NumericalSystem.DECIMAL) return Convert.ToString(minValue, 2);
			else if (numericalSystem == NumericalSystem.HEXADECIMAL) return Convert.ToString(minValue, 2);
			else if (numericalSystem == NumericalSystem.BINARY) return Convert.ToString(minValue, 2);
			else if (numericalSystem == NumericalSystem.OCTAL) return Convert.ToString(minValue, 2);
			else return minValue.ToString();
		}
		set
		{
			if (numericalSystem == NumericalSystem.DECIMAL) minValue = Convert.ToInt32(value, 10);
			else if (numericalSystem == NumericalSystem.HEXADECIMAL) minValue = Convert.ToInt32(value, 16);
			else if (numericalSystem == NumericalSystem.BINARY) minValue = Convert.ToInt32(value, 2);
			else if (numericalSystem == NumericalSystem.OCTAL) minValue = Convert.ToInt32(value, 8);
		}
	}
	private int minValue;

	private string lastValue;

	//public Regex inputMask = new Regex("[\\]+");	

	public NumericTextBox()
	{
		textBox = new TextBox()
		{
			VerticalContentAlignment = VerticalAlignment.Center,
		};
		upButton = new Button()
		{
			Content = $"{(char)0x25B4}",
			Padding = new Thickness(0, -20, 0, 0)
		};
		downButton = new Button()
		{
			Content = $"{(char)0x25BE}",
			Padding = new Thickness(0, -50, 0, 0)
		};

		textBox.SizeChanged += setSize;
		textBox.KeyDown += keyDown;
		textBox.LostFocus += lostFocus;
		textBox.TextChanged += textChanged;

		MinHeight = 10;
		MinWidth = 20;

		// Define the Rows
		rowDef1 = new RowDefinition();
		RowDefinition rowDef2 = new RowDefinition();
		RowDefinitions.Add(rowDef1);
		RowDefinitions.Add(rowDef2);

		// Define the Columns
		ColumnDefinition colDef1 = new ColumnDefinition();
		colDef2 = new ColumnDefinition();
		ColumnDefinitions.Add(colDef1);
		ColumnDefinitions.Add(colDef2);

		SetRow(textBox, 0);
		SetRowSpan(textBox, 2);
		SetColumn(textBox, 0);

		SetRow(upButton, 0);
		SetColumn(upButton, 1);
		SetRow(downButton, 1);
		SetColumn(downButton, 1);

		Children.Add(textBox);
		Children.Add(downButton);
		Children.Add(upButton);
	}

	private void lostFocus(object sender, RoutedEventArgs e)
	{
		int value;
		int.TryParse(((TextBox)sender).Text, out value);

		//0 <= value <= 99999
		if (value < 0) value = 0;               // Min value
		else if (value > 99999) value = 99999;  // Max value
		//Set value in TextBox
		((TextBox)sender).Text = value.ToString();
	}

	public void setSize(object sender, SizeChangedEventArgs e)
	{
		colDef2.Width = new GridLength(rowDef1.ActualHeight, GridUnitType.Pixel);
		textBox.FontSize = textBox.ActualHeight / 1.8;
		upButton.FontSize = textBox.FontSize;
		upButton.Padding = new Thickness(0, rowDef1.ActualHeight / -3, 0, 0);
		downButton.FontSize = textBox.FontSize;
		downButton.Padding = new Thickness(0, rowDef1.ActualHeight / -3, 0, 0);
	}

	private void textChanged(object sender, TextChangedEventArgs e)
	{
		if (numericalSystem == NumericalSystem.DECIMAL)
			if (new Regex("[^0-9-]+").IsMatch(((TextBox)sender).Text))
			{
				(sender as TextBox).Text = lastValue;
				textBox.Select(textBox.Text.Length, textBox.Text.Length);
			}
			else lastValue = (sender as TextBox).Text;

		if (numericalSystem == NumericalSystem.HEXADECIMAL)
			if (new Regex("[^0-9a-fA-F-]+").IsMatch(((TextBox)sender).Text))
			{
				(sender as TextBox).Text = lastValue;
				textBox.Select(textBox.Text.Length, textBox.Text.Length);
			}
			else lastValue = (sender as TextBox).Text;

		if (numericalSystem == NumericalSystem.BINARY)
			if (new Regex("[^0-1-]+").IsMatch(((TextBox)sender).Text))
			{
				(sender as TextBox).Text = lastValue;
				textBox.Select(textBox.Text.Length, textBox.Text.Length);
			}
			else lastValue = (sender as TextBox).Text;

		if (numericalSystem == NumericalSystem.OCTAL)
			if (new Regex("[^0-7-]+").IsMatch(((TextBox)sender).Text))
			{
				(sender as TextBox).Text = lastValue;
				textBox.Select(textBox.Text.Length, textBox.Text.Length);
			}
			else lastValue = (sender as TextBox).Text;
	}

	private void keyDown(object sender, KeyEventArgs e)
	{
		if (numericalSystem == NumericalSystem.DECIMAL)
			e.Handled = ((int)e.Key > 43 || (int)e.Key < 34) && ((int)e.Key > 83 || (int)e.Key < 74);
		else if (numericalSystem == NumericalSystem.HEXADECIMAL)
			e.Handled = ((int)e.Key > 49 || (int)e.Key < 34) && ((int)e.Key > 89 || (int)e.Key < 74);
		else if (numericalSystem == NumericalSystem.BINARY)
			e.Handled = ((int)e.Key > 35 || (int)e.Key < 34) && ((int)e.Key > 75 || (int)e.Key < 74);
		else if (numericalSystem == NumericalSystem.OCTAL)
			e.Handled = ((int)e.Key > 41 || (int)e.Key < 34) && ((int)e.Key > 81 || (int)e.Key < 74);
	}


	/*
	 private void positionXTextBox_LostFocus(object sender, RoutedEventArgs e)
	 {
		 int value;
		 Get value with TextBox and try parse to int
		 int.TryParse(((TextBox)sender).Text, out value);

		 0 <= value <= 99999
		 if (value < 0) value = 0;               // Min value
		 else if (value > 99999) value = 99999;  // Max value
		 Set value in TextBox

		((DataGridRecord)dataGrid.Items[dataGrid.SelectedIndex]).PositionX = value.ToString();
		 ((TextBox)sender).Text = value.ToString();
		 }
	 private void positionYTextBox_LostFocus(object sender, RoutedEventArgs e)
		 {
			 int value;
			 Get value with TextBox and try parse to int
			int.TryParse(((TextBox)sender).Text, out value);

			 0 <= value <= 99999
			if (value < 0) value = 0;               // Min value
			 else if (value > 99999) value = 99999;  // Max value
			 Set value in TextBox

			((DataGridRecord)dataGrid.Items[dataGrid.SelectedIndex]).PositionY = value.ToString();
			 ((TextBox)sender).Text = value.ToString();
			 }
	 private void colorTextBox_LostFocus(object sender, RoutedEventArgs e)
			 {
				 Convert string to Color
		 string color;
				 try
				 {
					 Color RGBColor = ((Color)ColorConverter.ConvertFromString(((TextBox)sender).Text));
					 color = "#" + RGBColor.R.ToString("X2") + RGBColor.G.ToString("X2") + RGBColor.B.ToString("X2");
				 }
				 catch (FormatException)
				 {
					 color = "#000000";
				 }

		 ((DataGridRecord)dataGrid.Items[dataGrid.SelectedIndex]).Color = color;
				 ((TextBox)sender).Text = color;
			 }
	*/
}
