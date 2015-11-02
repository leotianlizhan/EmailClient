using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1Tianli
{
    class EncryptedMessage : Message
    {
        /// <summary>
        /// Construct an encrypted message
        /// </summary>
        /// <param name="sender">Sender's email address</param>
        /// <param name="recipient">Recipient's email address</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Content of the email</param>
        public EncryptedMessage(int key, string sender, string recipient, string subject, string content)
            : base(sender, recipient, subject, content)
        {
            //create a char array of strings of content in order to manipulate it
            char[] charArray = Content.ToCharArray();
            //loop through the char array
            for (int i = 0; i < charArray.Length; i++)
            {
                //right shift every char by the shift amount
                charArray[i] += (char)key;
            }
            //set the new, encrypted content to the char array by converting into a string
            Content = new String(charArray);
        }
        /// <summary>
        /// Decrypt and display the decrypted message
        /// </summary>
        /// <param name="key">Encryption key used, or amount of the caesar shift</param>
        /// <param name="display">Textbox for display</param>
        public override void DecryptMessage(int key, System.Windows.Forms.TextBox display)
        {
            //Create char array to manipulate the encrypted message and create string to store the result
            char[] decryptedMessageChar;
            string decrypedMessage;
            //convert the encrypted message to a character array
            decryptedMessageChar = this.Content.ToCharArray();
            //loop through the char array
            for (int i = 0; i < decryptedMessageChar.Length; i++)
            {
                //left shift every character by the shift amount
                decryptedMessageChar[i] -= (char)key;
            }
            //store the resultant message content in the string variable
            decrypedMessage = new String(decryptedMessageChar);
            //display the decrypted message
            display.Text = "To: " + this.Recipient + " \r\n\r\nSubject: < " + this.Subject + " >\r\n\r\nContent: \r\n\r\n" + decrypedMessage + "\r\n\r\n" + this.DateTime.ToString("G");
        }

    }
}
