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
    public class SaveCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public IEnumerable<UserTag.Tag> Tags { get; set; }

        public SaveCommand(string userId, IEnumerable<UserTag.Tag> tags)
        {
            UserId = userId;
            Tags = tags;
        }
    }

    public class SaveCommandHandler : IRequestHandler<SaveCommand, Unit>
    {
        readonly IRepository _repository;
        public SaveCommandHandler(IRepository repository)
        {
            this._repository = repository;
        }
        public async Task<Unit> Handle(SaveCommand request, CancellationToken cancellationToken)
        {
            if (request.Tags == null)
                return Unit.Value;

            foreach(var tag in request.Tags)
            {
                if (string.IsNullOrWhiteSpace(tag.Id))
                    tag.Id = Guid.NewGuid().ToString();
            }

            var userTags = await _repository.GetAsync<UserTag>(request.UserId, request.UserId);
            if(userTags == null)
            {
                var newUserTags = new UserTag() { Pk = request.UserId, Sk = request.UserId, Tags = request.Tags };
                await _repository.CreateAsync<UserTag>(newUserTags);
            } else
            {
                userTags.Tags = request.Tags;
                await _repository.UpdateAsync<UserTag>(userTags);
            }

            return Unit.Value;
        }
    }
}
