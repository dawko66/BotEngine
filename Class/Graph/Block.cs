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
	public class Block : Grid
	{
		#region Constructor variable
		public int Id { get; set; }
		public string Title;
		public List<CPoint> CPoints;
		public void AddCPoints(List<CPoint> CPoints, bool reverse = false)
		{
			this.CPoints = CPoints;
			DrawConnectionPoints(reverse);
		}
		public bool IsReverse;
		public static int BlockWidth = 250;
		#endregion

		#region Component parameters
		public Point Position
		{
			get { return new Point(Margin.Left, Margin.Top); }
			set { Margin = new Thickness(value.X, value.Y, 50, 50); }
		}
		public new double Width
		{
			get { return Border.Width; }
			set { Border.Width = value; }
		}
		#endregion

		#region Element objects
		public Label TitleLabel;
		public Grid Container;
		public Border Border;
		private Label DeleteBtn;
		#endregion

		#region Event
		public class APEventArgs : EventArgs
		{
			public CPoint Value { get; set; }
			public bool MouseDown { get; set; }
		}
		public event EventHandler TryConnect;
		protected virtual void ActivePoint(APEventArgs e)
		{
			TryConnect?.Invoke(e.Value, e);
		}

		public event EventHandler BlockMove;
		protected virtual void MoveEvent(MouseEventArgs e)
		{
			BlockMove?.Invoke(this, e);
		}

		public event EventHandler Delete;
		protected virtual void DeleteEvent(MouseEventArgs e)
		{
			Delete?.Invoke(this, e);
		}
		#endregion

		public Block()
		{
			Border = new Border()
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Background = new SolidColorBrush(Color.FromRgb(58, 58, 58)),
				BorderBrush = new SolidColorBrush(Color.FromRgb(247, 160, 26)),
				BorderThickness = new Thickness(0, 0, 0, 3),
			};
			Children.Add(Border);

			IsReverse = true;
		}

		public Block(int Id, string Title)
		{
			#region Generate element objects
			TitleLabel = new Label()
			{
				HorizontalContentAlignment = HorizontalAlignment.Center,
				VerticalContentAlignment = VerticalAlignment.Center,
				Background = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
				Foreground = new SolidColorBrush(Colors.White),
				Content = Title,
				FontSize = 14,
				Margin = new Thickness(0)
			};
			Grid.SetRow(TitleLabel, 0);
			Children.Add(TitleLabel);

			DeleteBtn = new Label()
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Center,
				VerticalContentAlignment = VerticalAlignment.Center,
				Background = new SolidColorBrush(Color.FromRgb(200, 80, 80)),
				Foreground = new SolidColorBrush(Colors.White),
				Content = "X",
				FontSize = 14,
				Width = 30,
				Cursor = Cursors.Hand
				//Margin = new Thickness(0)
			};
			Grid.SetRow(DeleteBtn, 0);
			Children.Add(DeleteBtn);


			Border = new Border()
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				Background = new SolidColorBrush(Color.FromRgb(58, 58, 58)),
				BorderBrush = new SolidColorBrush(Color.FromRgb(247, 160, 26)),
				BorderThickness = new Thickness(0, 0, 0, 3),
			};
			Grid.SetRow(Border, 1);
			Children.Add(Border);

			#endregion

			#region Constructor parameters
			this.Title = Title;
			this.Id = Id;
			#endregion

			#region This object parametrs
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;
			Background = new SolidColorBrush(Colors.Gray);
			RowDefinitions.Add(new RowDefinition());
			RowDefinitions.Add(new RowDefinition());

			Width = BlockWidth;
			RowDefinitions[0].Height = new GridLength(TitleLabel.FontSize * 2);

			TitleLabel.MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				MoveEvent(e);
			};

			DeleteBtn.MouseDown += (object sender, MouseButtonEventArgs e) =>
			{
				DeleteEvent(e);
			};
			#endregion
		}

		public CPoint CreateCPoint(CPData cPData)
		{
			if (cPData.Title != null)
			{
				CPoint cPoint = new CPoint(cPData.Id, cPData.BlockId, cPData.CIO, cPData.CArray, cPData.CType, cPData.Title, cPData.IsInfinite);
				cPoint.MouseDown += (object sender, MouseButtonEventArgs e) =>
				{
					ActivePoint(new APEventArgs() { Value = cPoint, MouseDown = true });
				};
				cPoint.MouseUp += (object sender, MouseButtonEventArgs e) =>
				{
					ActivePoint(new APEventArgs() { Value = cPoint, MouseDown = false });
				};
				return cPoint;
			}
			return new CPoint(cPData.Id, cPData.BlockId, cPData.CIO, /*cPData.UIElement*/null);
		}

		private void DrawConnectionPoints(bool reverse = false)
		{
			Container = new Grid();
			Border.Child = Container;

			int leftQuantity = 0;
			int rightQuantity = 0;

			// Add elements to left/right site
			foreach (CPoint cp in CPoints)
			{
				if (reverse ^ (cp.CIO == CIO.Input))
				{
					cp.HorizontalAlignment = HorizontalAlignment.Left;
					Grid.SetRow(cp, leftQuantity);
					Container.Children.Add(cp);
					leftQuantity++;
				}
				else
				{
					cp.HorizontalAlignment = HorizontalAlignment.Right;
					Grid.SetRow(cp, rightQuantity);
					Container.Children.Add(cp);
					rightQuantity++;
				}
			}

			// Add to grid new Row
			int higher = rightQuantity;
			if (leftQuantity > rightQuantity) higher = leftQuantity;

			for (int i = 0; i < higher; i++)
				Container.RowDefinitions.Add(new RowDefinition());

			//Height = (ConnectionPoint.size + ((Ellipse)CPoints[0].Children[0]).Margin.Top * 2) * higher + RowDefinitions[0].Height.Value + 3;
		}

		public static CPoint FindCPoint(List<Block> blocks, int blockId, int Id)
		{
			return FindCPoint(blocks.Find(x => x.Id == blockId), Id);
		}
		public static CPoint FindCPoint(Block block, int Id)
		{
			return block.CPoints.Find(x => x.Id == Id);
		}
		public static CPoint FindCPoint(List<CPoint> cPoints, int Id)
		{
			return cPoints.Find(x => x.Id == Id);
		}

		public void RedrawInfiniteCPoints(int Index, CPoint NewCPoint)
		{
			CPoints.Insert(Index, NewCPoint);
			RedrawCPoints();
		}
		public void RedrawCPoints()
		{
			Container.Children.Clear();
			DrawConnectionPoints(IsReverse);
			UpdateLayout();
		}

		public void RemoveCpoint(CPoint cPoint)
		{
			CPoints.Remove(cPoint);
			Container.Children.Remove(cPoint);
			UpdateLayout();
		}
	}
}
