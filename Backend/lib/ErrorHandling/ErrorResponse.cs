using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorHandling
{
    public class ErrorResponse
    {
        public List<ErrorItem> PopupErrors { get; set; }
        public List<ErrorItem> FieldErrors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ErrorItem
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
