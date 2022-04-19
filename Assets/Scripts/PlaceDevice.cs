using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceDevice : MonoBehaviour {
    [SerializeField] ARSessionOrigin arOrigin;
    [SerializeField] ARRaycastManager aRRaycastManager;
    [SerializeField] GameObject placementIndicator;
    [SerializeField] public GameObject prefab;
    Pose _placementPose;
    bool _placementPoseIsValid = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (_placementPoseIsValid && Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began)
            PlaceObject();
    }

    void PlaceObject() {
        Instantiate(prefab, _placementPose.position, _placementPose.rotation);
    }

    private void UpdatePlacementIndicator() {
        if(_placementPoseIsValid) {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        else
            placementIndicator.SetActive(false);
    }

    void UpdatePlacementPose() {
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
