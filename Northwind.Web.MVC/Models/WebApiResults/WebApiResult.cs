using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Northwind.Web.MVC.Models.WebApiResults
{
    public class WebApiResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public object Data { get; set; }
    }
}
