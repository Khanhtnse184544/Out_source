using System;
using System.Collections.Generic;

namespace DAL.Context;

public partial class Memberpackage
{
    public string MemberTypeId { get; set; } = null!;

    public string? NameType { get; set; }

    public double? Price { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
