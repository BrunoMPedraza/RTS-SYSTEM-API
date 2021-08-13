using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyHandler", menuName = "Utils/KeyHandler")]
public class KeyHandler : ScriptableObject
{
    public enum AtFindingDuplicates { JUSTWARN, ACCEPT_INCOME, REJECT_INCOME}

    [Header("Editor behaviour")]
    public AtFindingDuplicates findingDuplicatesBehaviour;
    [Header("Keyhandler")]
    public string keyHandlerName = "Default";
    public List<Key> inputKeys = new List<Key>();
    public bool keyboardControlsCamera = false;
    public void OnValidate()
    {
        List<Key> usedKeys = new List<Key>();
        foreach (Key key in inputKeys)
        {
            //find: checks every key, 'k' being every key, and picking the first key where 'k' has the same code has 'key'
            if (usedKeys.Contains(key))
            {
                Key duplicate = usedKeys.Find(k => k.keyCode == key.keyCode);
                
                Debug.LogWarning("Key duplicated found in " + keyHandlerName + ": " +
                          key + " -- " + duplicate);
                switch (findingDuplicatesBehaviour)
                {
                    case AtFindingDuplicates.JUSTWARN:
                        Debug.Log("No action taken.");
                        break;
                    case AtFindingDuplicates.ACCEPT_INCOME:
                        Debug.Log(duplicate + " was disabled.");
                        duplicate.keyCode = KeyCode.None;
                        break;
                    case AtFindingDuplicates.REJECT_INCOME:
                        Debug.Log(key + " was disabled.");
                        key.keyCode = KeyCode.None;
                        break;
                }

                
            }
            
            usedKeys.Add(key);
        }
    }
}
