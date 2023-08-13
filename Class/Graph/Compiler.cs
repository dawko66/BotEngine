using AutoBot_2._0.Class.Data;
using AutoBot_v1._1.Class.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static MMultiplexer;

namespace AutoBot_2._0.Class.Graph
{
	class Compiler : Window
	{
		public BotData botData;

		private HotKeys hotKeys;
		private Thread task;
		private CancellationTokenSource ts;
		private bool wait;

		private Label timeStartLabel;
		private Label workTimeLabel;
		private DateTime startedTime;

		private ListBox startAt;

		public Compiler(BotData botData)
		{
			Width = 200;
			Height = 205;
			Title = "Compiler";
			WindowStyle = WindowStyle.ToolWindow;
			ResizeMode = ResizeMode.NoResize;
			
			this.botData = botData;

			#region window elements
			Grid container = new Grid();
			container.RowDefinitions.Add(new RowDefinition());
			container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });

			container.ColumnDefinitions.Add(new ColumnDefinition());
			container.ColumnDefinitions.Add(new ColumnDefinition());

			Content = container;

			Button startBtn = new Button() { Content = "Start (Ctrl + F1)" };

			startBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				Start();
			};
			Button stopBtn = new Button() { Content = "Stop (Ctrl + F2)" };
			stopBtn.Click += (object sender, RoutedEventArgs e) =>
			{
				Stop();
			};

			timeStartLabel = new Label() { Content = "Started at:" };
			workTimeLabel = new Label() { Content = "Work time:" };  // time to finish

			startAt = new ListBox()
			{
				
			};	// list of next connected to graph

			startAt.Items.Add("item1");
			startAt.Items.Add("item2");
			startAt.Items.Add("item3");
			startAt.SelectedItem = 0;
			
			Grid.SetColumn(startBtn, 0);
			Grid.SetColumn(stopBtn, 1);

			Grid.SetColumnSpan(timeStartLabel, 2);
			Grid.SetColumnSpan(workTimeLabel, 2);
			
			Grid.SetColumnSpan(startAt, 2);

			Grid.SetRow(startBtn, 3);
			Grid.SetRow(stopBtn, 3);

			Grid.SetRow(timeStartLabel, 2);
			Grid.SetRow(workTimeLabel, 1);

			Grid.SetRow(startAt, 0);

			container.Children.Add(startBtn);
			container.Children.Add(stopBtn);
			container.Children.Add(timeStartLabel);
			container.Children.Add(workTimeLabel);
			container.Children.Add(startAt);
			#endregion

			/*
			rekurencyjne wywoływanie otwieranie botów
			{
				bot w ścieżce otwiera tego samego bota (w nieskończoność)
			}
			przechodzenie do następnego bloku
			{
				po wykonywaniu akcji wyszukiwany jest następny blok do wykonania (next,out)
			}
			aktualizowanie danych
			{
				podczas wykonywania akcji lub po jej zakończeniu dane bloku zwracające wartości zostają przypisywane
			}
			przekazywanie danych
			{
				po zakończeniu akcji bloku rozsyłąne są dane do połączonych bloków,
				bloki danych (bez akcji) wykonywane są na samym początku przed rozpoczęciem
			}
			 */

			//BotData bd = botData;

			/*			for (int i = 0; i > 100; i++)
							if (!MergeOpenBot(ref bd))
								break;


						Serializer.SerializeObject(bd, "daw.xml");*/

			//CodeAnalyser(bd);
		}

		private async void Start()
		{
			// start pause
			if (task == null || task.ThreadState != ThreadState.Running)
			{
				startedTime = DateTime.Now;
				timeStartLabel.Content = "Started at: " + startedTime.ToString();
				ts = new CancellationTokenSource();
				CancellationToken ct = ts.Token;

				// clone
				BotData newBd = (BotData)Serializer.Clone(botData);


				try
				{
					task = new Thread(() => CodeAnalyser(newBd, task, wait, ct));
					task.Start();

				}
				catch
				{

				}
			}
			else if (wait)
			{
				wait = false;
			}
			else
			{
				wait = true;
			}


/*
			finally
			{
				ts.Dispose();
			}*/
		}
		private void Stop()
		{
			// stop
			try
			{
				task.Abort();
				ts.Cancel();
				wait = false;
			}
			catch { }
		}

		public static (int, List<VariableData>) CodeAnalyser(BotData botData, Thread task, bool wait, CancellationToken token)
		{
			/*
			 zwraca punkt do którego został podłączony ostatni wywołany element
			 */

			int CurrentlyUsedBlockId = 0;
			int returnVal = -1;
			// execute all data blocks
			// execute graph data

			// Find current block by ID
			var be = BotElement.FindById(botData.BotElements, CurrentlyUsedBlockId);

			while (be != null)
			{
				(be, returnVal) = BlockAnalyser(be, botData, task, wait, token);

				//if (token.IsCancellationRequested || be == null)
					//task.Abort();
					//token.ThrowIfCancellationRequested();
			}

			return (returnVal, botData.BotElements.Find(x => x.Id == 0).Variables);
		}


		private static (BotElement, int) BlockAnalyser(BotElement be, BotData bd, Thread task, bool wait, CancellationToken token)
		{
			#region delay sleep
			TimeSpan delay;
			if (be.IsDelay)
				delay = new TimeSpan(
					0,
					new Random(9573).Next(be.DelayAt.Hours, be.DelayTo.Hours),
					new Random(23478).Next(be.DelayAt.Minutes, be.DelayTo.Minutes),
					new Random(98554).Next(be.DelayAt.Seconds, be.DelayTo.Seconds),
					new Random(2344578).Next(be.DelayAt.Milliseconds, be.DelayTo.Milliseconds)
				);
			else
				delay = be.DelayAt;

			var watch = System.Diagnostics.Stopwatch.StartNew();
			while (true)
			{
				Thread.Sleep(1);
				if (wait == false)
					if (watch.ElapsedMilliseconds >= delay.TotalMilliseconds)
						break;

				if (token.IsCancellationRequested)
					token.ThrowIfCancellationRequested();
			}
			watch.Stop();
			#endregion

			BotElement NextBlock = null;
			int nextCPId = -1;

            #region Block actions
            //!!!~~ BotElements
            switch (be.Type)
			{
				case null: 
					break;
				case MCursor.name:
					MCursor.BotAction((MCursor)be);
					break;
				case "Key":
					MKey.BotAction((MKey)be);
					break;
				case "Text":
					MText.BotAction((MText)be);
					break;
				case "ScanColor":
					((MScanColor)be).Result = MScanColor.BotAction((MScanColor)be, token);
					break;
				case "OpenBot":
					BotData nBd;
					if (Serializer.DeserializeObject(((MOpenBOT)be).Source, out nBd))
					{
						int i = 0;
						while (true)    // times
						{
							i++;
							if (be.Times != 0 && i > be.Times)
								break;
							nBd.BotElements[0].Variables = ((MOpenBOT)be).Variables;

							(((MOpenBOT)be).lastBlockIdResult, ((MOpenBOT)be).Variables) = CodeAnalyser(nBd, task, wait, token);
						}
					}
					else
						Console.AddMessage("Error " + be.Name + "cannot open file :\"" + ((MOpenBOT)be).Source + "\n.");
				
					break;
                case MIf.name:
                    MIf.BotAction((MIf)be);
                    break;
                case MCallToAPI.name:
                    MCallToAPI.BotAction((MCallToAPI)be);
                    break;
                case MMultiplexer.name:
                    MMultiplexer.BotAction((MMultiplexer)be);
                    break;
                case MColorPositionSplitter.name:
                    MColorPositionSplitter.BotAction((MColorPositionSplitter)be);
                    break;
                case MArray.name:
                    MArray.BotAction((MArray)be);
                    break;
            }
			#endregion

			void ConAction(int BlockId, int CPId, int CurrentBlockCPId, BotElement CurrentBlobk)
			{
				void nextBlockAct(BotElement bot)
                {
					NextBlock = bot;
					nextCPId = CPId;
                }

				// Check type
				var nextBlock = BotElement.FindById(bd.BotElements, BlockId);
				var nextCpd = nextBlock.CPDatas.Find(x => x.Id == CPId);
				var cpd = CurrentBlobk.CPDatas.Find(x => x.Id == CurrentBlockCPId);

				if (nextCpd.CIO == CIO.Input)
                {
					// convert var
					if (nextCpd.CType == CType.Var)
                    {
						nextCpd.CType = cpd.CType;
                    }
					switch (nextCpd.CType)
					{
						case CType.Next:
							if (CurrentBlobk.Type == MScanColor.name)
							{
								if (((MScanColor)CurrentBlobk).Result)  // more than 1 connection 
								{
									if (CurrentBlockCPId == 2)  // id = 2 -> True connection point 
										nextBlockAct(nextBlock);
								}
								else if (CurrentBlockCPId == 4) // id = 4 -> False connection point 
									nextBlockAct(nextBlock);
							}
							else if (CurrentBlobk.Type == MOpenBOT.name)
							{
								if (((MOpenBOT)CurrentBlobk).lastBlockIdResult == CurrentBlockCPId) // more than 1 connection 
									nextBlockAct(nextBlock);
							}
							else if (CurrentBlobk.Type == MIf.name)
							{
								if (((MIf)CurrentBlobk).Condition)  // more than 1 connection 
								{
									if (CurrentBlockCPId == 2)  // id = 2 -> True connection point 
										nextBlockAct(nextBlock);
								}
								else if (CurrentBlockCPId == 4) // id = 4 -> False connection point 
									nextBlockAct(nextBlock);
							}
                            else if (CurrentBlobk.Type == MMultiplexer.name)
                            {
								if (CurrentBlockCPId == ((MMultiplexer)CurrentBlobk).LastCPDIndex)
									nextBlockAct(nextBlock);
                            }
                            else if (nextBlock.Type == null)
                            {
                                nextBlockAct(null);
                            }
                            else
								nextBlockAct(nextBlock);


                            
                            break;
					}
                }

				foreach (var v in CurrentBlobk.Variables)
					if (v.Id == cpd.Id)
                    {
						// send data
						SendData(nextBlock, v, nextCpd);

						//CurrentBlobk.Variables.Remove(v);
						break;
                    }
			}

			// Find all connections to this block
			var con = bd.CDatas.FindAll(x => x.BlockId1 == be.Id || x.BlockId2 == be.Id).ToList();

			// connecton actions (send data, find next block)
			foreach (var s in con)
			{
				// Find connected blocks
				if (s.BlockId1 != be.Id)
					ConAction(s.BlockId1, s.Id1, s.Id2, be);
				else
					ConAction(s.BlockId2, s.Id2, s.Id1, be);
			}

			be.Variables.Clear();

			return (NextBlock, nextCPId);
		}

		private static void SendData(BotElement nextBlock, VariableData v, CPData nextCpd)
		{
			//!!!~~ BotElements
			try 
			{ 
				switch (nextBlock.Type)
				{
					case null:
						v.Id = nextCpd.Id;
						nextBlock.Variables.Add(v);
						break;
					case MCursor.name:
						switch (nextCpd.CType)
						{
							case CType.Position:
								((MCursor)nextBlock).Point = (Point)v.Variable;
								break;
							case CType.ButtonAction:
								((MCursor)nextBlock).KeyState = (int)v.Variable;
								break;
						}
						break;
					case MKey.name:
						switch (nextCpd.CType)
						{
							case CType.Key:
								((MKey)nextBlock).KeyCode = (int)v.Variable;
								break;
							case CType.KeyAction:
								((MKey)nextBlock).KeyState = (int)v.Variable;
								break;
						}
						break;
					case MText.name:
						switch (nextCpd.CType)
						{
							case CType.String:
								((MText)nextBlock).text = (string)v.Variable;
								break;
						}
						break;
					case MScanColor.name:
						switch (nextCpd.CType)
						{
							case CType.Color:
								((MScanColor)nextBlock).ColorsAndPositions = (List<ColorAndPosition>)v.Variable;
								break;
							case CType.Position:
								((MScanColor)nextBlock).ColorsAndPositions = (List<ColorAndPosition>)v.Variable;
								break;
							case CType.ColorPosition:
								((MScanColor)nextBlock).ColorPositions = (List<ColorPosition>)v.Variable;
								break;
						}
						break;
					case MOpenBOT.name:
						v.Id = nextCpd.Id;
						nextBlock.Variables.Add(v);
						break;
					case MIf.name:
						switch (nextCpd.CType)
						{
							case CType.Bool:
								((MIf)nextBlock).Condition = (bool)v.Variable;
								break;
						}
						break;
					case MCallToAPI.name:
						((MCallToAPI)nextBlock).InputData.Add(new VariableData() { Id = nextCpd.Id, Variable = v.Variable });
						break;
                    case MMultiplexer.name:
                        ((MMultiplexer)nextBlock).ObjectsArray = new List<object>();
						
                        switch (v.Variable.GetType().GetProperty("Item").PropertyType.ToString())
						{
							case "ColorPosition":
								foreach (var i in (List<ColorPosition>)v.Variable)
									((MMultiplexer)nextBlock).ObjectsArray.Add(i);
								break;
                        }
						
						int times = ((MMultiplexer)nextBlock).Times == 0 ? ((MMultiplexer)nextBlock).ObjectsArray.Count : ((MMultiplexer)nextBlock).Times;
                        Random rnd = new Random(87438);

						List<int> ff = new List<int>();

                        for (int i = 0; i < times; i++)
						{
                            int index = i % ((MMultiplexer)nextBlock).ObjectsArray.Count;


                            switch (((MMultiplexer)nextBlock).SelectionElementsType)
							{
                                case SelectType.First:
                                    ((MMultiplexer)nextBlock).ObjectsIndexes.Add(0);
                                    break;
                                case SelectType.Last:
                                    ((MMultiplexer)nextBlock).ObjectsIndexes.Add(((MMultiplexer)nextBlock).ObjectsArray.Count-1);
                                    break;
                                case SelectType.Random:
                                    ((MMultiplexer)nextBlock).ObjectsIndexes.Add(rnd.Next(((MMultiplexer)nextBlock).ObjectsArray.Count));
                                    break;
                                case SelectType.Sequential:
                                    ((MMultiplexer)nextBlock).ObjectsIndexes.Add(index);
                                    break;
                                case SelectType.RandomSequential:
									if (ff.Count == 0)
										for (int j = 0; j < ((MMultiplexer)nextBlock).ObjectsArray.Count; j++)
											ff.Add(j);

                                    int item = ff[rnd.Next(ff.Count)];
                                    ((MMultiplexer)nextBlock).ObjectsIndexes.Add(item);
                                    ff.Remove(item);

									break;
                            }
                        }

						break;


                    case MColorPositionSplitter.name:
						((MColorPositionSplitter)nextBlock).InColorPosition = (ColorPosition)v.Variable;
                        break;

                    case MArray.name:
                        ((MArray)nextBlock).ListArray.Add((ColorPosition)v.Variable);
                        break;

                }
			}
			catch
			{
                AutoBot_2._0.Class.Graph.Console.AddMessage("Datatype error in " + nextBlock.Name);
            }

        }

        private static BotData JoinData(BotData botData1, BotData botData2)
		{
			foreach (var d in botData2.BotElements)
			{
				// create new Id
				int newID = BotElement.GiveID(botData1.BotElements);

				// Find all connections to this block
				var con = botData2.CDatas.FindAll(x => x.BlockId1 == d.Id || x.BlockId2 == d.Id);

				// set connection ids
				foreach (var s in con)
				{
					// Find connected blocks and add new Id's in negative
					if (s.BlockId1 == d.Id) s.BlockId1 = -newID;
					else s.BlockId2 = -newID;
				}

				// Set block ID
				d.Id = newID;

				// Add block to Data1
				botData1.BotElements.Add(d);
			}

			// convet every negative values to positive in connections
			botData2.CDatas.ForEach(x => {
				x.BlockId1 = Math.Abs(x.BlockId1);
				x.BlockId2 = Math.Abs(x.BlockId2);
			});

			// Join CDatas Data2 to Data1
			botData1.CDatas.AddRange(botData2.CDatas);

			return botData1;
		}
		private static bool MergeOpenBot(ref BotData botData)
		{
			/*
			załaduj dane
			usuń openbot i graf (tu by wyeliminować puste id)
			wyciągnij połączenia do grafu i openbot (wążne aby dane po zmianie tutaj się zmieniły)
			połącz dwa grafy zmieniając id połączeń i bloków
			!!zapisz id usuniętych bloków (nie trzeba bo bloki są usunięte i id płączeń się nie zmienią)
			utwóż nowe połączenia na podstawie wyciągniętych danych połączeń 
			usuń połączenia do grafu i open bot
			dodaj nowe połączenia

			!! graph ma ruchome id punktów połączeń (już nie )
			 */

			BotData botData1 = new BotData() { BotElements = botData.BotElements.ToList(), CDatas = botData.CDatas.ToList() };

			// Find OpenBot elements
			var openBots = botData1.BotElements.FindAll(x => x.Type == "OpenBot").ToList();

			// set connections Id
			foreach (var ob in openBots)
			{
				// open bot file
				BotData bd;
				Serializer.DeserializeObject(((MOpenBOT)ob).Source, out bd);
				// !! załaduj dane  !!!

				// remove botopen and grid element
				botData1.BotElements.Remove(BotElement.FindById(botData1.BotElements, ob.Id));
				bd.BotElements.Remove(BotElement.FindById(bd.BotElements, 0));
				// !! usuń openbot i graf (tu by wyeliminować puste id) !!

				// Find all connections to openbot block
				var conToOB = botData1.CDatas.FindAll(x => x.BlockId1 == ob.Id || x.BlockId2 == ob.Id);
				// Find all connections to graph opened bot
				var conToG2 = bd.CDatas.FindAll(x => x.BlockId1 == 0 || x.BlockId2 == 0);
				// !! wyciągnij połączenia do grafu i openbot(wążne aby dane po zmianie tutaj się zmieniły) !!

				// join data
				botData1 = JoinData(botData1, bd);
				// !! połącz dwa grafy zmieniając id połączeń i bloków !!
				
				// creating new connections
				List<CData> newCDatas = new List<CData>();
				foreach (var s in conToOB)
				{
					int blockId;    // id block connected to openbot
					int cpId;       // id connection point connected to openbot
					int cpOBId;     // id connection point in openbot

					if (s.BlockId1 == ob.Id)
					{
						blockId = s.BlockId2;
						cpId = s.Id2;
						cpOBId = s.Id1;
					}
					else
					{
						blockId = s.BlockId1;
						cpId = s.Id1;
						cpOBId = s.Id2;
					}

					foreach (var c in conToG2)
					{
						if (c.BlockId1 == 0)
						{
							if (cpOBId == c.Id1)
							{
								// Add new connection
								newCDatas.Add(new CData(cpId, blockId, c.Id2, c.BlockId2));
								// remove connection
								botData1.CDatas.Remove(s);
								bd.CDatas.Remove(c);
							}
						}
						else if (cpOBId == c.Id2)
						{
							// Add new connection
							newCDatas.Add(new CData(cpId, blockId, c.Id1, c.BlockId1));
							// remove connection
							botData1.CDatas.Remove(s);
							bd.CDatas.Remove(c);
						}
					}
				}

				// remove connection in openbot and graph
				conToOB.ForEach(x => botData1.CDatas.Remove(x));
				conToG2.ForEach(x => botData1.CDatas.Remove(x));
				// !! usuń połączenia do grafu i open bot !!

				// add new connections
				botData1.CDatas.AddRange(newCDatas);
			}

			botData = botData1;
			return openBots.Count > 0;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			HotKeys.HotKey[] keys = new HotKeys.HotKey[]
			{
				new HotKeys.HotKey(Start, (uint)ModifierKeys.Control, (uint)System.Windows.Forms.Keys.F1),
				new HotKeys.HotKey(Stop, (uint)ModifierKeys.Control, (uint)System.Windows.Forms.Keys.F2),
			};

			hotKeys = new HotKeys(this, keys);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			Stop();
			hotKeys.stop();
		}

	}
}
