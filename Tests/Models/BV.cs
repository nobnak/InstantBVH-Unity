using System.Collections;
using System.Collections.Generic;
using SimpleBVH.Interfaces;
using SimpleBVH.Models;
using UnityEngine;

public class BV : IBV {

    public MinMaxAABB Bounds { get; set; } = MinMaxAABB.Empty;

    public override string ToString() {
        return $"{Bounds}";
    }
}
