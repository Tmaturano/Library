using System;

namespace Library.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
                
        public string Title { get; private set; }
                
        public string Description { get; private set; }
                
        public Author Author { get; private set; }

        public Guid AuthorId { get; private set; }

        protected Book()
        {
            
        }

        public Book(string title)
        {            
            Title = title;
            Description = string.Empty;
        }

        public void AddAuthor(Author author)
        {            
            Author = author;
        }

        public void AddDescription(string description)
        {
            Description = description;
        }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Title))
                return false;

            return true;
        }
    }
}
