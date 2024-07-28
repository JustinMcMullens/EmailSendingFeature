using System;
using System.IO;
using System.Threading.Tasks;
using EmailSenderLibrary;
using Microsoft.Extensions.Configuration;

namespace EmailSenderConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
             // Build configuration to read settings from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve email sending settings from configuration
            var emailSendingSettings = config.GetSection("EmailSendingSettings");
            var emailSendingServer = emailSendingSettings["Server"];
            var emailSendingPort = int.Parse(emailSendingSettings["Port"]);
            var username = emailSendingSettings["Username"];
            var password = emailSendingSettings["Password"];

            // Create an instance of EmailSender with the settings
            var emailSender = new EmailSender(emailSendingServer, emailSendingPort, username, password);

            // Prompt User for information
            Console.WriteLine("Enter recipient email:");
            var to = Console.ReadLine();

            Console.WriteLine("Enter subject:");
            var subject = Console.ReadLine();

            Console.WriteLine("Enter email body:");
            var body = Console.ReadLine();

            Console.WriteLine("Enter minutes between attempts to send:");
            var delay = Console.ReadLine();

            await emailSender.SendEmailAsync(username, to, subject, body, delay);
            Console.WriteLine("Email sent successfully!");
        }
    }
}
