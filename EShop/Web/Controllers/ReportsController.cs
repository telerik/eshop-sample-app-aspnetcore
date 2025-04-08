namespace Web.Controllers
{
    using Telerik.Reporting.Services.AspNetCore;
    using Telerik.Reporting.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Mail;
    using System.Net;
    using Web.Attributes;

    [Route("api/reports")]
    [JsonConfigFilter]
    public class ReportsController : ReportsControllerBase
    {
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
            : base(reportServiceConfiguration)
        { }

        protected override HttpStatusCode SendMailMessage(MailMessage mailMessage)
        {
            throw new System.NotImplementedException("This method should be implemented in order to send mail messages");
        }
    }
}
