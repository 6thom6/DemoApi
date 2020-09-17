using Models.Data;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.Database;

namespace Models.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private Connection _connection;

        public PersonRepository(Connection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Person> Get()
        {
            Command command = new Command("Select Top 100 BusinessEntityId, LastName, FirstName from Person.Person;");
            return _connection.ExecuteReader(command, dr => new Person() { BusinessEntityId = (int)dr["BusinessEntityId"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"] });
        }

        public Person Get(int businessEntityId)
        {
            Command command = new Command("Select Top 100 BusinessEntityId, LastName, FirstName from Person.Person where BusinessEntityId = @BusinessEntityId;");
            command.AddParameter("BusinessEntityId", businessEntityId);
            return _connection.ExecuteReader(command, dr => new Person() { BusinessEntityId = (int)dr["BusinessEntityId"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"] }).SingleOrDefault();
        }
    }
}
