using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Edge : MonoBehaviour
{
    public Tile[] tiles; // holds 2 tiles that are connected through this edge
    public int weight; // sorts the order to process all edges

    public void DisableEdge() // when edge is between a not connected set of tiles it needs to be removed, to create a path
    {
        Destroy(this.gameObject);
    }

    public int CompareTo(Edge other) // a way to compare weights with another edge, for sorting purposes
    {
        return weight.CompareTo(other.weight);
    }
}
