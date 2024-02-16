using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> SpawnObjects;
    public int Direction; 

    private void Start()
    {
        InvokeRepeating(nameof(Spawn),0.2f,Random.Range(5f,7f));
    }

    private void Spawn()
    {
        var index =  Random.Range(0, SpawnObjects.Count);
var  target =  Instantiate(SpawnObjects[index], transform.position,Quaternion.identity,transform);
target.GetComponent<MoveForward>()._dir = Direction;
    }

}
