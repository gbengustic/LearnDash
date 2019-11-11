using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;
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
        string[] Answers = new string[13];
        int[] AnswerPoint = new int[13];
        System.Data.DataTable QuestionDt;
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
            if (InitialAnswerPoints != IUDTotalAnswers.Value)
            { 
                if (!string.IsNullOrWhiteSpace(txtAnswers.Text) && !string.IsNullOrWhiteSpace(txtPoints.Text) && Convert.ToInt32(cmbxAnswerNumber.Text) != IUDTotalAnswers.Maximum)
                {
                    AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1] = (Convert.ToInt32(txtPoints.Text));
                    Answers[Convert.ToInt32(cmbxAnswerNumber.Text) - 1] = txtAnswers.Text;
                    if (!cmbxAnswerNumber.Items.Contains(Convert.ToString(Convert.ToInt32(cmbxAnswerNumber.Text) + 1))) cmbxAnswerNumber.Items.Add(Convert.ToString(Convert.ToInt32(cmbxAnswerNumber.Text) + 1));
                    if (cmbxAnswerNumber.SelectedIndex + 1 < cmbxAnswerNumber.Items.Count)
                    {

                        cmbxAnswerNumber.SelectedIndex = cmbxAnswerNumber.SelectedIndex + 1;
                        LblPoint.Content = "Point " + cmbxAnswerNumber.Text;
                        if (AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1] == 0)
                        {
                            txtPoints.Clear();
                        }
                        else
                        {
                            txtPoints.Text = AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1].ToString();
                        }
                        txtAnswers.Text = Answers[Convert.ToInt32(cmbxAnswerNumber.Text) - 1];
                    }
                }
                InitialAnswerPoints = (int)IUDTotalAnswers.Value;
            }

            //if(InitialAnswerPoints!=IUDTotalAnswers.Value)
            //{
            //    cmbxAnswerNumber.Items.Clear();
            //    for (int i= Convert.ToInt32(cmbxAnswerNumber.Text)+1; i<=IUDTotalAnswers.Value;i++)
            //    {
            //        cmbxAnswerNumber.Items.Add(i.ToString());
            //    }
            //    //cmbxAnswerNumber.SelectedIndex = 0;
            //    InitialAnswerPoints = (int)IUDTotalAnswers.Value;
            //}
        }

       

        private void CmbxAnswerNumber_DropDownClosed(object sender, EventArgs e)
        {
            LblPoint.Content = "Point "+ cmbxAnswerNumber.Text;
            if(!string.IsNullOrWhiteSpace(txtAnswers.Text) && !string.IsNullOrWhiteSpace(txtPoints.Text))
            {

            }
            if(AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text)-1] ==0)
            {
                txtPoints.Clear();
            }
            else
            {
                txtPoints.Text = AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text)-1].ToString();
            }
            txtAnswers.Text= Answers[Convert.ToInt32(cmbxAnswerNumber.Text)-1];

        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        

        public static  bool Validate(Grid obj)
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
            if(atleastone)
            {
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
            IUDTotalAnswers.Value = 1;
            cmbxAnswerNumber.Items.Clear();
            for (int i = 1; i <= IUDTotalAnswers.Value; i++)
            {
                cmbxAnswerNumber.Items.Add(i.ToString());
            }
            cmbxAnswerNumber.SelectedIndex = 0;
            LblPoint.Content = "Point " + cmbxAnswerNumber.Text;
            txtQuizTitle.Focus();
        }

        private void BtnLessonVideoSetup_Click(object sender, RoutedEventArgs e)
        {
            VideoSetup videoSetup = new VideoSetup("Lesson");
            this.Hide();
            videoSetup.ShowDialog();
            this.Show();
        }

        private void BtnTopicVideoSetup_Click(object sender, RoutedEventArgs e)
        {
            VideoSetup videoSetup = new VideoSetup("Topic");
            this.Hide();
            videoSetup.ShowDialog();
            this.Show();
        }

        private void IUDTotalAnswers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBox_PreviewTextInput_2(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void BtnSubmitQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (Validate(this.QuestionTab))
            {
                int LastID =0;


                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    String query = "INSERT INTO dbo.Question ([DateEntered],[Quiz_Title],[Question_Type],[Title],[Total_Point],[Different_Points]" +
                        ",[Question_Text],[Answer_Type],[Answer],[Total_Answer],[Message_with_correct_answer],[Message_with_incorrect_answer],[Hint]) OUTPUT INSERTED.ID " +
                        "VALUES (@DateEntered,@Quiz_Title,@Question_Type,@Title,@Total_Point,@Different_Points,@Question_Text,@Answer_Type,@Answer," +
                        "@Total_Answer,@Message_with_correct_answer,@Message_with_incorrect_answer,@Hint)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DateEntered",DateTime.Now);
                        command.Parameters.AddWithValue("@Quiz_Title", txtQuizTitle.Text);
                        command.Parameters.AddWithValue("@Question_Type",cmbxQuestionType.Text);
                        command.Parameters.AddWithValue("@Title",txtTitle.Text);
                        command.Parameters.AddWithValue("@Total_Point", Convert.ToInt32(txtTotalPoints.Text));
                        command.Parameters.AddWithValue("@Different_Points",cmbxDifferentPoints.Text);
                        command.Parameters.AddWithValue("@Question_Text",txtQuestions.Text);
                        command.Parameters.AddWithValue("@Answer_Type",txtAnswerType.Text);
                        command.Parameters.AddWithValue("@Answer",txtAnswer.Text);
                        command.Parameters.AddWithValue("@Total_Answer", Convert.ToInt32(IUDTotalAnswers.Value));
                        command.Parameters.AddWithValue("@Message_with_correct_answer",txtMsgCorrectAnswer.Text);
                        command.Parameters.AddWithValue("@Message_with_incorrect_answer",txtMsgIncorrectAnswer.Text);
                        command.Parameters.AddWithValue("@Hint",txtHint.Text);
                        
                        connection.Open();
                        try
                        {
                            int result = command.ExecuteNonQuery();
                            LastID = (Int32)command.ExecuteScalar();

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message, "LearnDash");
                        }
                        connection.Close();
                    }
                }

                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                {
                    String query = "INSERT INTO dbo.Answer ([Question_ID],[Answer],[Point],[Answer_Number]) VALUES (@Question_ID,@Answer,@Point,@Answer_Number)";
                    var i = 0;
                    foreach (int point in AnswerPoint)
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Question_ID", LastID);
                            command.Parameters.AddWithValue("@Answer", (Answers[i] == null) ? "" : Answers[i]);
                            command.Parameters.AddWithValue("@Point", point);
                            command.Parameters.AddWithValue("@Answer_Number", i);

                            connection.Open();
                            try
                            {
                                int result = command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show(ex.Message, "LearnDash");
                            }
                            connection.Close();
                        }
                        i++;
                    }

                }

                MessageBox.Show("Question saved.", "LearnDash");
            }
            else
            {
                MessageBox.Show("All fields are mandatory", "LearnDash");
            }
        }

        private void BtnAddAnswer_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtAnswers.Text))
            {
                MessageBox.Show("Please add answer", "LearnDash");
                txtAnswers.Focus();
                return;
            }
            if(string.IsNullOrWhiteSpace(txtPoints.Text))
            {
                MessageBox.Show("Please add point", "LearnDash");
                txtPoints.Focus();
                return;
            }
            
                AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text)-1] = (Convert.ToInt32(txtPoints.Text));
                Answers[Convert.ToInt32(cmbxAnswerNumber.Text)-1] = txtAnswers.Text;
            if (Convert.ToInt32(cmbxAnswerNumber.Text)< IUDTotalAnswers.Value)
            {
               if(!cmbxAnswerNumber.Items.Contains(Convert.ToString(Convert.ToInt32(cmbxAnswerNumber.Text) + 1))) cmbxAnswerNumber.Items.Add(Convert.ToString(Convert.ToInt32(cmbxAnswerNumber.Text)+1));
                if (cmbxAnswerNumber.SelectedIndex+1<cmbxAnswerNumber.Items.Count)
                {

                    cmbxAnswerNumber.SelectedIndex = cmbxAnswerNumber.SelectedIndex + 1;
                    LblPoint.Content = "Point " + cmbxAnswerNumber.Text;
                    if (AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1] == 0)
                    {
                        txtPoints.Clear();
                    }
                    else
                    {
                        txtPoints.Text = AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1].ToString();
                    }
                    txtAnswers.Text = Answers[Convert.ToInt32(cmbxAnswerNumber.Text) - 1];
                }
            }
            
                
           
        }

        private void BtnExportQuestion_Click(object sender, RoutedEventArgs e)
        {
            QuestionDt = new System.Data.DataTable();
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString);
            var command = new SqlCommand("Select * FROM dbo.Question", connection);
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(QuestionDt);
            Export("Question", QuestionDt);
        }

        void Export(string Caller, System.Data.DataTable table)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("sheet1");
            ws.Cell(1, 1).InsertTable(table);
            ws.Columns().AdjustToContents();
            ws.Tables.FirstOrDefault().ShowAutoFilter = false;
            wb.SaveAs(System.IO.Path.Combine(TxtExcelPath.Text,Caller + ".xlsx"));
            MessageBox.Show((Caller  + " has been successully exported"), "LearnDash");
        }

        private void ExportGrid_Loaded(object sender, RoutedEventArgs e)
        {
            TxtExcelPath.Text = Properties.Settings.Default.ExcelPath;
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                TxtExcelPath.Text = dialog.SelectedPath;
                Properties.Settings.Default.ExcelPath = dialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }
    }
}
