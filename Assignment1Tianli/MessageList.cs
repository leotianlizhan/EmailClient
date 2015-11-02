using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1Tianli
{
    class MessageList
    {
        //create a list to save messages
        List<Message> _messageList = new List<Message>();
        /// <summary>
        /// Gets or sets the message list
        /// </summary>
        private List<Message> ListOfMessage
        {
            get
            {
                return _messageList;
            }
            set
            {
                _messageList = value;
            }
        }
        /// <summary>
        /// Add a message to the message list
        /// </summary>
        /// <param name="message">Message you want to add</param>
        public void CreateMessage(Message message)
        {
            //add the message the user passed in to the list
            ListOfMessage.Add(message);
        }
        /// <summary>
        /// Get the messages for display based on search keyword
        /// </summary>
        /// <param name="search">Search keyword</param>
        /// <returns>List of messages that matches the search</returns>
        public List<Message> GetMessageForDisplay(string search)
        {
            //create a list of messages to store messages found
            List<Message> resultsForDisplay = new List<Message>();
            //loop through the message list
            for (int i = 0; i < ListOfMessage.Count; i++)
            {
                //check if the subject or recipient contains the search keyword, if yes, add it to the result list
                //ToLower() is used to make this function case-insensitive, so that the user don't have to type exactly
                //the same case
                if (ListOfMessage[i].Subject.ToLower().Contains(search.ToLower()) || ListOfMessage[i].Recipient.ToLower().Contains(search.ToLower()))
                    resultsForDisplay.Add(ListOfMessage[i]);
            }
            //return the resultant list
            return resultsForDisplay;
        }

        /// <summary>
        /// Delete the targeted message from the message list
        /// </summary>
        /// <param name="target">Target to delete</param>
        public void DeleteMessage(Message target)
        {
            //remove the first matching message
            ListOfMessage.Remove(target);
        }
    }
}
