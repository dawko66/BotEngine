using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoBot_v1._1
{
	/// <summary>
	/// Logika interakcji dla klasy NumericUpDown.xaml
	/// </summary>
	public partial class NumericUpDown : UserControl
	{
		public int MinValue
        {
            get
            {
				return _MinValue;
			}
            set
            {
				_MinValue = value;
				int res = 0;
				Int32.TryParse(textBox.Text, out res);
				if (res < _MinValue)
					textBox.Text = _MinValue.ToString();
			}
        }
		private int _MinValue = 0;
		public int MaxValue
		{
			get
			{
				return _MaxValue;
			}
			set
			{
				_MaxValue = value;
				int res = 0;
				Int32.TryParse(textBox.Text, out res);
				if (res > _MaxValue)
					textBox.Text = _MaxValue.ToString();
			}
		}
		private int _MaxValue = 100;
		public int Step = 1;

		public int Value
		{
			get
			{
				int res = 0;
				Int32.TryParse(textBox.Text, out res);
				return res;
			}
			set
			{
				if (value > MaxValue)
					textBox.Text = MaxValue.ToString();
				else if (value < MinValue)
					textBox.Text = MinValue.ToString();
				else
					textBox.Text = value.ToString();
			}
		}

		public NumericUpDown()
		{
			InitializeComponent();
		}

		public event TextChangedEventHandler TextChanged;
		protected virtual void TextChange(TextChangedEventArgs e)
		{
			TextChanged?.Invoke(this, e);
		}

		private void up_Click(object sender, RoutedEventArgs e)
		{
			Value = Value + Step;
		}

		private void down_Click(object sender, RoutedEventArgs e)
		{
			Value = Value - Step;
		}

		private void textBox_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta > 0)
				up_Click(null, null);
			else
				down_Click(null, null);
		}

		private void textBox_LostFocus(object sender, RoutedEventArgs e)
		{
			Value = Value;
		}

		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (!Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) & e.Key != Key.Back)
			{
				e.Handled = true;
			}
		}

		public void textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextChange(e);
		}
	}
}
