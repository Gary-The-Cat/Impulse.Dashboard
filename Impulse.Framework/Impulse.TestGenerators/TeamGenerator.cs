using Management;
using System;

namespace Impulse.TestGenerators
{
    public static class TeamGenerator
    {
        public static Team Create()
        {
            var random = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
            var id = Guid.NewGuid().ToString();
            var name = NameGenerator.Create(3 + random.Next(10));
            var jiraProjectId = JiraProjectIdGenerator.CreateFromName(name);

            return new Team(id, name, jiraProjectId);
        }
    }
}
