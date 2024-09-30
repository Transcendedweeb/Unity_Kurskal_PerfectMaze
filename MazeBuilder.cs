/*
This is main file to build the maze. It does the following:
Make boundaries, make edges, place start and enpoints, place the player camera at the start, end run the disjoint set
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeBuilder : MonoBehaviour
{
    public GameObject player;
    public GameObject wallPrefab;
    public GameObject startPrefab;
    public GameObject endPrefab;

    [Range(10, 250)]
    public int width; // maze constraints

    [Range(10, 250)]
    public int height; // maze constraints

    public Button stopButton;
    public Button playButton;

    public bool nightmareMode = false; // specific nightmare mode flag
    

    [HideInInspector] public Tile[] tiles; // all generated cells are kept here
    [HideInInspector] public List<Edge> edges; // all generated edges are kept here

    DisjointSet disjointSet;

    public void PlaceVerticalBoundaries()
    {
        // places the vertical outerwalls
        for (int z = 1; z <= height; z++) // foreach tile on the z-axis
        {
            // 0 = left boundary, width = right boundary
            // 5 = a prefab lenght reference
            GameObject wallLeft = Instantiate(wallPrefab, new Vector3((0 * 5), 0, (z * 5)), Quaternion.Euler(0, 90, 0));
            GameObject wallRight = Instantiate(wallPrefab, new Vector3((width * 5), 0, (z * 5)), Quaternion.Euler(0, 90, 0));
            wallLeft.transform.parent = this.transform;
            wallRight.transform.parent = this.transform;
        }
    }

    public void PlaceHorizontalBoundaries()
    {
        // places the horizontal outerwalls
        for (int x = 0; x < width; x++) // foreach tile on the x-axis
        {
            // 0 = bottom boundary, height * 5 = top boundary
            // 2.5 = to center the walls in the grid instead of placing it in the corner (for alignment purposes)
            GameObject wallBottom = Instantiate(wallPrefab, new Vector3((x * 5) + 2.5f, 0, 2.5f), Quaternion.Euler(0, 0, 0));
            GameObject wallTop = Instantiate(wallPrefab, new Vector3((x * 5) + 2.5f, 0, (height * 5) + 2.5f), Quaternion.Euler(0, 0, 0));
            wallBottom.transform.parent = this.transform;
            wallTop.transform.parent = this.transform;
        }
    }


    public void PlaceHorizontalEdges()
    {
        // places horizontal walls between tiles to create the grid
        int edgeIndex = 0; // index for all the tiles we loop through

        for (int z = 1; z <= height; z++) // foreach row in the grid
        {
            for (int x = 1; x < width; x++) // foreach column in the row
            {
                // adds a new edge
                GameObject goWallPrefab = Instantiate(wallPrefab, new Vector3((x * 5), 0, (z * 5)), Quaternion.Euler(0, 90, 0));
                goWallPrefab.transform.parent = this.transform;
                Edge edge = goWallPrefab.AddComponent<Edge>();

                // define the 2 tiles that are connected between the edge
                edge.tiles = new Tile[2]; 
                edge.tiles[0] = tiles[edgeIndex]; // the tile on the current loop index
                edge.tiles[1] = tiles[edgeIndex + 1]; // the tile to the right
                
                edge.weight = Random.Range(1, 10); // assign a random weight to it
                edges.Add(edge);
                edgeIndex++; // move to the next tile
            }
            edgeIndex++; // skip to the next row
        }
    }

    public void PlaceVerticalEdges()
    {
        // same as horizontal, but vertical
        int edgeIndex = 0;

        for (int z = 1; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject goWallPrefab = Instantiate(wallPrefab, new Vector3((x * 5) + 2.5f, 0, (z * 5) + 2.5f), Quaternion.Euler(0, 0, 0));
                goWallPrefab.transform.parent = this.transform;
                Edge edge = goWallPrefab.AddComponent<Edge>();
                edge.tiles = new Tile[2];
                edge.tiles[0] = tiles[edgeIndex];
                edge.tiles[1] = tiles[edgeIndex + 1];
                edge.weight = Random.Range(1, 10);
                edges.Add(edge);
                edgeIndex++;
            }
        }
    }

    void PlaceStartAndEndPoints()
    {
        // places start and enpoints for the maze
        int startX = Random.Range(0, width); // randomizes a spot in the width
        int startZ = 0;
        Vector3 startPosition = new Vector3(startX * 5 + 2.5f, 0.5f, startZ * 5 + 2.5f);
        GameObject startPoint = Instantiate(startPrefab, startPosition, startPrefab.transform.rotation);
        startPoint.transform.parent = this.transform;

        MovePlayerToStart(startPosition); // places an inactive camera on the start point

        int endX = Random.Range(0, width);
        int endZ = height;
        Vector3 endPos = new Vector3(endX * 5 + 2.5f, 0.5f, endZ * 5 + 2.5f);
        GameObject endPoint = Instantiate(endPrefab, endPos, Quaternion.identity);
        endPoint.transform.parent = this.transform;
    }

    void MovePlayerToStart(Vector3 startPosition)
    {
        Vector3 playerPosition = new Vector3(startPosition.x, 1f, startPosition.z - 10f); // places the camera in front of the start point
        player.transform.position = playerPosition;

        Vector3 directionToStart = startPosition - player.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToStart);

        player.transform.rotation = Quaternion.Euler(0f, targetRotation.y, 0f);
    }

    public IEnumerator GenerateMaze()
    {
        // sort edges in descending order of weight
        // b.CompareTo(a) sets heigher weights before the lower ones
        edges.Sort((a, b) => b.CompareTo(a));

        foreach (Edge edge in edges) // loop through all the edges
        {
            Tile tile1 = edge.tiles[0]; // gets the 2 tiles that are connected through the current edge
            Tile tile2 = edge.tiles[1];

            if (disjointSet.Find(tile1) != disjointSet.Find(tile2)) // checks of the connected tiles are from the same set
            {
                disjointSet.Union(tile1, tile2); // if they are not connected to a set than we join them together
                edge.DisableEdge(); // removes the edge to make a path in the set
            }
            yield return null;
        }

        Debug.Log("Maze generated");
        if (!nightmareMode) // makes sure the UI-buttons are updated for generating a maze
        {
            stopButton.interactable = false;
            playButton.interactable = true;
        }
        else this.transform.parent.gameObject.GetComponent<NightmareModeController>().ActivateNextGenerator(); // nightmare mode specific
    }

    void OnEnable()
    {
        // on enable we want to start generating the maze

        tiles = new Tile[width * height]; // set array to the dimensions, so 10x10 would be 100 spot in the array
        for (int i = 0; i < width * height; i++) tiles[i] = new Tile(); // fill the spots with tiles

        edges = new List<Edge>(); // initiate the edges list for the boundaries and edges

        PlaceVerticalBoundaries();
        PlaceHorizontalBoundaries();
        PlaceHorizontalEdges();
        PlaceVerticalEdges();

        disjointSet = new DisjointSet();
        foreach (Tile tile in tiles) disjointSet.MakeSet(tile); // Create a disjoint set for each tile

        PlaceStartAndEndPoints();

        StartCoroutine(GenerateMaze());
    }

    void OnDisable()
    {
        // on disable we remove all the generated childeren so we can start over on enable
        if (nightmareMode) return; // except when we play nightmare mode

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
