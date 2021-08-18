using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Console : MonoBehaviour
{
    private GameObject _panelConsole;
    private static GameObject _textConsole;
    private bool _pause;
    public static GameObject textPrefab;



    // Start is called before the first frame update
    void Start()
    {
        _textConsole = transform.GetChild(1).gameObject;
        _panelConsole = transform.GetChild(0).gameObject;
        textPrefab = Resources.Load<GameObject>("UI/ConsoleLog");
        _panelConsole.SetActive(false);
        _pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(PlayerInputManager.GetKeyCode("console_switchConsole"))){
            _panelConsole.SetActive(!_panelConsole.activeSelf);
            
        }

    }
    public static void Log(string message){
        GameObject lastCreatedMessage = Instantiate(textPrefab, _textConsole.transform);
        DateTime _currentTime = DateTime.Now; //this is a leak
        lastCreatedMessage.GetComponent<TMP_Text>().SetText("> <i> ["+_currentTime+"] </i>"+message);
    }


    void HandleText(){

    }

    public void HandleFreezeGame(){
        _pause = !_pause;
        Time.timeScale = _pause ? 0 : 1;
        Log(_pause ? "Paused" : "Unpaused");
        
    }
}
