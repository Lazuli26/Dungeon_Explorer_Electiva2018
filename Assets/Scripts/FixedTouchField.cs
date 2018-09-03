using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    Vector2 first;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        first = eventData.position;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        TouchDist = eventData.position - first;
        Debug.Log(TouchDist);
    }

}
