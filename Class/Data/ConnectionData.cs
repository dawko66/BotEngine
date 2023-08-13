using AutoBot_v1._1.NewClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBot_v1._1.Class.Data
{
	public class ConnectionData
	{
		// Id connectionPoint innego bloku z którym jest połączony 
		public ConnectionTypes connectionType { get; set; }
		public int ConnectionPointId { get; set; }			//con point
		public int BlockId { get; set; }					//parent

		public ConnectionData(ConnectionTypes connectionType, int ConnectionPointId, int BlockId)
		{
			this.ConnectionPointId = ConnectionPointId;
			this.connectionType = connectionType;
			this.BlockId = BlockId;
		}
	}
}
