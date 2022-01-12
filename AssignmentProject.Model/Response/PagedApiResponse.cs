using AssignmentProject.Model.Paging;
using AssignmentProject.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AssignmentProject.Model.Response
{
    public class PagedApiResponse
    {
        public string StatusMessage { get; set; }
        public HttpStatusCode HttpStatus { get; set; }
        public string Status { get; set; }
        public PagingData Paging { get; set; }
        public IQueryable Data { get; set; }
    }
}
