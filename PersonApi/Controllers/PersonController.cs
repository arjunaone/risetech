using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PersonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonApiDbContext db = new PersonApiDbContext();

        [HttpPost("add")]
        public IActionResult Add([FromBody] JsonElement data)
        {
            string name = data.GetProperty("name").GetString();
            string surname = data.GetProperty("surname").GetString();
            string firm = data.GetProperty("firm").GetString();
            db.Add(new Person { Name = name, Surname = surname, Firm = firm });
            db.SaveChanges();
            return Ok("Person addition was successful.");
        }

        [HttpPost("contact/add/{userId:int}")]
        public IActionResult AddContact(int userId, [FromBody] JsonElement data)
        {
            string type = data.GetProperty("type").GetString();
            string content = data.GetProperty("content").GetString();
            db.Add(new Contact { UserId = userId, Type = type, Content = content });
            db.SaveChanges();
            return Ok("Contact addition was successful.");
        }

        [HttpGet("list")]
        public IActionResult List()
        {
            return Ok(db.People.ToList());
        }

        [HttpGet("detail/{userId:int}")]
        public IActionResult Detail(int userId)
        {
            Person person = db.People.Find(userId);
            List<Contact> cl = db.Contacts.Where(x => x.UserId == userId).ToList();
            return Ok(new { id = userId, name = person.Name, surname = person.Surname, firm = person.Firm, contacts = cl });
        }

        [HttpGet("delete/{userId:int}")]
        public IActionResult Delete(int userId)
        {
            Person person = db.People.Find(userId);
            db.People.Remove(person);
            List<Contact> cl = db.Contacts.Where(x => x.UserId == userId).ToList();
            db.Contacts.RemoveRange(cl);
            db.SaveChanges();
            return Ok(person.Id + " Id'ed " + person.Name + " " + person.Surname + " was deleted along with his/her contact info.");
        }

        [HttpGet("contact/delete/{contactId:int}")]
        public IActionResult DeleteContact(int contactId)
        {
            Contact contact = db.Contacts.Find(contactId);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return Ok(contact.Id + " Id'ed contact of " + contact.UserId + " Id'ed user was deleted.");
        }
    }
}
