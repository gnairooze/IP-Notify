using RestSharp;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_Notify
{
    class Program
    {
        static Serilog.Core.Logger _Logger;

        static void Main(string[] args)
        {
            _Logger = LogConfiguration.Setup(Properties.Settings.Default.LogFile);
            try
            {
                SendMail.Notify(_Logger).Wait();
            }
            catch (Exception ex)
            {
                _Logger.Error(ex, "Something wrong happened.");
                
            }

            Log.CloseAndFlush();
        }
    }
}
