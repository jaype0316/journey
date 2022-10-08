using AutoMapper;
using Journey.Core.Providers;
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
    public class SaveBookCommand : IRequest<Book>
    {
        public string Title { get; set; }
        public string? Pk { get; set; }
        public string? About { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Thumbnail { get; set; }
        public string? UserId { get; set; } //todo: move this out

        public SaveBookCommand()
        {

        }
        public SaveBookCommand(string userId)
        {
            this.UserId = UserId;
        }
    }

    public class SaveBookCommandHandler : IRequestHandler<SaveBookCommand, Book>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEntityKeyProvider _entityKeyProvider;

        public SaveBookCommandHandler(IRepository repository, IMapper mapper, IEntityKeyProvider entityKeyProvider)
        {
            this._entityKeyProvider = entityKeyProvider;
            this._repository = repository;
            this._mapper = mapper;
        }
        public async Task<Book> Handle(SaveBookCommand request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request);
            if (string.IsNullOrEmpty(request.Pk))
            {
                book.Pk = _entityKeyProvider.Provide(new Models.UserContext() { UserId = request.UserId });
                book.CreatedAt = book.CreatedAt ?? DateTime.UtcNow;
                await _repository.CreateAsync<Book>(book);
            } else
            {
                await _repository.UpdateAsync<Book>(book);
            }

            return  book;
        }
    }
}
