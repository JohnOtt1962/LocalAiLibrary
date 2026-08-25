using LocalAiLibrary.AiLibrary.AITools;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace LocalAiLibrary.AiLibrary
{
    public static class AiEmail
    {
        public static async Task<bool> SendMail(AiTool<EmailArgs> emailModel)
        {
            bool isSuccess = false;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("John Ott", "iteachrrrr@gmail.com"));
            message.To.Add(new MailboxAddress("John Ott", emailModel.Args.To));
            message.Subject = emailModel.Args.Subject;

            // 2. Build the email body (Supports plain text or HTML)
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailModel.Args.Body,
                TextBody = emailModel.Args.Body
            };
            message.Body = bodyBuilder.ToMessageBody();

            // 3. Connect and send using MailKit's SmtpClient
            using (var client = new SmtpClient())
            {
                try
                {
                    // Connect to the server (e.g., smtp.gmail.com, smtp.sendgrid.net)
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    // Authenticate with your credentials
                    await client.AuthenticateAsync("iteachrrrr@gmail.com", "rksgibfvrzmkjyjg");

                    // Send the email
                    await client.SendAsync(message);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    //No action at this time
                    //Console.WriteLine($"Error sending email: {ex.Message}");
                }
                finally
                {
                    // Cleanly disconnect from the server
                    await client.DisconnectAsync(true);
                }
            }

            return isSuccess;
        }
    }
}