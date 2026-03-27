using System.Linq.Expressions;
using PR.Domain;
using PR.Domain.Entities;

namespace PR.Persistence.RepositoryFacades
{
    public class PersonAssociationRepositoryFacade
    {
        private static DateTime _maxDate;

        static PersonAssociationRepositoryFacade()
        {
            _maxDate = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        }

        private UnitOfWorkFacade _unitOfWorkFacade;

        private IUnitOfWork UnitOfWork => _unitOfWorkFacade.UnitOfWork;
        private DateTime? DatabaseTime => _unitOfWorkFacade.DatabaseTime;

        private DateTime CurrentTime => _unitOfWorkFacade.TransactionTime;

        public PersonAssociationRepositoryFacade(
            UnitOfWorkFacade unitOfWorkFacade)
        {
            _unitOfWorkFacade = unitOfWorkFacade;
        }

        public async Task Add(
            PersonAssociation personAssociation)
        {
            personAssociation.ObjectId = Guid.NewGuid();
            personAssociation.Created = DateTime.UtcNow;
            personAssociation.Superseded = _maxDate;

            await UnitOfWork.PersonAssociations.Add(personAssociation);
        }

        public async Task<PersonAssociation> Get(
            Guid objectId)
        {
            var predicates = new List<Expression<Func<PersonAssociation, bool>>>
            {
                pa => pa.ObjectId == objectId
            };

            AddVersionPredicates(predicates, DatabaseTime);

            var personAssociations = await UnitOfWork.PersonAssociations.Find(predicates);
            var personAssociation = personAssociations.SingleOrDefault();

            if (personAssociation == null)
            {
                throw new InvalidOperationException("Tried retrieving person association that did not exist at the given time");
            }

            return personAssociation;
        }

        public async Task<IEnumerable<PersonAssociation>> Find(
            Expression<Func<PersonAssociation, bool>> predicate)
        {
            var predicates = new List<Expression<Func<PersonAssociation, bool>>>
            {
                predicate
            };

            AddVersionPredicates(predicates, DatabaseTime);

            return await UnitOfWork.PersonAssociations.Find(predicates);
        }

        public IEnumerable<Person> Find(
            IList<Expression<Func<PersonAssociation, bool>>> predicates)
        {
            throw new NotImplementedException();
        }

        public async Task Update(
            PersonAssociation personAssociation)
        {
            var objectFromRepository = await Get(personAssociation.ObjectId);
            var newObj = personAssociation.Clone();
            objectFromRepository.Superseded = CurrentTime;
            newObj.Id = Guid.Empty;
            newObj.Created = CurrentTime;
            await UnitOfWork.PersonAssociations.Add(newObj);
        }

        public void RemoveRange(
            IEnumerable<PersonAssociation> people)
        {
            people.ToList().ForEach(p => p.Superseded = CurrentTime);
        }

        private void AddVersionPredicates(
            ICollection<Expression<Func<PersonAssociation, bool>>> predicates,
            DateTime? databaseTime)
        {
            if (databaseTime.HasValue)
            {
                predicates.Add(pa =>
                    pa.Created <= DatabaseTime &&
                    pa.Superseded > DatabaseTime);
            }
            else
            {
                predicates.Add(pa =>
                    pa.Superseded.Year == 9999);
            }
        }
    }
}