using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;

namespace EmailSenderLibrary
{
    public class EmailSender
    {
        private readonly string _emailSendingServer;
        private readonly int _emailSendingPort;
        private readonly string _username;
        private readonly string _password;

        // Constructor to initialize the EmailSender with server details and credentials
        public EmailSender(string emailSendingServer, int emailSendingPort, string username, string password)
        {
            _emailSendingServer = emailSendingServer;
            _emailSendingPort = emailSendingPort;
            _username = username;
            _password = password;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/emailsender.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public async Task SendEmailAsync(string from, string to, string subject, string body, string delay)
        {
            // Create a new email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            // Parse the delay input (in minutes) to milliseconds
            int delayMinutes = int.Parse(delay);
            int delayMillisec = delayMinutes * 60000;
          
            // Initialize retry count and set max retries to 3
            int retryCount = 0;
            const int maxRetries = 3;
            bool sent = false;

            while (retryCount < maxRetries && !sent)
            {
                
                // Use a new instance of SmtpClient for each attempt
                using var client = new SmtpClient();
                try
                {
                    await client.ConnectAsync(_emailSendingServer, _emailSendingPort, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(_username, _password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    sent = true;

                    // Log successful email send
                    Log.Information("Email sent to: {From}", from);
                    Log.Information("Email sent to: {To}", to);                  
                    Log.Information("Subject: {Subject}", subject);
                    Log.Information("Body: {Body}", body);
                }
                catch (Exception ex)
                {
                    // Increment retry count and log the error
                    retryCount++;
                    Log.Error(ex, "Error sending email to {To} with subject {Subject}, attempt {Attempt}", to, subject, retryCount);

                    // Delay before the next retry attempt.
                    Thread.Sleep(delayMillisec);
                    
                    // Throw the exception if max retries are reached.
                    if (retryCount >= maxRetries) throw;
                }
            }
        }
    }
}
