using System.Diagnostics;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Mime;
using Microsoft.Extensions.Configuration;
using RoeiVerenigingLibary;
using Attachment = Aspose.Email.Attachment;

namespace RoeiVerenigingLibrary;

public class EmailToDb
{
    public static void GetImagesFromEmail(IImageRepository repository)
    {
        try
        {
            var config = new ConfigurationBuilder().AddUserSecrets<EmailToDb>().Build();
            var client = new ImapClient("imap.gmail.com", 993, config["Mail:username"],
                config["Mail:password"]);
            client.SelectFolder("images");
            var builder = new ImapQueryBuilder();
            builder.HasNoFlags(ImapMessageFlags.IsRead);
            var messages = client.ListMessages(builder.GetQuery());

            var attachments = new List<Attachment>();
            foreach (var messageInfo in messages)
            {
                // Access the email message
                string[] allowedFileTypes =
                {
                    MediaTypeNames.Image.Jpeg, MediaTypeNames.Image.Png, MediaTypeNames.Image.Gif,
                    MediaTypeNames.Image.Bmp, MediaTypeNames.Image.Tiff
                };
                var message = client.FetchMessage(messageInfo.UniqueId);
                var streams = new List<Stream>();
                foreach (var attachment in message.Attachments)
                    if (allowedFileTypes.Contains(attachment.ContentType.MediaType))
                        streams.Add(attachment.ContentStream);

                try
                {
                    repository.Add(int.Parse(messageInfo.Subject), streams);
                    client.MoveMessage(messageInfo.UniqueId, "proccesed");
                }
                catch (Exception e)
                {
                    if (int.TryParse(messageInfo.Subject, out var result))
                        client.RemoveMessageFlags(messageInfo.UniqueId, ImapMessageFlags.IsRead);
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
        var config = new ConfigurationBuilder().AddUserSecrets<EmailToDb>().Build();
        return $"mailto:{config["Mail:username"]}?subject={id}";
    }
}