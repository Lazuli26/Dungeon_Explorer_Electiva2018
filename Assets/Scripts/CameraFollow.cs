using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject player;
    public Vector3 displacement;
    public FixedTouchField touchField;
    public float delay = 0.5f;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(TouchScreen());
        Vector3 pos = player.transform.position;
        pos+= displacement;
        transform.position = Vector3.MoveTowards(transform.position,pos,1);
        transform.LookAt(player.transform);
    }

    IEnumerator TouchScreen()
    {
        if (touchField.TouchDist != Vector2.zero)
        {
            //Debug.Log("Touch dist :" + touchField.TouchDist + ", Camera: " + transform.position);
            if(Mathf.Abs(touchField.TouchDist.x) > Mathf.Abs(touchField.TouchDist.y) &&
                Mathf.Sqrt(Mathf.Pow(touchField.TouchDist.x, 2) + Mathf.Pow(touchField.TouchDist.y, 2)) > 100)
            {
                if (Mathf.Sign(touchField.TouchDist.x) == -1)
                {
                    if (displacement.z / Mathf.Abs(displacement.z) != displacement.x / Mathf.Abs(displacement.x))
                        displacement.z = displacement.z * -1;
                    else
                        displacement.x = displacement.x * -1;
                }
                else
                {
                    if (displacement.z / Mathf.Abs(displacement.z) == displacement.x / Mathf.Abs(displacement.x))
                        displacement.z = displacement.z * -1;
                    else
                        displacement.x = displacement.x * -1;
                }
                touchField.TouchDist = Vector2.zero;
            }
        }
        yield return new WaitForSeconds(delay);
    }
}
