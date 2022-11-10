namespace Order.Application.Models
{
    public class AddReportRequest
    {
        public string MethodName { get; set; }

        public AddReportRequest(string methodName)
        {
            MethodName = methodName;
        }
    }
}
