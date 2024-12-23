using Contact_info_app.Models;
using ContactsManagementAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contact_info_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Contact : ControllerBase
    {

        private readonly ContactRepository _repository;

        public Contact(ContactRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllContacts(string? search = null)
        {
            var contacts = _repository.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                contacts = contacts.Where(c =>
                    (c.FirstName?.ToLower().Contains(search) ?? false) ||
                    (c.LastName?.ToLower().Contains(search) ?? false) ||
                    (c.Email?.ToLower().Contains(search) ?? false)
                ).ToList();
            }

            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public IActionResult GetContactById(int id)
        {
            var contact = _repository.GetById(id);
            if (contact == null) return NotFound(new { message = "Contact not found." });
            return Ok(contact);
        }

        [HttpPost]
        public IActionResult AddContact([FromBody] ContactInfo contact)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newContact = _repository.Add(contact);
            return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id }, newContact);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id, [FromBody] ContactInfo updatedContact)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = _repository.Update(id, updatedContact);
            if (!success) return NotFound(new { message = "Contact not found." });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var success = _repository.Delete(id);
            if (!success) return NotFound(new { message = "Contact not found." });

            return NoContent();
        }
    }
}

