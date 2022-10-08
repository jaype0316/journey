using AutoMapper;
using Journey.Core.Providers;
using Journey.Core.Repository;
using Journey.Core.Services.Books;
using Journey.Core.Utilities;
using Journey.Models.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Core.Services.Chapters
{
    public class SaveChapterCommand : IRequest<string>
    {
        public int Sequence { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Thumbnail { get; set; }
        public string? UserId { get; set; }
        public string? Pk { get; set; }
        public string[]? Tags { get; set; }
    }

    public class SaveCommandHandler : IRequestHandler<SaveChapterCommand, string>
    {

        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEntityKeyProvider _entityKeyProvider;
        private readonly IMediator _mediator;

        public SaveCommandHandler(IRepository repository, IMapper mapper, IEntityKeyProvider entityKeyProvider, IMediator mediator)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._entityKeyProvider = entityKeyProvider;
            this._mediator = mediator;
        }
        public async Task<string> Handle(SaveChapterCommand request, CancellationToken cancellationToken)
        {
            var book = await _mediator.Send(new GetBookRequest(request.UserId));
            if (book == null)
                throw new InvalidOperationException("Cannot save a chapter without a book");

            if (request.Sequence == default(int))
                request.Sequence = GetNextSequence(book);

            var chapter = _mapper.Map<Chapter>(request);
            chapter.CreatedAt = request.CreatedAt ?? DateTime.UtcNow;

            if (string.IsNullOrEmpty(request.Pk))
            {
                chapter.Pk = _entityKeyProvider.Provide(new Models.UserContext() { UserId = request.UserId });
                await _repository.CreateAsync(chapter);
            }
            else
            {
                chapter.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(chapter);
            }

            //add the chapter to the book if applicable
            book.Chapters = book.Chapters ?? new List<ChapterHeader>(); 
            if(!book.Chapters.Any(c => c.Pk == chapter.Pk))
                book.Chapters.Add(new ChapterHeader() { CreatedAt = DateTime.UtcNow, Title = request.Title, Pk = chapter.Pk, Sequence = chapter.Sequence, Tags = request.Tags });

            var chapterHeader = book.Chapters.FirstOrDefault(c => c.Pk == chapter.Pk);
            if (chapterHeader != null)
            {
                chapterHeader.Title = chapter.Title;
                chapterHeader.Tags = chapter.Tags;
                chapterHeader.UpdatedAt = DateTime.UtcNow;
            }
                
            await _repository.UpdateAsync<Book>(book);

            return chapter.Pk;
        }

        private int GetNextSequence(Book book)
        {
            return book.Chapters == null ? 1 : book.Chapters.OrderByDescending(c => c.Sequence).FirstOrDefault().Sequence + 1;
        }
    }
}
