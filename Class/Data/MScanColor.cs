using AutoBot_2._0.Class.Data;
using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

public class ColorPosition
{
	public bool IsActive { get; set; }

	public Point Position { get; set; }
	public Color Color { get; set; }
}

public class ColorAndPosition
{
	public bool IsPositionActive { get; set; }
	public bool IsColorActive { get; set; }

	public Point Position { get; set; }
	public Color Color { get; set; }
}

[Serializable]
public class MScanColor : BotElement
{
	public const string name = "ScanColor";  // Name this class

	//public string[] bmpSources;			// Screen shot
	//public bool[] positoinToAllColors;  // positoinToAllColors[x] (true) search in positions[x] all colors, (false) search in positions[x] only colors[x]
	//public bool[] colorToAllPositions;  // colorToAllPositions[x] (true) search colors[x] in all positions, (false) search colors[x] only in positions[x]
	public List<ColorAndPosition> ColorsAndPositions;				// Colors to scan format (#[R][G][B]) ("#FF00FF")
	//public List<Point> Positions;           // Positions scanning colors
	public List<ColorPosition> ColorPositions;           // Positions scanning colors
	
	//public TimeSpan[] CheckDelay = new TimeSpan[2] { new TimeSpan(0, 0, 0, 0, 50), new TimeSpan(0, 0, 0, 0, 150) };
	public bool Find = true;
	public bool CheckAllTimes;			// sprawdzanie wszystkich mimo znalezienia poprawnego w celu zwrócenia większej ilości poprawnych ColorPoint
	//public int FalseBlockID;			// Chose table when wouldn't find color
	//public int TrueBlockID;				// Chose table when will find color
	
	public bool Result;
	public List<ColorPosition> ActivatedColorPositions;


	public MScanColor() { }

	public MScanColor(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + base.Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next, "True"));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, null));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next, "False"));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.Yes, CType.Color));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.Yes, CType.Color));	// 6
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.Yes, CType.Position));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.Yes, CType.Position));	// 8
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.Yes, CType.ColorPosition));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.Yes, CType.ColorPosition));	//10
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.Yes, CType.ColorPosition));	//11
	}

	#region Get point color on screen (Old Code)
	/*
	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
	public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

	public bool screenPixelColor(int X, int Y, string color)
	{
		Bitmap screenPixel = new Bitmap(1, 1);
		using (Graphics gdest = Graphics.FromImage(screenPixel))
		{
			using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
			{
				IntPtr hSrcDC = gsrc.GetHdc();
				IntPtr hDC = gdest.GetHdc();
				int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, X, Y, (int)CopyPixelOperation.SourceCopy);
				gdest.ReleaseHdc();
				gsrc.ReleaseHdc();
			}
		}
		var c = screenPixel.GetPixel(0, 0);
		string screenColor = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");

		if (screenColor == color)
			return true;
		else
			return false;
	}
	*/
	#endregion

