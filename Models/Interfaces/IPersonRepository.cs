using Models.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Interfaces
{
    public interface IPersonRepository
    {
        IEnumerable<Person> Get();
        Person Get(int businessEntityId);
    }
}
