using Journey.Core.Repository;
using Journey.Models.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Tags
{
    public class GetRequest : IRequest<IEnumerable<UserTag.Tag>>
    {     
        public string UserId { get; set; }
        public GetRequest(string userId)
        {
            UserId = userId;
        }
    }

    public class GetHandler : IRequestHandler<GetRequest, IEnumerable<UserTag.Tag>>
    {
        readonly IRepository _repository;
        public GetHandler(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<UserTag.Tag>> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            return (await _repository.GetAsync<UserTag>(request.UserId, request.UserId))?.Tags;
        }
    }
}
