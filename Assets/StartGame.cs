using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject MainCamera;
    CameraFollow script;
    public bool gameActive = false;
    Vector3 cameraMenuPos = new Vector3(787, 255, -436);
    Quaternion cameraMenuRot = new Quaternion(0, 0, 0, 1);

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameActive)
            {
                script.enabled = false;
                MainCamera.transform.position = cameraMenuPos;
                MainCamera.transform.rotation = cameraMenuRot;
                gameActive = false;
            }
            else
            {
                Application.Quit();
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }

    private void Start()
    {
        script = MainCamera.GetComponent<CameraFollow>();        
    }

    public void ActivateGame()
    {
        script.enabled = true;
        gameActive = true;
    }
}
