using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementDragDrop : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;


    [SerializeField]
    private GameObject placedPrefab;

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private Button dismissButton;

    private GameObject spawn;

    private Vector2 touchPosition = default;

    private ARRaycastManager arRaycastManager;

    private bool onTouchHold = false;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        dismissButton.onClick.AddListener(Dismiss);
    }

    private void Dismiss() => welcomePanel.SetActive(false);
    
    void Update()
    {
        if (welcomePanel.activeSelf)
            return;


        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if(Physics.Raycast(ray, out hitObject))
                {
                    if(hitObject.transform.name.Contains("PlacedObject"))

                    {
                        onTouchHold = true;
                    }
                }
           
            }

            if(touch.phase == TouchPhase.Moved)
            {
                touchPosition = touch.position;
            }

            if(touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
            }
        }

        if(onTouchHold)
        {
            if(arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                if (spawn == null)
                {
                    spawn = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                }

                else
                {

                    if (onTouchHold)
                    {
                        placedPrefab.transform.position = hitPose.position;
                        placedPrefab.transform.rotation = hitPose.rotation;
                    }
                }
            }
        }
    }

  

}
