using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/person")]
    //[Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly PersonContext personContext;

        public PersonController(ILogger<WeatherForecastController> logger, PersonContext personContext)
        {
            _logger = logger;
            this.personContext = personContext;
        }

        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return personContext.Person.ToArray();
        }

        [HttpPost]
        public IEnumerable<Person> Post(Person person)
        {
            personContext.Person.Add(person);
            personContext.SaveChanges();
            return personContext.Person.ToArray();
        }

        [HttpDelete]
        public IEnumerable<Person> Delete(int id)
        {
            var p = personContext.Person.Where(person => person.Id == id).FirstOrDefault();
            if (p != null)
            {
                personContext.Remove(p);
                personContext.SaveChanges();
            }
            return personContext.Person.ToArray();
        }
    }
}
