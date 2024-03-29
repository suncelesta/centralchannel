﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class Outside : TimingState<Outside> 
{
    private ConditionalButton _insideButton;
    private Transform _camera;
    private Transform _canal;

    public override void Enter()
    {
        _camera = Camera.main.transform;
        _canal = GameObject.Find("Canals").transform;
        
        var buttonObj = GameObject.Find("Canvas").transform
            .Find("EnterCanalButton").GetComponent<Button>();
        _insideButton = new ConditionalButton(buttonObj, FacesCanalTop);
    }

    public override void Exit()
    {
        _insideButton.SetActive(false);
    }
    
    public override void Update()
    {
        _insideButton.Update();
    }

    private bool FacesCanalTop()
    {
        var position = _camera.position;
        var direction = (_canal.position - position).normalized;
        var ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.normal == hit.transform.up;
        }

        return false;
    }
}
