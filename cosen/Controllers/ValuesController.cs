using cosen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace cosen.Controllers
{
    public class ValuesController : ApiController
    {
        private static IList<Contact> contacts = new List<Contact> {
            new Contact{Id="001",Name="cosen",PhoneNo="13520128540",EmailAddress="likeshan168@gmail.com"},
            new Contact{Id="002",Name="andy",PhoneNo="123344543221",EmailAddress="andylau@hotmail.com"}
        };
        // GET api/values
        //public IEnumerable<Contact>Get()
        //{
        //    return contacts;
        //}

        public IQueryable Get()
        {
            string[] names={"andy","cosen","jack"};
            return names.Select((p, index) => new { id = index, name = p }).AsQueryable();
        }
        // GET api/values/5
        public Contact Get(string id)
        {
            return contacts.FirstOrDefault(p=>p.Id==id);
        }

        // POST api/values
        public void Post(Contact contact)
        {
            Delete(contact.Id);
            contacts.Add(contact);
        }

        // PUT api/values/5
        public void Put(Contact contact)
        {
            contact.Id = Guid.NewGuid().ToString();
            contacts.Add(contact);
        }

        // DELETE api/values/5
        public void Delete(string id)
        {
            Contact contact = contacts.FirstOrDefault(p=>p.Id==id);
            contacts.Remove(contact);
        }
    }
}