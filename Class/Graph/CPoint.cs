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
	public class CPoint : StackPanel
	{
		#region Title
		public string Title
		{
			get { return (string)TittleLabel.Content; }
			set { TittleLabel.Content = value; }
		}
		public void SetAutoTiitle()
		{
			TittleLabel.Content = (CArray == CArray.Yes ? "Array" : "") + CType.ToString();
		}
		#endregion

		#region Constructor variable
		// Public only read
		public CIO CIO
		{
			get { return _CIO; }
			set
			{
				_CIO = value;
			}
		}
		private CIO _CIO;

		public CArray CArray;
		public CType CType;

		public int Id;
		public int ParentId;

		public bool IsInfinite;
		public int InfiniteParentId;
		#endregion

		#region Element objects
		public Label TittleLabel;
		public Ellipse Circle;
		#endregion

		#region Component parameters

		#endregion

		public CPoint(int Id, int ParentId, CIO CIO, UIElement UIElement)
		{
			this.Id = Id;
			this.ParentId = ParentId;
			this.CIO = CIO;
			if (UIElement != null)
				Children.Add(UIElement);
		}
		public CPoint(int Id, int ParentId, CIO CIO, CArray CArray, CType CType, string Title = "", bool IsInfinite = false)
		{
			#region Generate element objects
			TittleLabel = new Label()
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Padding = new Thickness(0),
				Foreground = new SolidColorBrush(Colors.White),
				FontSize = 13,
				Margin = new Thickness(3, 0, 3, 0)
			};

			if (IsInfinite) TittleLabel.Foreground = new SolidColorBrush(Color.FromArgb(150, 200, 90, 90));

			Circle = new Ellipse()
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Width = 13,
				Height = 13,
				//Fill = new SolidColorBrush(Color.FromRgb(58, 58, 58)),
				Stroke = new SolidColorBrush(Color.FromArgb(200, 255, 250, 0)),
				//Margin = new Thickness(3, 5, 3, 5),
				StrokeThickness = 2,
			};
			Margin = new Thickness(3, 3, 3, 3);
			#endregion

			#region Constructor parameters
			this.Id = Id;
			this.ParentId = ParentId;
			this.CIO = CIO;
			this.CArray = CArray;
			this.CType = CType;

			if (Title == "")
				SetAutoTiitle();
			else this.Title = Title;

			if (CIO == CIO.Output)
			{
				Children.Add(TittleLabel);
				Children.Add(Circle);
			}
			else
			{
				Children.Add(Circle);
				Children.Add(TittleLabel);
			}

			this.IsInfinite = IsInfinite;

			#endregion

			#region This object parametrs
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;
			Orientation = Orientation.Horizontal;

			Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
			Cursor = Cursors.Hand;
			#endregion
		}


		public static Point CenterPoint(CPoint cPoint, Graph graph)
		{
			Ellipse el = cPoint.Children.OfType<Ellipse>().First();
			return el.TranslatePoint(new Point(el.Width / 2, el.Height / 2), graph.Container);
		}

		public static void VarPoint(ref CPoint cPoint, string Title)
		{
			if (cPoint.CType == CType.Var)
				cPoint.Title = Title;
		}

		public static bool IsPossibleConnection(CPoint cData1, CPoint cData2)
		{
			if (cData1.CIO != cData2.CIO)
				if (cData1.CArray == cData2.CArray || (cData1.CType == CType.Var || cData2.CType == CType.Var))
					if (cData1.CType == cData2.CType)
						return true;
					else if (cData1.CType == CType.Var || cData2.CType == CType.Var)
						return true;

			return false;
		}

		public static int GiveId(List<CPoint> cPoints)
		{
			int highestID = 0;
			foreach (CPoint cp in cPoints)
			{
				if (cp.Id > highestID) highestID = cp.Id;
			}
			return highestID + 1;
		}
	}
}
