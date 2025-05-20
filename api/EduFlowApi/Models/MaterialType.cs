using System;
using System.Collections.Generic;

namespace EduFlowApi.Models;

public partial class MaterialType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
