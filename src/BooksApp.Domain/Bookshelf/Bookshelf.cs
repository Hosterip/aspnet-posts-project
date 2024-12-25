﻿using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Bookshelf.Entities;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Models;
using BooksApp.Domain.Common.Utils;
using BooksApp.Domain.User.ValueObjects;

namespace BooksApp.Domain.Bookshelf;

public class Bookshelf : AggregateRoot<BookshelfId>
{
    private Bookshelf(BookshelfId id) : base(id)
    {
    }

    private Bookshelf(BookshelfId id, User.User user, string name) : base(id)
    {
        UserId = user.Id;
        _bookshelfBooks = new List<BookshelfBook>();
        Name = name;
        ReferentialName = name.GenerateRefName();
    }

    public string Name { get; private set; }

    public string ReferentialName { get; private set; }
    public UserId UserId { get; init; }
    private readonly List<BookshelfBook> _bookshelfBooks = [];
    public IReadOnlyList<BookshelfBook> BookshelfBooks => _bookshelfBooks.AsReadOnly();

    public static Bookshelf Create(User.User user, string name)
    {
        return new Bookshelf(BookshelfId.CreateBookshelfId(), user, name);
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name could not be empty");
        
        Name = name
            .TrimStart()
            .TrimEnd();
        ReferentialName = name.GenerateRefName();
    }
    
    public bool HasBook(BookId bookId)
    {
        return _bookshelfBooks.Any(x => x.Book.Id == bookId);
    }

    public void AddBook(Book.Book book)
    {
        _bookshelfBooks.Add(BookshelfBook.Create(book));
    }

    public bool RemoveBook(Guid bookId)
    {
        var deletedCount = _bookshelfBooks.RemoveAll(book => book.Book.Id == BookId.CreateBookId(bookId));
        return deletedCount > 0;
    }
}