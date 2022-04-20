using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceDevice : MonoBehaviour {
    [SerializeField] ARSessionOrigin arOrigin;
    [SerializeField] ARRaycastManager aRRaycastManager;
    [SerializeField] GameObject placementIndicator;
    [SerializeField] GameObject setupUI;
    [SerializeField] GameObject firstStepUI;
    [SerializeField] GameObject bottomUI;
    [SerializeField] GameObject[] prefabs;
    public List<GameObject> demoObjs;
    Pose _placementPose;
    bool _placementPoseIsValid = false;
    bool _placedObj = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        // Only one instance can be placed per session. Enables the buttons and starts the tutorial while disabling setup/placement indicator.
        if(_placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !_placedObj) {
            _placedObj = true;
            PlaceObject();
            placementIndicator.SetActive(false);
            setupUI.SetActive(false);
            bottomUI.SetActive(true);
            firstStepUI.SetActive(true);
        }
    }

    // Places and hides the volumetric captures for later use based on ray-traced location from camera for AR
    void PlaceObject() {
        foreach(var obj in prefabs) {
            var temp = Instantiate(obj, _placementPose.position, _placementPose.rotation);
            temp.SetActive(false);
            demoObjs.Add(temp);
        }
    }

    // Updates the placement indicator's position/rotation, and disables it if a spot is invalid for placement.
    private void UpdatePlacementIndicator() {
        if(_placementPoseIsValid) {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        else
            placementIndicator.SetActive(false);
    }

    // Raycasts and checks if a plane can be projected; if it can, it updates the position/rotation of the placement pose.
    void UpdatePlacementPose() {
        if (Camera.current == null)
            return;
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        _placementPoseIsValid = hits.Count > 0;
        if(_placementPoseIsValid) {
            _placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
            _placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
