using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Hangfire;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Celender_System.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CelenderController : ControllerBase
    {
       

        [HttpGet]
       // [Obsolete]
        public IActionResult Get()
        {
            //BackgroundJob.Enqueue(() => SendMessage());

            //Console.WriteLine(DateTime.Now);
            //BackgroundJob.Schedule(() => Celender(), TimeSpan.FromMinutes(1));

            //DateTime currentDate = DateTime.Now.Date;
            //Console.WriteLine(currentDate.ToShortDateString());


            //Celender("Minutely");
            RecurringJob.AddOrUpdate(() => Celender("Minutely"), Cron.Minutely);
            //RecurringJob.AddOrUpdate(() => Celender("Daily"), Cron.Daily);
            //RecurringJob.AddOrUpdate(() => Celender("Monthly"), Cron.Monthly);
            //RecurringJob.AddOrUpdate(() => Celender("Yearly"), Cron.Yearly);

            return Ok("OK");
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public void Celender(string SendTime)
        {
            // string connectionString = "Data Source=192.168.7.9;Initial Catalog=Z2DataCore;Integrated Security=False;Persist Security Info=False;User ID=sobhy;Password=sobhy@1234;MultipleActiveResultSets=True;Connect Timeout=9999"
            string connectionString = "Server=.; Database=CelenderDB; Integrated Security=True;TrustServerCertificate=True";
            // Fetch data from the table (assuming you have a table named 'Users' with 'Email' column)
            DataTable usersTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM dbo.EmailCalenderSystem";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(usersTable);
                    }
                    connection.Close();
                }
            }


            // Send email to each user

            foreach (DataRow row in usersTable.Rows)
            {
                string fromEmail = row["FromEmail"].ToString();
                string toEmail = row["ToEmail"].ToString();
                string FromPassword = row["FromPassword"].ToString();
                string emailSubject = row["EmailSubject"].ToString();
                string emailBody = row["EmailBody"].ToString();
                string sendTime = row["SendTime"].ToString();

                Console.WriteLine(fromEmail);
                Console.WriteLine(toEmail);
                Console.WriteLine(emailSubject);
                Console.WriteLine(emailBody);
                Console.WriteLine(sendTime);
                SendEmail(fromEmail, FromPassword, toEmail, emailSubject, emailBody);


            }
        }

        static void SendEmail(string fromEmail, string fromPassword, string toEmail, string subject, string body)
        {


            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(fromEmail, fromPassword);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
            //mailMessage.Subject = subject;
            //mailMessage.Body = body;
            //mailMessage.IsBodyHtml = true;

            try
            {
                smtpClient.Send(fromEmail, toEmail, subject, body);
                Console.WriteLine($"Email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email to {toEmail}: {ex.Message}");
            }
        }

        //public void SendMessage()
        //{
        //    // Select All Data In current Date
        //    // Daily
        //    //if ()
        //    //{

        //    //}
        //    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        //    smtpClient.UseDefaultCredentials = false;
        //    smtpClient.EnableSsl = true;
        //    smtpClient.Credentials = new NetworkCredential("healthcarerecord3@gmail.com", "o q e a s q z b p v i f p p j w");
        //    smtpClient.EnableSsl = true;
        //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    //MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
        //    //mailMessage.Subject = subject;
        //    //mailMessage.Body = body;
        //    //mailMessage.IsBodyHtml = true;

        //    try
        //    {
        //        smtpClient.Send("healthcarerecord3@gmail.com", "mohammedsallam812@gmail.com", "Celender", "aaaCelenderCelenderCelender");
        //        Console.WriteLine($"Email sent successfully ");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error sending email to ");
        //    }

        //}
    }
}