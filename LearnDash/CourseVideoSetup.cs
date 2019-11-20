using System.Data;

namespace LearnDash
{
    class CourseVideoSetup
    {
        private static DataTable courseTable;
        private static string lesson_Enable_Video_Progression;
        private static string lesson_Video_URL;
        private static string lesson_Auto_Start_Video;
        private static string lesson_Show_Video_Control;
        private static string lesson_When_to_Show;
        private static string lesson_Auto_Complete;
        private static string lesson_Hide_Complete_Button;
        private static string lesson_Allow_Comment;

        private static string topic_Enable_Video_Progression;
        private static string topic_Video_URL;
        private static string topic_Auto_Start_Video;
        private static string topic_Show_Video_Control;
        private static string topic_When_to_Show;
        private static string topic_Auto_Complete;
        private static string topic_Hide_Complete_Button;
        private static string topic_Allow_Comment;
        private static bool lessonComplete;
        private static bool topicComplete;

        public static string Lesson_Enable_Video_Progression { get => lesson_Enable_Video_Progression; set => lesson_Enable_Video_Progression = value; }
        public static string Lesson_Video_URL { get => lesson_Video_URL; set => lesson_Video_URL = value; }
        public static string Lesson_Auto_Start_Video { get => lesson_Auto_Start_Video; set => lesson_Auto_Start_Video = value; }
        public static string Lesson_Show_Video_Control { get => lesson_Show_Video_Control; set => lesson_Show_Video_Control = value; }
        public static string Lesson_When_to_Show { get => lesson_When_to_Show; set => lesson_When_to_Show = value; }
        public static string Lesson_Auto_Complete { get => lesson_Auto_Complete; set => lesson_Auto_Complete = value; }
        public static string Lesson_Hide_Complete_Button { get => lesson_Hide_Complete_Button; set => lesson_Hide_Complete_Button = value; }
        public static string Lesson_Allow_Comment { get => lesson_Allow_Comment; set => lesson_Allow_Comment = value; }
        public static string Topic_Enable_Video_Progression { get => topic_Enable_Video_Progression; set => topic_Enable_Video_Progression = value; }
        public static string Topic_Video_URL { get => topic_Video_URL; set => topic_Video_URL = value; }
        public static string Topic_Auto_Start_Video { get => topic_Auto_Start_Video; set => topic_Auto_Start_Video = value; }
        public static string Topic_Show_Video_Control { get => topic_Show_Video_Control; set => topic_Show_Video_Control = value; }
        public static string Topic_When_to_Show { get => topic_When_to_Show; set => topic_When_to_Show = value; }
        public static string Topic_Auto_Complete { get => topic_Auto_Complete; set => topic_Auto_Complete = value; }
        public static string Topic_Hide_Complete_Button { get => topic_Hide_Complete_Button; set => topic_Hide_Complete_Button = value; }
        public static string Topic_Allow_Comment { get => topic_Allow_Comment; set => topic_Allow_Comment = value; }
        public static bool LessonComplete { get => lessonComplete; set => lessonComplete = value; }
        public static bool TopicComplete { get => topicComplete; set => topicComplete = value; }
        public static DataTable CourseTable { get => courseTable; set => courseTable = value; }
    }
}
