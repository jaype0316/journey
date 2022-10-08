using AutoMapper;
using Journey.Core.Repository;
using Journey.Core.Services.Books;
using Journey.Models.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Chapters
{
    public class GetHeadersListRequest : IRequest<IEnumerable<ChapterHeader>>
    {
        public string UserId { get; private set; }
        public string? OrderBy { get; private set; }
        public GetHeadersListRequest(string userId, string? orderBy)
        {
            this.UserId = userId;
            OrderBy = orderBy;
        }
    }
    public class GetHeadersListHandler : IRequestHandler<GetHeadersListRequest, IEnumerable<ChapterHeader>>
    {
        readonly IMapper _mapper;
        readonly IRepository _repository;
        readonly IMediator _mediator;

        public GetHeadersListHandler(IRepository repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<IEnumerable<ChapterHeader>> Handle(GetHeadersListRequest request, CancellationToken cancellationToken)
        {
            var book = await _mediator.Send(new GetBookRequest(request.UserId));

            return request.OrderBy == "sequence" ? book?.Chapters?.OrderBy(c => c.Sequence) 
                   : book?.Chapters?.OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt);
        }
    }
}
