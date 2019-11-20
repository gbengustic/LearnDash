using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LearnDash
{
    /// <summary>
    /// Interaction logic for Data_View.xaml
    /// </summary>
    public partial class Data_View : Window
    {
        DataSet tempDataSet;
        DataTable tempQuestionTable;
        DataTable tempCourseTable;
        public Data_View(DataSet dataSet, string Caller)
        {
            InitializeComponent();
            tempDataSet = new DataSet();
            tempDataSet = dataSet.Copy();
            tempQuestionTable = dataSet.Tables["Question"].Copy();
            tempCourseTable = CourseVideoSetup.CourseTable.Copy();
            tempQuestionTable.Columns.Remove("DateEntered");
            tempQuestionTable.Columns.Remove("Id");
            QuestionView.DataContext = tempQuestionTable.DefaultView;
            tempCourseTable.Columns.Remove("DateEntered");
            //tempCourseTable.Columns.Remove("Id");
            CourseView.DataContext = tempCourseTable.DefaultView;
            if (Caller== "Question")
            {
                DataViewControl.SelectedIndex = 0;
            }
            else if (Caller == "Course")
            {
                DataViewControl.SelectedIndex = 1;
            }
            
            var TopicTitle = (from Rows in tempCourseTable.AsEnumerable() select Rows["Topic_Title"]).Distinct().ToList().OfType<string>().ToArray();
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(TopicTitle);
            TxtCourseViewTopic.AutoCompleteCustomSource = allowedTypes;
            TxtCourseViewTopic.AutoCompleteMode = AutoCompleteMode.Suggest;
            TxtCourseViewTopic.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var CourseCategory = (from Rows in tempCourseTable.AsEnumerable() select Rows["Course_Category"]).Distinct().ToList().OfType<string>().ToArray();
            allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(CourseCategory);
            TxtCourseViewCategory.AutoCompleteCustomSource = allowedTypes;
            TxtCourseViewCategory.AutoCompleteMode = AutoCompleteMode.Suggest;
            TxtCourseViewCategory.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var QuestionCategory = (from Rows in tempQuestionTable.AsEnumerable() select Rows["Category"]).Distinct().ToList().OfType<string>().ToArray();
            allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(QuestionCategory);
            TxtQuestionViewCategory.AutoCompleteCustomSource = allowedTypes;
            TxtQuestionViewCategory.AutoCompleteMode = AutoCompleteMode.Suggest;
            TxtQuestionViewCategory.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }
        private void FilterCourseTable(DataTable table, DateTime startDate, DateTime endDate,string category,string topic)
        {
            if (startDate > endDate)
            {
                System.Windows.MessageBox.Show("Invalid Date Range", "LearnDash");
                return;
            }
            var filteredRows =
                from row in table.Rows.OfType<DataRow>()
                where (DateTime)row[0] >= startDate
                where (DateTime)row[0] <= endDate
                where (string)row[22] == topic
                where (string)row[2] == category
                select row;

            var filteredTable = table.Clone();

            filteredRows.ToList().ForEach(r => filteredTable.ImportRow(r));
            filteredTable.Columns.Remove("DateEntered");
            //filteredTable.Columns.Remove("Id");
            CourseView.DataContext = filteredTable.DefaultView;
        }
        private void FilterQuestionTable(DataTable table, DateTime startDate, DateTime endDate, string category)
        {
            if(startDate> endDate)
            {
                System.Windows.MessageBox.Show("Invalid Date Range", "LearnDash");
                return;
            }
            var filteredRows =
                from row in table.Rows.OfType<DataRow>()
                where (DateTime)row[1] >= startDate
                where (DateTime)row[1] <= endDate
                where (string)row[4] == category
                select row;

            var filteredTable = table.Clone();

            filteredRows.ToList().ForEach(r => filteredTable.ImportRow(r));
            filteredTable.Columns.Remove("DateEntered");
            filteredTable.Columns.Remove("Id");
            QuestionView.DataContext = filteredTable.DefaultView;
        }
        private void BtnFilterCourses_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtCourseViewCategory.Text) && !string.IsNullOrWhiteSpace(TxtCourseViewTopic.Text) && Dpk1Course.SelectedDate!=null && Dpk2Course.SelectedDate!=null)
            {
                FilterCourseTable(tempDataSet.Tables["Course"], (DateTime)Dpk1Course.SelectedDate, (DateTime)Dpk2Course.SelectedDate, TxtCourseViewCategory.Text, TxtCourseViewTopic.Text);
            }
        }

        private void BtnFilterQuestion_Click(object sender, RoutedEventArgs e)
        {
            if ( !string.IsNullOrWhiteSpace(TxtQuestionViewCategory.Text) && QuestionDpk1.SelectedDate != null && QuestionDpk2.SelectedDate != null)
            {
                FilterQuestionTable(tempDataSet.Tables["Question"], (DateTime)QuestionDpk1.SelectedDate, (DateTime)QuestionDpk2.SelectedDate, TxtQuestionViewCategory.Text);
            }
        }

        private void BtnClearCourseFilter_Click(object sender, RoutedEventArgs e)
        {
            var tempTable = tempDataSet.Tables["Course"].Copy();
            tempTable.Columns.Remove("DateEntered");
            //tempTable.Columns.Remove("Id");
            CourseView.DataContext = tempTable.DefaultView;
        }

        private void BtnClearQuestionFilter_Click(object sender, RoutedEventArgs e)
        {
            var tempTable = tempDataSet.Tables["Question"].Copy();
            tempTable.Columns.Remove("DateEntered");
            tempTable.Columns.Remove("Id");
            QuestionView.DataContext = tempTable.DefaultView;
        }

      

        private void TxtCourseViewTopic_Click(object sender, EventArgs e)
        {
            TxtCourseViewTopic.SelectionStart = 0;
            TxtCourseViewTopic.SelectionLength = TxtCourseViewTopic.Text.Length;
        }

        private void TxtCourseViewCategory_Click(object sender, EventArgs e)
        {
            TxtCourseViewCategory.SelectionStart = 0;
            TxtCourseViewCategory.SelectionLength = TxtCourseViewCategory.Text.Length;
        }

        private void TxtQuestionViewCategory_Click(object sender, EventArgs e)
        {
            TxtQuestionViewCategory.SelectionStart = 0;
            TxtQuestionViewCategory.SelectionLength = TxtQuestionViewCategory.Text.Length;
        }




        //private void addItem(string text, TextBox textBox)
        //{
        //    TextBlock block = new TextBlock();

        //    // Add the text   
        //    block.Text = text;

        //    // A little style...   
        //    block.Margin = new Thickness(2, 3, 2, 3);
        //    block.Cursor = Cursors.Hand;

        //    // Mouse events   
        //    block.MouseLeftButtonUp += (sender, e) =>
        //    {
        //        textBox.Text = (sender as TextBlock).Text;
        //    };

        //    block.MouseEnter += (sender, e) =>
        //    {
        //        TextBlock b = sender as TextBlock;
        //        b.Background = Brushes.PeachPuff;
        //    };

        //    block.MouseLeave += (sender, e) =>
        //    {
        //        TextBlock b = sender as TextBlock;
        //        b.Background = Brushes.Transparent;
        //    };

        //    // Add to the panel   
        //    resultStack.Children.Add(block);
        //}
    }
}
