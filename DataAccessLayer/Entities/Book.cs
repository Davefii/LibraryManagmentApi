using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Isbn { get; set; } = null!;

    public string? Description { get; set; }

    public int? PublishYear { get; set; }

    public int TotalCopies { get; set; }

    public int AvailableCopies { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CoverImage { get; set; }

    public virtual IEnumerable<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

    public virtual IEnumerable<Author> Authors { get; set; } = new List<Author>();

    public virtual IEnumerable<Category> Categories { get; set; } = new List<Category>();
}
