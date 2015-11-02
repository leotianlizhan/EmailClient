/*
 * Tianli Zhan
 * Oct 23, 2015
 * This is an email client program for grade 12 Computer science class
 * It should allow user to create a message, send a message, create
 * contacts in address book, select contacts from address book, and
 * read a sent message
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace Assignment1Tianli
{
    public partial class EmailForm : Form
    {
        //default email and password constants
        public const string DEFAULT_EMAIL = "user53080@gmail.com";
        public const string DEFAULT_PASSWORD = "testuser53080";
        //encryption key, or amount to shift the message content
        public const int KEY = 5;
        //stores the email being used to send
        private string _email;
        //an addressbook and a messagelist object are created to store a person's information and a sent-message's information
        private AddressBook _addressBook = new AddressBook();
        private MessageList _messageList = new MessageList();
        //two lists for buffer are created, to store the currently being displayed messages and people, so that
        //the index of the message being clicked in the listbox can be correctly used to fetch the message/person
        private List<Message> _displayedMessageBuffer = new List<Message>();
        private List<Person> _displayedContactBuffer = new List<Person>();
        //stores the current message being read
        private Message _readMessageBuffer;
        //instantiate smtp client to send gmail messages. ONLY WORKS FOR GMAIL SINCE DIFFERNT EMAILS USES DIFFERENT PORTS
        private SmtpClient _client = new SmtpClient("smtp.gmail.com", 587);
        
        public EmailForm()
        {
            InitializeComponent();
            //allow double buffer to increase performance
            this.DoubleBuffered = true;
            //enable ssl encryption for the SMTP client, since gmail requires it
            _client.EnableSsl = true;

        }

        /// <summary>
        /// Saves message into messagelist
        /// </summary>
        private void SaveMessage(Message message)
        {
            //saves message into messagelist
            _messageList.CreateMessage(message);
            //refresh the listbox since you just added a new message
            RefreshMessageList("");
        }
        /// <summary>
        /// Send message via SMTP server
        /// </summary>
        /// <param name="message">Message you want to send</param>
        private void SendMessage(Message message)
        {
            //first try to see if sending was successful
            try
            {
                //instantiate a new built-in MailMessage object
                MailMessage msg = new MailMessage();
                //set the recipient, sender, subject, and body of the message
                msg.To.Add(new MailAddress(message.Recipient));
                msg.From = new MailAddress(message.Sender);
                msg.Subject = message.Subject;
                msg.Body = message.Content;
                //send the email
                _client.Send(msg);
            }
            catch (Exception ex)
            {
                //if an exception occurs, display the exception message
                MessageBox.Show(ex.Message);
                //exit this procedure so SaveMessage and CloseSendMessage doesn't get executed
                //so the user can fix the recipient address if that's the cause of the exception
                return;
            }
            //if all is fine, save the message and close the send message panel
            SaveMessage(message);
            CloseSendMessage();
        }


        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            SendMessage(new Message(_email,txtRecipient.Text,txtSubject.Text,txtContent.Text));
        }

        //GUI enhancements, change the "search" text to empty when clicked on searchbox.
        //Will not comment on this for other textboxes
        private void txtMessageListSearch_Enter(object sender, EventArgs e)
        {
            txtMessageListSearch.Text = "";
        }
        private void txtMessageListSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshMessageList(txtMessageListSearch.Text);
        }
        /// <summary>
        /// Refresh the message list with new search keywords
        /// </summary>
        public void RefreshMessageList(string search)
        {
            //fetch the messages for display that meets the search criteria and store in the buffer list
            _displayedMessageBuffer = _messageList.GetMessageForDisplay(search);
            //reverse the buffer list, because we want messages in order of the latest to earliest message
            _displayedMessageBuffer.Reverse();
            //clear the items already in the listbox
            lstMessageList.Items.Clear();
            //add each message's title in the buffer list
            for (int i=0; i<_displayedMessageBuffer.Count; i++)
            {
                lstMessageList.Items.Add(_displayedMessageBuffer[i].Subject);
            }
            //refresh the form so the listbox can update
            Refresh();
        }

        private void btnCompose_Click(object sender, EventArgs e)
        {
            ShowSendMessage();
        }
        private void btnCloseSending_Click(object sender, EventArgs e)
        {
            CloseSendMessage();
        }
        /// <summary>
        /// Shows the send-message interface
        /// </summary>
        private void ShowSendMessage()
        {
            pnlMesssageList.Visible = false;
            pnlCreateMessage.Visible = true;
            //update the select-from-contact listbox in order to show any new contact added
            RefreshContact("", lstSelectFromContact);
        }
        /// <summary>
        /// Closes the send-message interface
        /// </summary>
        private void CloseSendMessage()
        {
            pnlMesssageList.Visible = true;
            pnlCreateMessage.Visible = false;
            //reset the textboxes to empty
            txtRecipient.Text = "";
            txtSubject.Text = "";
            txtContent.Text = "";
            txtSelectFromContactSearch.Text = "";
            txtMessageListSearch.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }
        /// <summary>
        /// Login the user with his email and password, unless he chose to use default email
        /// </summary>
        private void Login()
        {
            //check if the gmail textbox is empty
            if (txtLoginEmail.Text == "")
            {
                //if it is use default email and password
                _email = DEFAULT_EMAIL;
                _client.Credentials = new NetworkCredential(_email, DEFAULT_PASSWORD);
            }
            else
            {
                //if not use the user-inputed gmail and password
                _email = txtLoginEmail.Text;
                _client.Credentials = new NetworkCredential(_email, txtLoginPassword.Text);
            }
            //hide the login interface and show the main interface
            pnlLogin.Visible = false;
            pnlMesssageList.Visible = true;
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Logout();
        }
        /// <summary>
        /// Logout the user
        /// </summary>
        private void Logout()
        {
            //set login textboxes to empty
            txtLoginEmail.Text = "";
            txtLoginPassword.Text = "";
            //hide the main panel and show the login panel
            pnlMesssageList.Visible = false;
            pnlLogin.Visible = true;
        }

        private void lblAddressBook_Click(object sender, EventArgs e)
        {
            //change to address book interface if clicked
            pnlAddressBook.Visible = true;
            pnlMessageReader.Visible = false;
        }
        //GUI enhancements, same with all the ones below for SentMail and About
        private void lblAddressBook_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            lblAddressBook.BackColor = SystemColors.ControlLight;
        }
        private void lblAddressBook_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            lblAddressBook.BackColor = SystemColors.Control;
        }

        private void lblSentMail_Click(object sender, EventArgs e)
        {
            //change to sent mail interface if clicked
            pnlAddressBook.Visible = false;
            pnlMessageReader.Visible = false;
        }
        private void lblSentMail_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            lblSentMail.BackColor = SystemColors.ControlLight;
        }
        private void lblSentMail_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            lblSentMail.BackColor = SystemColors.Control;
        }

        private void lblAbout_Click(object sender, EventArgs e)
        {
            //shows friendly information about the program and how to deal with failed-to-send-message
            MessageBox.Show("This is an Email Client Project for grade 12 Computer Science for Mr.Hsiung\n\nIf messages fail to send, please check:\n1.If you're using Gmail, since other email services has diffent ports\n2.If your school blocks port 587\n3.If your Gmail account does not allow less secure apps from accessing\n\n*Mr.Hsiung please give mercy*");
        }
        private void lblAbout_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            lblAbout.BackColor = SystemColors.ControlLight;
        }
        private void lblAbout_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            lblAbout.BackColor = SystemColors.Control;
        }

        private void txtSearchAddressBook_Enter(object sender, EventArgs e)
        {
            txtSearchAddressBook.Text = "";
        }
        private void txtSearchAddressBook_TextChanged(object sender, EventArgs e)
        {
            RefreshContact(txtSearchAddressBook.Text, lstAddressBook);
        }

        private void lstAddressBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadContact(lstAddressBook.SelectedIndex);
        }
        
        /// <summary>
        /// Display contact information based on the contact the user choose
        /// </summary>
        /// <param name="index">Index of the contact the user chose, usually from a listbox</param>
        private void ReadContact(int index)
        {
            //prevent it from accessing negative index, since default index of listbox is -1
            if(index >= 0)
                //set the displaying label with info
                lblContactInfoDisplay.Text = "First Name: " + _displayedContactBuffer[index].FirstName + "\nLast Name: " + _displayedContactBuffer[index].LastName + "\nEmail Address: " + _displayedContactBuffer[index].EmailAddress;
        }
        /// <summary>
        /// Add new contact into the addressbook
        /// </summary>
        private void AddNewContact()
        {
            //add the new contact based on the textbox input
            _addressBook.AddNewContact(new Person(txtNewFirstName.Text, txtNewLastName.Text, txtNewEmailAddress.Text));
        }
        /// <summary>
        /// Refresh contact listbox with new search keyword
        /// </summary>
        /// <param name="search">Keyword for searching such contact</param>
        /// <param name="display">ListBox for displaying the results</param>
        private void RefreshContact(string search, ListBox display)
        {
            //fetch the messages for display that matches the search keyword and store in a list for buffer
            _displayedContactBuffer = _addressBook.FindContact(search);
            //clear the listbox
            display.Items.Clear();
            //add in contacts all email address from the buffer list to the listbox
            for (int i=0; i<_displayedContactBuffer.Count; i++)
            {
                display.Items.Add(_displayedContactBuffer[i].EmailAddress);
            }
            //refresh the form to show the refreshed contact
            Refresh();
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {
            //show the addcontact interface
            pnlAddContact.Visible = true;
        }
        /// <summary>
        /// Closes the add-contact interface
        /// </summary>
        private void CloseCreateNewContact()
        {
            //reset the textboxes to empty
            txtNewFirstName.Text = "";
            txtNewLastName.Text = "";
            txtNewEmailAddress.Text = "";
            txtSearchAddressBook.Text = "";
            //close the add-contact interface
            pnlAddContact.Visible = false;
        }
        private void btnCancelNewContact_Click(object sender, EventArgs e)
        {
            //close the add-contact interface
            CloseCreateNewContact();
        }
        private void btnSubmitNewContact_Click(object sender, EventArgs e)
        {
            //add new contact
            AddNewContact();
            //refresh the listbox since a new contact is added
            RefreshContact("", lstAddressBook);
            //close the add-contact interface
            CloseCreateNewContact();
        }

        private void txtSelectFromContactSearch_MouseEnter(object sender, EventArgs e)
        {
            txtSelectFromContactSearch.Text = "";
        }
        private void txtSelectFromContactSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshContact(txtSelectFromContactSearch.Text, lstSelectFromContact);
        }
        private void btnSelectFromContact_Click(object sender, EventArgs e)
        {
            SelectFromContact();
        }
        /// <summary>
        /// Set the recipient to whoever the user select from the contact
        /// </summary>
        private void SelectFromContact()
        {
            //prevent accessing index of -1 in the buffer list, since default without any item for an listbox is -1
            if (lstSelectFromContact.SelectedIndex >= 0)
            {
                //change the recipient textbox to the one user selected
                txtRecipient.Text = _displayedContactBuffer[lstSelectFromContact.SelectedIndex].EmailAddress;
            }
        }

        private void lstMessageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadMessage(lstMessageList.SelectedIndex);
        }
        /// <summary>
        /// Display the message the user selected
        /// </summary>
        /// <param name="index">Index of the message the user selected in an listbox</param>
        private void ReadMessage(int index)
        {
            //prevent accessing index of -1 since default index with no item for a listbox is -1
            if (lstMessageList.SelectedIndex >= 0)
            {
                //hide the encrypted or spy message reader button from last time, and change color to normal
                btnSpyMessageReader.Visible = false;
                btnEncryptedMessageReader.Visible = false;
                txtMessageReader.BackColor = Color.White;
                txtMessageReader.ForeColor = Color.Black;
                //Fetch the message the user selected and store it in a single message buffer
                _readMessageBuffer = _displayedMessageBuffer[index].ReadMessage();
                //show the message reader interface
                pnlMessageReader.Visible = true;
                //display the message
                txtMessageReader.Text = "To: " + _readMessageBuffer.Recipient + " \r\n\r\nSubject: < " + _readMessageBuffer.Subject + " >\r\n\r\nContent: \r\n\r\n" + _readMessageBuffer.Content + "\r\n\r\n" + _readMessageBuffer.DateTime.ToString("G");
                //show a spy message reader button if it's a spy message and it's not yet destroyed
                if (_readMessageBuffer is SpyMessage && !((SpyMessage)_readMessageBuffer).Destroyed)
                    btnSpyMessageReader.Visible = true;
                //show an encrypted message reader button if it's an encrypted message, but not a spy message
                else if(_readMessageBuffer is EncryptedMessage && !(_readMessageBuffer is SpyMessage))
                    btnEncryptedMessageReader.Visible = true;
            }
        }

        private void btnQuitMessageReader_Click(object sender, EventArgs e)
        {
            pnlMessageReader.Visible = false;
        }

        private void btnEncryptedMessageReader_Click(object sender, EventArgs e)
        {
            //decrypt the message and change the color of the display for coolness factor
            _readMessageBuffer.DecryptMessage(KEY, txtMessageReader);
            txtMessageReader.BackColor = Color.Black;
            txtMessageReader.ForeColor = Color.LimeGreen;
            btnEncryptedMessageReader.Visible = false;
        }

        private void btnSendEncryptedMessage_Click(object sender, EventArgs e)
        {
            //calls the send message method to send the email, but pass in as an encrypted message
            SendMessage(new EncryptedMessage(KEY, _email, txtRecipient.Text, txtSubject.Text, txtContent.Text));
        }

        private void btnSendSpyMessage_Click(object sender, EventArgs e)
        {
            //calls the send meessage method to send the email but passed as a spy message
            SendMessage(new SpyMessage(KEY, _email, txtRecipient.Text, txtSubject.Text, txtContent.Text));
        }

        private void btnSpyMessageReader_Click(object sender, EventArgs e)
        {
            //make messagereader invisible, and make SpyMessageReader visible
            pnlMessageReader.Visible = false;
            pnlSpyMessageReader.Visible = true;
            //decrypt and desplay the message
            ((SpyMessage)_readMessageBuffer).DecryptMessage(KEY, txtSpyMessageReader, this, lblTimer, pnlSpyMessageReader);
            //set the search bar to empty string so that when you exit the reader it's consistent
            txtMessageListSearch.Text = "";
        }

        private void btnDestroy_Click(object sender, EventArgs e)
        {
            //destroy the message
            ((SpyMessage)_readMessageBuffer).Destroy(this);
        }
    }
}
