using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


public class IntPoint
{
    public int X;
    public int Y;

    public IntPoint() { }
    public IntPoint(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

}
public class Connection
{
    public Path line;

    public IntPoint connPoint1;
    public IntPoint connPoint2;

    public bool mouseBtnUp
    {
        get;
        private set;
    }
    public Connection(IntPoint connPoint1, IntPoint connPoint2)
    {
        mouseBtnUp = false;

        for(int i = 0; i < 2; i++)
        {
            this.connPoint1 = connPoint1;
            this.connPoint2 = connPoint2;
        }

        line = new Path()
        {
            Stroke = new SolidColorBrush(Color.FromArgb(200, 226, 226, 226)),
            //Fill = new SolidColorBrush(Colors.Green),
            StrokeThickness = 3,
            Cursor = Cursors.Hand
        };

        line.MouseUp += mouseUp;
    }

    private void mouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        mouseBtnUp = true;
    }
}
