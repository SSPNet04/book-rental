using System;
using System.Linq;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;

namespace Hangfire.Helper
{   
    public class UseQueue : JobFilterAttribute, IElectStateFilter
    {
        public UseQueue(SpecifyQueue specify = SpecifyQueue.Default, string name = null)
        {

            switch (specify)
            {
                case SpecifyQueue.MachineName:
                    Queue = QueueName.Machine;
                    break;
                case SpecifyQueue.Default:
                    Queue = QueueName.DefaultQueueName;
                    break;
                case SpecifyQueue.Schedule:
                    Queue = QueueName.Schedule;
                    break;
                case SpecifyQueue.Monitoring:
                    Queue = QueueName.Monitoring;
                    break;
                case SpecifyQueue.ScoreCheck:
                    Queue = QueueName.ScoreCheck;
                    break;
                case SpecifyQueue.QuestionsMonitoring:
                    Queue = QueueName.QuestionsMonitoring;
                    break;
                case SpecifyQueue.StartExam:
                    Queue = QueueName.StartExam;
                    break;
                case SpecifyQueue.AnswerExam:
                    Queue = QueueName.AnswerExam;
                    break;
                case SpecifyQueue.SubmitExam:
                    Queue = QueueName.SubmitExam;
                    break;
                case SpecifyQueue.Log:
                    Queue = QueueName.Log;
                    break;
                case SpecifyQueue.Name:
                    Queue = string.IsNullOrEmpty(name) ? QueueName.DefaultQueueName : name;
                    break;
                default:
                    Queue = QueueName.DefaultQueueName;
                    break;
            }

            Order = Int32.MaxValue;
        }

        public string Queue { get; private set; }

        public void OnStateElection(ElectStateContext context)
        {
            var enqueuedState = context.CandidateState as EnqueuedState;
            if (enqueuedState != null)
            {
                enqueuedState.Queue = String.Format(Queue, context.BackgroundJob.Job.Args.ToArray());
            }
        }
    }
}
