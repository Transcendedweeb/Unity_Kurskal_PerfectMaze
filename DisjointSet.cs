using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisjointSet
{
    Dictionary<Tile, Tile> parent = new Dictionary<Tile, Tile>(); // connects 2 tiles togehter with its parent
    Dictionary<Tile, int> rank = new Dictionary<Tile, int>(); // stores the rank of all tiles in a set 

    public void MakeSet(Tile tile)
    {
        parent[tile] = tile; // at the start all tiles are its own parent
        rank[tile] = 0; // as parent there value will be 0
    }

    public Tile Find(Tile tile) // finds the root of the set containing the specified tile
    {
        /*
            uses path compression to simplify the structure of the a set whenever find is called
            this makes future find operations faster
        */
        if (parent[tile] != tile)
        {
            parent[tile] = Find(parent[tile]); // path compression
        }
        return parent[tile]; /// return the root of the set containing the tile
    }

    // merges two sets that are connected through an edge, but are not a set
    public void Union(Tile tile1, Tile tile2)
    {
        Tile root1 = Find(tile1);
        Tile root2 = Find(tile2);

        if (root1 != root2)
        {
            if (rank[root1] > rank[root2])
            {
                parent[root2] = root1; // attach root2's set to root1's set
            }
            else if (rank[root1] < rank[root2])
            {
                parent[root1] = root2; // attach root1's set to root2's set
            }
            else
            {
                parent[root2] = root1; // attach root2's set to root1's set
                rank[root1] = rank[root1]++; // increase the rank of root1's set
            }
        }
    }
}
