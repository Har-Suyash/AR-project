using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class CenterPlace : MonoBehaviour
{


    public GameObject PlacementIndicator;
    public GameObject objectToPlace;
    public GameObject ringaToRing;
    public GameObject welcomePanel;
    public GameObject movePhonePanel;
   
    public Camera arCamera;

    private GameObject spawnObject;
    private GameObject spawnObject2;

    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private ARRaycastManager _arRaycastManager;
   

    private void Awake()
    {
       // phoneMovePanel.SetActive(false);
        _arRaycastManager = GetComponent<ARRaycastManager>();
        //arLaunch.onClick.AddListener(Dismiss);
        //arLaunch.onClick.AddListener(Intro);



    }

  


    void Update()
    {
        if (welcomePanel.activeSelf)
        {
            return;
        }
            

        UpdatePlacementPose();
        UpdatePlacementIndicator();

        


        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
            
        }

        
    }

    private void PlaceObject()
    {
        if (spawnObject == null)
        {
            spawnObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
            spawnObject2 = Instantiate(ringaToRing, placementPose.position, placementPose.rotation);
            Destroy(PlacementIndicator);
            movePhonePanel.SetActive(false);

        }
      

      
           
        
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            PlacementIndicator.SetActive(true);
            PlacementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }

        else
        {
            PlacementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        _arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
        
            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid)
            {
                placementPose = hits[0].pose;
            
            }
        
        
    }
}
