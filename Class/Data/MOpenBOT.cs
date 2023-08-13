using AutoBot_2._0.Class.Data;
using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1;
using AutoBot_v1._1.Class.Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace AutoBot_2._0.Class.Graph
{
	[Serializable]
	public class MOpenBOT : BotElement
	{
		public const string name = "OpenBot"; // Name this class

		public List<(int BlockID, string Type, int cpID)> SubCP;
		public string Source
		{
			get { return _Source; }
			set
			{
				_Source = value;
			}
		}
		private string _Source;

		public int lastBlockIdResult;
		public MOpenBOT() { }

		public MOpenBOT(int Id)
		{
			this.Id = Id;
			Type = name;
			Name = Type + base.Id;
        }

		public static bool SetPatch(MOpenBOT b, Graph g)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "bot files (*.bot)|*.xml|All files (*.*)|*.*";
			openFileDialog.FilterIndex = 1;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.FileName = b.Source;
			if (openFileDialog.ShowDialog() == true)
			{
				b.Source = openFileDialog.FileName;
				return true;
			}
			return false;
		}

		public static void CheckSource(Graph graph)
		{
			// checking all sources of open bot blocks
			var obs = graph.BotData.BotElements.FindAll(x => x.Type == "OpenBot");

			foreach (MOpenBOT ob in obs)
			{
				if (!CheckSource(ob, graph))
					graph.RemoveConnection(ob.Id);
			}
		}
        public static bool CheckSource(MOpenBOT botElement, Graph graph)
        {
			if (!CheckSource1(botElement, graph))
            {
				botElement.CPDatas.Clear();
				botElement.Source = "";
				graph.RemoveConnection(botElement.Id);
				graph.Refresh();

				return false;
			}
            else
            {
				graph.Refresh();
				return true;
            }
		}
        public static bool CheckSource1(MOpenBOT botElement, Graph graph)
        {
			if (!CheckSourceExist(botElement.Source))
				return false;

			if (!CheckRecurence(botElement, new List<string>() { graph.Source }))
				return false;

			if (!SourceConnections(botElement, graph))
				return false;

			return true;
        }

		public static bool SourceConnections(MOpenBOT botElement, Graph graph)
		{
			try
			{
				BotData bd;
				Serializer.DeserializeObject(botElement.Source, out bd);

				// Find all connections to openbot block
				var conToOB = graph.BotData.CDatas.FindAll(x => x.BlockId1 == botElement.Id || x.BlockId2 == botElement.Id);
				// Find all connections to graph opened bot
				var conToG2 = bd.CDatas.FindAll(x => x.BlockId1 == 0 || x.BlockId2 == 0);

				// find every connection points in open bot connected to graph
				var sortedIds = sortIds(conToG2, 0);

				List<(int bId, int cpId, int cpId2)> sortIds(List<CData> cData, int noId)
                {
					var res = new List<(int bId, int cpId, int cpId2)>();
					foreach (var c in cData)
						if (c.BlockId1 == 0)
							res.Add((c.BlockId2, c.Id2, c.Id1));
						else
							res.Add((c.BlockId1, c.Id1, c.Id2));

					return res;
                }

				var cpdatas = new List<CPData>();
				var subCPs = new List<(int BlockID, string Type, int cpID)>();
				foreach (var c in conToG2)
				{
					int bId, cpId, cPId2;
					if (c.BlockId1 == 0)
					{
						bId = c.BlockId2;
						cpId = c.Id2;
						cPId2 = c.Id1;
					}
					else
					{
						bId = c.BlockId1;
						cpId = c.Id1;
						cPId2 = c.Id2;
					}

					foreach (BotElement bb in bd.BotElements)
						if (bb.Id == bId)
							foreach (CPData cpd in bb.CPDatas)
								if (cpd.Id == cpId)
                                {
									subCPs.Add((cpd.BlockId, bb.Type, cpd.Id));
									cpdatas.Add(new CPData(botElement.Id, cPId2, cpd.CIO, cpd.CArray, cpd.CType, cpd.Title));
								}
				}

				for (int i = 0; i < cpdatas.Count; i++)
                {
					CPData cp1 = cpdatas[i];
					var subCPs1 = subCPs[i];
					for (int j = 0; j < botElement.CPDatas.Count; j++)
					{
						CPData cp2 = botElement.CPDatas[j];
						var subCPs2 = botElement.SubCP[j];
						if (subCPs1.BlockID == subCPs2.BlockID && subCPs1.cpID == subCPs2.cpID &&
							new CPData(cp1.BlockId, cp1.Id, cp1.CIO, cp1.CArray, cp1.CType, cp1.Title, cp1.IsInfinite).Equals(
							new CPData(cp2.BlockId, cp2.Id, cp2.CIO, cp2.CArray, cp2.CType, cp2.Title, cp2.IsInfinite)))
						{
							botElement.SubCP.Remove(subCPs2);
							botElement.CPDatas.Remove(cp2);
							break;
						}
					}
				}

				int countOfRemovedCLines = 0;
				foreach (var e in conToOB)
                {
					var f = botElement.CPDatas.Find(x => (e.BlockId1 == x.BlockId || e.BlockId2 == x.BlockId) && (e.Id1 == x.Id || e.Id2 == x.Id));
					if (f != null)
                    {
						graph.RemoveConnection(e);
						countOfRemovedCLines++;
					}
				}

				if (countOfRemovedCLines > 0)
					Console.AddMessage("Inside Bot \"" + graph.Source + "\" in " + botElement.Name + " removed " + countOfRemovedCLines + " connection lines.");

				botElement.CPDatas = cpdatas;
				botElement.SubCP = subCPs;

				return true;
			}
			catch
			{
				Console.AddMessage("Open error >> " + botElement.Source);
				return false;
			}
		}
		public static bool CheckRecurence(MOpenBOT botElement, List<string> allSorces)
        {
			Console.BufforedMessage(botElement.Name + " Source: \"" + botElement.Source + "\" ->\n");

			if (allSorces.Contains(botElement.Source))
            {
				Console.AddBuffMessage(prefix: "Recurence ERROR:\nThis source: " + allSorces[0] + " ->\n", surfix:"...\nTry use Loop block.");
				return false;
            }

			allSorces.Add(botElement.Source);

			BotData bd;
			Serializer.DeserializeObject(botElement.Source, out bd);
			
			var bot = bd.BotElements.FindAll(x => x.Type == "OpenBot");

			foreach (var b in bot)
				if (!CheckRecurence((MOpenBOT)b, allSorces))
					return false;

			Console.ClearBuffMessage();
			return true;
		}
		public static bool CheckSourceExist(string source)
		{
			bool IsExists = File.Exists(source);
			if (!IsExists)
				Console.AddMessage("Can not find file >> \"" + source + "\"");
			return IsExists;
		}
	}
}