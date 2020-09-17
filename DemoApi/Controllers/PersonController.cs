using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Data;
using Models.Interfaces;

namespace DemoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRepository _personRepository;

        public PersonController(ILogger<PersonController> logger, IPersonRepository personRepository)
        {
            _logger = logger;
            _personRepository = personRepository;
        }

        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return _personRepository.Get();
        }

        [HttpGet("{id:int}")]
        public Person Get(int id)
        {
            return _personRepository.Get(id);
        }
    }
}
