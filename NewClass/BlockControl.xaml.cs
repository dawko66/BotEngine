using AutoBot_v1._1.Class.Data;
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

namespace AutoBot_v1._1.NewClass
{
	/// <summary>
	/// Logika interakcji dla klasy BlockControl.xaml
	/// </summary>
	/// 
	public enum ConnectionTypes
	{
		None, Var,
		Inp, Out, True, False,
		Point, ArrPoint, _Point, _ArrPoint,
		CurAct, KeyAct, _CurAct, _KeyAct,
		Key, Text, _Key, _Text,
		Color, ArrColor, _Color, _ArrColor,
		ColorPoint, ArrColorPoint, _ColorPoint, _ArrColorPoint,
		Area, _Area
	}
	public partial class BlockControl : UserControl
	{
		public int Size
		{
			get { return _Size; }
			set
			{
				_Size = value;
				Width = 15 * value;
				this.FontSize = value;
				close.Width = 2 * value;

				border.Height = (int)(.3 * value);

				foreach (var i in content.Children)
					foreach (var c in ((Grid)i).Children)
						if (c.GetType().Name == "Ellipse")
						{
							((Ellipse)c).Width = value;
							((Ellipse)c).Height = value;
							((Ellipse)c).Margin = new Thickness(.23 * FontSize, .38 * FontSize, .23 * FontSize, .38 * FontSize);
						}
			}
		}
		private int _Size = 13;

		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title = value;
				type.Content = value;
			}

		}
		private string _Title = "Undefined";
		public Point Position
		{
			get { return _Position; }
			set {
				_Position = value;

			}
		}
		Point _Position = new Point();

		public int BlockID;

		public Point PhisicalPosition
		{
			get
			{
				return _PhisicalPosition;
			}
			set
			{
				_PhisicalPosition = value;
				Margin = new Thickness(value.X, value.Y, 0, 0);
			}
		}
		private Point _PhisicalPosition;

		public bool Move = false;

		private int LeftColumnCount;
		private int RightColumnCount;

		#region Events
		public event EventHandler Close;
		protected virtual void CloseFunction(EventArgs e)
		{
			Close?.Invoke(this, e);
		}

		public class BoolEventArgs : EventArgs
		{
			public bool Value { get; set; }
		}
		public event EventHandler MoveBlock;
		protected virtual void MoveBool(BoolEventArgs e)
		{
			MoveBlock?.Invoke(this, e);
		}

		public class ConnPointEventArgs : EventArgs
		{
			public ConnectionPoint ConPoint { get; set; }
		}
		public event EventHandler ConPoint;
		protected virtual void GetConPoint(ConnPointEventArgs e)
		{
			ConPoint?.Invoke(this, e);
		}
		#endregion

		public BlockControl()
		{
			InitializeComponent();

			Size = 13;

			content.Background = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));

			type.MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				MoveBool(new BoolEventArgs() { Value = true });
			};
			type.MouseUp += (object sender, MouseButtonEventArgs e) =>
			{
				MoveBool(new BoolEventArgs() { Value = false });
			};
			
			type.Content = Title;
			
			bool click = false;
			close.MouseMove += (object sender, MouseEventArgs e) =>
			{
				close.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
			};
			close.MouseLeave += (object sender, MouseEventArgs e) =>
			{
				click = false;
				close.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
			};
			close.MouseUp += (object sender, MouseButtonEventArgs e) =>
			{
				if (click == true)
				{
					/* delete this object */
					CloseFunction(null);
					click = false;
				}
			};
			close.MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				click = true;
			};
		}

		public void ContentElement(UIElement ui, HorizontalAlignment horizontalAlignment)
		{
			if (content.RowDefinitions.Count >= RightColumnCount ||
				content.RowDefinitions.Count >= LeftColumnCount)
				content.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

			if (horizontalAlignment == HorizontalAlignment.Right)
				Grid.SetRow(ui, ++RightColumnCount);
			else if (horizontalAlignment == HorizontalAlignment.Left)
				Grid.SetRow(ui, ++LeftColumnCount);

			content.Children.Add(ui);
		}

		public static BlockControl FindById(List<BlockControl> b, int Id)
		{
			return b.Find(x => x.BlockID == Id);
		}
		public ConnectionPoint Connection(ConnectionData connection, HorizontalAlignment horizontalAlignment)
		{
			ConnectionPoint conn = new ConnectionPoint(connection, horizontalAlignment);
			conn.MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				GetConPoint(new ConnPointEventArgs() { ConPoint = (ConnectionPoint)sender });
			};
			conn.MouseUp += (object sender, MouseButtonEventArgs e) =>
			{
				GetConPoint(new ConnPointEventArgs() { ConPoint = (ConnectionPoint)sender });
			};
			return conn;
		}

		#region Controls
		public void MCursor(BotElement botElem)
		{
			Title = botElem.Type;
			BlockID = botElem.Id;
			Position = botElem.Poisition;
			MConnection(ConnectionTypes.Inp, HorizontalAlignment.Left);
			MConnection(ConnectionTypes.Out, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.Point, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._Point, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.CurAct, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._CurAct, HorizontalAlignment.Right);
		}
		public void MKey(BotElement botElem)
		{
			Title = botElem.Type;
			BlockID = botElem.Id;
			Position = botElem.Poisition;
			MConnection(ConnectionTypes.Out, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.Inp, HorizontalAlignment.Left);
			MConnection(ConnectionTypes.Key, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._Key, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.KeyAct, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._KeyAct, HorizontalAlignment.Right);
		}
		public void MText(BotElement botElem)
		{
			Title = botElem.Type;
			BlockID = botElem.Id;
			Position = botElem.Poisition;
			MConnection(ConnectionTypes.Inp, HorizontalAlignment.Left);
			MConnection(ConnectionTypes.Out, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.Text, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._Text, HorizontalAlignment.Right);
		}
		public void MScanColor(BotElement botElem)
		{
			Title = botElem.Type;
			BlockID = botElem.Id;
			Position = botElem.Poisition;
			MConnection(ConnectionTypes.Inp, HorizontalAlignment.Left);
			MConnection(ConnectionTypes.True, HorizontalAlignment.Right);
			ContentElement(new Label(), HorizontalAlignment.Left);
			MConnection(ConnectionTypes.False, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.ArrPoint, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._ArrPoint, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.ArrColor, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._ArrColor, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.ColorPoint, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._ColorPoint, HorizontalAlignment.Right);
			MConnection(ConnectionTypes._ColorPoint, HorizontalAlignment.Right);
		}
		public void MScanAreaColor(BotElement botElem)
		{
			Title = botElem.Type;
			BlockID = botElem.Id;
			Position = botElem.Poisition;
			MConnection(ConnectionTypes.Inp, HorizontalAlignment.Left);
			MConnection(ConnectionTypes.True, HorizontalAlignment.Right);
			ContentElement(new Label(), HorizontalAlignment.Left);
			MConnection(ConnectionTypes.False, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.Area, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._Area, HorizontalAlignment.Right);
			MConnection(ConnectionTypes.ArrColorPoint, HorizontalAlignment.Left);
			MConnection(ConnectionTypes._ArrColorPoint, HorizontalAlignment.Right);
		}

		public void MConnection(ConnectionTypes c, HorizontalAlignment h)
        {
			ContentElement(Connection(new ConnectionData(c, content.Children.Count, BlockID), h), h);
		}
		#endregion
	}
}
