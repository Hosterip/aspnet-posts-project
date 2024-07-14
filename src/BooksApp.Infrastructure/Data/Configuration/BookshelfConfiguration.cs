﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostsApp.Domain.Book;
using PostsApp.Domain.Bookshelf;
using PostsApp.Domain.Bookshelf.Entities;
using PostsApp.Domain.Bookshelf.ValueObjects;

namespace PostsApp.Infrastructure.Data.Configuration;

public class BookshelfConfiguration : IEntityTypeConfiguration<Bookshelf>
{
    public void Configure(EntityTypeBuilder<Bookshelf> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => BookshelfId.CreateBookshelfId(value));

        builder.OwnsMany<BookshelfBook>(b => b.BookshelfBooks, bb =>
        {
            bb.HasKey(b => b.Id);

            bb.HasOne<Book>(b => b.Book)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            
            bb.Property(o => o.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => BookshelfBookId.CreateBookshelfBookId(value));
            
            bb.Navigation(b => b.Book)
                .AutoInclude();
        });

        builder.Navigation(b => b.User)
            .AutoInclude();
        builder.Navigation(b => b.BookshelfBooks)
            .AutoInclude();
    }
}