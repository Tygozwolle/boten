using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Mime;
using Microsoft.Extensions.Configuration;
using RoeiVerenigingLibrary;
using System.Diagnostics;
using Attachment = Aspose.Email.Attachment;

namespace RoeiVerenigingLibrary
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

                var attachments = new List<Attachment>();
                foreach (ImapMessageInfo messageInfo in messages)
                {
                    // Access the email message
                    string[] allowedFileTypes =
                    {
                        MediaTypeNames.Image.Jpeg, MediaTypeNames.Image.Png, MediaTypeNames.Image.Gif,
                        MediaTypeNames.Image.Bmp, MediaTypeNames.Image.Tiff
                    };
                    MailMessage message = client.FetchMessage(messageInfo.UniqueId);
                    var streams = new List<Stream>();
                    foreach (Attachment attachment in message.Attachments)
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
            return $"mailto:{config["Mail:username"]}?subject={id}&&Body=Upload uw foto's hier";
        }
    }
}