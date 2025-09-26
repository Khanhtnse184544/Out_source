using System;
using System.Collections.Generic;

namespace DAL.Context;

public partial class Scene
{
    public string UserId { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? DateSave { get; set; }
}
