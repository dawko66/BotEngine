using AutoBot_v1._1.Class.Data;
using AutoBot_v1._1.NewClass;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AutoBot_v1._1
{
	/// <summary>
	/// Logika interakcji dla klasy TreeGrid.xaml
	/// </summary>
	public partial class TreeGrid : UserControl
	{
		public int Size = 13;
		public List<BotElement> botElements = new List<BotElement>();
		public List<BlockControl> blockControls = new List<BlockControl>();
		public BlockControl blockControl = new BlockControl();
		 
		private bool Move = false;
		private Point PositionBuffer = new Point(-1, 0);
		private BlockControl ActiveBlock;

		private bool Down = false;
		private Point WindowPosition = new Point();
		//private Vector zoomMove = new Vector();

		public class BotElemEventArgs : EventArgs
		{
			public BotElement Value { get; set; }
		}
		public event EventHandler Select;
		protected virtual void SelectedBlock (BotElemEventArgs e)
		{
			Select?.Invoke(this, e);
		}

		public event EventHandler DeleteBlock;
		protected virtual void Delete(EventArgs e)
		{
			DeleteBlock?.Invoke(this, e);
		}

		private int ThisConnectionPointsId = 0;

		#region Connection variables
		private Connection NewConnect = new Connection(0, new BlockControl(), new BlockControl(), new ConnectionData(ConnectionTypes.None, 0, 0), new ConnectionData(ConnectionTypes.None, 0, 0));
		private bool ActiveConnect = false;
		private Point CursorDown;
		private List<Connection> connections;
		private BlockControl Point1;
		private ConnectionPoint Connection1;
        #endregion

        public TreeGrid()
		{
			InitializeComponent();

			botElements.Add(new BotElement() { Id = botElements.Count });
			connections = new List<Connection>();
			ShowConnections();
			ActiveConnection.Children.Add(NewConnect);

			MouseMove += (object sender, MouseEventArgs e) =>
			{
				/* Block move */
				if (Move == true)
				{
					Vector position = e.GetPosition(this) - PositionBuffer;

					Point blockPosition = new Point(ActiveBlock.Position.X + position.X, ActiveBlock.Position.Y + position.Y);
					ActiveBlock.Position = blockPosition;
					ActiveBlock.PhisicalPosition = blockPosition + (Vector)WindowPosition;

					botElements.Find(x => x.Id == ActiveBlock.BlockID).Poisition = blockPosition;

					PositionBuffer = e.GetPosition(this);
				}
				/* Window Move */
				else if (Down == true)
				{
					Vector position = e.GetPosition(this) - PositionBuffer;
					WindowPosition = new Point(WindowPosition.X + position.X, WindowPosition.Y + position.Y);

					foreach (BlockControl b in grid.Children)
					{
						b.PhisicalPosition = b.Position + (Vector)WindowPosition;
					}

					PositionBuffer = e.GetPosition(this);
				}
				if (ActiveConnect == true)
				{
					NewConnect.P2 = e.GetPosition(this);
				}
			};
			MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				if (ActiveConnect == true)
					CursorDown = e.GetPosition(this);

				var mouseWasDownOn = e.Source as FrameworkElement;
				if (mouseWasDownOn != null)
				{
					string elementName = mouseWasDownOn.Name;
				}
				PositionBuffer = e.GetPosition(this);
			};
			MouseUp += (object sender, MouseButtonEventArgs e) =>
			{
				var d = e.GetPosition(this);
				if (ActiveConnect == true)
				{
					ActiveConnect = false;
					NewConnect.Visibility = Visibility.Hidden;
				}

				Down = false;
			};
			MouseWheel += (object sender, MouseWheelEventArgs e) =>
			{
				/*				if (fal
				 *				se)
								{
									*//* set size *//*
									if (e.Delta > 0 && Size < 30)
										++Size;
									else if (e.Delta < 0 && Size > 1)
										--Size;

									foreach (NewClass.BlockControl b in grid.Children)
									{
										b.Size = Size;
										Point cursorPosition = e.GetPosition(this);
										Vector vector = (Vector)b.Position + (Vector)WindowPosition; //* - cursorPosition*//*;
										Vector zoomMove = new Vector(vector.X * ((Size - 13) + 15), vector.Y * ((Size - 13) + 15));
										//Point zoomPadding = new Point(WindowPosition.X * Size / cursorPosition.X, WindowPosition.Y * Size / cursorPosition.Y);


										b.PhisicalPosition = (Point)zoomMove;
									}
								}*/
			};
			MouseLeave += (object sender, MouseEventArgs e) =>
			{
				Move = false;
				Down = false;
			};
		}

		public BlockControl CreateBlock()
		{
			BlockControl block = new BlockControl();

			/* Delete Block with connection nodes */
			block.Close += (object sender, EventArgs e) =>
			{
				Blocks.Children.Remove((BlockControl)sender);
				//remove all connections
				var RemoveBotElem = BotElement.FindById(botElements, ((BlockControl)sender).BlockID);
				var AllConnections = connections.FindAll(x => x.C1.BlockId == RemoveBotElem.Id || x.C2.BlockId == RemoveBotElem.Id);

				foreach (var c in AllConnections)
				{
					// find bot data


					connections.Remove(c);
					Connections.Children.Remove(c);
				}
				botElements.Remove(BotElement.FindById(botElements, ((BlockControl)sender).BlockID));
				Delete(null);
			};
			/* Move block */
			block.MoveBlock += (object sender, EventArgs e) =>
			{
				Move = ((BlockControl.BoolEventArgs)e).Value;
				ActiveBlock = (BlockControl)sender;
			};
			block.ConPoint += (object sender, EventArgs e) =>
			{
				if (ActiveConnect == false)
				{
					Ellipse cp = (Ellipse)((BlockControl.ConnPointEventArgs)e).ConPoint.grid.Children[1];
					BlockControl b = (BlockControl)sender;
					NewConnect.P1 = cp.TransformToAncestor(this)
						.Transform(new Point(cp.Width / 2, cp.Height / 2));
					NewConnect.P2 = NewConnect.P1;
					ActiveConnect = true;
					NewConnect.Visibility = Visibility.Visible;
					Point1 = b;
					Connection1 = ((BlockControl.ConnPointEventArgs)e).ConPoint;
				}
				else
				{
					CreateCPHelper2(
						Point1,
						(BlockControl)sender,
						Connection1.connection,
						((BlockControl.ConnPointEventArgs)e).ConPoint.connection
						);
				}
			};


			return block;
		}

        public static void CreateBlock(TreeGrid treeGrid)
		{
			for (int i = 1; i < treeGrid.botElements.Count; i++)
			{
				var obj = treeGrid.botElements[i];
				var block = treeGrid.CreateBlock();
				treeGrid.blockControls.Add(block);
				block.Position = obj.Poisition;
				block.PhisicalPosition = obj.Poisition;

				Type thisType = block.GetType();
				MethodInfo theMethod = thisType.GetMethod(obj.GetType().Name);
				theMethod.Invoke(block, new[] { (BotElement)obj });

				block.MouseDown += (object sender, MouseButtonEventArgs e) =>
				{
					var d = treeGrid.botElements.Find(x => x.Id == ((BlockControl)sender).BlockID);
					treeGrid.SelectedBlock(new BotElemEventArgs() { Value = d } );
				};
				treeGrid.Blocks.Children.Add(block);
			}
		}
		public void RenderConnections()
		{

		}
		public void CreateCPHelper(BlockControl B1, BlockControl B2, ConnectionData C1, ConnectionData C2)
		{
			if (PConnection(C1.connectionType) == C2.connectionType)
				ObjectConnectionPoint(C1.ConnectionPointId, B1, B2, C1, C2);
			else if (PConnection(C2.connectionType) == C1.connectionType)
				ObjectConnectionPoint(C2.ConnectionPointId, B2, B1, C2, C1);
		}
		public void CreateCPHelper2(BlockControl B1, BlockControl B2, ConnectionData C1, ConnectionData C2)
		{
			if (PConnection(C1.connectionType) == C2.connectionType)
				CreateConnectionPoint(C1.ConnectionPointId, B1, B2, C1, C2);
			else if (PConnection(C2.connectionType) == C1.connectionType)
				CreateConnectionPoint(C2.ConnectionPointId, B2, B1, C2, C1);
		}
		public void ObjectConnectionPoint(int IdConnection, BlockControl B1, BlockControl B2, ConnectionData C1, ConnectionData C2)
		{
			connections.Add(new Connection(IdConnection, B1, B2, C1, C2));
			connections[connections.Count - 1].DeleteConnection += (object sender, EventArgs e) =>
			{

				Connections.Children.Remove(((Connection)sender));
			};
			Connections.Children.Add(connections[connections.Count - 1]);
		}
		public void CreateConnectionPoint(int IdConnection, BlockControl B1, BlockControl B2, ConnectionData C1, ConnectionData C2)
		{

		}

		public ConnectionTypes PConnection(ConnectionTypes c)
        {
			if (c == ConnectionTypes.Out) return ConnectionTypes.Inp;
			else if (c == ConnectionTypes.True) return ConnectionTypes.Inp;
			else if (c == ConnectionTypes.False) return ConnectionTypes.Inp;
			else if (c == ConnectionTypes.Point) return ConnectionTypes._Point;
			else if (c == ConnectionTypes.CurAct) return ConnectionTypes._CurAct;
			else if (c == ConnectionTypes.Key) return ConnectionTypes._Key;
			else if (c == ConnectionTypes.KeyAct) return ConnectionTypes._KeyAct;
			else if (c == ConnectionTypes.Text) return ConnectionTypes._Text;
			else if (c == ConnectionTypes.ArrPoint) return ConnectionTypes._ArrPoint;
			else if (c == ConnectionTypes.ArrColor) return ConnectionTypes._ArrColor;
			else if (c == ConnectionTypes.ColorPoint) return ConnectionTypes._ColorPoint;
			else if (c == ConnectionTypes.ArrColorPoint) return ConnectionTypes._ArrColorPoint;
			else if (c == ConnectionTypes.Area) return ConnectionTypes._Area;

			return ConnectionTypes.None;
		}

		public void ShowConnections()
        {
			/* load connection points */

			/*foreach(var i in be)
				grid.Children.Add(i[0].);*/

			ConnectionPoint cp = new ConnectionPoint(new ConnectionData(ConnectionTypes.Out, ThisConnectionPointsId++, 0), HorizontalAlignment.Left);
			cp.MouseUp += (object sender, MouseButtonEventArgs e) =>
            {

            };
			ConnPoints.Children.Add(cp);
        }

	}
}
