using AutoBot_2._0.Class.Graph;
using AutoBot_v1._1.NewClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBot_v1._1.Class.Data
{
	public class CData
	{
		public int Id1 { get; set; }
		public int BlockId1 { get; set; }

		public int Id2 { get; set; }
		public int BlockId2 { get; set; }

        public CData() { }

        public CData(int id1, int blockId1, int id2, int blockId2)
        {
            Id1 = id1;
            BlockId1 = blockId1;
            Id2 = id2;
            BlockId2 = blockId2;
        }
    }
}
