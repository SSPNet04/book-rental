using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorHandling
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {

        }
        public UnauthorizedException(string msg) : base(msg)
        {

        }
    }
}
