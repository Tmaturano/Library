using System;
using System.Collections.Generic;

namespace Library.Domain.Entities
{
    public class Author
    {           
        public Guid Id { get; private set; }
                
        public string FirstName { get; private set; }
                
        public string LastName { get; private set; }
                
        public DateTimeOffset DateOfBirth { get; private set; }
                
        public string Genre { get; private set; }

        public ICollection<Book> Books { get; private set; }

        protected Author()
        {
            Id = Guid.NewGuid();
            Books = new List<Book>();
        }

        //Criar um complex object Name e trocar aqui
        public Author(string firstName, string lastName, DateTimeOffset dateOfBirth, string genre)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Genre = genre;

            Books = new List<Book>();
        }

        public void AddBook(Book book)
        {
            Books.Add(book);
        }
                
    }
}
