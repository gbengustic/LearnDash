using System.Windows;

namespace LearnDash
{
    /// <summary>
    /// Interaction logic for PleaseWait.xaml
    /// </summary>
    public partial class PleaseWait : Window
    {
        public PleaseWait(string Caller)
        {
            InitializeComponent();
            LblExporting.Content = "Exporting "+ Caller +".....";
        }
    }
}
