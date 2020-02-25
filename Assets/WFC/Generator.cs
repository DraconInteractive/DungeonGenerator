using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WFC
{
    public class Generator : MonoBehaviour
    {
        public Solver solver;
        public GameObject tilePrefab;
        public Vector3 mapSize;
        public Vector3 separation;
        public Transform tileHolder;
        public bool scaleWithSeparation;
        // Start is called before the first frame update
        void Start()
        {
            solver.Instantiate();
            StartCoroutine(Generate());

        }

        IEnumerator Generate()
        {
            List<Tile> all = new List<Tile>();
            DateTime start = DateTime.Now;
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    for (int z = 0; z < mapSize.z; z++)
                    {
                        GameObject g = Instantiate(tilePrefab, new Vector3(x * separation.x, y * separation.y, z * separation.z), Quaternion.identity, tileHolder.transform);
                        all.Add(g.GetComponent<Tile>());
                        /*
                        if (scaleWithSeparation)
                        {
                            Vector3 l = g.transform.localScale;
                            l.x *= separation.x;
                            l.y *= separation.y;
                            l.z *= separation.z;
                            g.transform.localScale = l;
                        }*/
                    }
                    yield return null;
                }
                yield return null;
            }
            yield return null;

            solver.allTiles = new List<Tile>(all);

            yield return StartCoroutine(solver.Do(separation));

            foreach (Tile t in solver.allTiles)
            {
                if (t.output.prefab != null)
                {
                    Instantiate(t.output.prefab, t.transform);
                }
            }

            DateTime end = DateTime.Now;
            print((end - start).TotalSeconds.ToString());
        }


        // Update is called once per frame
        void Update()
        {

        }
    }

    [System.Serializable]
    public class TileModel
    {
        public string name;
        public bool available;
        public int upExit = 0;
        public int downExit = 0;
        public int leftExit = 0;
        public int rightExit = 0;
        public int forwardExit = 0;
        public int backExit = 0;

        public GameObject prefab;

        public float frequency;
    }

    [System.Serializable]
    public class Solver
    {
        public static Solver Instance;
        public void Instantiate()
        {
            Instance = this;
        }
        public List<Tile> allTiles;
        public List<ModelSet> modelSets;
        public List<TileModel> allPossibleTiles;
        public float neighbourDist = 0.1f;
        public bool autoProgress;
        public IEnumerator Do(Vector3 separation)
        {
            if (modelSets != null && modelSets.Count > 0)
            {
                allPossibleTiles = new List<TileModel>();
                foreach (ModelSet set in modelSets)
                {
                    foreach (TileModel model in set.models)
                    {
                        allPossibleTiles.Add(model);
                    }
                }
            }
            GetTileNeighbours(separation);

            List<Tile> openList = new List<Tile>();
            List<Tile> solvedList = new List<Tile>();

            foreach (Tile t in allTiles)
            {
                t.possibleTiles = new List<TileModel>(allPossibleTiles);
            }
            Tile current = allTiles[0];
            openList.Add(current);
            openList[0].output = GetModelFromFrequency(openList[0].possibleTiles.Where(element => element.available).ToArray());

            foreach (Tile n in openList[0].Neighbours.Keys)
            {
                bool nonmatches = true;
                while (nonmatches)
                {
                    nonmatches = false;
                    foreach (TileModel m in n.possibleTiles)
                    {
                        bool matches = CompareMatchingExits(openList[0].output, m, openList[0].Neighbours[n]);
                        if (!m.available)
                        {
                            matches = false;
                        }
                        if (!matches)
                        {
                            nonmatches = true;
                            n.possibleTiles.Remove(m);
                            break;
                        }
                    }
                }

                openList.Add(n);
            }

            solvedList.Add(current);
            openList.Remove(current);
            Debug.Log("Starter: " + current.name);
            while (openList.Count > 0)
            {
                current = openList[0];

                TileModel currentOutput = null;

                if (current.possibleTiles.Count > 0)
                {
                    currentOutput = GetModelFromFrequency(current.possibleTiles.ToArray());
                }

                if (currentOutput != null)
                {
                    current.output = currentOutput;
                }

                Array.ForEach(current.Neighbours.Keys.ToArray(), element =>
                {
                    bool nonmatches = true;
                    while (nonmatches)
                    {
                        nonmatches = false;
                        foreach (TileModel m in element.possibleTiles)
                        {
                            bool matches = CompareMatchingExits(current.output, m, current.Neighbours[element]);
                            if (!m.available)
                            {
                                matches = false;
                            }
                            if (!matches)
                            {
                                nonmatches = true;
                                element.possibleTiles.Remove(m);
                                break;
                            }
                        }
                    }
                    if (!openList.Contains(element) && !solvedList.Contains(element))
                    {
                        openList.Add(element);
                    }
                });
                solvedList.Add(current);
                openList.Remove(current);

                bool spacePressed = false;
                while (!spacePressed && !autoProgress)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        spacePressed = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.O))
                    {
                        string o = "";
                        Array.ForEach(openList.ToArray(), element => o += element.name + "\n");
                        Debug.Log(o);
                    }
                    yield return null;
                }
                yield return null;
            }

            yield break;
        }

        TileModel GetModelFromFrequency(TileModel[] possibleTiles)
        {
            float total = 0;
            foreach (TileModel t in possibleTiles)
            {
                total += t.frequency;
            }
            float roll = UnityEngine.Random.value * total;
            for (int i = 0; i < possibleTiles.Length - 1; i++)
            {
                if (roll <= possibleTiles[i].frequency)
                {
                    return possibleTiles[i];
                }
                roll -= possibleTiles[i].frequency;
            }
            return possibleTiles[possibleTiles.Length - 1];
        }

        void GetTileNeighbours(Vector3 separation)
        {
            foreach (Tile t in allTiles)
            {
                Tile uKey = GetTileAtPos(t.transform.position + Vector3.up * separation.y);
                Tile dKey = GetTileAtPos(t.transform.position + Vector3.down * separation.y);
                Tile lKey = GetTileAtPos(t.transform.position + Vector3.left * separation.x);
                Tile rKey = GetTileAtPos(t.transform.position + Vector3.right * separation.x);
                Tile fKey = GetTileAtPos(t.transform.position + Vector3.forward * separation.z);
                Tile bKey = GetTileAtPos(t.transform.position + Vector3.back * separation.z);

                if (uKey != null)
                {
                    t.Neighbours.Add(uKey, Tile.TileRelationShip.Above);
                }
                if (dKey != null)
                {
                    t.Neighbours.Add(dKey, Tile.TileRelationShip.Below);
                }
                if (lKey != null)
                {
                    t.Neighbours.Add(lKey, Tile.TileRelationShip.ToLeft);
                }
                if (rKey != null)
                {
                    t.Neighbours.Add(rKey, Tile.TileRelationShip.ToRight);
                }
                if (fKey != null)
                {
                    t.Neighbours.Add(fKey, Tile.TileRelationShip.Ahead);
                }
                if (bKey != null)
                {
                    t.Neighbours.Add(bKey, Tile.TileRelationShip.Behind);
                }
            }
        }

        Tile GetTileAtPos(Vector3 pos)
        {
            float maxDist = Mathf.Infinity;
            Tile target = null;

            foreach (Tile t in allTiles)
            {
                float dist = Vector3.Distance(pos, t.transform.position);
                if (dist < maxDist && dist <= neighbourDist)
                {
                    maxDist = dist;
                    target = t;
                }
            }

            return target;
        }

        public static bool CompareMatchingExits(TileModel target, TileModel other, Tile.TileRelationShip relation)
        {
            bool matching = true;

            if (relation == Tile.TileRelationShip.Above)
            {
                if (target.upExit != other.downExit)
                {
                    matching = false;
                }
            }
            else if (relation == Tile.TileRelationShip.Below)
            {
                if (target.downExit != other.upExit)
                {
                    matching = false;
                }
            }
            else if (relation == Tile.TileRelationShip.ToLeft)
            {
                if (target.leftExit != other.rightExit)
                {
                    matching = false;
                }
            }
            else if (relation == Tile.TileRelationShip.ToRight)
            {
                if (target.rightExit != other.leftExit)
                {
                    matching = false;
                }
            }
            else if (relation == Tile.TileRelationShip.Ahead)
            {
                if (target.forwardExit != other.backExit)
                {
                    matching = false;
                }
            }
            else if (relation == Tile.TileRelationShip.Behind)
            {
                if (target.backExit != other.forwardExit)
                {
                    matching = false;
                }
            }

            return matching;
        }


    }

}
