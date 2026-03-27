using PR.Domain.Entities;

namespace PR.Application
{
    public class PeopleEventArgs : EventArgs
    {
        public readonly IEnumerable<Person> People;

        public PeopleEventArgs(
            IEnumerable<Person> people)
        {
            People = people;
        }
    }
}