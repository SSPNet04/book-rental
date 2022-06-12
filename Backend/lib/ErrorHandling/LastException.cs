using System;
namespace ErrorHandling
{
    public class LastException : Exception
    {
        public LastException(Exception e)
        {
            this.SourceException = e;
            this.LastMessage = e.Message;
            this.LastStackTrace = e.StackTrace;
        }

        public Exception SourceException { get; set; }
        public string LastMessage { get; set; }
        public string LastStackTrace { get; set; }
    }
}
