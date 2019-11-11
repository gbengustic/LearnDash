using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnDash
{
    class Question
    {
        private static DateTime dateTimeEntered;
        private static string quizTitle;
        private static string questionType;
        private static string category;
        private static string title;
        private static int totalPoint;
        private static string differentPointForEachQuestion;
        private static string questionText;
        private static string answerType;
        private static List<string> answers;
        private static List<int> points;
        private static string answer;
        private static int totalAnswers;
        private static string messageWithCorrectAnswer;
        private static string messageWithIncorrectAnswer;
        private static string hint;
        public static DateTime DateTimeEntered { get => dateTimeEntered; set => dateTimeEntered = value; }
        public static string QuizTitle { get => quizTitle; set => quizTitle = value; }
        public static string QuestionType { get => questionType; set => questionType = value; }
        public static string Category { get => category; set => category = value; }
        public static string Title { get => title; set => title = value; }
        public static int TotalPoint { get => totalPoint; set => totalPoint = value; }
        public static string DifferentPointForEachQuestion { get => differentPointForEachQuestion; set => differentPointForEachQuestion = value; }
        public static string QuestionText { get => questionText; set => questionText = value; }
        public static string AnswerType { get => answerType; set => answerType = value; }
        public static List<string> Answers { get => answers; set => answers = value; }
        public static List<int> Points { get => points; set => points = value; }
        public static string Answer { get => answer; set => answer = value; }
        public static int TotalAnswers { get => totalAnswers; set => totalAnswers = value; }
        public static string MessageWithCorrectAnswer { get => messageWithCorrectAnswer; set => messageWithCorrectAnswer = value; }
        public static string MessageWithIncorrectAnswer { get => messageWithIncorrectAnswer; set => messageWithIncorrectAnswer = value; }
        public static string Hint { get => hint; set => hint = value; }
    }
}
