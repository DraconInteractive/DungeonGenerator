using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourHolder : MonoBehaviour
{
    public List<GameObject> neighbours = new List<GameObject>();
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (neighbours != null && neighbours.Count > 0)
        {
            foreach (GameObject t in neighbours)
            {
                Gizmos.DrawLine(transform.position, t.transform.position);
            }
        }
    }
}