/*	public static string scanPixelColor(Point position)
	{
		System.Drawing.Bitmap pixelColor = new System.Drawing.Bitmap(1, 1); // Create bitmap to save pixel
		using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(pixelColor))
		{
			g.CopyFromScreen((int)position.X, (int)position.Y, 0, 0, pixelColor.Size);			// Copy area(pixel) from screen
		}
		System.Drawing.Color color = pixelColor.GetPixel(0, 0);    // Get pixel from bitmap
		return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");         // Convert System.Drawing.Color to System.Windows.Media.Color and return
	}*/



	/*public static bool action3(Point position, string color)
	{
		*//* check one color in position *//*
		if (scanPixelColor(position) == color)   // Check is the same color
			return true;

		return false;
	}
	public static bool action1(Point[] positions, string[] colors)
	{
		*//* check one color in position *//*
		for (int i = 0; i < positions.Length; i++)  // All positions
			if (scanPixelColor(positions[i]) == colors[i])   // Check is the same color
				return true;

		return false;
	}
	public static bool action2(Point[] positions, string[] colors)
	{
		*//* check all color in array in position *//*
		for (int i = 0; i < positions.Length; i++)		// All positions
			for (int j = 0; j < colors.Length; j++)		// All colors
				if (scanPixelColor(positions[i]) == colors[j])   // Check is the same color
					return true;

		return false;
	}*/

	public static BitmapSource makeScreenShot()
	{
		/* Create clear bitmap */
		System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);

		using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
		{
			g.Clear(System.Drawing.Color.Black);
			/* Make screen shot */
			g.CopyFromScreen((int)SystemParameters.VirtualScreenLeft, (int)SystemParameters.VirtualScreenTop, 0, 0, bitmap.Size);

			/* Convert to BitmapSource and return */
			return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		}
	}

	public static System.Drawing.Bitmap MakeScreenShot()
	{
		/* Create clear bitmap */
		System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);

		using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
		{
			g.Clear(System.Drawing.Color.Black);
			/* Make screen shot */
			g.CopyFromScreen((int)SystemParameters.VirtualScreenLeft, (int)SystemParameters.VirtualScreenTop, 0, 0, bitmap.Size);

			/* Convert to BitmapSource and return */
			return bitmap;
		}
	}

	public static System.Drawing.Bitmap GetImagePart(System.Drawing.Bitmap bmp, Point p1, Point p2)
	{
		System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)(p2.X - p1.X), (int)(p2.Y - p1.Y));

		using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
		{
			g.Clear(System.Drawing.Color.Black);
			/* Make screen shot */
			//g.DrawImage(bmp, (int)p1.X, (int)p1.Y, bitmap.Size.Width, bitmap.Size.Height);
			//g.DrawImageUnscaled(bmp, (int)p1.X, (int)p1.Y, bitmap.Size.Width, bitmap.Size.Height);
			g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
				 new System.Drawing.Rectangle((int)p1.X, (int)p1.Y, bitmap.Size.Width, bitmap.Size.Height),
				 System.Drawing.GraphicsUnit.Pixel);
			//g.CopyFromScreen((int)p1.X, (int)p1.Y, 0, 0, bitmap.Size);

			/* Convert to BitmapSource and return */
			return bitmap;
		}
	}

#pragma warning disable CS0246 // Nie można znaleźć nazwy typu lub przestrzeni nazw „MScanColor” (brak dyrektywy using lub odwołania do zestawu?)
	public static bool BotAction(MScanColor o, CancellationToken token)
