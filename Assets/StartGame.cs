using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject MainCamera;
    CameraFollow script;

    private void Start()
    {
        script = MainCamera.GetComponent<CameraFollow>();        
    }

    public void ActivateGame()
    {
        script.enabled = true;
    }
}
