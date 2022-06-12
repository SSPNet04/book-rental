using System;
using System.Collections.Generic;
using System.Text;

namespace PagingExtensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PagingResponseHeadersAttribute : Attribute
    {
    }
}
