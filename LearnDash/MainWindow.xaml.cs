using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace LearnDash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int InitialAnswerPoints =1;
        public MainWindow()
        {
            InitializeComponent();
            cmbxAnswerNumber.Items.Clear();
            for (int i = 1; i <= IUDTotalAnswers.Value; i++)
            {
                cmbxAnswerNumber.Items.Add(i.ToString());
            }
            cmbxAnswerNumber.SelectedIndex = 0;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TxtTotalPoints_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void IntegerUpDown_MouseLeave(object sender, MouseEventArgs e)
        {
            if(InitialAnswerPoints!=IUDTotalAnswers.Value)
            {
                cmbxAnswerNumber.Items.Clear();
                for (int i=1; i<=IUDTotalAnswers.Value;i++)
                {
                    cmbxAnswerNumber.Items.Add(i.ToString());
                }
                cmbxAnswerNumber.SelectedIndex = 0;
                InitialAnswerPoints = (int)IUDTotalAnswers.Value;
            }
        }

       

        private void CmbxAnswerNumber_DropDownClosed(object sender, EventArgs e)
        {
            LblPoint.Content = "Point "+ cmbxAnswerNumber.Text;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if(Validate(this.QuestionTab))
            {


                MessageBox.Show("Question saved.", "LearnDash");
            }
            else
            {
                MessageBox.Show("All fields are mandatory", "LearnDash");
            }
        }

        

        private  bool Validate(Grid obj)
        {
            var trueforall = true;
            var atleastone = false;
            IEnumerable<TextBox> collection = obj.Children.OfType<TextBox>();
            foreach (var txtBox in collection)
            {
               if(string.IsNullOrWhiteSpace(txtBox.Text))
                {
                    atleastone = false;
                    break;
                }
               else
                {
                    atleastone = true;
                }
            }
            IEnumerable<ComboBox>  collection1 = obj.Children.OfType<ComboBox>();
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
            return  trueforall && atleastone;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<TextBox> collection = this.QuestionTab.Children.OfType<TextBox>();
            foreach (var txtBox in collection)
            {
                txtBox.Text = string.Empty;
            }
            IEnumerable<ComboBox> collection1 = this.QuestionTab.Children.OfType<ComboBox>();
            foreach (var txtBox in collection1)
            {
                txtBox.Text = string.Empty;
            }
            txtQuizTitle.Focus();
        }

        
    }
}
