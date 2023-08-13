using AutoBot_v1._1.Class.Data;
using AutoBot_v1._1.NewClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	/// Logika interakcji dla klasy ConnectionPoint.xaml
	/// </summary>
	public partial class ConnectionPoint : UserControl
	{
		public int Size
		{
			get { return _Size; }
			set
			{
				_Size = value;
				FontSize = value;
				ConnPoint.Width = value;
				ConnPoint.Height = value;
				ConnPoint.Margin = new Thickness(.23 * value, .38 * value, .23 * value, .38 * value);
				CenterConnetcion = new Point(
					Width - ConnPoint.Width / 2,
					Height / 2
					);
			}
		}
		private int _Size;
		public Point CenterConnetcion;

		public SolidColorBrush Stroke
		{
            get { return _Stroke; }
			set
			{
				_Stroke = value;
				ConnPoint.Stroke = value;
			}
		}
		public SolidColorBrush _Stroke = new SolidColorBrush(Color.FromArgb(200, 255, 250, 0));
		public SolidColorBrush StrokeHover
		{
			get { return _StrokeHover; }
			set
			{
				_StrokeHover = value;
				ConnPoint.Stroke = value;
			}
		}
		public SolidColorBrush _StrokeHover = new SolidColorBrush(Color.FromArgb(255, 255, 250, 0));
		public SolidColorBrush Fill
		{
			get { return _Fill; }
			set
			{
				_Fill = value;
				ConnPoint.Fill = value;
			}
		}
		public SolidColorBrush _Fill = new SolidColorBrush(Color.FromArgb(0, 255, 250, 0));
		public SolidColorBrush FillHover
		{
			get { return _FillHover; }
			set
			{
				_FillHover = value;
				ConnPoint.Fill = value;
			}
		}
		public SolidColorBrush _FillHover = new SolidColorBrush(Color.FromArgb(100, 255, 250, 0));

		private Label Text;
		private Ellipse ConnPoint;

		public ConnectionData connection;

        public ConnectionPoint()
		{
			InitializeComponent();
		}
		public ConnectionPoint(ConnectionData c, HorizontalAlignment horizontalAlignment)
		{
			InitializeComponent();
			
			Text = new Label()
			{
				Content = c.connectionType.ToString(),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Padding = new Thickness(0),
				Foreground = new SolidColorBrush(Colors.Black),
			};

			ConnPoint = new Ellipse()
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Width = Size,
				Height = Size,
				Fill = Fill,
				Stroke = Stroke,
				Margin = new Thickness(.23 * Size, .38 * Size, .23 * Size, .38 * Size),
				StrokeThickness = 2,
			};

			Size = 13;
			connection = c;

			grid.VerticalAlignment = VerticalAlignment.Top;
			grid.HorizontalAlignment = horizontalAlignment;
			grid.Cursor = Cursors.Hand;

			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

			if (horizontalAlignment == HorizontalAlignment.Right)
			{
				Grid.SetColumn(Text, 0);
				Grid.SetColumn(ConnPoint, 1);
			}
			else if (horizontalAlignment == HorizontalAlignment.Left)
			{
				Grid.SetColumn(Text, 1);
				Grid.SetColumn(ConnPoint, 0);
			}

			grid.Children.Add(Text);
			grid.Children.Add(ConnPoint); // connnection point
		}

        private void grid_MouseMove(object sender, MouseEventArgs e)
        {
			ConnPoint.Fill = FillHover;
			ConnPoint.Stroke = StrokeHover;
		}
        private void grid_MouseLeave(object sender, MouseEventArgs e)
        {
			ConnPoint.Fill = Fill;
			ConnPoint.Stroke = Stroke;
		}
    }
}
