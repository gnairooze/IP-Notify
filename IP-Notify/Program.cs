using RestSharp;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_Notify
{
    class Program
    {
        static void Main(string[] args)
        {
            Notify().Wait();
        }

        static async Task Notify()
        {
            var ip = GetPublicIP();
            var machineName = Environment.MachineName;
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.Machine);
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("sender email as registered in send grid", "sender name");
            var subject = $"{machineName} Public IP is {ip}";
            
            var tos = new List<EmailAddress>();
            foreach (var item in Properties.Settings.Default.EmailTo)
            {
                tos.Add(new EmailAddress(item));
            }
            
            var plainTextContent = $"{machineName} Public IP is {ip}";
            var htmlContent = $"<strong>{machineName}</strong> Public IP is <strong>{ip}</strong>";
            
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            Console.WriteLine(response.StatusCode);
            var result = response.Body.ReadAsStringAsync().Result;
            Console.WriteLine(result);
        }

        private static object GetPublicIP()
        {
            var url = Properties.Settings.Default.PublicIP_URL;
            var client = new RestClient(url);

            var response = client.Execute(new RestRequest());

            return response.Content;
        }
    }
}
