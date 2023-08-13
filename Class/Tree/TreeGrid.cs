using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

class TreeGrid : Grid
{
	public List<TreeGridObject> objects
	{
		get;
		private set;
	}
	public void AddObject(TreeGridObject ob)
	{
		objects.Add(ob);
		Children.Add(ob);
	}

	private bool mouseBtnDown = false;
	private bool moveAndDown = false;
	private bool selectRectangle = false;
	private Point mouseDownPosition;

	public List<Connection> connections = new List<Connection>();
	private IntPoint connPointID = new IntPoint(-1, -1);    // X => TreeGreedObject ID, Y => ConnectionPoint ID
	private Line connLine;
	private Rectangle selectObjects;
	private Grid grid = new Grid();

	//private int zoomSize = 1;

	public TreeGrid()
	{
		objects = new List<TreeGridObject>();
		Background = new SolidColorBrush(Color.FromRgb(27, 27, 27));

		this.Children.Add(grid);

		selectObjects = new Rectangle()
		{
			Fill = new SolidColorBrush(Color.FromArgb(50, 180, 100, 100)),
			VerticalAlignment = VerticalAlignment.Top,
			HorizontalAlignment = HorizontalAlignment.Left,
			StrokeDashArray = new DoubleCollection() { 5, 3, 1, 3 },
			Stroke = new SolidColorBrush(Color.FromArgb(150, 255, 255, 255))
		};
		this.Children.Add(selectObjects);

		connLine = new Line()
		{
			Stroke = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255)),
			
		};
		this.Children.Add(connLine);

		MouseDown += mouseDown;
		MouseUp += mouseUp;
		MouseMove += mouseMove;
	}

	private void mouseDown(object sender, MouseButtonEventArgs e)
	{
		mouseBtnDown = true;
		mouseDownPosition = e.GetPosition(this);

		connect(e);

		if (e.OriginalSource.ToString() == "TreeGrid")  // hover on object
			selectRectangle = true;
		else selectRectangle = false;
	}
	private void mouseUp(object sender, MouseButtonEventArgs e)
	{
		mouseBtnDown = false;
		if (moveAndDown == true && connPointID.X != -1)
			connect(e);

		selectObjects.Margin = new Thickness(0, 0, 0, 0);
		selectObjects.Width = 0;
		selectObjects.Height = 0;
		checkDelete();
	}
	private void mouseMove(object sender, MouseEventArgs e)
	{
		if (mouseBtnDown == true)
		{
			Point mousePosition = e.GetPosition(this);

			#region move block
			foreach (TreeGridObject ob in objects)
			{
				if (ob.mouseBtnDown == true)
				{
					Point shiftInAxis = new Point(mousePosition.X - mouseDownPosition.X, mousePosition.Y - mouseDownPosition.Y);
					ob.position = new Point(ob.position.X + shiftInAxis.X, ob.position.Y + shiftInAxis.Y);
					mouseDownPosition = e.GetPosition(this);
					drawConnections();
				}
			}
			#endregion
			moveAndDown = true;

			#region select objects
			if (selectRectangle == true)
			{
				Rect r = selectObjectSpace(mouseDownPosition, mousePosition);
				selectObjects.Margin = new Thickness(r.Left, r.Top, 0, 0);
				selectObjects.Width = r.Width;
				selectObjects.Height = r.Height;
			}
			#endregion
		}
		else
			moveAndDown = false;

		if (connPointID.X != -1)
		{
			Point mousePosition = e.GetPosition(this);
			connLine.X2 = mousePosition.X + 1;
			connLine.Y2 = mousePosition.Y + 1;

			ConnectionPoint.connectionPointHover(objects[connPointID.X].connectionPoints[connPointID.Y], true);
		}
	}

	private void connect(MouseEventArgs e)
	{
		for (int obIter = 0; obIter < objects.Count; obIter++)
		{
			TreeGridObject ob = objects[obIter];
			if (obIter != connPointID.X)   // Not the same object
			{
				for (int cpIter = 0; cpIter < ob.connectionPoints.Count; cpIter++)
				{
					ConnectionPoint cp = ob.connectionPoints[cpIter];
					if (cp.mouseHover == true)	// check pressed connection point
						if (connPointID.X == -1)   // create first point to connection or another
						{
							connPointID = new IntPoint(obIter, cpIter);
							Point position = connPointPosition(ob, cpIter);
							connLine.X1 = position.X;
							connLine.Y1 = position.Y;
							connLine.X2 = connLine.X1;
							connLine.Y2 = connLine.Y1;

							return;
						}
						else if (ConnectionPoint.possibleConnections(cp, objects[connPointID.X].connectionPoints[connPointID.Y]))
						{
							connections.Add(new Connection(connPointID, new IntPoint(obIter, cpIter)));
							grid.Children.Add(connections[connections.Count-1].line);

							cp.fillColor = cp.borderColor;
							objects[connPointID.X].connectionPoints[connPointID.Y].fillColor =
								objects[connPointID.X].connectionPoints[connPointID.Y].borderColor;

							connPointIDDefaultValue();
							drawConnections();
							return;
						}
				}
			}
		}
		if (connPointID.X != -1)
			ConnectionPoint.connectionPointHover(objects[connPointID.X].connectionPoints[connPointID.Y], false);

		connPointIDDefaultValue();
	}

	private void drawConnections()
	{
		foreach(Connection c in connections)
		{
			Point p1 = connPointPosition(objects[c.connPoint1.X], c.connPoint1.Y);
			Point p2 = connPointPosition(objects[c.connPoint2.X], c.connPoint2.Y);

			Point bezier1 = new Point(p2.X, p1.Y);
			Point bezier2 = new Point(p1.X, p2.Y);

/*            Point bezier1 = new Point();
            Point bezier2 = new Point();
            if (p1.X <= p2.X)
            {
                bezier1 = new Point(p2.X, p1.Y);
                bezier2 = new Point(p1.X, p2.Y);
            }
            else if (p1.X >= p2.X)
            {
                bezier1 = new Point(p1.X + 150, p1.Y - 150);
                bezier2 = new Point(p2.X - 150, p2.Y - 150);
            }
            else if (p1.X > p2.X && p1.Y > p2.Y)
            {
                bezier1 = new Point(p1.X + 100, p1.Y + 100);
                bezier2 = new Point(p2.X - 100, p2.Y + 100);
            }*/

            c.line.Data = Geometry.Parse(
				"M" + ((int)p1.X).ToString() + ", " + ((int)p1.Y).ToString() +
				"C" + ((int)bezier1.X).ToString() + ", " + ((int)bezier1.Y).ToString() + " " +
				((int)bezier2.X).ToString() + ", " + ((int)bezier2.Y).ToString() + " " +
				((int)p2.X).ToString() + ", " + ((int)p2.Y).ToString());
		}
	}
	
	private Point connPointPosition(TreeGridObject ob, int cpIndex)
	{
		ConnectionPoint cp = ob.connectionPoints[cpIndex];
		int rowindex = Grid.GetRow(cp);

		double centerCircleX = 0;
		if (cp.connectionIO == ConnectionIO.Output)
		{
			centerCircleX = ob.ActualWidth - cp.circle.Margin.Right - cp.circle.ActualWidth / 2;
		}
		else if (cp.connectionIO == ConnectionIO.Input)
		{
			centerCircleX = cp.circle.Margin.Left + cp.circle.ActualWidth / 2;
		}

		double centerCircleY = ob.label.ActualHeight + 
			(cp.circle.Margin.Top + cp.circle.Margin.Bottom + cp.circle.ActualHeight) * rowindex +
			cp.circle.Margin.Top + cp.circle.ActualHeight / 2;

		return new Point(ob.Margin.Left + centerCircleX, ob.Margin.Top + centerCircleY);
	}
	private Rect selectObjectSpace(Point p1, Point p2)
	{
		/* p1 margin, p2 width & height */
		Rect r = new Rect();

		if (p1.X > p2.X)
		{
			r.X = p2.X;
			r.Width = p1.X - p2.X;
		}
		else
		{
			r.X = p1.X;
			r.Width = p2.X - p1.X;
		}

		if (p1.Y > p2.Y)
		{
			r.Y = p2.Y;
			r.Height = p1.Y - p2.Y;
		}
		else
		{
			r.Y = p1.Y;
			r.Height = p2.Y - p1.Y;
		}

		return r;
	}

	public void deleteConnection(int connectionIndex)
	{
		grid.Children.Remove(connections[connectionIndex].line);
		connections.Remove(connections[connectionIndex]);
	}
	public void checkDelete()
	{
		for (int i = 0; i < connections.Count; i++)
		{
			Connection cp = connections[i];

			if (cp.mouseBtnUp == true)
				deleteConnection(i);
		}
	}

	private void connPointIDDefaultValue()
    {
		connPointID = new IntPoint(-1, -1);
		connLine.X1 = 0;
		connLine.Y1 = 0;
		connLine.X2 = connLine.X1;
		connLine.Y2 = connLine.Y1;
	}
}
