using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using System.Globalization;
using System.IO;

namespace LearnDash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int InitialAnswerPoints = 1;
        string[] Answers = new string[8];
        int[] AnswerPoint = new int[8];
        DataTable QuestionDt;
        DataSet ViewsDataset;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        PleaseWait pleaseWait;
        string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" 
            + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LearnDash Database", "LearnDash.mdf") 
            + ";Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
            FillCriteria();
            cmbxAnswerNumber.Items.Clear();
            for (int i = 1; i <= IUDTotalAnswers.Value; i++)
            {
                cmbxAnswerNumber.Items.Add(i.ToString());
            }
            cmbxAnswerNumber.SelectedIndex = 0;

            ViewsDataset = new DataSet();
            ViewsDataset.Tables.Add("Question");
            QueryTable("Question");
            ViewsDataset.Tables.Add("Course");
            QueryTable("Course");
            worker.DoWork += worker_DoWork;
        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text

        public string QuizCriteria { get; private set; }
        public string CourseCriteria { get; private set; }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void QueryTable(string table)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand("Select * FROM " + table, connection);
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(ViewsDataset.Tables[table]);
       
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
                            btnAddAnswer.Content = "Add";
                        }
                        else
                        {
                            txtPoints.Text = AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1].ToString();
                            btnAddAnswer.Content = "Update";
                        }
                        txtAnswers.Text = Answers[Convert.ToInt32(cmbxAnswerNumber.Text) - 1];
                    }
                }
                InitialAnswerPoints = (int)IUDTotalAnswers.Value;
            }
        }



        private void CmbxAnswerNumber_DropDownClosed(object sender, EventArgs e)
        {
            LblPoint.Content = "Point " + cmbxAnswerNumber.Text;
            if (!string.IsNullOrWhiteSpace(txtAnswers.Text) && !string.IsNullOrWhiteSpace(txtPoints.Text))
            {

            }
            if (AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1] == 0)
            {
                txtPoints.Clear();
                btnAddAnswer.Content = "Add";
            }
            else
            {
                txtPoints.Text = AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1].ToString();
                btnAddAnswer.Content = "Update";
            }
            txtAnswers.Text = Answers[Convert.ToInt32(cmbxAnswerNumber.Text) - 1];

        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (Validate(this.CourseTab))
            {
                if (!CourseVideoSetup.LessonComplete)
                {
                    CourseVideoSetup.Lesson_Enable_Video_Progression = "";
                    CourseVideoSetup.Lesson_Video_URL = "";
                    CourseVideoSetup.Lesson_Auto_Start_Video = "";
                    CourseVideoSetup.Lesson_Show_Video_Control = "";
                    CourseVideoSetup.Lesson_When_to_Show = "";
                    CourseVideoSetup.Lesson_Auto_Complete = "";
                    CourseVideoSetup.Lesson_Hide_Complete_Button = "";
                    CourseVideoSetup.Lesson_Allow_Comment = "";
                    //MessageBox.Show("Please complete Lesson Video Setup", "LearnDash");
                    //BtnLessonVideoSetup.Focus();
                    //BtnLessonVideoSetup_Click(this, null);
                }
                if (!CourseVideoSetup.TopicComplete)
                {
                    CourseVideoSetup.Topic_Enable_Video_Progression = "";
                    CourseVideoSetup.Topic_Video_URL = "";
                    CourseVideoSetup.Topic_Auto_Start_Video = "";
                    CourseVideoSetup.Topic_Show_Video_Control = "";
                    CourseVideoSetup.Topic_When_to_Show = "";
                    CourseVideoSetup.Topic_Auto_Complete = "";
                    CourseVideoSetup.Topic_Hide_Complete_Button = "";
                    CourseVideoSetup.Topic_Allow_Comment = "";
                    //MessageBox.Show("Please complete Topic Video Setup", "LearnDash");
                    //BtnTopicVideoSetup.Focus();
                    //BtnTopicVideoSetup_Click(this, null);
                }
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    String query = "INSERT INTO dbo.Course ([DateEntered],[Course_Title],[Course_Category],[Course_Tag],[Course_Content]," +
                        "[Course_Featured_Image],[Course_Status],[Lesson_Title],[Lesson_Category],[Lesson_Tag],[Lesson_Content],[Lesson_Enable_Video_Progression]," +
                        "[Lesson_Video_URL],[Lesson_Auto_Start_Video],[Lesson_Show_Video_Control],[Lesson_When_to_Show],[Lesson_Auto_Complete],[Lesson_Hide_Complete_Button]," +
                        "[Lesson_Allow_Comment],[Lesson_Order],[Lesson_Featured_Image],[Lesson_Status],[Topic_Title],[Topic_Category],[Topic_Tag],[Topic_Content],[Topic_Enable_Video_Progression]," +
                        "[Topic_Video_URL],[Topic_Auto_Start_Video],[Topic_Show_Video_Control],[Topic_When_to_Show],[Topic_Auto_Complete],[Topic_Hide_Complete_Button],[Topic_Allow_Comment]," +
                        "[Topic_Order],[Topic_Featured_Image],[Topic_Status])" +
                        "VALUES (@DateEntered,@Course_Title,@Course_Category,@Course_Tag,@Course_Content," +
                        "@Course_Featured_Image,@Course_Status,@Lesson_Title,@Lesson_Category,@Lesson_Tag," +
                        "@Lesson_Content,@Lesson_Enable_Video_Progression,@Lesson_Video_URL,@Lesson_Auto_Start_Video," +
                        "@Lesson_Show_Video_Control,@Lesson_When_to_Show,@Lesson_Auto_Complete,@Lesson_Hide_Complete_Button," +
                        "@Lesson_Allow_Comment,@Lesson_Order,@Lesson_Featured_Image,@Lesson_Status,@Topic_Title,@Topic_Category," +
                        "@Topic_Tag,@Topic_Content,@Topic_Enable_Video_Progression,@Topic_Video_URL,@Topic_Auto_Start_Video," +
                        "@Topic_Show_Video_Control,@Topic_When_to_Show,@Topic_Auto_Complete,@Topic_Hide_Complete_Button," +
                        "@Topic_Allow_Comment,@Topic_Order,@Topic_Featured_Image,@Topic_Status)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DateEntered", DateTime.Now);
                        command.Parameters.AddWithValue("@Course_Title", TxtCourseTitle.Text);
                        command.Parameters.AddWithValue("@Course_Category", TxtCourseCategory.Text);
                        command.Parameters.AddWithValue("@Course_Tag", TxtCourseTag.Text);
                        command.Parameters.AddWithValue("@Course_Content", TxtCourseContent.Text);
                        command.Parameters.AddWithValue("@Course_Featured_Image", TxtCourseFeaturedImage.Text);
                        command.Parameters.AddWithValue("@Course_Status", CmbxCourseStatus.Text);
                        command.Parameters.AddWithValue("@Lesson_Title", TxtLessonTitle.Text);
                        command.Parameters.AddWithValue("@Lesson_Category", TxtLessonCategory.Text);
                        command.Parameters.AddWithValue("@Lesson_Tag", TxtLessonTag.Text);
                        command.Parameters.AddWithValue("@Lesson_Content", TxtLessonContent.Text);
                        command.Parameters.AddWithValue("@Lesson_Enable_Video_Progression", CourseVideoSetup.Lesson_Enable_Video_Progression);
                        command.Parameters.AddWithValue("@Lesson_Video_URL", CourseVideoSetup.Lesson_Video_URL);
                        command.Parameters.AddWithValue("@Lesson_Auto_Start_Video", CourseVideoSetup.Lesson_Auto_Start_Video);
                        command.Parameters.AddWithValue("@Lesson_Show_Video_Control", CourseVideoSetup.Lesson_Show_Video_Control);
                        command.Parameters.AddWithValue("@Lesson_When_to_Show", CourseVideoSetup.Lesson_When_to_Show);
                        command.Parameters.AddWithValue("@Lesson_Auto_Complete", CourseVideoSetup.Lesson_Auto_Complete);
                        command.Parameters.AddWithValue("@Lesson_Hide_Complete_Button", CourseVideoSetup.Lesson_Hide_Complete_Button);
                        command.Parameters.AddWithValue("@Lesson_Allow_Comment", CourseVideoSetup.Lesson_Allow_Comment);
                        command.Parameters.AddWithValue("@Lesson_Order", Convert.ToInt32(TxtLessonOrder.Text));
                        command.Parameters.AddWithValue("@Lesson_Featured_Image", TxtLessonFeaturedImage.Text);
                        command.Parameters.AddWithValue("@Lesson_Status", CmbxLessonStatus.Text);
                        command.Parameters.AddWithValue("@Topic_Title", TxtTopicTitle.Text);
                        command.Parameters.AddWithValue("@Topic_Category", TxtTopicCategory.Text);
                        command.Parameters.AddWithValue("@Topic_Tag", TxtTopicTag.Text);
                        command.Parameters.AddWithValue("@Topic_Content", TxtTopicContent.Text);
                        command.Parameters.AddWithValue("@Topic_Enable_Video_Progression", CourseVideoSetup.Topic_Enable_Video_Progression);
                        command.Parameters.AddWithValue("@Topic_Video_URL", CourseVideoSetup.Topic_Video_URL);
                        command.Parameters.AddWithValue("@Topic_Auto_Start_Video", CourseVideoSetup.Topic_Auto_Start_Video);
                        command.Parameters.AddWithValue("@Topic_Show_Video_Control", CourseVideoSetup.Topic_Show_Video_Control);
                        command.Parameters.AddWithValue("@Topic_When_to_Show", CourseVideoSetup.Topic_When_to_Show);
                        command.Parameters.AddWithValue("@Topic_Auto_Complete", CourseVideoSetup.Topic_Auto_Complete);
                        command.Parameters.AddWithValue("@Topic_Hide_Complete_Button", CourseVideoSetup.Topic_Hide_Complete_Button);
                        command.Parameters.AddWithValue("@Topic_Allow_Comment", CourseVideoSetup.Topic_Allow_Comment);
                        command.Parameters.AddWithValue("@Topic_Order", Convert.ToInt32(TxtTopicOrder.Text));
                        command.Parameters.AddWithValue("@Topic_Featured_Image", TxtTopicFeaturedImage.Text);
                        command.Parameters.AddWithValue("@Topic_Status", CmbxTopicStatus.Text);
                        connection.Open();
                        try
                        {
                            int result = command.ExecuteNonQuery();
                            MessageBox.Show("Course saved.", "LearnDash");
                            CourseVideoSetup.LessonComplete = false;
                            CourseVideoSetup.TopicComplete = false;
                            FillCriteria();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "LearnDash");
                        }
                        connection.Close();
                    }
                }

               
               
            }
           
            else
            {
                MessageBox.Show("All fields are mandatory", "LearnDash");
            }
        }



        public static bool Validate(Grid obj)
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
            if (atleastone)
            {
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
            }

            return trueforall && atleastone;
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
            btnAddAnswer.Content = "Add";
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
                int LastID = 0;


                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    String query = "INSERT INTO dbo.Question ([DateEntered],[Quiz_Title],[Question_Type],[Category],[Title],[Total_Point],[Different_Points]" +
                        ",[Question_Text],[Answer_Type],[Answer],[Total_Answer],[Message_with_correct_answer],[Message_with_incorrect_answer],[Hint]) OUTPUT INSERTED.ID " +
                        "VALUES (@DateEntered,@Quiz_Title,@Question_Type,@Category,@Title,@Total_Point,@Different_Points,@Question_Text,@Answer_Type,@Answer," +
                        "@Total_Answer,@Message_with_correct_answer,@Message_with_incorrect_answer,@Hint)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DateEntered", DateTime.Now);
                        command.Parameters.AddWithValue("@Quiz_Title", txtQuizTitle.Text);
                        command.Parameters.AddWithValue("@Question_Type", cmbxQuestionType.Text);
                        command.Parameters.AddWithValue("@Category", txtCategory.Text);
                        command.Parameters.AddWithValue("@Title", txtTitle.Text);
                        command.Parameters.AddWithValue("@Total_Point", Convert.ToInt32(txtTotalPoints.Text));
                        command.Parameters.AddWithValue("@Different_Points", cmbxDifferentPoints.Text);
                        command.Parameters.AddWithValue("@Question_Text", txtQuestions.Text);
                        command.Parameters.AddWithValue("@Answer_Type", txtAnswerType.Text);
                        command.Parameters.AddWithValue("@Answer", txtAnswer.Text);
                        command.Parameters.AddWithValue("@Total_Answer", Convert.ToInt32(IUDTotalAnswers.Value));
                        command.Parameters.AddWithValue("@Message_with_correct_answer", txtMsgCorrectAnswer.Text);
                        command.Parameters.AddWithValue("@Message_with_incorrect_answer", txtMsgIncorrectAnswer.Text);
                        command.Parameters.AddWithValue("@Hint", txtHint.Text);

                        connection.Open();
                        try
                        {
                            //int result = command.ExecuteNonQuery();
                            LastID = (Int32)command.ExecuteScalar();

                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message, "LearnDash");
                        }
                        connection.Close();
                    }
                }

                using (SqlConnection connection = new SqlConnection(ConnectionString))
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
                FillCriteria();
                MessageBox.Show("Question saved.", "LearnDash");
            }
            else
            {
                MessageBox.Show("All fields are mandatory", "LearnDash");
            }
        }

        private void BtnAddAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAnswers.Text))
            {
                MessageBox.Show("Please add answer", "LearnDash");
                txtAnswers.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPoints.Text))
            {
                MessageBox.Show("Please add point", "LearnDash");
                txtPoints.Focus();
                return;
            }

            AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1] = (Convert.ToInt32(txtPoints.Text));
            Answers[Convert.ToInt32(cmbxAnswerNumber.Text) - 1] = txtAnswers.Text;
            if (Convert.ToInt32(cmbxAnswerNumber.Text) < IUDTotalAnswers.Value)
            {
                if (!cmbxAnswerNumber.Items.Contains(Convert.ToString(Convert.ToInt32(cmbxAnswerNumber.Text) + 1))) cmbxAnswerNumber.Items.Add(Convert.ToString(Convert.ToInt32(cmbxAnswerNumber.Text) + 1));
                if (cmbxAnswerNumber.SelectedIndex + 1 < cmbxAnswerNumber.Items.Count)
                {

                    cmbxAnswerNumber.SelectedIndex = cmbxAnswerNumber.SelectedIndex + 1;
                    LblPoint.Content = "Point " + cmbxAnswerNumber.Text;
                    if (AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1] == 0)
                    {
                        txtPoints.Clear();
                        btnAddAnswer.Content = "Add";
                    }
                    else
                    {
                        txtPoints.Text = AnswerPoint[Convert.ToInt32(cmbxAnswerNumber.Text) - 1].ToString();
                        btnAddAnswer.Content = "Update";
                    }
                    txtAnswers.Text = Answers[Convert.ToInt32(cmbxAnswerNumber.Text) - 1];
                }
            }
            else
            {
                MessageBox.Show("All answers and points have been added","LearnDash");
                btnAddAnswer.Content = "Update";
            }



        }

        private void BtnExportQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbxQuizTitle.Text) && !string.IsNullOrWhiteSpace(TxtExcelPath.Text))
            {
                pleaseWait = new PleaseWait("Question");
                worker.RunWorkerAsync("Question");
                pleaseWait.ShowDialog();
            }
            else if (string.IsNullOrWhiteSpace(cmbxQuizTitle.Text))
            {
                MessageBox.Show("Please select quiz title", "LearnDash");
            }
            else
            {
                MessageBox.Show("Please select excel directory", "LearnDash");
            }
        }
        void ExportQuestion()
        {
            var FinalQuestionTable = new DataTable();

            QuestionDt = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand("Select * FROM dbo.Question Where Quiz_Title='" + QuizCriteria + "'", connection);
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(QuestionDt);
            foreach (DataColumn s in QuestionDt.Columns)
            {
                if (s.Ordinal > 1 && s.Ordinal <= 9) FinalQuestionTable.Columns.Add(s.ColumnName);
            }
            for (int i = 1; i <= 8; i++)
            {
                FinalQuestionTable.Columns.Add("Answer_" + i);
                FinalQuestionTable.Columns.Add("Point_" + i);
            }
            foreach (DataColumn s in QuestionDt.Columns)
            {
                if (s.Ordinal > 9) FinalQuestionTable.Columns.Add(s.ColumnName);
            }

            foreach (DataRow dataRow in QuestionDt.Rows)
            {
                DataRow dr = FinalQuestionTable.NewRow();
                dr["Quiz_Title"] = dataRow["Quiz_Title"];
                dr["Question_Type"] = dataRow["Question_Type"];
                dr["Category"] = dataRow["Category"];
                dr["Title"] = dataRow["Title"];
                dr["Total_Point"] = dataRow["Total_Point"];
                dr["Different_Points"] = dataRow["Different_Points"];
                dr["Question_Text"] = dataRow["Question_Text"];
                dr["Answer_Type"] = dataRow["Answer_Type"];
                dr["Answer"] = dataRow["Answer"];
                dr["Total_Answer"] = dataRow["Total_Answer"];
                dr["Message_with_correct_answer"] = dataRow["Message_with_correct_answer"];
                dr["Message_with_incorrect_answer"] = dataRow["Message_with_incorrect_answer"];
                dr["Hint"] = dataRow["Hint"];
                var AnswerDt = new DataTable();
                connection = new SqlConnection(ConnectionString);
                command = new SqlCommand("Select * FROM dbo.Answer Where Question_ID='" + dataRow["Id"].ToString() + "'", connection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(AnswerDt);
                AnswerDt.DefaultView.Sort = "Answer_Number ASC";
                if (AnswerDt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRowView ans in AnswerDt.DefaultView)
                    {
                        dr[i + 8] = ans[2];
                        dr[i + 9] = ans[3];
                        i = i + 2;
                    }

                }
                else
                {
                    for (int i = 8; i <= 23; i++)
                    {
                        if (i % 2 == 0)
                        {
                            dr[i] = "";
                        }
                        else
                        {
                            dr[i] = 0;
                        }
                    }
                }

                FinalQuestionTable.Rows.Add(dr);
            }
            Export(QuizCriteria +"_Export_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss", CultureInfo.InvariantCulture), FinalQuestionTable);
        }

        void ExportCourse()
        {
            var FinalCourseTable = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand("Select * FROM dbo.Course Where Course_Title='"+ CourseCriteria+"'", connection);
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(FinalCourseTable);
            FinalCourseTable.Columns.RemoveAt(0);
            FinalCourseTable.Columns.RemoveAt(0);
            Export(CourseCriteria + "_Export_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss", CultureInfo.InvariantCulture), FinalCourseTable);
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument.ToString() == "Question")
            {
                ExportQuestion();
            }
            else if(e.Argument.ToString() == "Course")
            {
                ExportCourse();
            }
            
        }
        
        void Export(string Caller, DataTable table)
        {
            var wb = new ClosedXML.Excel.XLWorkbook();
            var ws = wb.Worksheets.Add("sheet1");
            ws.Cell(1, 1).InsertTable(table);
            ws.Columns().AdjustToContents();
            ws.Tables.FirstOrDefault().ShowAutoFilter = false;
            try
            {
                wb.SaveAs(System.IO.Path.Combine(Properties.Settings.Default.ExcelPath, Caller + ".xlsx"));
                if (pleaseWait.Dispatcher.CheckAccess())
                    pleaseWait.Close();
                else
                    pleaseWait.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(pleaseWait.Close));
                MessageBox.Show((Caller+".xlsx" + " has been successully exported"), "LearnDash");
            }
            catch (Exception ex)
            {
                if (pleaseWait.Dispatcher.CheckAccess())
                    pleaseWait.Close();
                else
                    pleaseWait.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(pleaseWait.Close));
                MessageBox.Show(ex.Message, "LearnDash");
            }
            
            
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

        private void BtnExportCourse_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(cmbxCourseTitle.Text) && !string.IsNullOrWhiteSpace(TxtExcelPath.Text))
            {
                pleaseWait = new PleaseWait("Course");
                worker.RunWorkerAsync("Course");
                pleaseWait.ShowDialog();
            }
            else if(string.IsNullOrWhiteSpace(cmbxCourseTitle.Text))
            {
                MessageBox.Show("Please select course title", "LearnDash");
            }
            else
            {
                MessageBox.Show("Please select excel directory", "LearnDash");
            }
           
        }

        private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void CmbxQuizTitle_DropDownClosed(object sender, EventArgs e)
        {
            QuizCriteria = cmbxQuizTitle.Text;
        }

        private void CmbxCourseTitle_DropDownClosed(object sender, EventArgs e)
        {
            CourseCriteria = cmbxCourseTitle.Text;
        }
        void FillCriteria()
        {
            try
            {
                var CourseTitleTable = new DataTable();
                var QuestionTitleTable = new DataTable();
                SqlConnection connection = new SqlConnection(ConnectionString);
                var command = new SqlCommand("Select DISTINCT  [Course_Title] FROM dbo.Course", connection);
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(CourseTitleTable);
                command = new SqlCommand("Select DISTINCT  [Quiz_Title] FROM dbo.Question", connection);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(QuestionTitleTable);
                cmbxQuizTitle.ItemsSource = QuestionTitleTable.DefaultView;
                cmbxCourseTitle.ItemsSource = CourseTitleTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "LearnDash");
                File.WriteAllText("error.txt",ex.Message);
            }

        }
        private void CmbxCourseTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void LearnDashTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void QuestionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnViewCourse_Click(object sender, RoutedEventArgs e)
        {
            QueryTable("Course");
            var dataView = new Data_View(ViewsDataset, "Course").ShowDialog();
        }

        private void BtnViewQuestions_Click(object sender, RoutedEventArgs e)
        {
            QueryTable("Question");
            var dataView = new Data_View(ViewsDataset, "Question").ShowDialog();
        }
    }
}
