 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{

    public GameObject gameObjectToManipulate;
    private GameObject spawnObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
       
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
       
            touchPosition = default;
            return false;
       
      
    }
   
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if(spawnObject == null)
            {
                spawnObject = Instantiate(gameObjectToManipulate, hitPose.position, hitPose.rotation);

            }
            else
            {
                spawnObject.transform.position = hitPose.position;
            }

           
        }
    }
}
