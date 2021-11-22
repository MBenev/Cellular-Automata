using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextLevel : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && Player.Instance.GetCollectiblesAmount() == 3)
        {
            Debug.Log("Loading Next Level");
        }
    }
}
