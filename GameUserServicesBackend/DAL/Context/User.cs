using System;
using System.Collections.Generic;

namespace DAL.Context;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? Level { get; set; }

    public int? ExpPerLevel { get; set; }

    public int? Coin { get; set; }

    public string? MemberTypeId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Categorydetail> Categorydetails { get; set; } = new List<Categorydetail>();

    public virtual Memberpackage? MemberType { get; set; }

    public virtual ICollection<Transactionhistory> Transactionhistories { get; set; } = new List<Transactionhistory>();
}
