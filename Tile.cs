using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    // a tile represents 1 cell in the grid of a maze
    public Tile parent; // this is a reference for the disjointset and keeps track to which set the tile belongs to

    public static Tile GetHighestParent(Tile tile) // also used for the disjointset
    {
        if (tile.parent == null) // if it is null, this means that tile is a parent and is the highest value of the set
        {
            return tile;
        }
        else
        {
            return GetHighestParent(tile.parent); // if it is not null we need to look to the next tile in the set to find the parent of the set
        }
    }
}
