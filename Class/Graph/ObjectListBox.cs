using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoBot_2._0.Class.Graph
{
    class ObjectListBox : ListBox
    {
        public ObjectListBox()
        {
            //!!!~~ BotElements
            Items.Add(MText.name);
            Items.Add(MCursor.name);
            Items.Add(MKey.name);
            Items.Add(MScanColor.name);
            //Items.Add(MScanAreaColor.name);
            Items.Add(MOpenBOT.name);
            //Items.Add(MPosition.name);
            Items.Add(MIf.name);
            Items.Add(MCallToAPI.name);
            Items.Add(MMultiplexer.name);
            Items.Add(MColorPositionSplitter.name);
            Items.Add(MArray.name);

        }
    }
}
