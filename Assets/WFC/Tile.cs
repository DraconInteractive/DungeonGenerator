using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFC
{
    [System.Serializable]
    public class Tile : MonoBehaviour
    {
        public enum TileRelationShip
        {
            Above,
            Below,
            ToLeft,
            ToRight,
            Ahead,
            Behind
        }
        public List<TileModel> possibleTiles;
        public TileModel output;
        public Dictionary<Tile, TileRelationShip> Neighbours = new Dictionary<Tile, TileRelationShip>();

        Dictionary<int, Color> exitColor = new Dictionary<int, Color>()
    {
        {0, Color.white },
        {1, Color.blue },
        {2, Color.red },
        {3, Color.green },
        {4, Color.yellow },
        {5, Color.black }
    };

        [ContextMenu("List Neighbours")]
        public void ListNeighbours()
        {
            string o = "";
            foreach (Tile t in Neighbours.Keys)
            {
                o += (t.name + ": " + Neighbours[t].ToString()) + "\n";
            }
            print(o);
        }

        [ContextMenu("Check N Viability")]
        public bool CheckNViability()
        {
            if (output.prefab == null)
            {
                print("No output");
                return false;
            }
            bool overall = true;
            string o = "";
            foreach (Tile t in Neighbours.Keys)
            {
                if (t.output.prefab != null)
                {
                    bool v = Solver.CompareMatchingExits(output, t.output, Neighbours[t]);
                    o += (Neighbours[t].ToString() + " - " + t.name + ": " + v.ToString()) + "\n";
                    if (!v)
                    {
                        overall = false;
                    }
                }
            }
            o += ("Overall: " + overall.ToString());
            print(o);
            bool vFound = false;
            foreach (TileModel m in Solver.Instance.allPossibleTiles)
            {
                bool viable = true;
                foreach (Tile t in Neighbours.Keys)
                {
                    if (t.output.prefab != null)
                    {
                        bool v = Solver.CompareMatchingExits(output, t.output, Neighbours[t]);
                        if (!v)
                        {
                            viable = false;
                        }
                    }
                }
                if (viable)
                {
                    output = m;
                    if (transform.childCount > 0)
                    {
                        foreach (Transform child in transform)
                        {
                            Destroy(child.gameObject);
                        }
                    }
                    Instantiate(output.prefab, this.transform);
                    print("Viable alternative found");
                    vFound = true;
                    break;
                }
            }
            if (!vFound)
            {
                print("no viable alternative out of " + Solver.Instance.allPossibleTiles.Count.ToString() + " tile prefabs");
            }
            return overall;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position + Vector3.up * -2, Vector3.one * 0.075f);
            if (output.prefab == null)
            {
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.up * 0.4f, 0.025f);
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.down * 0.4f, 0.025f);
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.left * 0.4f, 0.025f);
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.right * 0.4f, 0.025f);
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.forward * 0.4f, 0.025f);
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.back * 0.4f, 0.025f);
            }
            else
            {
                Gizmos.color = exitColor[output.upExit];
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.up * 0.4f, 0.075f);
                Gizmos.color = exitColor[output.downExit];
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.down * 0.4f, 0.075f);
                Gizmos.color = exitColor[output.leftExit];
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.left * 0.4f, 0.075f);
                Gizmos.color = exitColor[output.rightExit];
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.right * 0.4f, 0.075f);
                Gizmos.color = exitColor[output.forwardExit];
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.forward * 0.4f, 0.075f);
                Gizmos.color = exitColor[output.backExit];
                Gizmos.DrawSphere(transform.position + Vector3.up * -2 + Vector3.back * 0.4f, 0.075f);
            }

        }
    }

}
