using AutoMapper;
using Journey.Core.Repository;
using Journey.Models.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Chapters
{
    public class GetById : IRequest<Chapter>
    {
        public string Key { get; private set; }
        public string UserId { get; private set; }
        public GetById(string key, string userId)
        {
            Key = key;
            UserId = userId;
        }
    }

    public class GetByIdHandler : IRequestHandler<GetById, Chapter>
    {
        readonly IMapper _mapper;
        readonly IRepository _repository;
        public GetByIdHandler(IMapper mapper, IRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public Task<Chapter> Handle(GetById request, CancellationToken cancellationToken)
        {
            return  _repository.GetAsync<Chapter>(request.Key, request.UserId);
        }
    }
}
