using System;
using System.Collections.Generic;

namespace User.Model;

public partial class UserInformation
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? Age { get; set; }

    public string? Email { get; set; }

    public string? UserPassword { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<RefreshTokenHistory> RefreshTokenHistories { get; } = new List<RefreshTokenHistory>();
}
