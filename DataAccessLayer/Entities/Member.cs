using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Member
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime MembershipExpiryDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

    public virtual User User { get; set; } = null!;
}
