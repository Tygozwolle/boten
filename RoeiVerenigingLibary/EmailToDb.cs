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

namespace RoeiVerenigingLibary
{
    public class EmailToDb
    {
       public EmailToDb()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddUserSecrets<EmailToDb>().Build();
            ImapClient client = new ImapClient("imap.gmail.com", 993, config["Mail:username"], config["Mail:password"]);
            client.SelectFolder(ImapFolderInfo.InBox);//Unread [Gmail]/Starred
            ImapQueryBuilder builder = new ImapQueryBuilder();
            builder.HasNoFlags(ImapMessageFlags.IsRead);
            ImapMessageInfoCollection messages = client.ListMessages(builder.GetQuery());//
            
            List<Aspose.Email.Attachment> attachments = new List<Aspose.Email.Attachment>();
            foreach (ImapMessageInfo messageInfo in messages)
            {
                // Access the email message
                Aspose.Email.MailMessage message = client.FetchMessage(messageInfo.UniqueId);
                foreach (Aspose.Email.Attachment attachment in message.Attachments)
                {
                    attachments.Add(attachment);
                    // Handle other attachment types similarly
                }
            //   client.RemoveMessageFlags(message.MessageId, ImapMessageFlags.Flagged );
               //if (messageInfo.Flags.HasFlag(ImapMessageFlags.Flagged))
               //{
               //    client.RemoveMessageFlags(messageInfo.UniqueId, ImapMessageFlags.Flagged); //message.
               //}
            }
            Console.WriteLine();
            attachments.ToArray();
        }

    }
}
