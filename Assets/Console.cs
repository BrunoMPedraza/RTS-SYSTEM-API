using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Console : MonoBehaviour
{
    private GameObject _panelConsole;
    private bool _pause;

    // Start is called before the first frame update
    void Start()
    {
        _panelConsole = transform.GetChild(0).gameObject;
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

    public void HandleFreezeGame(){
        _pause = !_pause;
        Time.timeScale = _pause ? 0 : 1;
    }
}
