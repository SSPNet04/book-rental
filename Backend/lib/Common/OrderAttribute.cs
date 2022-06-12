using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class OrderAttribute : Attribute
    {
        public int Order { get; private set; }
        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
}
