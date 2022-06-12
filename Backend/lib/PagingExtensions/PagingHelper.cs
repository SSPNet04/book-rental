using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PagingExtensions
{
    public class PagingHelper
    {
        public static PageOutput Paging<T>(PageParam pageParam, ref IQueryable<T> queryable)
        {
            if (pageParam.Page != null && pageParam.PageSize != null)
            {
                var totalNumberOfRecords = queryable.Distinct().Count();
                var mod = totalNumberOfRecords % pageParam.PageSize;
                var totalPageCount = (totalNumberOfRecords / pageParam.PageSize) + (mod == 0 ? 0 : 1);

                var skipAmount = pageParam.PageSize * (pageParam.Page - 1);

                var output = new PageOutput();
                output.Page = pageParam.Page ?? 0;
                output.PageSize = pageParam.PageSize ?? 0;
                output.PageCount = totalPageCount ?? 0;
                output.RecordCount = totalNumberOfRecords;

                queryable = queryable.Distinct().Skip(skipAmount ?? 0).Take(pageParam.PageSize ?? 0);

                return output;
            }
            else
            {
                return null;
            }
        }

        public static PageOutput PagingList<T>(PageParam pageParam, ref List<T> list)
        {
            if (pageParam.Page != null && pageParam.PageSize != null)
            {
                var totalNumberOfRecords = list.Distinct().Count();
                var mod = totalNumberOfRecords % pageParam.PageSize;
                var totalPageCount = (totalNumberOfRecords / pageParam.PageSize) + (mod == 0 ? 0 : 1);

                var skipAmount = pageParam.PageSize * (pageParam.Page - 1);

                var output = new PageOutput();
                output.Page = pageParam.Page ?? 0;
                output.PageSize = pageParam.PageSize ?? 0;
                output.PageCount = totalPageCount ?? 0;
                output.RecordCount = totalNumberOfRecords;

                list = list.Skip(skipAmount ?? 0).Take(pageParam.PageSize ?? 0).ToList();

                return output;
            }
            else
            {
                return null;
            }
        }
    }
}
