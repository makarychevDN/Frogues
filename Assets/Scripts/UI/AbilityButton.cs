using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private AOEWeapon ability;
    [SerializeField] private bool usingNow; 
    private PlayerInput _playerInput;
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Image>().material;
        _playerInput = FindObjectOfType<PlayerInput>();
    }

    public bool UsingNow
    {
        get => usingNow;
        set
        {
            usingNow = value;
            _material.SetInt("_AbilityUsingNow", value ? 1 : 0);
        }
    }

    private void Update()
    {
        _material.SetInt("_AbilityUsingNow", (_playerInput.currentAbility == ability) ? 1 : 0);
    }

    public void PickAbility()
    {
        _playerInput.currentAbility = ability;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }
}
