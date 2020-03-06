using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public bool overTime, lookAt;
    public float moveDelta, rotateDelta;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (overTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position + target.up * offset.y + target.right * offset.x + target.forward * offset.z, moveDelta * Time.deltaTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, rotateDelta * Time.deltaTime);
        } else
        {
            transform.position = target.position;
            
        }
        
        if (lookAt)
        {
            transform.LookAt(target.position);
        } else
        {
            transform.rotation = target.rotation;
        }
    }
}
