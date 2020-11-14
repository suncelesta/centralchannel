using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private const float GrabInterval = 0.05f;

    private bool _hasGrabbed;
    private Vector3 _grabbedDirection;
    private float _grabBegin = 0.0f;

    private Collider _trajColl;
    private Plane _rotationExtension;
    private Camera _camera;
    private NavigationController _navigationController;

    private void Start()
    {
        _camera = Camera.main;
        _trajColl = GameObject.Find("Rotation Trajectory").GetComponent<Collider>();
        _rotationExtension = new Plane(Vector3.forward, transform.transform.position);
        _navigationController = GameObject.Find("Actions").GetComponent<NavigationController>();
    }
    private void Update()
    {
        if (!_hasGrabbed || !ShouldRotate()) return;
        
        var targetPoint = TargetDirection();
        var rotation = Quaternion.FromToRotation(_grabbedDirection, targetPoint);
        Rotate(rotation);
    }

    private bool ShouldRotate()
    {
        return !Input.touchSupported || Time.time - _grabBegin > GrabInterval && Input.touchCount == 1;
    }

    private Vector3 TargetDirection()
    {
        Vector3 point;

        var mouseRay = _camera.ScreenPointToRay(Input.mousePosition);

        if (_trajColl.Raycast(mouseRay, out var hit, float.PositiveInfinity))
        {
            point = hit.point;
        }
        else
        {
            _rotationExtension.Raycast(mouseRay, out var enter);
            point = mouseRay.GetPoint(enter);
        }

        return transform.transform.InverseTransformDirection(point - transform.transform.position);
    }

    public void Rotate(Quaternion rotation)
    {
        transform.localRotation *= rotation;
        KeepLightRotation();
    }

    public void AxisRotateBy(float angle)
    {
        transform.Rotate(Vector3.down, angle + transform.eulerAngles.y);
        KeepLightRotation();
    }

    private void KeepLightRotation()
    {
        var light = transform.Find("Light");
        for (int i = 0; i < light.childCount; i++)
        {
            light.GetChild(i).LookAt(_camera.transform);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Grab();   
        _grabbedDirection = transform.transform.InverseTransformDirection(
            eventData.pointerCurrentRaycast.worldPosition - transform.transform.position);
        _navigationController.CallMethodOnState("OnObjectGrab");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        if (_hasGrabbed) _hasGrabbed = false;
        _navigationController.CallMethodOnState("OnObjectUnGrab");
    }

    private void Grab()
    {
        if (Input.touchSupported)
        {
            _grabBegin = Time.time;
        }
        _hasGrabbed = true; 
    }
}