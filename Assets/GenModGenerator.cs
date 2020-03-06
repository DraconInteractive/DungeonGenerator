using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularAdditionsV2
{
    public class GenModGenerator : MonoBehaviour
    {
        public List<MA_Prefab> existingTiles = new List<MA_Prefab>();
        public List<GameObject> allPrefabs = new List<GameObject>();
        GameObject tempSpawned;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Generate());
        }

        // Update is called once per frame
        void Update()
        {

        }
        List<MA_Exit> allExits = new List<MA_Exit>();
        List<MA_Exit> availableExits = new List<MA_Exit>();
        void GetAllExits ()
        {
            allExits.Clear();
            foreach (MA_Prefab tile in existingTiles)
            {
                if (tile.exits != null && tile.exits.Length > 0)
                {
                    foreach (MA_Exit exit in tile.exits)
                    {
                        allExits.Add(exit);
                    }
                }
            }
        }

        void GetAvailableExits ()
        {
            availableExits.Clear();
            foreach (MA_Exit exit in allExits)
            {
                if (!exit.attached)
                {
                    availableExits.Add(exit);
                }
            }
        }

        T GetRandomFromList<T> (List<T> choices)
        {
            if (choices == null || choices.Count <= 0)
            {
                return default;
            }

            int index = Random.Range(0, choices.Count - 1);
            return choices[index];
        }

        T GetRandomFromArray<T>(T[] choices)
        {
            if (choices == null || choices.Length <= 0)
            {
                return default;
            }

            int index = Random.Range(0, choices.Length - 1);
            return choices[index];
        }

        IEnumerator Generate()
        {
            //Generate One tile (Am doing this offline)

            GetAllExits();
            GetAvailableExits();
            
            while (availableExits.Count > 0)
            {
                List<GameObject> viablePrefabs = new List<GameObject>(allPrefabs);
                MA_Exit targetExit = GetRandomFromList(availableExits);
                bool prefabViable = false;
                while (!prefabViable && viablePrefabs.Count > 0)
                {
                    prefabViable = true;
                    GameObject currentPrefab = GetRandomFromList(viablePrefabs);
                    GameObject spawned = Instantiate(currentPrefab, targetExit.transform.position, targetExit.transform.rotation, this.transform);
                    Collider[] spawnedColliders = spawned.GetComponentsInChildren<Collider>();
                    foreach (Collider c in spawnedColliders)
                    {
                        c.enabled = false;
                    }

                    foreach (MA_RCCheck checkpoint in spawned.GetComponent<MA_Prefab>().checkPoints)
                    {
                        bool free = Physics.CheckSphere(checkpoint.transform.position, 0.01f);
                        if (!free)
                        {
                            prefabViable = false;
                        }
                    }

                    if (prefabViable)
                    {
                        MA_Prefab spawnedPrefab = spawned.GetComponent<MA_Prefab>();

                        targetExit.attached = true;
                        targetExit.attachedPrefab = spawnedPrefab;
                        
                        existingTiles.Add(spawnedPrefab);
                        foreach (MA_Exit exit in spawnedPrefab.exits)
                        {
                            allExits.Add(exit);
                            exit.UpdateAttached();
                        }
                    } else
                    {
                        Destroy(spawned);
                        viablePrefabs.Remove(currentPrefab);
                    }
                    yield return null;
                }
                GetAvailableExits();
            }
            //while (tileCount < totalTileCount && tileExits > 0)
            //select exit
            //get list of possible tiles, using available space and bounds
            //possible technique - do physics check using bounds. If empty, generate. 
            //Another technique - make tiles have check points. Do small physics checks at multiple points. Only slightly less accurate, and possibly more performant
            
            yield break;
        }
    }
}

