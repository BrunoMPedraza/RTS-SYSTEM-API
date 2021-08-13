using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerInputManager : MonoBehaviour
{
    public static KeyHandler currentKeyHandler;
    private PlayerMotor _playerMotor;

    public void Awake()
    {
        _playerMotor = GetComponent<PlayerMotor>();
    }

    public static KeyCode GetKeyCode(string keyName) => currentKeyHandler.inputKeys.FirstOrDefault(k => k.keyName == keyName).keyCode;

    public void OnEnable()
    {
        if (!currentKeyHandler) currentKeyHandler = Resources.Load<KeyHandler>("KeyHandlerPrefabs/DefaultKeyConfig");
    }
}
