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
using System.Windows.Shapes;

namespace LearnDash
{
    /// <summary>
    /// Interaction logic for VideoSetup.xaml
    /// </summary>
    public partial class VideoSetup : Window
    {
        public VideoSetup(string typeOfCaller)
        {
            InitializeComponent();

            TxbAutoComp.Text = "Auto Complete "+ typeOfCaller +" (Y/N)";
            TxbVideoURL.Content = "Video "+ typeOfCaller +" URL";
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (Validate(this.VideoSetupGrid))
            {


                MessageBox.Show("Setup saved.", "LearnDash");
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields are mandatory", "LearnDash");
            }
        }



        private bool Validate(Grid obj)
        {
            var trueforall = true;
            var atleastone = false;
            IEnumerable<TextBox> collection = obj.Children.OfType<TextBox>();
            foreach (var txtBox in collection)
            {
                if (string.IsNullOrWhiteSpace(txtBox.Text))
                {
                    atleastone = false;
                    break;
                }
                else
                {
                    atleastone = true;
                }
            }
            IEnumerable<ComboBox> collection1 = obj.Children.OfType<ComboBox>();
            foreach (var txtBox in collection1)
            {
                if (string.IsNullOrWhiteSpace(txtBox.Text))
                {
                    atleastone = false;
                    break;
                }
                else
                {
                    atleastone = true;
                }
            }
            return trueforall && atleastone;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<TextBox> collection = this.VideoSetupGrid.Children.OfType<TextBox>();
            foreach (var txtBox in collection)
            {
                txtBox.Text = string.Empty;
            }
            IEnumerable<ComboBox> collection1 = this.VideoSetupGrid.Children.OfType<ComboBox>();
            foreach (var txtBox in collection1)
            {
                txtBox.Text = string.Empty;
            }
            TxtVideoURL.Focus();
        }
    }
}
