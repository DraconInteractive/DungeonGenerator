using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MA_Prefab : MonoBehaviour
{
    public MA_Exit[] exits;
    public MA_RCCheck[] checkPoints;
    private void Reset()
    {
        exits = GetComponentsInChildren<MA_Exit>(true);
        checkPoints = GetComponentsInChildren<MA_RCCheck>(true);

        if (exits != null && exits.Length > 0)
        {
            foreach (MA_Exit exit in exits)
            {
                exit.ownerPrefab = this;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (exits != null && exits.Length > 0)
        {
            foreach (MA_Exit exit in exits)
            {
                Gizmos.DrawWireSphere(exit.transform.position, 0.05f);
                Gizmos.DrawLine(exit.transform.position, exit.transform.position + Vector3.up * 2);
            }
        }
        Gizmos.color = Color.blue;
        if (checkPoints != null && checkPoints.Length > 0)
        {
            foreach (MA_RCCheck point in checkPoints)
            {
                Gizmos.DrawWireSphere(point.transform.position, 0.05f);
                Gizmos.DrawLine(point.transform.position, point.transform.position + Vector3.up * 2);
            }
        }
    }
}
