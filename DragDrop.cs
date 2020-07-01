using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class DragDrop : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private GameObject Ring;

    [SerializeField]
    private GameObject anchor;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private Button lockButton;

    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private float defaultRotation = 0;

    private GameObject placedObject;

    private Vector2 touchPosition = default;

    private ARRaycastManager arRaycastManager;

    private bool isLocked = false;

    private bool onTouchHold = false;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Pose hitPose;

    void Awake()
    {
        anchor.SetActive(false);
        arRaycastManager = GetComponent<ARRaycastManager>();
        dismissButton.onClick.AddListener(Dismiss);
        dismissButton.onClick.AddListener(Anchor);
        lockButton.onClick.AddListener(AnDiss);
       // lockButton.onClick.AddListener(ripple);

        if (lockButton != null)
        {
            lockButton.onClick.AddListener(Lock);
        }
    }
    private void Dismiss() => welcomePanel.SetActive(false);
    private void Anchor() => anchor.SetActive(true);
    private void AnDiss() => anchor.SetActive(false);
    private void ripple() => Instantiate(Ring, hitPose.position, hitPose.rotation);


    private void Lock()
    {
        isLocked = !isLocked;
        lockButton.GetComponentInChildren<Text>().text = isLocked ? "Move" : "Place";
        if (placedObject != null)
        {
            placedObject.GetComponent<PlacementObject>()
                .SetOverlayText(isLocked ? " " : "Drag to move around");

            placedObject.GetComponent<PlacementObject>()
                .SetMoveIcon(isLocked ? false : true);
        }
    }

    void Update()
    {
          lockButton.onClick.AddListener(AnDiss);
        //lockButton.onClick.AddListener(ripple);

        // do not capture events unless the welcome panel is hidden
        if (welcomePanel.activeSelf)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();
                    if (placementObject != null)
                    {
                        onTouchHold = isLocked ? true : true;
                        placementObject.GetComponent<PlacementObject>().SetOverlayText(isLocked ? " " : "Drag to move around");
                        
                        placementObject.SetMoveIcon(onTouchHold);
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
            }
        }

        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
             hitPose = hits[0].pose;

            if (placedObject == null)
            {
                if (defaultRotation > 0)
                {
                    placedObject = Instantiate(placedPrefab, hitPose.position, Quaternion.identity);
                    placedObject.transform.Rotate(Vector3.up, defaultRotation);
                }
                else
                {
                    placedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                }
            }
            else
            {
                if (onTouchHold)
                {
                    placedObject.transform.position = hitPose.position;
                    if (defaultRotation == 0)
                    {
                        placedObject.transform.rotation = hitPose.rotation;
                    }
                }
            }
        }
    }
}