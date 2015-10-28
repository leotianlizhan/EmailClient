using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1Tianli
{
    class Person
    {
        //variables to store a person's first name, last name, and email
        private string _firstName;
        private string _lastName;
        private string _emailAddress;

        /// <summary>
        /// Create a person with passed in first name, last name, and email
        /// </summary>
        /// <param name="firstName">Person's first name</param>
        /// <param name="lastName">Person's last name</param>
        /// <param name="emailAddress">Person's email</param>
        public Person(string firstName, string lastName, string emailAddress)
        {
            //sets the person's info with user's passed in value
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }
        /// <summary>
        /// Gets the person's first name
        /// </summary>
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            private set
            {
                _firstName = value;
            }
        }
        /// <summary>
        /// Gets the person's last name
        /// </summary>
        public string LastName
        {
            get
            {
                return _lastName;
            }
            private set
            {
                _lastName = value;
            }
        }
        /// <summary>
        /// Gets the person's email address
        /// </summary>
        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            private set
            {
                _emailAddress = value;
            }
        }
    }
}
