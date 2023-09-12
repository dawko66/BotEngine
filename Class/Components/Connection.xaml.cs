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
	/// Logika interakcji dla klasy Connection.xaml
	/// </summary>
	public partial class Connection : UserControl
	{
		public Point P1
		{
			get
			{
				return _P1;
			}
			set
			{
				_P1 = value;
				Draw();
			}
		}
		private Point _P1 = new Point();
		public Point P2
		{
			get
			{
				return _P2;
			}
			set
			{
				_P2 = value;
				Draw();
			}
		}
		private Point _P2 = new Point();

		public int IdConnection;

		public BlockControl Block1;
		public BlockControl Block2;
		public ConnectionData C1;
		public ConnectionData C2;

		public event EventHandler DeleteConnection;
		protected virtual void Delete(EventArgs e)
		{
			DeleteConnection?.Invoke(this, e);
		}

		private void Draw()
		{
			Point bezier1 = new Point(P2.X, P1.Y);
			Point bezier2 = new Point(P1.X, P2.Y);
			path.Data = Geometry.Parse(
				"M" + ((int)P1.X).ToString() + ", " + ((int)P1.Y).ToString() +
				"C" + ((int)bezier1.X).ToString() + ", " + ((int)bezier1.Y).ToString() + " " +
				((int)bezier2.X).ToString() + ", " + ((int)bezier2.Y).ToString() + " " +
				((int)P2.X).ToString() + ", " + ((int)P2.Y).ToString());
		}
		public Connection(int IdConnection, BlockControl Block1, BlockControl Block2, ConnectionData C1, ConnectionData C2)
		{
			InitializeComponent();

			this.IdConnection = IdConnection;
			this.Block1 = Block1;
			this.Block2 = Block2;
			this.C1 = C1;
			this.C2 = C2;

			P1 = FindPoints(Block1, C1);
			P2 = FindPoints(Block2, C2);

			Block1.MouseMove += (object sender, MouseEventArgs e) =>
			{
				P1 = FindPoints(Block1, C1);
			};
			Block2.MouseMove += (object sender, MouseEventArgs e) =>
			{
				P2 = FindPoints(Block2, C2);
			};

			bool IsMouseDown = false;
			MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				IsMouseDown = true;
			};
			MouseUp += (object sender, MouseButtonEventArgs e) =>
			{
				if (IsMouseDown == true)
				{
					Delete(null);
				}

				IsMouseDown = false;
			};
		}

		public Point FindPoints(BlockControl Block1, ConnectionData C1)
		{
			for (int i = 0; i < Block1.content.Children.Count; i++)
			{
				var d = Block1.content.Children[i];
				if (d.GetType().Name == "ConnectionPoint")
				{
					var con = (ConnectionPoint)d;
					//var label = (Label)con.grid.Children[0];
					var point = (Ellipse)con.grid.Children[1];
					
					if (con.connection.ConnectionPointId == C1.ConnectionPointId)
					{
						Point relativePoint = point.TransformToAncestor(Block1)
							  .Transform(new Point(point.Width/2, point.Height/2));
						return Block1.PhisicalPosition + (Vector)relativePoint;
					}
				}
			}
			return new Point();
		}
	}
}
