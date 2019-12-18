namespace Northwind.Web.MVC.Models.Errors
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Error { get; set; }

        public string StackTrace { get; set; }
    }
}
