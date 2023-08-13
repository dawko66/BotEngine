using System;
using System.Windows;

[Serializable]
public class MChoose
{

	public static readonly string name = "Choose";	// Name this class

    [Serializable]
    public class ChooseTable
    {
		public int tableId;
		public int percent;

        public ChooseTable(int tableId, int percent)
        {
            this.tableId = tableId;
            this.percent = percent;
        }
    }
	public ChooseTable[] chooseTables;  // Choose random table in tableId

    public MChoose(ChooseTable[] chooseTables)
    {
        this.chooseTables = chooseTables;
    }

    /*    //~~public static int action(ChooseTable[] chooseTables)
        {
            int foo = new Random().Next(1);


            int drawnTableID = foo;
            foreach (ChooseTable table in chooseTables)
            {
                //if(foo <)
            }

            return Table.findTableById(chooseTables[drawnTableID].tableId);
        }*/

    public static void showDialog(Window thisWindow)
	{
		thisWindow.Hide();
        //~~new WindowChoose().ShowDialog();
        thisWindow.Show();
	}
}
