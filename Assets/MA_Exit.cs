using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MA_Exit : MonoBehaviour
{
    public bool attached;
    public MA_Prefab ownerPrefab;
    public MA_Prefab attachedPrefab;
    [Range(0,1)]
    public float forwardOffset;
    public float checkRadius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("Update Attached Status")]
    public void UpdateAttached ()
    {
        Collider[] results = Physics.OverlapSphere(transform.position + transform.forward * forwardOffset, checkRadius);
        foreach (Collider c in results)
        {
            if (c.gameObject != this.gameObject)
            {
                attached = true;
                MA_Prefab a = null;
                if (c.TryGetComponent(out a))
                {
                    attachedPrefab = a;
                    break;
                } else
                {
                    if (c.transform.parent.TryGetComponent(out a))
                    {
                        attachedPrefab = a;
                        break;
                    } else
                    {
                        if (c.transform.parent.parent.TryGetComponent(out a))
                        {
                            attachedPrefab = a;
                            break;
                        } else
                        {
                            print("Thing is too nested");
                        }
                    }
                }
            }
        }

        
    }

    public void UpdateAttached(float radius, float offset)
    {
        Collider[] results = Physics.OverlapSphere(transform.position + transform.forward * offset, radius);
        foreach (Collider c in results)
        {
            if (c.gameObject != this.gameObject)
            {
                attached = true;
                MA_Prefab a = null;
                if (c.TryGetComponent(out a))
                {
                    attachedPrefab = a;
                    break;
                }
                else
                {
                    if (c.transform.parent.TryGetComponent(out a))
                    {
                        attachedPrefab = a;
                        break;
                    }
                    else
                    {
                        if (c.transform.parent.parent.TryGetComponent(out a))
                        {
                            attachedPrefab = a;
                            break;
                        }
                        else
                        {
                            print("Thing is too nested");
                        }
                    }
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * forwardOffset, checkRadius);
    }
}
