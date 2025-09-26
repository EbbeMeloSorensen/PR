using System.Globalization;
using AutoMapper;
using MediatR;
using PR.Persistence;
using PR.Persistence.Versioned;
using PR.Web.Application.Core;
using PR.Web.Application.Interfaces;
using System.Linq.Expressions;
using PR.Domain.Entities.PR;

namespace PR.Web.Application.People;

public class List
{
    public class Query : IRequest<Result<PagedList<PersonDto>>>
    {
        public PersonParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<PersonDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public Handler(
            IMapper mapper, 
            IUserAccessor userAccessor,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
            _unitOfWorkFactory = new UnitOfWorkFactoryFacade(unitOfWorkFactory);
        }

        public async Task<Result<PagedList<PersonDto>>> Handle(
            Query request, 
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Params.HistoricalTime))
            {
                try
                {
                    var dbTime = DateTime.ParseExact(request.Params.HistoricalTime, "yyyy-MM-ddTHH:mm:ssZ",
                        CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

                    (_unitOfWorkFactory as UnitOfWorkFactoryFacade)!.HistoricalTime = dbTime;
                }
                catch (Exception e)
                {
                    return Result<PagedList<PersonDto>>.Failure("Invalid time format");
                }
            }

            if (request.Params.IncludeHistoricalObjects.HasValue)
            {
                (_unitOfWorkFactory as UnitOfWorkFactoryFacade)!.IncludeHistoricalObjects = request.Params.IncludeHistoricalObjects.Value;
            }

            if (!string.IsNullOrEmpty(request.Params.DatabaseTime))
            {
                try
                {
                    var dbTime = DateTime.ParseExact(request.Params.DatabaseTime, "yyyy-MM-ddTHH:mm:ssZ",
                        CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

                    (_unitOfWorkFactory as UnitOfWorkFactoryFacade)!.DatabaseTime = dbTime;
                }
                catch (Exception e)
                {
                    return Result<PagedList<PersonDto>>.Failure("Invalid time format");
                }
            }

            using var unitOfWork = _unitOfWorkFactory.GenerateUnitOfWork();

            var predicates = new List<Expression<Func<Person, bool>>>();

            if (!string.IsNullOrEmpty(request.Params.Name))
            {
                var filter = request.Params.Name.ToLower();

                predicates.Add(x =>
                    x.FirstName.ToLower().Contains(filter) ||
                    (!string.IsNullOrEmpty(x.Surname) && x.Surname.ToLower().Contains(filter)));
            }

            var people = await unitOfWork.People.Find(predicates);

            var result = _mapper.Map<IEnumerable<PersonDto>>(people);

            return Result<PagedList<PersonDto>>.Success(
                PagedList<PersonDto>.Create(result, request.Params.PageNumber,
                    request.Params.PageSize)
            );
        }
    }
}