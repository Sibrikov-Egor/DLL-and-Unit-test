using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ContactManagement;

namespace ContactManagement
{
    public class Contact
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Group { get; set; }
    }
    public class ContactManager
    {
        private List<Contact> contacts = new List<Contact>();
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


        public Contact ContactById(int id)
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


        public List<Contact> GetContacts(string group)
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
        
            static void Main(string[] args)
            {
                ContactManager manager = new ContactManager();

                
                manager.AddContact("Иван Иванов", "1234567890", "ivan@example.com", "Друзья");
                manager.AddContact("Петр Петров", "0987654321", "petr@example.com", "Работа");

                var contact = manager.ContactById(1);
                Console.WriteLine($"Контакт: {contact.Name}, Телефон: {contact.Phone}");

                var allContacts = manager.GetAllContacts();
                Console.WriteLine($"Всего контактов: {manager.GetContactCount()}");

                manager.ExportContacts("contacts.csv");

            }
        
    }
}
