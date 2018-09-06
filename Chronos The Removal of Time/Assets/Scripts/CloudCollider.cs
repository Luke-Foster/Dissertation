using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCollider : MonoBehaviour
{
    Transform ChronosTransform;
    Transform ChronosSpawnTransform;

    void Start ()
    {
        ChronosTransform = GameObject.Find("Chronos").transform;
        ChronosSpawnTransform = GameObject.Find("ChronosSpawn").transform;
    }
	
	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == ("Chronos"))
        {
            ChronosTransform.position = ChronosSpawnTransform.position;
            ChronosTransform.rotation = ChronosSpawnTransform.rotation;
        }
    }
}
