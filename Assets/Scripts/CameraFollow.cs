using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject player;
    public Vector3 displacement;


    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (displacement.z / Mathf.Abs(displacement.z) != displacement.x / Mathf.Abs(displacement.x))
                displacement.z = displacement.z * -1;
            else
                displacement.x = displacement.x * -1;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (displacement.z / Mathf.Abs(displacement.z) == displacement.x / Mathf.Abs(displacement.x))
                displacement.z = displacement.z * -1;
            else
                displacement.x = displacement.x * -1;
        }
        Vector3 pos = player.transform.position;
        pos+= displacement;
        transform.position = Vector3.MoveTowards(transform.position,pos,1);
        transform.LookAt(player.transform);
    }
}
