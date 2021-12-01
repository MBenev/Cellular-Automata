using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadNextLevel : MonoBehaviour
{
    private GameObject mapGenerator;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && Player.Instance.AreAllCollected())
        {
            //Debug.Log("Loading Next Level");
            Player.Instance.GetComponent<StartGame>().GoBackToTool();
            mapGenerator = GameObject.Find("Map Generator");
            mapGenerator.GetComponent<MapGenerator>().GenerateButtonClick();
        }
    }
}
