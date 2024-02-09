using System;
using System.Collections.Generic;

namespace User.Model;

public partial class RefreshTokenHistory
{
    public int TokenHistoryId { get; set; }

    public int? UserId { get; set; }

    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual UserInformation? User { get; set; }
}
