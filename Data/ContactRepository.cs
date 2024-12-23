using Contact_info_app.Models;
using Newtonsoft.Json;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace ContactsManagementAPI.Data
{
    public class ContactRepository
    {
        private readonly string _filePath = "Data/contacts.json";

        public List<ContactInfo> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<ContactInfo>();
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<ContactInfo>>(json) ?? new List<ContactInfo>();
        }

        public ContactInfo? GetById(int id)
        {
            var contacts = GetAll();
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        public ContactInfo Add(ContactInfo contact)
        {
            var contacts = GetAll();
            contact.Id = contacts.Count == 0 ? 1 : contacts.Max(c => c.Id) + 1;
            contacts.Add(contact);
            SaveAll(contacts);
            return contact;
        }

        public bool Update(int id, ContactInfo updatedContact)
        {
            var contacts = GetAll();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null) return false;

            contact.FirstName = updatedContact.FirstName;
            contact.LastName = updatedContact.LastName;
            contact.Email = updatedContact.Email;
            SaveAll(contacts);
            return true;
        }

        public bool Delete(int id)
        {
            var contacts = GetAll();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null) return false;

            contacts.Remove(contact);
            SaveAll(contacts);
            return true;
        }

        private void SaveAll(List<ContactInfo> contacts)
        {
            var json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
