using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoBot_v1._1
{
    /// <summary>
    /// Logika interakcji dla klasy Watermark.xaml
    /// </summary>
    public partial class Watermark : UserControl
    {
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                SearchTermTextBox.Text = value;
            }
        }
        private string _Text = "";
        public string Placeholder
        {
            get
            {
                return _Placeholder;
            }
            set
            {
                _Placeholder = value;
                placeholder.Text = value;
            }
        }
        private string _Placeholder = "";
        public Watermark()
        {
            InitializeComponent();
        }
    }
}
