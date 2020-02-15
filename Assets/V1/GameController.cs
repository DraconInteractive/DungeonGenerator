using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Start_GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Start_GenerateLevel ()
    {
        StartCoroutine(GetComponent<LevelGenerator>().GenerateLevel());
    }


}
