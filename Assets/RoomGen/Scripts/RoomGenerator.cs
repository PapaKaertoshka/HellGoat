using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RoomGen
{
    [RequireComponent(typeof(PresetEditorComponent))]
    public class RoomGenerator : MonoBehaviour
    {

        public RoomPreset preset;
        public bool debug;

        [Space]
        [Header("Room Size")]
        [Range(1, 100)]
        public int gridX = 2;
        [Range(1, 100)]
        public int gridZ = 2;

        public List<Level> levels = new List<Level>();

        [Range(0.01f, 10f)]
        public float tileSize = 5f;


        [HideInInspector]
        public List<Floor> floors = new List<Floor>();
        [HideInInspector]
        public List<Wall> walls = new List<Wall>();
        [HideInInspector]
        public List<Door> doors = new List<Door>();
        [HideInInspector]
        public List<Window> windows = new List<Window>();

        [HideInInspector]
        public List<Decoration> characters = new List<Decoration>();
        [HideInInspector]
        public List<Decoration> wallDecorations = new List<Decoration>();
        [HideInInspector]
        public List<Decoration> floorDecorations = new List<Decoration>();


        [HideInInspector]
        public GameObject parent;


        List<Node> nodes = new List<Node>();
        public List<GameObject> tiles = new List<GameObject>();
        List<DecoratorPoint> decoratorPoints = new List<DecoratorPoint>();
        List<GameObject> generatedCharacters = new List<GameObject>();
        List<GameObject> generatedWallDecor = new List<GameObject>();
        List<GameObject> generatedFloorDecor = new List<GameObject>();



        [Tooltip("This is a global property that will adjust all wall decorations forward by this value. Use this if all your wall decorations " +
           "are clipping through the wall tiles.")]
        public float wallDecorationOffset = 0.2f;

        [Tooltip("This is a global property that will adjust all floor decorations up by this value. Use this if all your floor decorations " +
            "are clipping through the floor tiles.")]
        public float floorDecorationOffset = 0f;

        [Tooltip("This will give your floor props a safe area distance from the surrounding walls. Nothing will spawn within the safe area.")]
        [Range(0, 25)]
        public int decorSafeArea = 1;

        [HideInInspector]
        public int points;

        [Tooltip("Increasing this property will multiply the number of available decoration points, resulting in a less grid-like layout," +
            "and more desne decorations and props." +
            "Be aware increasing this too much can impact performance.")]
        [Range(0, 3)]
        public int pointSpacing = 1;





        public void GenerateRoom()
        {
            DestroyRoom();
            GenerateLevels();

        }




        public void GenerateLevels()
        {
            if (parent == null)
            {
                parent = new GameObject("RoomPreview");
                parent.transform.position = transform.position;
            }

            RemoveBounds();

            float offset = 0;

            foreach (Level level in levels)
            {

                if (level.preset == null)
                    continue;

                GameObject newParent = new GameObject("LevelBounds");
                newParent.transform.position = transform.position;
                newParent.transform.parent = transform;
                BoxCollider bounds = newParent.gameObject.AddComponent<BoxCollider>();

                bounds.size = new Vector3(gridX * tileSize, level.levelHeight * tileSize, gridZ * tileSize);
                bounds.center = new Vector3(0, offset + (bounds.size.y / 2), 0);
                bounds.center += level.levelOffset;
                offset += bounds.size.y;
                SpawnPoints(bounds, level);
                StartCoroutine(SpawnTiles(bounds, level.preset, level));
            }


        }






        public void SpawnPoints(BoxCollider boxCollider, Level level)
        {

            Vector3 max = boxCollider.bounds.max;
            Vector3 min = boxCollider.bounds.min;

            int levelIndex = levels.IndexOf(level);

            for (float x = min.x; x < max.x; x += tileSize)
            {
                for (float y = min.y; y < max.y; y += tileSize)
                {

                    //zMin
                    Vector3 zMinWall = new Vector3(x + tileSize, y, min.z);
                    nodes.Add(new Node(zMinWall, Quaternion.Euler(0, 0, 0), TileType.Wall, levelIndex));

                    //zMax
                    Vector3 zMaxWall = new Vector3(x, y, max.z);
                    nodes.Add(new Node(zMaxWall, Quaternion.Euler(0, 180, 0), TileType.Wall, levelIndex));
                }
            }

            for (float z = min.z; z < max.z; z += tileSize)
            {
                for (float y = min.y; y < max.y; y += tileSize)
                {

                    //xMin
                    Vector3 xMinWall = new Vector3(min.x, y, z);
                    nodes.Add(new Node(xMinWall, Quaternion.Euler(0, 90, 0), TileType.Wall, levelIndex));

                    //xMax
                    Vector3 xMaxWall = new Vector3(max.x, y, z + tileSize);
                    nodes.Add(new Node(xMaxWall, Quaternion.Euler(0, -90, 0), TileType.Wall, levelIndex));
                }
            }


            for (float x = min.x; x < max.x; x += tileSize)
            {
                for (float z = min.z; z < max.z; z += tileSize)
                {
                    for (float y = min.y; y < max.y; y += tileSize * level.levelHeight)
                    {
                        Vector3 yMinFloor = new Vector3(x, y, z);
                        nodes.Add(new Node(yMinFloor, Quaternion.Euler(0, 90, 0), TileType.Floor, levelIndex));
                    }
                }
            }

        }

        void CalculatePoints()
        {

            int pointsMax = (int)tileSize + 1;
            int numPoints = (pointsMax * pointSpacing) - (pointSpacing - 1);
            points = numPoints;
        }


        void SpawnWallDecoratorPoints(BoxCollider boxCollider, GameObject obj, Tile tile, PointType pointType)
        {

            if (boxCollider == null)
                return;

            Vector3 max = boxCollider.bounds.max;
            Vector3 min = boxCollider.bounds.min;

            CalculatePoints();
            for (int x = 0; x < points - 1; x++)
            {
                for (int y = 0; y < points; y++)
                {



                    Vector3 raypos = new Vector3(-x, y, 0);
                    Vector3 adjustedPos = Tools.AdjustedPosition(obj, -tile.positionOffset);
                    //Vector3 rayStart = obj.transform.position + (obj.transform.forward * 5f) + obj.transform.rotation * (raypos / pointSpacing);
                    Vector3 rayEnd = adjustedPos + obj.transform.rotation * (raypos / pointSpacing);
                    //Debug.DrawLine(rayStart, rayEnd, Color.yellow, 2f);

                    if (rayEnd.x == max.x && rayEnd.z == min.z)
                    {
                        continue;
                    }
                    if (rayEnd.x == max.x && rayEnd.z == max.z)
                    {
                        continue;
                    }
                    if (rayEnd.x == min.x && rayEnd.z == max.z)
                    {
                        continue;
                    }
                    if (rayEnd.x == min.x && rayEnd.z == min.z)
                    {
                        continue;
                    }


                    DecoratorPoint newPoint = new DecoratorPoint(obj, rayEnd + obj.transform.forward * wallDecorationOffset, pointType);
                    decoratorPoints.Add(newPoint);
                }
            }
        }


        IEnumerator SpawnFloorDecoratorPoints(BoxCollider boxCollider, GameObject obj, Tile tile, PointType pointType)
        {

            yield return new WaitForSeconds(0.0001f);


            if (boxCollider != null)
            {

                Vector3 max = boxCollider.bounds.max;
                Vector3 min = boxCollider.bounds.min;


                CalculatePoints();
                for (int x = 0; x < points - 1; x++)
                {
                    for (int z = 0; z < points; z++)
                    {



                        Vector3 raypos = new Vector3(-x, 0, z);
                        Vector3 objPoint = obj.transform.position + obj.transform.rotation * (raypos / pointSpacing);
                        Vector3 adjustedPoint = objPoint + (obj.transform.up * floorDecorationOffset);

                        Vector3 rayStart = obj.transform.position + obj.transform.up * 3f + obj.transform.rotation * (raypos / pointSpacing);


                        if (adjustedPoint.x == min.x && adjustedPoint.y == min.y)
                        {
                            continue;
                        }
                        if (adjustedPoint.x >= max.x && adjustedPoint.y == min.y)
                        {
                            continue;
                        }
                        if (adjustedPoint.z <= min.z && adjustedPoint.y == min.y)
                        {
                            continue;
                        }
                        if (adjustedPoint.z >= max.z && adjustedPoint.y == min.y)
                        {
                            continue;
                        }



                        if (tile.alignToSurface)
                        {
                            // Calculate varying height floor tiles

                            Ray ray = new Ray(rayStart, -obj.transform.up);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, 1000f, tile.tileLayer))
                            {
                                DecoratorPoint newRaycastPoint = new DecoratorPoint(obj, hit.point, pointType);
                                decoratorPoints.Add(newRaycastPoint);

                               // Debug.DrawLine(rayStart, hit.point, Color.yellow, 1f);
                            }
                        }
                        else
                        {

                            DecoratorPoint newPoint = new DecoratorPoint(obj, adjustedPoint, pointType);
                            decoratorPoints.Add(newPoint);
                        }
                    }
                }
            }
        }





        IEnumerator SpawnTiles(BoxCollider boxCollider, RoomPreset preset, Level level)
        {

            Random.InitState(level.decorSeed);
            Random.InitState(level.roomSeed);

            if (level.levelHeight > 0)
            {
                SpawnDoors(boxCollider, level);
                SpawnWindows(boxCollider, level);
            }

            SpawnWallsAndFloors(boxCollider, preset, levels.IndexOf(level));
            yield return new WaitForSeconds(0.0001f);
            AddCharacters(boxCollider, level);
            DecorateWalls(boxCollider, level);
            DecorateFloors(boxCollider, level);


        }



        void SpawnWallsAndFloors(BoxCollider boxCollider, RoomPreset preset, int levelNumber)
        {

            foreach (Node node in nodes)
            {

                // Spawn walls
                if (node.tileType == TileType.Wall && node.isAvailable && node.levelNumber == levelNumber)
                {
                    Wall wall = Tools.RandomWall(preset.wallTiles);
                    if (wall == null || wall.prefab == null)
                        continue;
                    GameObject obj = Instantiate(wall.prefab, node.position, node.rotation);
                    Tools.AdjustPosition(obj, wall.positionOffset);
                    Tools.AdjustRotation(obj, wall.rotationOffset);
                    tiles.Add(obj);
                    obj.transform.parent = parent.transform;

                    if (wall.allowDecor)
                    {
                        SpawnWallDecoratorPoints(boxCollider, obj, wall, PointType.Wall);
                    }
                }


                if (node.tileType == TileType.Floor && node.isAvailable && node.levelNumber == levelNumber)
                {

                    Floor floor = Tools.RandomFloor(preset.floorTiles);
                    if (floor == null || floor.prefab == null)
                        continue;

                    GameObject obj = Instantiate(floor.prefab, node.position, node.rotation);
                    Tools.AdjustPosition(obj, floor.positionOffset);
                    Tools.AdjustRotation(obj, floor.rotationOffset);
                    tiles.Add(obj);
                    obj.transform.parent = parent.transform;

                    if (floor.allowDecor)
                    {
                        StartCoroutine(SpawnFloorDecoratorPoints(boxCollider, obj, floor, PointType.Floor));
                    }

                    
                    
                }
            }
        }


        Node RandomNode(List<float> heights, TileType tileType)
        {
            List<Node> matchingNodes = new List<Node>();

            foreach (Node node in nodes)
            {
                foreach (float h in heights)
                {
                    if (node.isAvailable && node.position.y == h && node.tileType == tileType)
                    {
                        matchingNodes.Add(node);
                    }
                }
            }

            if (matchingNodes.Count > 0)
            {
                return matchingNodes[Random.Range(0, matchingNodes.Count)];
            }
            return null;

        }



        void SpawnDoors(BoxCollider boxCollider, Level level)
        {

            float yMin = boxCollider.bounds.min.y;

            List<float> floorHeights = new List<float>();

            floorHeights.Add(yMin);

            for (int i = 0; i < level.numDoors; i++)
            {
                Node node = RandomNode(floorHeights, TileType.Wall);

                if (node == null)
                {
                    continue;
                }
                Door door = Tools.RandomDoor(level.preset.doorTiles);

                if (door == null || door.prefab == null)
                    continue;

                GameObject doorObj = Instantiate(door.prefab, node.position, node.rotation);
                Tools.AdjustPosition(doorObj, door.positionOffset);
                Tools.AdjustRotation(doorObj, door.rotationOffset);
                tiles.Add(doorObj);
                doorObj.transform.parent = parent.transform;
                node.isAvailable = false;

                if (door.allowDecor)
                {
                    SpawnWallDecoratorPoints(boxCollider, doorObj, door, PointType.Door);
                }
            }
        }

        void SpawnWindows(BoxCollider boxCollider, Level level)
        {

            float yMin = boxCollider.bounds.min.y;

            List<float> floorHeights = new List<float>();

            float wHeight = yMin + ((level.windowHeight - 1) * tileSize);
            floorHeights.Add(wHeight);

            for (int i = 0; i < level.numWindows; i++)
            {

                Node node = RandomNode(floorHeights, TileType.Wall);

                if (node == null)
                {
                    continue;
                }
                Window window = Tools.RandomWindow(level.preset.windowTiles);

                if (window == null || window.prefab == null)
                    continue;


                GameObject windowObj = Instantiate(window.prefab, node.position, node.rotation);
                Tools.AdjustPosition(windowObj, window.positionOffset);
                Tools.AdjustRotation(windowObj, window.rotationOffset);
                tiles.Add(windowObj);
                windowObj.transform.parent = parent.transform;
                node.isAvailable = false;

                if (window.allowDecor)
                {
                    SpawnWallDecoratorPoints(boxCollider, windowObj, window, PointType.Window);
                }
            }
        }


        void DecorateWalls(BoxCollider boxCollider, Level level)
        {
            if (boxCollider == null)
                return;

            if (level.preset.wallDecorations.Count == 0)
            {
                //   Debug.Log("No wall decorations selected in preset " + selectedLevel.preset.name);
                return;
            }
            Random.InitState(level.decorSeed);
            //ClearWallDecor();


            Bounds bounds = boxCollider.bounds;


            foreach (DecoratorPoint point in decoratorPoints)
            {

                if (point.pointType != PointType.Wall)
                    continue;


                Decoration decoration = Tools.RandomDecoration(level.preset.wallDecorations);

                float probability = Random.Range(0f, 1f);
                float minNodeHeight = bounds.min.y + decoration.verticalRange.x;
                float maxNodeHeight = bounds.min.y + decoration.verticalRange.y;

                if (probability <= decoration.probability && point.point.y >= minNodeHeight &&
                    point.point.y <= maxNodeHeight)
                {

                    if (decoration.prefab == null)
                    {
                        continue;
                    }

                    GameObject decor = Instantiate(decoration.prefab, point.point, Quaternion.identity * point.pointObject.transform.rotation);
                    Tools.AdjustPosition(decor, decoration.positionOffset);
                    decor.transform.Rotate(transform.forward * Random.Range(0, decoration.randomRotation), Space.Self);
                    decor.transform.localScale *= Random.Range(decoration.scaleRange.x, decoration.scaleRange.y);
                    decor.transform.parent = parent.transform;
                    tiles.Add(decor);
                }
            }
        }

        void DecorateFloors(BoxCollider boxCollider, Level level)
        {
            if (boxCollider == null)
                return;

            if (level.preset.floorDecorations.Count == 0)
            {
                Debug.Log("No floor decorations selected in preset " + level.preset.name);
                return;
            }
            Random.InitState(level.decorSeed);

            Bounds bounds = boxCollider.bounds;

            foreach (DecoratorPoint point in decoratorPoints)
            {

                if (point.pointType != PointType.Floor)
                    continue;

                Decoration decoration = Tools.RandomDecoration(level.preset.floorDecorations);
                float probability = Random.Range(0f, 1f);
                float minNodeHeight = decoration.verticalRange.x + bounds.min.y;
                float maxNodeHeight = decoration.verticalRange.y + bounds.min.y;

                if (probability <= decoration.probability && WallDistanceCheck(point.point, boxCollider) && point.point.y >= minNodeHeight &&
                    point.point.y <= maxNodeHeight)
                {
                    if (decoration.prefab == null)
                    {
                        continue;
                    }

                    GameObject decor = Instantiate(decoration.prefab, point.point, Quaternion.identity * point.pointObject.transform.rotation);
                    Tools.AdjustPosition(decor, decoration.positionOffset);
                    decor.transform.Rotate(Vector3.up * Random.Range(0, decoration.randomRotation), Space.World);
                    decor.transform.localScale *= Random.Range(decoration.scaleRange.x, decoration.scaleRange.y);
                    decor.transform.parent = parent.transform;
                    tiles.Add(decor);
                }
            }
        }

        void AddCharacters(BoxCollider boxCollider, Level level)
        {
            if (boxCollider == null)
                return;

            if (level.preset.characters.Count == 0)
            {
                // Debug.Log("No characters selected in preset " + selectedLevel.preset.name);
                return;
            }
            Random.InitState(level.characterSeed);
            //ClearCharacters();

            Bounds bounds = boxCollider.GetComponent<BoxCollider>().bounds;

            foreach (DecoratorPoint point in decoratorPoints)
            {
                if (point.pointType != PointType.Floor)
                    continue;

                Decoration decoration = Tools.RandomDecoration(level.preset.characters);
                float probability = Random.Range(0f, 1f);
                float minNodeHeight = decoration.verticalRange.x + bounds.min.y;
                float maxNodeHeight = decoration.verticalRange.y + bounds.min.y;

                if (probability <= decoration.probability && WallDistanceCheck(point.point, boxCollider) && point.point.y >= minNodeHeight &&
                    point.point.y <= maxNodeHeight)
                {

                    if (decoration.prefab == null)
                    {
                        continue;
                    }

                    GameObject character = Instantiate(decoration.prefab, point.point, Quaternion.identity * point.pointObject.transform.rotation);
                    Tools.AdjustPosition(character, decoration.positionOffset);
                    character.transform.Rotate(Vector3.up * Random.Range(0, decoration.randomRotation), Space.World);
                    character.transform.localScale *= Random.Range(decoration.scaleRange.x, decoration.scaleRange.y);
                    character.transform.parent = parent.transform;
                    tiles.Add(character);
                }
            }
        }

        bool WallDistanceCheck(Vector3 gridPoint, BoxCollider boxCollider)
        {

            float stepSize = (1 / tileSize);
            float offsetPoints = decorSafeArea * tileSize;
            float halfTileSize = tileSize / 2;
            float offset = offsetPoints * stepSize;

            float xMax = boxCollider.bounds.max.x + (gridX * halfTileSize) - offset;
            float xMin = boxCollider.bounds.min.x - (gridX * halfTileSize) + offset;
            float zMax = boxCollider.bounds.max.z + (gridZ * halfTileSize) - offset;
            float zMin = boxCollider.bounds.min.z - (gridZ * halfTileSize) + offset;

            if (gridPoint.x < xMax && gridPoint.x > xMin && gridPoint.z < zMax && gridPoint.z > zMin)
            {
                return true;
            }
            return false;
        }




        public void RemoveBounds()
        {
            //Destroy box collider bounds
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }


        public void DestroyRoom()
        {


            foreach (GameObject tile in tiles)
            {
                DestroyImmediate(tile);
            }


            nodes.Clear();
            decoratorPoints.Clear();
            generatedWallDecor.Clear();
            generatedFloorDecor.Clear();
            generatedCharacters.Clear();
            tiles.Clear();
            RemoveBounds();
        }



        #region inspectorValidations;


        public void UpdateStoredValues()
        {
            walls = preset.wallTiles;
            doors = preset.doorTiles;
            floors = preset.floorTiles;
            windows = preset.windowTiles;
            characters = preset.characters;
            wallDecorations = preset.wallDecorations;
            floorDecorations = preset.floorDecorations;

            // save them to the preset.
            UpdatePreset();
        }

        public void UpdatePreset()
        {
            preset.wallTiles = walls;
            preset.doorTiles = doors;
            preset.floorTiles = floors;
            preset.windowTiles = windows;
            preset.characters = characters;
            preset.wallDecorations = wallDecorations;
            preset.floorDecorations = floorDecorations;
        }

        #endregion




        private void OnDrawGizmos()
        {

            if (!debug)
                return;
            foreach (BoxCollider boxCollider in GetComponentsInChildren<BoxCollider>())
            {
                Vector3 max = boxCollider.bounds.max;
                Vector3 min = boxCollider.bounds.min;

                foreach (Node point in nodes)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(point.position, 0.125f);
                }


                Gizmos.color = Color.red;
                Gizmos.DrawSphere(max, 0.21f);

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(min, 0.21f);

            }

            foreach (DecoratorPoint point in decoratorPoints)
            {

                //if (point.pointType == PointType.Wall)
                //{
                //    Gizmos.color = Color.white;
                //    Gizmos.DrawWireSphere(point.point, 0.125f);
                //}

                if (point.pointType == PointType.Floor)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(point.point, 0.125f);
                }

                if (point.pointType == PointType.Window)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(point.point, 0.125f);
                }

                if (point.pointType == PointType.Door)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(point.point, 0.125f);
                }
            }


        }


        public void Save()
        {
            generatedFloorDecor.Clear();
            generatedWallDecor.Clear();
            decoratorPoints.Clear();
            DestroyImmediate(parent);


            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }


            Debug.Log("room prefab saved.");
        }



    }

    [System.Serializable]
    public enum TileType
    {
        Floor, Wall
    }

    [System.Serializable]
    public enum DecoratorType
    {
        Door, Window, WallDecor, FloorDecor
    }

}