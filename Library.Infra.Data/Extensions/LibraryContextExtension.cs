using Library.Domain.Entities;
using Library.Infra.Data.Context;
using System;
using System.Collections.Generic;

namespace Library.Infra.Data.Extensions
{
    public static class LibraryContextExtension
    {
        public static void EnsureSeedDataForContext(this LibraryContext context)
        {
            context.Books.RemoveRange(context.Books);
            context.Authors.RemoveRange(context.Authors);

            var authors = new List<Author>();
            var books = new List<Book>();

            var author = new Author("Stephen", "King", new DateTimeOffset(new DateTime(1947, 9, 21)), "Horror");

            var book = new Book("The Shining");
            book.AddDescription("The Shining is a horror novel by American author Stephen King. Published in 1977, it is King's third published novel and first hardback bestseller: the success of the book firmly established King as a preeminent author in the horror genre. ");
            book.AddAuthor(author);
            author.AddBook(book);
            
            book = new Book("It");
            book.AddDescription("It is a 1986 horror novel by American author Stephen King. The story follows the exploits of seven children as they are terrorized by the eponymous being, which exploits the fears and phobias of its victims in order to disguise itself while hunting its prey. 'It' primarily appears in the form of a clown in order to attract its preferred prey of young children.");
            book.AddAuthor(author);
            author.AddBook(book);
            
            authors.Add(author);

            author = new Author("George", "RR Martin", new DateTimeOffset(new DateTime(1948, 9, 20)), "Fantasy");
            book = new Book("Game of Thrones");
            book.AddDescription("Game of Thrones is the first novel in A Song of Ice and Fire, a series of fantasy novels by American author George R. R. Martin. It was first published on August 1, 1996.");
            book.AddAuthor(author);
            author.AddBook(book);

            book = new Book("The Winds of Winter");
            book.AddDescription("Forthcoming 6th novel in A Song of Ice and Fire.");
            book.AddAuthor(author);
            author.AddBook(book);

            authors.Add(author);

            context.Authors.AddRange(authors);
            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
