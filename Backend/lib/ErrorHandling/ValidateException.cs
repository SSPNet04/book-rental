using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErrorHandling
{
    public class ValidateException : Exception
    {
        public ValidateException()
        {
            this.ErrorResponse = new ErrorResponse();
            this.ErrorResponse.FieldErrors = new List<ErrorItem>();
            this.ErrorResponse.PopupErrors = new List<ErrorItem>();
        }

        public ErrorResponse ErrorResponse { get; set; }

        public void AddError(string code, string message, int type)
        {
            if (type == 0)
            {
                this.ErrorResponse.FieldErrors.Add(new ErrorItem()
                {
                    Code = code,
                    Message = message,
                });
            }
            else
            {
                this.ErrorResponse.PopupErrors.Add(new ErrorItem()
                {
                    Code = code,
                    Message = message,
                });
            }
        }

        public bool HasError
        {
            get { return this.ErrorResponse.FieldErrors.Any() || this.ErrorResponse.PopupErrors.Any(); }
        }

    }
}
