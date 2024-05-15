using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Mime;
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
                ImapClient client = new ImapClient("imap.gmail.com", 993, config["Mail:username"],
                    config["Mail:password"]);
                client.SelectFolder("images");
                ImapQueryBuilder builder = new ImapQueryBuilder();
                builder.HasNoFlags(ImapMessageFlags.IsRead);
                ImapMessageInfoCollection messages = client.ListMessages(builder.GetQuery());

                List<Aspose.Email.Attachment> attachments = new List<Aspose.Email.Attachment>();
                foreach (ImapMessageInfo messageInfo in messages)
                {
                    // Access the email message
                    string[] allowedFileTypes = new[]
                    {
                        MediaTypeNames.Image.Jpeg, MediaTypeNames.Image.Png, MediaTypeNames.Image.Gif,
                        MediaTypeNames.Image.Bmp, MediaTypeNames.Image.Tiff
                    };
                    Aspose.Email.MailMessage message = client.FetchMessage(messageInfo.UniqueId);
                    List<Stream> streams = new List<Stream>();
                    foreach (Aspose.Email.Attachment attachment in message.Attachments)
                    {
                        if (allowedFileTypes.Contains(attachment.ContentType.MediaType))
                        {
                            streams.Add(attachment.ContentStream);
                        }
                    }

                    try
                    {
                        repository.Add(Int32.Parse(messageInfo.Subject), streams);
                        client.MoveMessage(messageInfo.UniqueId, "proccesed");
                    }
                    catch (Exception e)
                    {
                        if (Int32.TryParse(messageInfo.Subject, out int result))
                        {
                            client.RemoveMessageFlags(messageInfo.UniqueId, ImapMessageFlags.IsRead);
                        }
                    }
                }
            }
            catch (ImapException ex)
            {
                Debug.WriteLine($"Unable to connect to the server: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static string GetStringForEmail(int id)
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<EmailToDb>().Build();
            return $"mailto:{config["Mail:username"]}?subject={id}";
        }
    }
}