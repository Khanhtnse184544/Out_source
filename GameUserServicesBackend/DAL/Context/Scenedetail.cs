using System;
using System.Collections.Generic;

namespace DAL.Context;

public partial class Scenedetail
{
    public string? UserId { get; set; }

    public string? ItemId { get; set; }

    public string? Name { get; set; }

    public int? Level { get; set; }

    public int? ExpPerLevel { get; set; }

    public double? PositionX { get; set; }

    public double? PositionY { get; set; }

    public virtual Scene? User { get; set; }
}
