using System;
using System.Collections.Generic;

namespace DAL.Context;

public partial class Item
{
    public string ItemId { get; set; } = null!;

    public string? Name { get; set; }

    public string? Type { get; set; }

    public double? Price { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Categorydetail> Categorydetails { get; set; } = new List<Categorydetail>();
}
