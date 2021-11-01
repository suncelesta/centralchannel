using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = System.Diagnostics.Debug;

public class Zoom : MonoBehaviour {

    public Transform target;
    public float zoomSpd = 2.0f;
    public float zoomOutRate = 0.5f;

    private Rigidbody _rigidbody;
    private Collider _targetCollider;
    private new Transform _camera;
    private NavigationController _controller;

    public void Start()
    {
        SetTargetCollider();

        _controller = GameObject.Find("Actions").GetComponent<NavigationController>();
        
        _camera = Camera.main.transform;

        _rigidbody = GameObject.Find("Canals").GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (_rigidbody)
            _rigidbody.freezeRotation = true;

    }

    private void SetTargetCollider()
    {
        if (!target) return;
        var colliders = target.GetComponents<Collider>();
        foreach (var coll in colliders)
        {
            if (!coll.CompareTag("Closest collider")) continue;
            _targetCollider = coll;
            break;
        }
    }

    public void Update()
    {
        if (!target) return;

        var cameraPosition = _camera.position;
        var toTarget = target.position - cameraPosition;

        var zoomValue = ZoomValue();
        if (Math.Abs(zoomValue) > 0.00001f)
        {
            _controller.CallMethodOnState("OnZoom");
        }

        var zoomDirection = 0.02f * zoomValue * zoomSpd * toTarget;
        var maxZoomDistance = zoomValue > 0 ? DistanceToTarget(toTarget) : zoomOutRate;

        cameraPosition += Vector3.ClampMagnitude(zoomDirection, maxZoomDistance);
        _camera.position = cameraPosition;
    }

    private static float ZoomValue()
    {
        if (Input.touchSupported && Input.touchCount == 2)
        {
            var touches = Input.touches;
            var distanceBefore = Vector2.Distance(PrevPosition(touches[0]), PrevPosition(touches[1]));
            var distanceAfter = Vector2.Distance(touches[0].position, touches[1].position);

            return (distanceAfter - distanceBefore) * 0.01f;
        }
        else
        {
            var wheelZoom = Input.GetAxis("Mouse ScrollWheel");
            var arrowZoom = Input.GetAxis("Vertical");
            return Math.Abs(wheelZoom) > 0 ? wheelZoom : arrowZoom;
        }
    }

    private static Vector2 PrevPosition(Touch touch)
    {
        return touch.position - touch.deltaPosition;
    }

    private float DistanceToTarget(Vector3 direction)
    {
        Ray ray = new Ray(_camera.position, direction);
        RaycastHit hit;

        _targetCollider.Raycast(ray, out hit, direction.magnitude);

        return _targetCollider.Raycast(ray, out hit, direction.magnitude) ? hit.distance : 0;
    }
}
