using AutoBot_2._0.Class.Data;
using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.Class.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

[Serializable]
public class MCallToAPI : BotElement
{
	public const string name = "CallToAPI";    // Name this class

	public string IPAddress = "";

	public List<VariableData> InputData;

	public enum APIMethod { GET, POST }
	public APIMethod Method;

	public MCallToAPI() { }
	public MCallToAPI(int Id)
	{
		this.Id = Id;
		Type = name;
		Name = Type + Id;

		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Next));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Input, CArray.No, CType.Var, IsInfinite: true));
		CPDatas.Add(new CPData(Id, CPData.GiveID(CPDatas), CIO.Output, CArray.No, CType.Var, IsInfinite: true));
	}

	public static async void BotAction(MCallToAPI o)
	{
		/*
		create
		send
		get 
		 */

		try
		{
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // input data
            var values = new Dictionary<string, string>();
            for (int i = 2; i < o.CPDatas.Count; i++)
            {
                var cp = o.CPDatas[i];

                if (cp.CIO == CIO.Input)
				{
					string variable = "";

					for (int j = 0; j < o.InputData.Count; j++)
						if (o.CPDatas[i].Id == o.InputData[j].Id)
							variable = o.InputData[j].Variable.ToString();

                    AutoBot_2._0.Class.Graph.Console.AddBuffMessage();

                    if (o.CPDatas[i].Title != "")
                        values.Add(o.CPDatas[i].Title, variable);
					else
						AutoBot_2._0.Class.Graph.Console.BufforedMessage("(" + o.Name + ") Var" + o.CPDatas[i].Id + " key name is emnty!");
                }
                else
                {
					values.Remove("");
                    AutoBot_2._0.Class.Graph.Console.ClearBuffMessage();
                    break;
                }
            }

            var content = new FormUrlEncodedContent(values);

			HttpResponseMessage response = new HttpResponseMessage();
            if (o.Method == APIMethod.POST)
				response = client.PostAsync(o.IPAddress, content).Result;   // POST
            else if (o.Method == APIMethod.GET)
                response = client.GetAsync(o.IPAddress).Result;				// GET

            if (response.IsSuccessStatusCode)
            {
				var reciveStream = await response.Content.ReadAsStreamAsync();
				StreamReader streamReader = new StreamReader(reciveStream);
				var result = streamReader.ReadToEnd();
				try
				{
                    JObject json = JObject.Parse(result);
					foreach (var i in json)
					{
						foreach (var cp in o.CPDatas)
						{
							if (cp.Title == i.Key)
							{
								object value = i.Value.ToString();

								if (i.Value.Type == JTokenType.Array)
								{
									int x = 0;
									int.TryParse(i.Value[0].ToString(), out x);

									int y = 0;
									int.TryParse(i.Value[1].ToString(), out y);

									value = new Point(x, y);
								}
								else if (i.Value.Type == JTokenType.Integer)
								{
									int output = 0;
									int.TryParse(i.Value.ToString(), out output);
									value = output;
								}

                                o.Variables.Add(new VariableData() { Id = cp.Id, Variable = value });
                                AutoBot_2._0.Class.Graph.Console.AddMessage("Added value " + i.Key + ": " + i.Value);
                                break;
							}
						}
					}

                }
				catch
				{
                    AutoBot_2._0.Class.Graph.Console.AddMessage("Request convert to JSON error");
                }

            }
        }
		catch(Exception e)
		{
            AutoBot_2._0.Class.Graph.Console.AddMessage("request error");
		}


	}
}
