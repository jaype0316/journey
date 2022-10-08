using Journey.Core.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.User
{
    public class GetOrAddRequest : IRequest<Journey.Models.User>
    {
        public string ExternalUserId { get; private set; }
        public string Email { get; private set; }
        public GetOrAddRequest(string externalUserId, string email)
        {
            this.ExternalUserId = externalUserId;
            this.Email = email;
        }
    }
    public class GetOrAddRequestHandler : IRequestHandler<GetOrAddRequest, Journey.Models.User>
    {
        readonly IRepository _repository;
        public GetOrAddRequestHandler(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Journey.Models.User> Handle(GetOrAddRequest request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetAsync<Journey.Models.User>(request.ExternalUserId, request.ExternalUserId);
            if(user == null)
            {
                await _repository.CreateAsync<Journey.Models.User>(new Journey.Models.User()
                {
                    Email = request.Email,
                    ExternalUserId = request.ExternalUserId,
                    Pk = request.ExternalUserId
                });
            }
                
            return user ?? await _repository.GetAsync<Journey.Models.User>(request.ExternalUserId, request.ExternalUserId);
        }
    }
}
