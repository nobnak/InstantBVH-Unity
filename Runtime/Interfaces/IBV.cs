using SimpleBVH.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleBVH.Interfaces {

    public interface IBV {

        MinMaxAABB Bounds { get; }
    }
}
