using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email;
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
            client.SelectFolder(ImapFolderInfo.InBox);
            ImapMessageInfoCollection messages = client.ListMessages(1);
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
            }

        }

    }
}
