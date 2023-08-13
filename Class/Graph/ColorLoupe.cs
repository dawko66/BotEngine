using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AutoBot_2._0.Class.Graph
{
	class ColorLoupe : Window
	{
		private Grid Container;
		private Ellipse circle;
		private Ellipse ellipse;

		MouseHooker MouseHook;

		public int Pixels = 13;
		public int LoupePixelSize = 130;

		public ColorLoupe()
		{
			Width = LoupePixelSize;
			Height = LoupePixelSize;
			AllowsTransparency = true;
			WindowStyle = WindowStyle.None;
			Background = Brushes.Transparent;
			RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);

			Cursor = Cursors.None;

			MouseHook = new MouseHooker();
			MouseHook.ActivateHook();
			MouseHook.MouseActionEvent += (object sender, MouseHooker.MouseeEventArgs e) =>
			{
				Point mPosition = MCursor.GetMousePosition();
				Point s = new Point(mPosition.X - (Pixels / 2), mPosition.Y - (Pixels / 2));
				Left = s.X - 2;
				Top = s.Y - 2;
				var bitmap = MScanAreaColor.makeScreenArea(s, new Point(Pixels, Pixels));
				try
				{
					var ScreenCapture = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
					   bitmap.GetHbitmap(),
					   IntPtr.Zero,
					   System.Windows.Int32Rect.Empty,
					   System.Windows.Media.Imaging.BitmapSizeOptions.FromWidthAndHeight(Pixels, Pixels));

					circle.Fill = new ImageBrush(ScreenCapture);
				}
				catch { }
				ellipse.Fill = new SolidColorBrush(Color.FromArgb(0, 127, 127, 127));
			};
			
			Container = new Grid();
			Content = Container;

			circle = Loupe();
			Container.Children.Add(circle);


			ellipse = new Ellipse()
			{
				Width = Pixels + 4,
				Height = Pixels + 4,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				StrokeThickness = 1,
				Stroke = Brushes.Black,
			};

			Container.Children.Add(ellipse);
		}

		private Ellipse Loupe()
		{
			Ellipse ellipse = new Ellipse()
			{
				Width = LoupePixelSize,
				Height = LoupePixelSize,
				StrokeThickness = 1,
				Stroke = Brushes.Black,
			};

			return ellipse;
		}


		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			MouseHook.DeactivateHook();
		}
	}
}
