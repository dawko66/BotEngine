using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

public enum ConnectionType
{
	Char,
	String,
	Integer,
	Area,
	Position, ArrayPosition,
	Color, ArrayColor,
	Next, True, False, // Usunąć True i false są to elementy next z nazwą true i false
	ButtonAction
}
public enum ConnectionIO
{
	Input, Output
}

class ConnectionPoint : StackPanel
{
	public ConnectionType connectionType
	{
		get { return _connectionType; }
		set { _connectionType = value; nameLabel.Content = connectionTypeToString(value); }
	}
	private ConnectionType _connectionType;

	public ConnectionIO connectionIO
	{
		get;
		private set;
	}
	public Size pointSize;

	public bool mouseHover
	{
		get { return _mouseHover; }
		private set { _mouseHover = value; }
	}
	private bool _mouseHover = false;

	public static int size = 13;
	public static Thickness margin = new Thickness(3, 5, 3, 5);

	public Color fillColor
	{
		get { return ((SolidColorBrush)circle.Fill).Color; }
		set { circle.Fill = new SolidColorBrush(value); }
	}
	public Color borderColor
	{
		get { return ((SolidColorBrush)circle.Stroke).Color; }
		set { circle.Stroke = new SolidColorBrush(value); }
	}
	public Ellipse circle;
	public Label nameLabel;

	public ConnectionPoint(ConnectionIO connectionIO)
	{
		this.connectionIO = connectionIO;
	}
	public ConnectionPoint(ConnectionType connectionType, ConnectionIO connectionIO)
	{
		nameLabel = new Label()
		{
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			Padding = new Thickness(0),
			Foreground = new SolidColorBrush(Colors.White),
			FontSize = size
		};
		circle = new Ellipse()
		{
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			Width = size,
			Height = size,
			//Fill = new SolidColorBrush(Color.FromRgb(58, 58, 58)),
			Stroke = new SolidColorBrush(Color.FromArgb(200, 255, 250, 0)),
			Margin = margin,
			StrokeThickness = 2,
		};

		this.connectionType = connectionType;
		this.connectionIO = connectionIO;

		MouseMove += mouseMove;
		MouseLeave += mouseLeave;

		HorizontalAlignment = HorizontalAlignment.Left;
		VerticalAlignment = VerticalAlignment.Top;
		Orientation = Orientation.Horizontal;

		Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
		Cursor = Cursors.Hand;

		pointSize = new Size(circle.Width, circle.Height);

		if (connectionIO == ConnectionIO.Output)
		{
			Children.Add(nameLabel);
			Children.Add(circle);
		}
		else if (connectionIO == ConnectionIO.Input)
		{
			Children.Add(circle);
			Children.Add(nameLabel);
		}
	}

	private void mouseLeave(object sender, MouseEventArgs e)
	{
		mouseHover = false;
		borderColor = Color.FromArgb(200, borderColor.R, borderColor.G, borderColor.B);
	}
	private void mouseMove(object sender, MouseEventArgs e)
	{
		mouseHover = true;
		borderColor = Color.FromArgb(255, borderColor.R, borderColor.G, borderColor.B);
	}

	public static bool possibleConnections(ConnectionPoint cp1, ConnectionPoint cp2)
	{
		if ((cp1.connectionIO == ConnectionIO.Input && cp2.connectionIO == ConnectionIO.Output) ||
			(cp1.connectionIO == ConnectionIO.Output && cp2.connectionIO == ConnectionIO.Input))
		{
			if (cp1.connectionType == cp2.connectionType) return true;
		}

		if ((cp1.connectionIO == ConnectionIO.Input && cp1.connectionType == ConnectionType.Next) ||
			(cp2.connectionIO == ConnectionIO.Input && cp2.connectionType == ConnectionType.Next))
		{
			if (cp1.connectionType == ConnectionType.True || cp2.connectionType == ConnectionType.True ||
				cp1.connectionType == ConnectionType.False || cp2.connectionType == ConnectionType.False)
				return true;
		}

		return false;
	}
	public static void connectionPointHover(ConnectionPoint cp, bool hover)
    {
		if (hover == true)
			cp.borderColor = Color.FromArgb(255, cp.borderColor.R, cp.borderColor.G, cp.borderColor.B);
		else
			cp.borderColor = Color.FromArgb(200, cp.borderColor.R, cp.borderColor.G, cp.borderColor.B);
	}

	public string connectionTypeToString(ConnectionType ct)
	{
		if (ct == ConnectionType.ArrayPosition)
			return "Array Position";
		else if (ct == ConnectionType.ArrayColor)
			return "Array Color";
		else if (ct == ConnectionType.ButtonAction)
			return "Button Action";
		else
			return ct.ToString();

/*		if (ct == ConnectionType.Char || ct == ConnectionType.Char)
			nameLabel.Content = "chr";
		else if (ct == ConnectionType.Integer || ct == ConnectionType.Integer)
			nameLabel.Content = "str";
		else if (ct == ConnectionType.String || ct == ConnectionType.String)
			nameLabel.Content = "int";
		else if (ct == ConnectionType.Area || ct == ConnectionType.Area)
			nameLabel.Content = "ara";
		else if (ct == ConnectionType.Position || ct == ConnectionType.Position)
			nameLabel.Content = "pos";
		else if (ct == ConnectionType.Color || ct == ConnectionType.Color)
			nameLabel.Content = "col";
		else if (ct == ConnectionType.Next || ct == ConnectionType.Next)
			nameLabel.Content = "nex";
		else if (ct == ConnectionType.True)
			nameLabel.Content = "tru";
		else if (ct == ConnectionType.False)
			nameLabel.Content = "fal";*/
	}
}
