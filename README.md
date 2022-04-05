# Instant BVH builder based on k-ary heap for Unity

It builds a BVH (Bounding Volume hierarchy) on complete k-ary heap.

[Unity project](https://github.com/nobnak/TestInstantBVH-Unity) of this module.

```csharp
using SimpleBVH.Models;
using SimpleBVH;
    
public class BV : IBV {
  public MinMaxAABB Bounds { get; set; };
}

var objes = new List<BV>();
// Add objects into objes
var bvh = objes.Build(2);
```

## References
1.Cline, D., Steele, K. & Egbert, P. Lightweight Bounding Volumes for Ray Tracing. J Graph Gpu Game Tools 11, 61–71 (2006).
