using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assignment1Tianli
{
    class Message
    {
        //stores the sender, recipient, subject, content, and datetime of a message
        protected string _sender;
        protected string _recipient;
        protected string _subject;
        protected string _content;
        protected DateTime _dateTime;

        /// <summary>
        /// Creates a message with passed in values
        /// </summary>
        /// <param name="sender">Sender's email address</param>
        /// <param name="recipient">Recipient's email address</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="content">Body of the message</param>
        public Message(string sender, string recipient, string subject, string content)
        {
            //set each variables to the values the user passed in
            Sender = sender;
            Recipient = recipient;
            Subject = subject;
            Content = content;
            DateTime = DateTime.Now;
        }
        /// <summary>
        /// Used when one needs to read the message
        /// </summary>
        /// <returns>The message itself</returns>
        public Message ReadMessage()
        {
            return this;
        }
        /// <summary>
        /// Gets or sets the sender's email address
        /// </summary>
        public string Sender
        {
            get
            {
                return _sender;
            }
            protected set
            {
                _sender = value;
            }
        }
        /// <summary>
        /// Gets or sets the recipient's email address
        /// </summary>
        public string Recipient
        {
            get
            {
                return _recipient;
            }
            protected set
            {
                _recipient = value;
            }
        }
        /// <summary>
        /// Gets or sets the subject of the email
        /// </summary>
        public string Subject
        {
            get
            {
                return _subject;
            }
            protected set
            {
                _subject = value;
            }
        }
        /// <summary>
        /// Gets or sets the content of the email
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            protected set
            {
                _content = value;
            }
        }
        /// <summary>
        /// Gets or sets the datetime of the email
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return _dateTime;
            }
            protected set
            {
                _dateTime = value;
            }
        }
    }
}
