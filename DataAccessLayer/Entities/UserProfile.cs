using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class UserProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public virtual User User { get; set; } = null!;
}
