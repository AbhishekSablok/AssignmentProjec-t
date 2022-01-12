using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssignmentProject.Model.Paging
{
    public class PagingData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int ItemsCount { get; set; }
        public int TotalItemsCount { get; set; }
    }
}
