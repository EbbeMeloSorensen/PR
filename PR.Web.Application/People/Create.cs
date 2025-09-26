using FluentValidation;
using MediatR;
using PR.Domain.Entities.PR;
using PR.Persistence;
using PR.Persistence.Versioned;
using PR.Web.Application.Core;
using PR.Web.Application.Interfaces;

namespace PR.Web.Application.People;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public Person Person { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Person).SetValidator(new PersonValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public Handler(
            IUserAccessor userAccessor,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _userAccessor = userAccessor;
            _unitOfWorkFactory = new UnitOfWorkFactoryFacade(unitOfWorkFactory);
        }

        public async Task<Result<Unit>> Handle(
            Command request, 
            CancellationToken cancellationToken)
        {
            using (var unitOfWork = _unitOfWorkFactory.GenerateUnitOfWork())
            {
                // HUSK AWAIT HER, ELLERS VIRKER DET IKKE!!
                await unitOfWork.People.Add(request.Person);
                unitOfWork.Complete();
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}