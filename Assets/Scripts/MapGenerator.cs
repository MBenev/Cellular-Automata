using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;
//using UnityEditor;



public class MapGenerator : MonoBehaviour
{

    int width;
    int height;

    string seed;
    bool useRandomSeed;

    public InputField seedInput;
    public Toggle randomSeedInput;
    public Slider widthInput;
    public Slider heightInput;
    public Slider fillPercentSlider;
    public TextMeshProUGUI collectiblesText;
    int randomFillPercent;

    int[,] map;

    private GameObject oldCube;
    private GameObject oldDoor;
    private GameObject oldTorch;
    //private GameObject oldFence;
    public Transform prefabCollectible;
    public Transform prefabDoor;
    public Transform prefabTorch;
    //public Transform prefabFence;

    void Start()
    {

    }

    void Update()
    {
    }




    public void RandomSeed(bool selected)
    {
        useRandomSeed = selected;
        if (!selected)
        {
            ChangeSeed();
        }
    }

    public void ChangeSeed()
    {
        seed = seedInput.text;
    }

    public void ChangeFillPercent()
    {
        randomFillPercent = (int)fillPercentSlider.value;
    }

    public void ChangeWidth()
    {
        width = (int)widthInput.value;       
    }

    public void ChangeHeight()
    {
        height = (int)heightInput.value;
    }

    public void GenerateButtonClick()
    {
        DestroyCubes();
        DestoryDoor();
        DestroyTorches();
        GenerateMap();
        SpawnObjects();
        ClearCollectiblesText();
    }

    private void DestroyTorches()
    {
        for (int i = 5; i < 8; i++)
        {
            oldTorch = GameObject.Find("Torch " + i);
            Destroy(oldTorch);
        }
    }

    private void ClearCollectiblesText()
    {
        Player.Instance.ClearCollected();
        collectiblesText.text = "Door locked.";
    }

