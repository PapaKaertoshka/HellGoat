using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HorizontalSelector<T> : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    
    [SerializeField] private Button _rightButton;

    [SerializeField] protected TMP_Text _text;

    [SerializeField] protected List<T> _data = new List<T>();

    [SerializeField] private int _defaultValueIndex = 0;

    [SerializeField] private AudioSource _audioOnClick;

    protected int _currentIndex = 0;

    protected virtual void Awake()
    {
        _leftButton.onClick.AddListener(OnLeftButtonClicked);    
        _rightButton.onClick.AddListener(OnRightButtonClicked);
        
        _currentIndex = _defaultValueIndex;
        UpdateText();
        Execute();
    }

    protected virtual void UpdateText() {}

    protected virtual void OnRightButtonClicked()
    {
        if(_currentIndex == _data.Count - 1) return;
        
        if(_audioOnClick) _audioOnClick.Play();
        ++_currentIndex;
        UpdateText();
        Execute();
    }

    protected virtual void OnLeftButtonClicked()
    {
        if(_currentIndex == 0) return;
        if(_audioOnClick) _audioOnClick.Play();
        --_currentIndex;
        UpdateText();
        Execute();
    }

    protected virtual void Execute() {}

    private void OnDestroy()
    {
        _leftButton.onClick.RemoveAllListeners();
        _rightButton.onClick.RemoveAllListeners();
    }
}
