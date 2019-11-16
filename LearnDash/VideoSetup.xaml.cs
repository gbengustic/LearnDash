using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LearnDash
{
    /// <summary>
    /// Interaction logic for VideoSetup.xaml
    /// </summary>
    public partial class VideoSetup : Window
    {
        string Caller = "";
        public VideoSetup(string typeOfCaller)
        {
            InitializeComponent();
            Caller = typeOfCaller;
            TxbAutoComp.Text = "Auto Complete " + Caller + " (Y/N)";
            TxbVideoURL.Content = "Video " + Caller + " URL";
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Validate(this.VideoSetupGrid))
            {
                if(Caller=="Lesson")
                {
                    CourseVideoSetup.Lesson_Allow_Comment = CmbxAllowComment.Text;
                    CourseVideoSetup.Lesson_Auto_Complete = CmbxAutoComplete.Text;
                    CourseVideoSetup.Lesson_Auto_Start_Video = CmbxAutoStartVideo.Text;
                    CourseVideoSetup.Lesson_Enable_Video_Progression = CmbxEnableVideoProgression.Text;
                    CourseVideoSetup.Lesson_Hide_Complete_Button = CmbxHideCompleteButton.Text;
                    CourseVideoSetup.Lesson_Show_Video_Control = CmbxShowVideoControl.Text;
                    CourseVideoSetup.Lesson_Video_URL = TxtVideoURL.Text;
                    CourseVideoSetup.Lesson_When_to_Show = CmbxWhenToShow.Text;
                    CourseVideoSetup.LessonComplete = true;
                }
                else if(Caller=="Topic")
                {
                    CourseVideoSetup.Topic_Allow_Comment = CmbxAllowComment.Text;
                    CourseVideoSetup.Topic_Auto_Complete = CmbxAutoComplete.Text;
                    CourseVideoSetup.Topic_Auto_Start_Video = CmbxAutoStartVideo.Text;
                    CourseVideoSetup.Topic_Enable_Video_Progression = CmbxEnableVideoProgression.Text;
                    CourseVideoSetup.Topic_Hide_Complete_Button = CmbxHideCompleteButton.Text;
                    CourseVideoSetup.Topic_Show_Video_Control = CmbxShowVideoControl.Text;
                    CourseVideoSetup.Topic_Video_URL = TxtVideoURL.Text;
                    CourseVideoSetup.Topic_When_to_Show = CmbxWhenToShow.Text;
                    CourseVideoSetup.TopicComplete = true;
                }

                MessageBox.Show("Setup saved.", "LearnDash");
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields are mandatory", "LearnDash");
            }
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
