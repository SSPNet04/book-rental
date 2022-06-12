using System;
using System.Runtime.CompilerServices;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace Hangfire.Helper
{
    public class ExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        private readonly TimeSpan Timeout; 
        public ExpirationTimeAttribute(int day = 7)
        {
            Timeout = TimeSpan.FromDays(day);

        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            // Succeeded
            // Enqueued
            // Deleted
            // Failed
            if (context.NewState.Name != "Succeeded") return;
            context.JobExpirationTimeout = Timeout;
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            // context.JobExpirationTimeout = TimeSpan.FromDays(7);
        }
    }

    public class SubmitExamCounterAttribute: JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            // Succeeded
            // Enqueued
            // Deleted
            // Failed


            var examTestId = context.BackgroundJob.Job.Args[0];
            var state = context.NewState.Name;
            if (state == "Enqueued")
            {
                // Add Counter to cache
            }
            else if (state == "Succeeded" || state == "Deleted" || state == "Failed")
            {
                // Remove Counter from Cache 
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            // context.JobExpirationTimeout = TimeSpan.FromDays(7);
        }
    }



    
}
