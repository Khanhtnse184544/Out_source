using System;
using System.Collections.Generic;

namespace DAL.Context;

public partial class Categorydetail
{
    public string UserId { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public int? Quantity { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
