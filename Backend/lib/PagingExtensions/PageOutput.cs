using System;

namespace PagingExtensions
{
    public class PageOutput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
    }
}
