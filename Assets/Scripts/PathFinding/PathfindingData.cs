using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Some thign", menuName = "Path Data")]
public class PathfindingData : ScriptableObject
{
    public List<Vector3> TilePath = new List<Vector3>();
}
