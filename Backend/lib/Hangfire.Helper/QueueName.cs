using System;
using System.Globalization;

namespace Hangfire.Helper
{
    public class QueueName
    {
        public static string? MachinePrefix { get; set; }
        public static string Machine { get => $"{MachinePrefix.ToLower()}-{Environment.MachineName.ToLower(CultureInfo.CurrentCulture)}"; }
        public static string Schedule { get => "schedule"; }
        public static string Monitoring { get => "monitoring"; }
        public static string ScoreCheck { get => "scorecheck"; }
        public static string DefaultQueueName { get => "default"; }
        public static string QuestionsMonitoring { get => "questionsmonitoring"; }
       
        public static string StartExam { get => "startexam"; }
        public static string AnswerExam { get => "answerexam"; }
        public static string SubmitExam { get => "submitexam"; }
        public static string Log { get => "log"; }

    }
}
