using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Pop3;
using Microsoft.Extensions.Configuration;
using Attachment = Aspose.Email.Attachment;

namespace RoeiVerenigingLibary
{
    public class EmailToDb
    {

        public static void GetImagesFromEmail(IImageRepository repository)
        {
            try
            {
                IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<EmailToDb>().Build();
                ImapClient client = new ImapClient("imap.gmail.com", 993, config["Mail:username"], config["Mail:password"]);
                client.SelectFolder("images"); 
                ImapQueryBuilder builder = new ImapQueryBuilder();
                builder.HasNoFlags(ImapMessageFlags.IsRead);
                ImapMessageInfoCollection messages = client.ListMessages(builder.GetQuery());

                List<Aspose.Email.Attachment> attachments = new List<Aspose.Email.Attachment>();
                foreach (ImapMessageInfo messageInfo in messages)
                {
                    // Access the email message
                    Aspose.Email.MailMessage message = client.FetchMessage(messageInfo.UniqueId);
                    List<Stream> streams = new List<Stream>();
                    foreach (Aspose.Email.Attachment attachment in message.Attachments)
                    {

                        
                        streams.Add(attachment.ContentStream);
 
             
                    }

                    int result;

                    try
                    {
                        repository.Add(Int32.Parse(messageInfo.Subject), streams);
                    }
                    catch (Exception e)
                    {
                        if (Int32.TryParse(messageInfo.Subject, out result))
                        {
                            client.RemoveMessageFlags(messageInfo.UniqueId, ImapMessageFlags.IsRead);
                        }
                    }
                }
            }
            catch (ImapException ex)
            {
                Console.WriteLine($"Unable to connect to the server: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}