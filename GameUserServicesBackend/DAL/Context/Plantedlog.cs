using System;
using System.Collections.Generic;

namespace DAL.Context;

public partial class Plantedlog
{
    public string? UserId { get; set; }

    public string? ItemId { get; set; }

    public string? Status { get; set; }

    public virtual Item? Item { get; set; }

    public virtual User? User { get; set; }
}
