using AutoMapper;
using Journey.Core.Repository;
using Journey.Core.Utilities;
using Journey.Models.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Books
{
    public class GetBookRequest : IRequest<Book>
    {
        public string UserId { get; private set; }
        public GetBookRequest(string userId)
        {
            UserId = userId;
        }
    }

    public class GetHandler : IRequestHandler<GetBookRequest, Book>
    {
        private readonly IRepository _repository;
        private readonly IIndexedRepository _indexedRepository;
        private readonly ICacheProvider _cache;
        private readonly IMapper _mapper;

        public GetHandler(IRepository repository, IIndexedRepository indexedRepository, IMapper mapper, ICacheProvider cacheProvider)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._indexedRepository = indexedRepository;
            this._cache = cacheProvider;
        }
        public async Task<Book> Handle(GetBookRequest request, CancellationToken cancellationToken)
        {
            var book = (await _cache.GetOrAdd<Book>($"Book_{request.UserId}", async () =>
            {
                var userBook = (await _indexedRepository.QueryAsync<UserBooks>(new Query.QueryOption() { Field = "userId", Value = request.UserId })).FirstOrDefault();
                if (userBook != null)
                    return await _repository.GetAsync<Book>(userBook.Pk, request.UserId);

                return null;
            }));

            //var userBook = (await _indexedRepository.QueryAsync<UserBooks>(new Query.QueryOption() { Field = "userId", Value = request.UserId})).FirstOrDefault();
            //for now just return the first one since we're only allowing one at the moment

            return book;
        }
    }
}
