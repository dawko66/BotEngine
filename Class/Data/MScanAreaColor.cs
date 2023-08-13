using AutoBot_v1._1.Class.Data;
using System;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Media;

[Serializable]
public class MScanAreaColor : BotElement
{
	public static readonly string name = "Scan area color";	// Name this class

	//BitmapCache screen;		// Screen shot 
	public string[] colors;		// Colors to scan
	public Point position;		// Start position rectangle to scan
	public Point size;          // Size rectangle to scan (>0)
    public int[] tablesId;		// Chose table when (false-tablesId[0], true-tablesId[1])
	public MScanAreaColor(string[] colors, Point position, Point size, string trueName, string falseName)
	{
		this.colors = colors;
		this.position = position;
		this.size = size;

		tablesId = new int[] { Settings.nextTableId, Settings.nextTableId + 1 };

		//~~Table.addTable(falseName, 1);
		//~~Table.addTable(trueName, 2);
	}

	/* I don't know is work (to check) */
	public static System.Drawing.Bitmap makeScreenArea(Point position, Point size)
	{
		System.Drawing.Bitmap pixelColor = new System.Drawing.Bitmap((int)size.X, (int)size.Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb); // Create bitmap to save pixel
		using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(pixelColor))
		{
			g.Clear(System.Drawing.Color.White);
			g.CompositingMode = CompositingMode.SourceOver;
            try
            {
				g.CopyFromScreen((int)position.X, (int)position.Y, 0, 0, pixelColor.Size);          // Copy area(pixel) from screen
			}
			catch { }
			
		}
		return pixelColor;
	}
	public static bool action(Point position, Point size, string[] colors)
    {
		System.Drawing.Bitmap pixelColor = makeScreenArea(position, size);

		for (int i = 0; i < size.X; i++)        // X axis
			for (int j = 0; j < size.Y; j++)    // Y axis
			{
				System.Drawing.Color color = pixelColor.GetPixel(i, j);    // Get pixel from bitmap 

				for (int k = 0; k < colors.Length; k++)		// Check all colors in array
					if ("#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2") == colors[k])   // Convert System.Drawing.Color to System.Windows.Media.Color and check is the same color
						return true;
			}
		return false;
	}
	public static Point actionn(Point position, Point size, string[] colors)
	{
		/* return position where it found color in colors[] */
		System.Drawing.Bitmap pixelColor = makeScreenArea(position, size);

		for (int i = 0; i < size.X; i++)        // X axis
			for (int j = 0; j < size.Y; j++)    // Y axis
			{
				System.Drawing.Color color = pixelColor.GetPixel(i, j);    // Get pixel from bitmap 

				for (int k = 0; k < colors.Length; k++)     // Check all colors in array
					if ("#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2") == colors[k])   // Convert System.Drawing.Color to System.Windows.Media.Color and check is the same color
						return new Point(position.X + i, position.Y + j);
			}
		return new Point();
	}

}
