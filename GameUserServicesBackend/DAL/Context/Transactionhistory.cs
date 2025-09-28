using System;
using System.Collections.Generic;

namespace DAL.Context;

public partial class Transactionhistory
{
    public string Id { get; set; } = null!;

    public string? UserId { get; set; }

    public DateTime? DateTrade { get; set; }

    public double? Amount { get; set; }

    public string? Status { get; set; }

    public virtual User? User { get; set; }
}