#pragma warning restore CS0246 // Nie można znaleźć nazwy typu lub przestrzeni nazw „MScanColor” (brak dyrektywy using lub odwołania do zestawu?)
	{
		bool returnValue = !o.Find;

		o.ActivatedColorPositions = new List<ColorPosition>();

		List<Color> colors = new List<Color>();
		List<Point> positions = new List<Point>();
		foreach (var cap in o.ColorsAndPositions)
		{
			if (cap.IsColorActive)
				colors.Add(cap.Color);

			if (cap.IsPositionActive)
				positions.Add(cap.Position);
		}

		/*
		checkAllTimes sprawdza wszystko tyle razy ile wynosi warość times
		jeżeli w tych wszystkich razach wystąpi rządana wartość to zwróci rządaną wartość (true,false)
		jeżeli wartość times ustawiona jest na 0 sprawi że będzie sprawdzane aż do wystąpienia 
		rządanych wartości (true lub false) ze zmiennej Find
		*/

		bool afterReturn(bool val)
		{
			o.Variables.Add(new VariableData() { Id = 6, Variable = o.ColorsAndPositions });
			o.Variables.Add(new VariableData() { Id = 8, Variable = o.ColorsAndPositions });
			o.Variables.Add(new VariableData() { Id = 10, Variable = o.ColorPositions });
			o.Variables.Add(new VariableData() { Id = 11, Variable = o.ActivatedColorPositions });

			return val;
		}

		int i = 0;
		while (o.Times == 0 ? true : i < o.Times)
		{
			if (o.Times > 0)
				i++;

			foreach (var p in positions)
				foreach (var c in colors)
					if (o.Find == (GetPixelColorFromScreen(p) == c))
					{
						o.ActivatedColorPositions.Add(new ColorPosition() { Position = p, Color = c, IsActive = true });
						if (o.CheckAllTimes)
							returnValue = o.Find;
						else
							return afterReturn(o.Find);
					}

			foreach (var cp in o.ColorPositions)
				if (cp.IsActive)
					if (o.Find == (GetPixelColorFromScreen(cp.Position) == cp.Color))
					{
						o.ActivatedColorPositions.Add(cp);
						if (o.CheckAllTimes)
							returnValue = o.Find;
						else
							return afterReturn(o.Find);
					}

			if (token.IsCancellationRequested)
				token.ThrowIfCancellationRequested();
		}





		return afterReturn(returnValue);
	}

	public static Color GetPixelColorFromScreen(Point position)
	{
		System.Drawing.Bitmap pixelColor = new System.Drawing.Bitmap(1, 1); // Create bitmap to save pixel
		using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(pixelColor))
		{
			g.CopyFromScreen((int)position.X, (int)position.Y, 0, 0, pixelColor.Size);          // Copy area(pixel) from screen
		}
		System.Drawing.Color color = pixelColor.GetPixel(0, 0);    // Get pixel from bitmap
		return Color.FromRgb(color.R, color.G, color.B);
	}

	public static Color GetPixelColorFromBitmap(System.Drawing.Bitmap bmp, Point position)
	{

		System.Drawing.Color color;
		if (position.X >= 0 && position.Y >= 0 && position.X < bmp.Width && position.Y < bmp.Height)
			color = bmp.GetPixel((int)position.X, (int)position.Y);     // Get pixel from bitmap
		else
			color = new System.Drawing.Color();
		return Color.FromRgb(color.R, color.G, color.B);			// Convert System.Drawing.Color to System.Windows.Media.Color and return
	}

	#region DRAW ON SCREEN
	[DllImport("User32.dll")]
	public static extern IntPtr GetDC(IntPtr hwnd);
	[DllImport("User32.dll")]
	public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
	/*public static void drawOnScreen(Point point)
	{
		int size = 10;
		System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)point.X-size, (int)point.Y-size, (int)point.X+size, (int)point.Y+size);

		IntPtr desktopPtr = GetDC(IntPtr.Zero);
		System.Drawing.Graphics g = System.Drawing.Graphics.FromHdc(desktopPtr);

		System.Drawing.SolidBrush b = new System.Drawing.SolidBrush(System.Drawing.Color.White);
		g.FillRectangle(b, rect);

		g.Dispose();
		ReleaseDC(IntPtr.Zero, desktopPtr);
	}*/

	public static Window drawOnScreen(Point point)
	{
		SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
		Grid grid = new Grid();

		Window window = new Window();
		window.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
		window.Left = point.X - 25;
		window.Top = point.Y - 25;
		window.Width = 10;
		window.Height = 10;
		window.WindowStyle = WindowStyle.None;
		window.AllowsTransparency = true;
		window.UseLayoutRounding = true;
		window.SnapsToDevicePixels = true;
		window.ShowInTaskbar = false;
		window.Topmost = true;

		window.Content = grid;


		Rectangle rectangle = new Rectangle();
		rectangle.Fill = brush;
		Line line = new Line() { X1=0, Y1=0, X2=10, Y2=10 };
		line.Stroke = brush;
				/*		line.StrokeThickness = 3;
						line.SnapsToDevicePixels = true;
						line.UseLayoutRounding = true;
						line.ClipToBounds = true;
						line.Stretch = Stretch.None;*/



		grid.Children.Add(line);



		
		return window;
	}
	#endregion
}
