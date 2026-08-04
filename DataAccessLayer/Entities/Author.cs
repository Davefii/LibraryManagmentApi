using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Author
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Biography { get; set; }

    public string? Nationality { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? ImageAuthor { get; set; }
    public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
