using AutoMapper;
using Journey.Core.Repository;
using Journey.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Chapters
{
    public class GetListRequest : IRequest<IEnumerable<Chapter>>
    {
        public string UserId { get; private set; }
        public GetListRequest(string userId)
        {
            this.UserId = userId;
        }
    }
    public class GetListHandler : IRequestHandler<GetListRequest, IEnumerable<Chapter>>
    {
        readonly IMapper _mapper;
        readonly IIndexedRepository _repository;
        readonly IMediator _mediator;

        public GetListHandler(IIndexedRepository repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IEnumerable<Chapter>> Handle(GetListRequest request, CancellationToken cancellationToken)
        {
            var chapters = await _repository.QueryAsync<Chapter>(new Query.QueryOption() { Field = "userId", Value = request.UserId });
            return chapters.OrderBy(c => c.Sequence);
        }
    }
}
