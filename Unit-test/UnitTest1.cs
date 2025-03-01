using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContactManagement;
using System.Collections.Generic;
using System.IO;
using ;
namespace ContactManagement.Tests
{
    [TestClass]
    public class ContactManagerTests
    {
        

        private List<Contact> contacts = new List<Contact>();
        private ContactManager ContactManager;
        private int nextId = 1;
        public void AddContact(string name, string phone, string email, string group)
        {
            var contact = new Contact
            {
                Id = nextId++,
                Name = name,
                Phone = phone,
                Email = email,
                Group = group
            };
            contacts.Add(contact);
        }
        public bool RemoveContact(int id)
        {
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                contacts.Remove(contact);
                return true;
            }
            return false;
        }
        public bool UpdateContact(int id, string name, string phone, string email)
        {
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                contact.Name = name;
                contact.Phone = phone;
                contact.Email = email;
                return true;
            }
            return false;
        }
        public Contact GetContactById(int id)
        {
            return contacts.FirstOrDefault(c => c.Id == id);
        }
        public List<Contact> GetAllContacts()
        {
            return contacts;
        }
        public List<Contact> SearchContacts(string searchTerm)
        {
            return contacts.Where(c => c.Name.Contains(searchTerm) || c.Phone.Contains(searchTerm)).ToList();
        }
        public int GetContactCount()
        {
            return contacts.Count;
        }
        public List<Contact> GetContactsByGroup(string group)
        {
            return contacts.Where(c => c.Group.Equals(group, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        public void ImportContacts(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 4)
                {
                    AddContact(parts[0], parts[1], parts[2], parts[3]);
                }
            }
        }
        public void ExportContacts(string filePath)
        {
            var lines = contacts.Select(c => $"{c.Name},{c.Phone},{c.Email},{c.Group}");
            File.WriteAllLines(filePath, lines);
        }
        public List<Contact> GetDuplicateContacts()
        {
            return contacts.GroupBy(c => new { c.Name, c.Phone })
                           .Where(g => g.Count() > 1)
                           .SelectMany(g => g)
                           .ToList();
        }
        [TestInitialize]
        
        public void Setup()
        {
            ContactManager = new ContactManager();
        }

        [TestMethod]
        public void AddContact()
        {

            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            var contact = ContactManager.ContactById(1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(contact);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Иван Иванов", contact.Name);
        }
        
        [TestMethod]
        public void RemoveContact()
        {
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            bool result = ContactManager.RemoveContact(1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(ContactManager.ContactById(1));
        }
        public bool UpdateContact_(int id, string name, string phone, string email)
        {
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                contact.Name = name;
                contact.Phone = phone;
                contact.Email = email;
                return true;
            }
            return false;
        }
        [TestMethod]
        public void UpdateContact()
        {
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            bool result = ContactManager.UpdateContact(1, "Иван Петров", "0987654321", "ivanpetrov@example.com");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result);
            var contact = ContactManager.ContactById(1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Иван Петров", contact.Name);
        }
        public Contact GetContactById_(int id)
        {
            return contacts.FirstOrDefault(c => c.Id == id);
        }
        [TestMethod]
        public void GetContactById()
        {
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            var contact = ContactManager.ContactById(1);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(contact);
        }
        
        [TestMethod]
        public void GetAllContacts_()
        {
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            ContactManager.AddContact("Петр Петров", "0987654321", "petr@example.com", "Работа");
            var contacts = ContactManager.GetAllContacts();
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(2, contacts.Count);
        }
        public List<Contact> SearchContacts_(string searchTerm)
        {
            return contacts.Where(c => c.Name.Contains(searchTerm) || c.Phone.Contains(searchTerm)).ToList();
        }
        [TestMethod]
        public void SearchContacts()
        {
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            ContactManager.AddContact("Петр Петров", "0987654321", "petr@example.com", "Работа");
            var results = ContactManager.SearchContacts("Иван");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void GetContactCount_()
        {
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            var count = ContactManager.GetContactCount();
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void GetContactsByGroup()
        {
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            ContactManager.AddContact("Петр Петров", "0987654321", "petr@example.com", "Работа");
            var friends = ContactManager.GetContacts("Друзья");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(1, friends.Count);
        }

        [TestMethod]
        public void ImportContacts()
        {
            
            ContactManager.ImportContacts("contacts.csv");
            var count = ContactManager.GetContactCount();
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(3, count); 
        }

        [TestMethod]
        public void ExportContacts()
        {
            
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            ContactManager.ExportContacts("contacts.csv");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(File.Exists("contacts.csv"));
        }

        [TestMethod]
        public void GetDuplicateContacts_()
        {
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            ContactManager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
            var duplicates = ContactManager.GetDuplicateContacts();
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(2, duplicates.Count);
        }

    }
}