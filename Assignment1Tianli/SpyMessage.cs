using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Assignment1Tianli
{
    class SpyMessage : EncryptedMessage
    {
        //a constant to store amount of miliseconds the user can read before destroying the message
        public const int TIME_LIMIT = 30000;
        //a constant to store the state of the spy message, destroyed or not
        private bool _destroyed = false;
        //a timer to countdown time before destroyed, an interval of 1 second is assigned
        private System.Timers.Timer _destroyTimer = new System.Timers.Timer(1000);
        //stores the time left
        private int _timeLeft;
        //stores the label that count down time
        private System.Windows.Forms.Label _countDown;
        //stores the panel for destroy
        private System.Windows.Forms.Panel _readerPanel;
        //stores the form for destroy
        private EmailForm _form;
        /// <summary>
        /// Construct a spy message
        /// </summary>
        /// <param name="key">Encryption key to use, amount to shift characters by</param>
        /// <param name="sender">Sender's email address</param>
        /// <param name="recipient">Recipient's email address</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Content of the email</param>
        public SpyMessage(int key, string sender, string recipient, string subject, string content)
            : base(key, sender, recipient, subject, content)
        {
            //assign an event handler for timer so I can do actions on each tick
            _destroyTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //set the time left to the maximum amount of time
            TimeLeft = TIME_LIMIT;
        }
        //called every tick of the timer
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //check if the time left is greater than 0
            if (TimeLeft > 0)
            {
                //decrease the time by the interval of the timer
                TimeLeft -= (int)_destroyTimer.Interval;
                //invoke this on the main thread, since the timer runs on a separate thread
                _form.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                {   
                    //change the countdown label on the main thread
                    _countDown.Text = "Time Left: " + TimeLeft / 1000;
                }));
            }
            else
            {
                //invoke this on the main thread, since the timer runs on a separate thread
                _form.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    //destroy the current message
                    Destroy(_form);
                }));
                
            }
        }
        /// <summary>
        /// Gets or sets the boolean that store if the message is destroyed, for when checking if spy message button should appear
        /// </summary>
        public bool Destroyed
        {
            get
            {
                return _destroyed;
            }
            private set
            {
                _destroyed = value;
            }
        }
        /// <summary>
        /// Gets or sets the time left
        /// </summary>
        private int TimeLeft
        {
            get
            {
                return _timeLeft;
            }
            set
            {
                _timeLeft = value;
            }
        }
        /// <summary>
        /// Decrypt and displays the spy message
        /// </summary>
        /// <param name="key">Encryption key, amount used to shift the characters by</param>
        /// <param name="display">Textbox for displaying the message</param>
        /// <param name="form">Form of the UI</param>
        /// <param name="countDown">Label used to display count down</param>
        /// <param name="readerPanel">Panel of the Spy Message Reader</param>
        public void DecryptMessage(int key, System.Windows.Forms.TextBox display, EmailForm form, System.Windows.Forms.Label countDown, System.Windows.Forms.Panel readerPanel)
        {
            //calls the parent function to decrypt and display the message like before
            base.DecryptMessage(key, display);
            //starts the timer to countdown the time you have left reading the message
            _destroyTimer.Enabled = true;
            //save the form to use in destroy
            _form = form;
            //save the countdown label to use in timer tick
            _countDown = countDown;
            //save the spy message reader panel to use later in destroy
            _readerPanel = readerPanel;
        }
        /// <summary>
        /// Destroys this current message
        /// </summary>
        /// <param name="form">Form of the UI</param>
        public void Destroy(EmailForm form)
        {
            //stops the timer
            _destroyTimer.Enabled = false;
            //destroys the content of this message
            this.Content = "This message has be destroyed";
            //closes the spy message reader
            _readerPanel.Visible = false;
            //set the destroyed boolean to true
            this.Destroyed = true;
            //refresh the message list
            form.RefreshMessageList("");
        }
    }
}
