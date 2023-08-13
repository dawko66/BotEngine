using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

enum Object
{
	Cursor,
	Key, Text, RandomText,
	ScanColor, ScanAreaColor,
	//Choose,
	OpenBot,
	Script,
}

class TreeGridObject : Grid
{
	public Point position
	{
		get { return new Point(Margin.Left, Margin.Top); }
		set { Margin = new Thickness(value.X, value.Y, 0, 0); }
	}
	private string borderColor = Color.FromRgb(255, 255, 255).ToString();
	private string fillColor = Color.FromRgb(255, 255, 255).ToString();
	public Object _object;
	public bool mouseBtnDown = false;

	private Size size
	{
		get { return new Size(Width, Height); }
		set { Width = value.Width; Height = value.Height; }
	}

	public List<ConnectionPoint> connectionPoints = new List<ConnectionPoint>();
	public Grid grid;
	public Label label;
	public TreeGridObject(Object _object, Point position)
	{
		HorizontalAlignment = HorizontalAlignment.Left;
		VerticalAlignment = VerticalAlignment.Top;
		Background = new SolidColorBrush(Colors.Gray);
		RowDefinitions.Add(new RowDefinition());
		RowDefinitions.Add(new RowDefinition());

		this._object = _object;
		this.position = position;

		label = new Label()
		{
			HorizontalContentAlignment = HorizontalAlignment.Center,
			VerticalContentAlignment = VerticalAlignment.Center,
			Background = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
			Foreground = new SolidColorBrush(Colors.White),
			Content = objectToString(_object),
			FontSize = 14,
			Margin = new Thickness(0)
		};
		Grid.SetRow(label, 0);
		Children.Add(label);
		
		Width = 250;
		RowDefinitions[0].Height = new GridLength(label.FontSize * 2);
		
		label.MouseDown += mouseDown;
		label.MouseUp += mouseUp;

		Border border = new Border()
		{
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Top,
			Background = new SolidColorBrush(Color.FromRgb(58, 58, 58)),
			BorderBrush = new SolidColorBrush(Color.FromRgb(247, 160, 26)),
			BorderThickness = new Thickness(0, 0, 0, 3),
			Width = Width
		};
		Grid.SetRow(border, 1);
		Children.Add(border);

		grid = new Grid();
		border.Child = grid;
		
		objectConnectoins(_object);
		drawConnectionPoints();
		
	}

	private void drawConnectionPoints()
	{
		int leftQuantity = 0;
		int rightQuantity = 0;

		foreach (ConnectionPoint cp in connectionPoints)
		{
			if (cp.connectionIO == ConnectionIO.Input)
			{
				cp.HorizontalAlignment = HorizontalAlignment.Left;
				Grid.SetRow(cp, leftQuantity);
				grid.Children.Add(cp);
				leftQuantity++;
			}
			else if (cp.connectionIO == ConnectionIO.Output)
			{
				cp.HorizontalAlignment = HorizontalAlignment.Right;
				Grid.SetRow(cp, rightQuantity);
				grid.Children.Add(cp);
				rightQuantity++;
			}
		}

		int higher = rightQuantity;
		if (leftQuantity > rightQuantity) higher = leftQuantity;

		for (int i = 0; i < higher; i++)
			grid.RowDefinitions.Add(new RowDefinition());

		Height = (ConnectionPoint.size + ((Ellipse)connectionPoints[0].Children[0]).Margin.Top * 2) * higher + RowDefinitions[0].Height.Value + 3;
	}

	private void mouseDown(object sender, MouseButtonEventArgs e)
	{
		mouseBtnDown = true;
	}
	private void mouseUp(object sender, MouseButtonEventArgs e)
	{
		mouseBtnDown = false;
	}

	private void objectConnectoins(Object ob)
	{
		if (ob == Object.Key)
		{
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Next, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Next, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Char, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ButtonAction, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Char, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ButtonAction, ConnectionIO.Output));
		}
		else if(ob == Object.Text)
		{
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Next, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Next, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.String, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.String, ConnectionIO.Output));
		}
		else if (ob == Object.Cursor)
		{
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Next, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Next, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Position, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ButtonAction, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Position, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ButtonAction, ConnectionIO.Output));
		}
		else if (ob == Object.ScanAreaColor)
		{
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Next, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.True, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.False, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayColor, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Area, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayColor, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Area, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayPosition, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayColor, ConnectionIO.Output));
		}
		else if (ob == Object.ScanColor)
		{
			connectionPoints.Add(new ConnectionPoint(ConnectionType.Next, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.True, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.False, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayPosition, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayColor, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayColor, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayPosition, ConnectionIO.Input));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayColor, ConnectionIO.Output));
			connectionPoints.Add(new ConnectionPoint(ConnectionType.ArrayPosition, ConnectionIO.Output));
		}
	}

	private string objectToString(Object ob)
	{
		if (ob == Object.RandomText)
			return "Random Text";
		else if (ob == Object.ScanColor)
			return "Scan Color";
		else if (ob == Object.ScanAreaColor)
			return "Scan Area Color";
		else if (ob == Object.OpenBot)
			return "Open Bot";
		else
			return ob.ToString();
	}
}
