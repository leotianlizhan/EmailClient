using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1Tianli
{
    class EncryptedMessage : Message
    {
        //the amount for letters to be shifted by
        private const int SHIFT_AMOUNT = 5;
        public EncryptedMessage(string sender, string recipient, string subject, string content)
            : base(sender, recipient, subject, content)
        {
            char[] charArray = Subject.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                charArray[i] += (char)SHIFT_AMOUNT;
            }
            Subject = new String(charArray);
        }
        public void DecryptMessage(

    }
}