    private void DestoryDoor()
    {
        oldDoor = GameObject.Find("Door");
        Destroy(oldDoor);
        //oldFence = GameObject.Find("Fence");
        //Destroy(oldFence);
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        ProcessMap();

        int borderSize = 5;
        int[,] borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            { 
                // x value is inside of the map  
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                {
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                }
                else
                {
                    borderedMap[x, y] = 1;
                }
            }
        }

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderedMap, 1);
    }

    void DestroyCubes()
    {
        for (int i = 0; i < 3; i++)
        {
            oldCube = GameObject.Find("Cube " + i);
            Destroy(oldCube);
        }
    }

    void SpawnObjects()
    {
        List<List<Coord>> roomRegions = GetRegions(0);
        var rnd = new System.Random();

        List<int> randomNumbers = new List<int>();
        foreach (List<Coord> roomRegion in roomRegions)
        {
            // Get random open spaces
            randomNumbers.Add(rnd.Next(0, roomRegion.Count));
            randomNumbers.Add(rnd.Next(0, roomRegion.Count));
            randomNumbers.Add(rnd.Next(0, roomRegion.Count));
            randomNumbers.Add(rnd.Next(0, roomRegion.Count));
            randomNumbers.Add(rnd.Next(0, roomRegion.Count));

            randomNumbers.Add(rnd.Next(0, roomRegion.Count));
            randomNumbers.Add(rnd.Next(0, roomRegion.Count));
            randomNumbers.Add(rnd.Next(0, roomRegion.Count));

            // Check for duplicates and get new numbers until there are no duplicates
            var HasDuplicates = (randomNumbers.Distinct().Count() < randomNumbers.Count);
            while (HasDuplicates)
            {
                //Debug.Log("same");
                randomNumbers.Clear();
                randomNumbers.Add(rnd.Next(0, roomRegion.Count));
                randomNumbers.Add(rnd.Next(0, roomRegion.Count));
                randomNumbers.Add(rnd.Next(0, roomRegion.Count));
                randomNumbers.Add(rnd.Next(0, roomRegion.Count));
                randomNumbers.Add(rnd.Next(0, roomRegion.Count));
                randomNumbers.Add(rnd.Next(0, roomRegion.Count));
                randomNumbers.Add(rnd.Next(0, roomRegion.Count));
                randomNumbers.Add(rnd.Next(0, roomRegion.Count));

                HasDuplicates = (randomNumbers.Distinct().Count() < randomNumbers.Count);
            }

            //Debug.Log(HasDuplicates);

            // Generate 3 collectible objects in open spaces
            for (int j = 0; j < 3; j++)
            {
                var newPrefab = Instantiate(prefabCollectible, CoordToWorldPoint(roomRegion[randomNumbers[j]]), Quaternion.identity);
                //newPrefab.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
                Vector3 currentPositiong = new Vector3(newPrefab.transform.position.x, -5, newPrefab.transform.position.z);
                newPrefab.transform.position = currentPositiong;
                newPrefab.name = "Cube " + j;            
            }

            // Generate the door in an open space
            var doorPrefab = Instantiate(prefabDoor, CoordToWorldPoint(roomRegion[randomNumbers[3]]), Quaternion.identity);
            doorPrefab.GetComponent<Renderer>().material.color = new Color(0, 255, 255);
            Vector3 currentPositionDoor = new Vector3(doorPrefab.transform.position.x, -5, doorPrefab.transform.position.z);
            doorPrefab.transform.position = currentPositionDoor;
            doorPrefab.name = "Door";

            // Move player to an open space
            Player.Instance.transform.position = CoordToWorldPoint(roomRegion[randomNumbers[4]]);

            // Generate 3 torches on the ground
            for(int t = 5;t<8;t++)
            {
                var torchPrefab = Instantiate(prefabTorch, CoordToWorldPoint(roomRegion[randomNumbers[t]]), Quaternion.identity);
                //torchPrefab.GetComponent<Renderer>().material.color = new Color(0, 255, 255);
                Vector3 currentPositionTorch = new Vector3(torchPrefab.transform.position.x, -5, torchPrefab.transform.position.z);
                torchPrefab.transform.position = currentPositionTorch;
                torchPrefab.name = "Torch " + t;
            }

        }
    }


    void ProcessMap()
    {
        List<List<Coord>> wallRegions = GetRegions(1);
        int wallThresholdSize = 50; // if region is under 50 walls remove it
        foreach (List<Coord> wallRegion in wallRegions)
        {
            if (wallRegion.Count < wallThresholdSize)
            {
                foreach(Coord tile in wallRegion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<List<Coord>> roomRegions = GetRegions(0);
        int roomThresholdSize = 50; // if region is under 50 empty tiles remove it
        List<Room> survivingRooms = new List<Room>();
        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThresholdSize)
            {
                foreach (Coord tile in roomRegion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }
        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;
        ConnectClosestRooms(survivingRooms);
    }

    void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom)
        {
            foreach(Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach(Room roomA in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if(roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }

            foreach (Room roomB in roomListB)
            {
                if (roomA == roomB || roomA.IsConnected(roomB))
                {
                    continue;
                }

                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(allRooms, true);
        }
    }

    void CreatePassage (Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);
        Debug.DrawLine(CoordToWorldPoint(tileA), CoordToWorldPoint(tileB), Color.red, 100);

        List<Coord> line = GetLine(tileA, tileB);
        foreach (Coord c in line)
        {
            DrawCircle(c, 1);
        }
    }

    void DrawCircle(Coord c, int radius)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x*x + y*y <= radius*radius) // is inside of circle
                {
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;
                    if (IsInMapRange(drawX, drawY))
                    {
                        map[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();
        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }
        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }

    Vector3 CoordToWorldPoint(Coord tile)
    {
        return new Vector3(-width / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            { 
                if (mapFlags[x,y] == 0 && map[x,y] == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);
                    
                    foreach(Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }
        return regions;
    }

    List<Coord> GetRegionTiles (int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (IsInMapRange(x,y) && (y == tile.tileY || x == tile.tileX))
                    {
                        if (mapFlags[x, y] == 0 && map[x,y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }

            }
        }
        return tiles;
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < width && y >=0 && y < height;
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }

    }

    class Room : IComparable<Room>
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;
        public Room() { }

        public Room(List<Coord> roomTiles, int[,] map)
        {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles)
            {
                for (int x = tile.tileX - 1; x <= tile.tileX +1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        if (x == tile.tileX || y == tile.tileY) // excluding diagonal neighbours
                        {
                            if (map[x,y] == 1) // if current is wall
                            {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }
        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if (roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }
        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room otherRoom)
        {
            return otherRoom.roomSize.CompareTo(roomSize);
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach(Room connectedRoom in connectedRooms)
                {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }
    }

}