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
        public Data_View(DataSet dataSet, string Caller)
        {
            InitializeComponent();
            tempDataSet = new DataSet();
            tempDataSet = dataSet.Copy();
            dataSet.Tables["Question"].Columns.Remove("DateEntered");
            dataSet.Tables["Question"].Columns.Remove("Id");
            QuestionView.DataContext = dataSet.Tables["Question"].DefaultView;
            dataSet.Tables["Course"].Columns.Remove("DateEntered");
            dataSet.Tables["Course"].Columns.Remove("Id");
            CourseView.DataContext = dataSet.Tables["Course"].DefaultView;
            if(Caller== "Question")
            {
                DataViewControl.SelectedIndex = 0;
            }
            else if (Caller == "Course")
            {
                DataViewControl.SelectedIndex = 1;
            }

        }
        private void FilterCourseTable(DataTable table, DateTime startDate, DateTime endDate,string category,string topic)
        {
            if (startDate > endDate)
            {
                MessageBox.Show("Invalid Date Range", "LearnDash");
                return;
            }
            var filteredRows =
                from row in table.Rows.OfType<DataRow>()
                where (DateTime)row[1] >= startDate
                where (DateTime)row[1] <= endDate
                where (string)row[23] == topic
                where (string)row[3] == category
                select row;

            var filteredTable = table.Clone();

            filteredRows.ToList().ForEach(r => filteredTable.ImportRow(r));
            filteredTable.Columns.Remove("DateEntered");
            filteredTable.Columns.Remove("Id");
            CourseView.DataContext = filteredTable.DefaultView;
        }
        private void FilterQuestionTable(DataTable table, DateTime startDate, DateTime endDate, string category)
        {
            if(startDate> endDate)
            {
                MessageBox.Show("Invalid Date Range", "LearnDash");
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
            tempTable.Columns.Remove("Id");
            CourseView.DataContext = tempTable.DefaultView;
        }

        private void BtnClearQuestionFilter_Click(object sender, RoutedEventArgs e)
        {
            var tempTable = tempDataSet.Tables["Question"].Copy();
            tempTable.Columns.Remove("DateEntered");
            tempTable.Columns.Remove("Id");
            QuestionView.DataContext = tempTable.DefaultView;
        }
    }
}
