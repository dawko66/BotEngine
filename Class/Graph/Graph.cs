using AutoBot_2._0.Class.Data;
using AutoBot_v1._1.Class.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AutoBot_2._0.Class.Graph
{
	public class Graph : ScrollViewer
	{

		#region Constructor variable
		public BotData BotData;
		//public List<BotElement> BotElements;
		//public List<CData> CDatas;
		public string Source;
		#endregion

		#region Graph data lists
		private List<CLine> CLines
		{
			get { return CLinesLayer.Children.Cast<CLine>().ToList(); }
		}
		public CData AddCLine(CLine CLine)
		{
			CLines.Add(CLine);
			CLinesLayer.Children.Add(CLine);
			return CLine.CData;
		}

		public List<Block> Blocks
		{
			get { return BlocksLayer.Children.Cast<Block>().ToList(); }
		}
		public void AddBlocks(Block Block)
		{
			Blocks.Add(Block);
			BlocksLayer.Children.Add(Block);
		}
		#endregion

		#region Element objects
		private Grid BlocksLayer;
		private Grid CLinesLayer;
		private CLine CLine;    // Create connection line
		//private BotElement BotElement;
		public Grid Container;
		#endregion

		#region Try connect
		private CPoint ActiveCPoint;
		private CPoint ActiveCPoint2;
		// nie działa jak należy
		private bool IsPointClick;
		private bool helper;
		#endregion

		#region Move block
		private bool IsMouseDown;
		private Block ActiveBlock;
		private Point LastMousePosition;
		#endregion

		#region Event
		public event EventHandler Selected;
		protected virtual void SelectedEvent(Block b, MouseEventArgs e)
		{
			Selected?.Invoke(b, e);
		}
		#endregion

		public Graph()
		{
			#region Constructor variable
			VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
			#endregion

			#region Generate element objects
			Container = new Grid();
			Content = Container;

			BlocksLayer = new Grid()
			{

			};
			Container.Children.Add(BlocksLayer);

			CLinesLayer = new Grid()
			{

			};
			Container.Children.Add(CLinesLayer);

			CLine = new CLine(null)
			{
				IsHitTestVisible = false,
			};
			Container.Children.Add(CLine);

			BotData = new BotData()
			{
				BotElements = new List<BotElement>(),
				CDatas = new List<CData>()
			};

			BotElement be = GraphBlock();

			BotData.BotElements.Add(be);
			Block block = CreateReverseBlock(be);

			AddBlocks(block);
			#endregion

			#region This object parametrs
			Container.Background = new SolidColorBrush(Color.FromRgb(27, 27, 27));
			#endregion

			#region Events
			Container.MouseMove += Graph_MouseMove;
			Container.MouseDown += Graph_MouseDown;
			Container.MouseUp += Graph_MouseUp;
			Container.MouseLeave += (object sender, MouseEventArgs e) =>
			{
				IsMouseDown = false;
				CancelConnect();
			};
            ScrollChanged += Graph_ScrollChanged;
			#endregion
		}

        public void Graph_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
			((Block)BlocksLayer.Children[0]).Margin = new Thickness(HorizontalOffset, VerticalOffset, 0, 0);
			((Block)BlocksLayer.Children[0]).Width = ViewportWidth;
			Container.UpdateLayout();

			RedrawCLines(0);
		}

        private void Graph_MouseUp(object sender, MouseButtonEventArgs e)
		{
			// Try connect
			if (helper == true) IsPointClick = true;
			else IsPointClick = false;

			if (IsMouseDown == true)
				if (ActiveCPoint2 != null)
				{
					//create
					CreateConnection();
					CancelConnect();
				}
				else
				{
					//delete
					CancelConnect();
				}

			// Move Block
			IsMouseDown = false;
			ActiveBlock = null;
		}
		private void Graph_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// Move block
			IsMouseDown = true;
			LastMousePosition = e.GetPosition(Container); // (Grid)Parent

			// Try connect
			helper = true;
			if (IsPointClick == true)
				if(ActiveCPoint2 != null)
				{
					//create
					CreateConnection();
					CancelConnect();
				}
				else
				{
					//delete
					CancelConnect();
				}

			IsPointClick = false;

			SelectedEvent(ActiveBlock, e);
		}
		private void Graph_MouseMove(object sender, MouseEventArgs e)
		{
			// Try connect
			helper = false;
			IsPointClick = false;
			if (ActiveCPoint != null)
			{
				CLine.P1 = CPoint.CenterPoint(ActiveCPoint, this);
				CLine.P2 = new Point(((MouseEventArgs)e).GetPosition(Container).X, ((MouseEventArgs)e).GetPosition(Container).Y - 1);
			}

			// Block move
			if (IsMouseDown == true && ActiveBlock != null && ActiveCPoint == null)
			{
				Point actualPosition = e.GetPosition(Container); // (Grid)Parent

				ActiveBlock.Margin = new Thickness(
					ActiveBlock.Margin.Left - (LastMousePosition.X - actualPosition.X),
					ActiveBlock.Margin.Top - (LastMousePosition.Y - actualPosition.Y),
					ActiveBlock.Margin.Right,
					ActiveBlock.Margin.Bottom);

				LastMousePosition = actualPosition;
				BotData.BotElements.Find(x => x.Id == ActiveBlock.Id).Poisition = new Point(ActiveBlock.Margin.Left, ActiveBlock.Margin.Top);
				
				Container.UpdateLayout();
				RedrawCLines(ActiveBlock);
			}
		}
		
		private void CancelConnect()
		{
			CLine.P1 = new Point();
			CLine.P2 = new Point();
			ActiveCPoint = null;
			ActiveCPoint2 = null;
		}

		public void ClearGraph()
		{
			CLinesLayer.Children.Clear();
			BlocksLayer.Children.Clear();
		}

		public void RemoveConnection(CData cData)
		{
			// Set connected type Var
			Block b1 = Blocks.Find(x => x.Id == cData.BlockId1);
			Block b2 = Blocks.Find(x => x.Id == cData.BlockId2);

			CPoint c1 = b1.CPoints.Find(x => x.Id == cData.Id1);
			CPoint c2 = b2.CPoints.Find(x => x.Id == cData.Id2);

			if (c1.CType == CType.Var ) c1.Title = CType.Var.ToString();
			else if (c2.CType == CType.Var ) c2.Title = CType.Var.ToString();

			// remove "single" connection
			int count = BotData.CDatas.RemoveAll(x => x == cData);
			if (count > 0)
				CLinesLayer.Children.RemoveAt(CLinesLayer.Children.OfType<CLine>().ToList().FindIndex(x => x.CData == cData));

			// remove infinite point
			void removeInfinite(CPoint cp, Block b)
			{
				if (cp.IsInfinite)
				{
					b.CPoints.Remove(cp);
					b.RedrawCPoints();
					BotData.BotElements.Find(x => x.Id == cp.ParentId).CPDatas.RemoveAll(x => x.Id == cp.Id);
					
					RedrawCLines(b);
				}
			}

			removeInfinite(c1, b1);
			removeInfinite(c2, b2);
		}
		public void RemoveConnection(int BlockId)
		{
			// Remove all connections to block at this ID
			var d = BotData.CDatas.FindAll(x => x.BlockId1 == BlockId || x.BlockId2 == BlockId);
			foreach (CData cd in d)
				RemoveConnection(cd);
		}

		public void RemoveBlock(int BlockId)
		{
			// remove connection to block
			RemoveConnection(BlockId);
			
			//Blocks.FindAll(x => x.Id == BlockId).ForEach(x => x.Children.Clear());
			Blocks.RemoveAll(x => x.Id == BlockId);

			BlocksLayer.Children.RemoveAt(BlocksLayer.Children.OfType<Block>().ToList().FindIndex(x => x.Id == BlockId));
			BotData.BotElements.RemoveAll(x => x.Id == BlockId);
		}

		public void RemoveAllBlocks()
		{
			Blocks.ForEach(x => RemoveBlock(x.Id));
		}

		public void RenderBlocks()
		{
			foreach (BotElement be in BotData.BotElements)
			{
				if (be.Id != 0) AddBlocks(CreateBlock(be));
				else AddBlocks(CreateReverseBlock(be));
			}
			Container.UpdateLayout();
		}
		public Block CreateBlock(BotElement botElement)
		{
			Block block = new Block(botElement.Id, botElement.Name) { Position = botElement.Poisition };

			List<CPoint> cPoints = new List<CPoint>();

			foreach (CPData cpd in botElement.CPDatas)
			{
				cPoints.Add(block.CreateCPoint(cpd));
			}

			block.AddCPoints(cPoints);
			block.TryConnect += TryConnect;
			block.BlockMove += (object sender, EventArgs e) => { 
				ActiveBlock = (Block)sender; 
			};
			block.Delete += (object sender, EventArgs e) =>
			{
				RemoveBlock(((Block)sender).Id);
			};

			return block;
		}
		public Block CreateReverseBlock(BotElement botElement)
		{
			Block block = new Block() { Position = botElement.Poisition };

			block.Background = Brushes.Transparent;
			block.Border.Background = Brushes.Transparent;
			block.Border.BorderThickness = new Thickness(0);
			//block.Width = ActualWidth;

			List<CPoint> cPoints = new List<CPoint>();

			foreach (CPData cpd in botElement.CPDatas)
			{
				cPoints.Add(block.CreateCPoint(cpd));
			}

			block.AddCPoints(cPoints, true);
			block.TryConnect += TryConnect;
			block.Delete += (object sender, EventArgs e) =>
			{
				RemoveBlock(((Block)sender).Id);
			};

			return block;
		}

		public void RenderCLines()
		{
			foreach (CData cd in BotData.CDatas)
			{
				AddCLine(CreateCLine(cd, false));
			}
			Container.UpdateLayout();
		}
		public CLine CreateCLine(CData cData, bool InfCPoint = true)
		{
			CPoint cp1 = Block.FindCPoint(Blocks, cData.BlockId1, cData.Id1);
			CPoint cp2 = Block.FindCPoint(Blocks, cData.BlockId2, cData.Id2);

			#region Infinite connection create new subpoint
			if (cp1.IsInfinite == true && InfCPoint)
			{
				cp1 = InfiniteCPoint(cp1, cp2);

				BotElement bot = BotData.BotElements.Find(x => x.Id == cp1.ParentId);
				bot.CPDatas.Insert(bot.CPDatas.FindIndex(x => x.Id == cData.Id1),
					new CPData(cp1.ParentId, CPData.GiveID(bot.CPDatas), cp1.CIO, cp1.CArray, cp1.CType, "", true));

				cData.Id1 = cp1.Id;
				RedrawCLines(cData.BlockId1);
			}

			if (cp2.IsInfinite == true && InfCPoint)
			{
				cp2 = InfiniteCPoint(cp2, cp1);

				BotElement bot = BotData.BotElements.Find(x => x.Id == cp2.ParentId);
				bot.CPDatas.Insert(bot.CPDatas.FindIndex(x => x.Id == cData.Id2),
					new CPData(cp2.ParentId, CPData.GiveID(bot.CPDatas), cp2.CIO, cp2.CArray, cp2.CType, "", true));

				cData.Id2 = cp2.Id;
				RedrawCLines(cData.BlockId2);
			}
			#endregion

			CPoint.VarPoint(ref cp1, cp2.Title);
			CPoint.VarPoint(ref cp2, cp1.Title);

			Container.UpdateLayout();
			CLine cLine = new CLine(cData);
			cLine.P1 = CPoint.CenterPoint(cp1, this);
			cLine.P2 = CPoint.CenterPoint(cp2, this);
			cLine.Delete += (object sender, EventArgs e) =>
			{
				RemoveConnection(((CLine)sender).CData);
			};

			return cLine;
		}

		private void TryConnect(object sender, EventArgs e)
		{
			if (sender != null)
				if (ActiveCPoint == null)
				{
					if (((Block.APEventArgs)e).MouseDown == true)
						ActiveCPoint = (CPoint)sender;
				}
				else if ((CPoint)sender != ActiveCPoint)
					ActiveCPoint2 = (CPoint)sender;
		}
		private void CreateConnection()
		{
			if (CPoint.IsPossibleConnection(ActiveCPoint, ActiveCPoint2) && ActiveCPoint.ParentId != ActiveCPoint2.ParentId)
			{
                CData cData = new CData(
                    ActiveCPoint.Id, ActiveCPoint.ParentId,
                    ActiveCPoint2.Id, ActiveCPoint2.ParentId);

				BotData.CDatas.Add(AddCLine(CreateCLine(cData)));
			}
			else
				CancelConnect();
		}
		private CPoint InfiniteCPoint(CPoint InfiniteCPoint, CPoint cp2)
		{
			if (InfiniteCPoint.CArray == CArray.Yes && InfiniteCPoint.CType == CType.Var)
			{
				InfiniteCPoint.CType = cp2.CType;
				InfiniteCPoint.CArray = cp2.CArray;
				InfiniteCPoint.SetAutoTiitle();
			}

			Block b = Blocks.Find(x => x.Id == InfiniteCPoint.ParentId);
			CPoint cp = new CPoint(CPoint.GiveId(b.CPoints), b.Id, InfiniteCPoint.CIO, InfiniteCPoint.CArray, InfiniteCPoint.CType, "", InfiniteCPoint.IsInfinite) { InfiniteParentId = InfiniteCPoint.Id };

			b.RedrawInfiniteCPoints(b.CPoints.FindIndex(x => x.Id == InfiniteCPoint.Id), cp);
			Container.UpdateLayout();
			return cp;
		}

		public void RedrawCLines(int BlockId)
		{
			List<CLine> list = CLines.FindAll(x => x.CData.BlockId1 == BlockId || x.CData.BlockId2 == BlockId);
			foreach (Block b in Blocks)
				if (b.Id == BlockId)
					foreach (CLine s in list)
						if (b.Id == s.CData.BlockId1)
						{
							foreach (CPoint cp in b.CPoints)
								if (cp.Id == s.CData.Id1)
									s.P1 = CPoint.CenterPoint(cp, this);
						}
						else if (b.Id == s.CData.BlockId2)
						{
							foreach (CPoint cp in b.CPoints)
								if (cp.Id == s.CData.Id2)
									s.P2 = CPoint.CenterPoint(cp, this);
						}
		}
		public void RedrawCLines(Block b)
		{
			List<CLine> list = CLines.FindAll(x => x.CData.BlockId1 == b.Id || x.CData.BlockId2 == b.Id);
			foreach (CLine s in list)
				if (b.Id == s.CData.BlockId1)
				{
					foreach (CPoint cp in b.CPoints)
						if (cp.Id == s.CData.Id1)
							s.P1 = CPoint.CenterPoint(cp, this);
				}
				else if (b.Id == s.CData.BlockId2)
				{
					foreach (CPoint cp in b.CPoints)
						if (cp.Id == s.CData.Id2)
							s.P2 = CPoint.CenterPoint(cp, this);
				}
		}

		public void Refresh()
        {
			ClearGraph();
			RenderBlocks();
			RenderCLines();
			Graph_ScrollChanged(null, null);
			UpdateLayout();
        }

		public static BotElement GraphBlock()
        {
			BotElement BotElement = new BotElement()
			{
				Id = 0,
				Poisition = new Point(0, 0)
			};
			List<CPData> CPDatas = new List<CPData>();
			CPDatas.Add(new CPData(BotElement.Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Var, "", true));
			CPDatas.Add(new CPData(BotElement.Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Var, "", true));
			BotElement.CPDatas = CPDatas;

			return BotElement;
		}
		public void NewProject()
        {
			RemoveAllBlocks();
			BotData.CDatas.Clear();

			//GraphBlock();
		}
	}
}
