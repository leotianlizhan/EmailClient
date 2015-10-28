using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1Tianli
{
    class AddressBook
    {
        //create a list to store person objects
        private List<Person> _contacts = new List<Person>();
        /// <summary>
        /// Gets or sets the list of contacts
        /// </summary>
        private List<Person> Contacts
        {
            get
            {
                return _contacts;
            }
            set
            {
                _contacts = value;
            }
        }
        /// <summary>
        /// Add a new contact to the address book
        /// </summary>
        /// <param name="newPerson"></param>
        public void AddNewContact(Person newPerson)
        {
            //loop through the contacts first to check if an contact with the exact same info already exists
            foreach (var person in Contacts)
            {
                if (newPerson == person)
                    return;
            }
            //if not, add the new person to the contact
            Contacts.Add(newPerson);
        }
        /// <summary>
        /// Find a contact in the address book using search keyword
        /// </summary>
        /// <param name="search">Search keyword</param>
        /// <returns>List of all the people that satisfy the search keyword</returns>
        public List<Person> FindContact(string search)
        {
            //create a list of Person objects to store result
            List<Person> searchResults = new List<Person>();
            //loop through the contacts list
            for (int i=0; i < Contacts.Count; i++)
            {
                //if the a person's first name, last name, or email address satisfy the search condition, add
                //the person to the results list. Case-insensitive so the user don't have to type exact keywords
                if (Contacts[i].FirstName.Contains(search) || Contacts[i].LastName.ToLower().Contains(search.ToLower()) || Contacts[i].EmailAddress.ToLower().Contains(search.ToLower()))
                    searchResults.Add(Contacts[i]);
            }
            //return the resultant list
            return searchResults;
        }
    }
}
