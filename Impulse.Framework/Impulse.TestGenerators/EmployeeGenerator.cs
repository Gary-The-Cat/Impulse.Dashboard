using Management;
using Management.Models;
using System;

namespace Impulse.TestGenerators
{
    public static class EmployeeGenerator
    {
        public static (Employee, Team) Create()
        {
            var random = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));

            var team = TeamGenerator.Create();
            var id = 0;
            var maptekWikiId = "";
            var firstName = NameGenerator.Create(3 + random.Next(10));
            var lastName = NameGenerator.Create(3 + random.Next(10));

            var databaseEmployee = new DatabaseEmployee()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                MaptekWikiId = maptekWikiId,
            };

            var employee = new Employee(databaseEmployee)
            {
                TeamId = team.Id,
                MaptekWikiId = maptekWikiId,
            };

            return (employee, team);
        }
    }
}
