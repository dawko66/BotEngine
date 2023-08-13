using AutoBot_v1._1.Class.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AutoBot_2._0.Class.Graph
{
	public class CLine : Grid
	{
		#region Constructor variable
		public Point P1
		{
			get { return _P1; }
			set { _P1 = value; DrawLine(); }
		}
		private Point _P1;
		public Point P2
		{
			get { return _P2; }
			set { _P2 = value; DrawLine(); }
		}
		private Point _P2;
		private void DrawLine()
		{
			Path.Data = Geometry.Parse(
			"M" + ((int)_P1.X).ToString() + ", " + ((int)_P1.Y).ToString() +
			"C" + ((int)_P2.X).ToString() + ", " + ((int)_P1.Y).ToString() + " " +
			((int)_P1.X).ToString() + ", " + ((int)_P2.Y).ToString() + " " +
			((int)_P2.X).ToString() + ", " + ((int)_P2.Y).ToString());
		}

		public CData CData;
		#endregion

		#region Element objects
		private Path Path;
		private Ellipse DeleteButton;
		#endregion

		#region Event
		public event EventHandler Delete;
		protected virtual void DeleteEvent(MouseEventArgs e)
		{
			Delete?.Invoke(this, e);
		}
		#endregion

		public CLine(CData CData)
		{
			#region Generate element objects
			DeleteButton = new Ellipse()
			{
				Width = 30,
				Height = 30,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				Fill = new SolidColorBrush(Color.FromRgb(200, 80, 80)),
				Stroke = new SolidColorBrush(Color.FromArgb(200, 226, 226, 226)),
				StrokeThickness = 2,
				Cursor = Cursors.Hand,
				Visibility = Visibility.Hidden

			};
			DeleteButton.MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				DeleteEvent(e);
			};

			Path = new Path()
			{
				Stroke = new SolidColorBrush(Color.FromArgb(200, 226, 226, 226)),
				StrokeThickness = 3,
			};
			MouseMove += (object sender, MouseEventArgs e) =>
			{
				// Show delete button
				DeleteButton.Margin = new Thickness(
					(P1.X + P2.X)/2 - DeleteButton.Width/2,
					(P1.Y + P2.Y)/2 - DeleteButton.Height/2,
					0, 0);
				DeleteButton.Visibility = Visibility.Visible;

			};
			MouseLeave += (object sender, MouseEventArgs e) =>
			{
				// hide delete button
				DeleteButton.Visibility = Visibility.Hidden;
			};
			Children.Add(Path);
			Children.Add(DeleteButton);

			#endregion

			#region Constructor parameters
			this.CData = CData;
			#endregion
		}

	}
}
