using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityScript : MonoBehaviour
{
    Image sanityMeter;
    float maxSanity = 100f;
    public static float sanity;
    public GameObject eventButton;
    WaitTime waitTimeScript;
    public GameObject player;
    bool happenedInTheLast5Seconds = false;

    private void Start()
    {
        sanityMeter = GetComponent<Image>();
        sanity = maxSanity;
        waitTimeScript = player.GetComponent<WaitTime>();
    }

    private void Update()
    {
        sanityMeter.fillAmount = sanity / maxSanity;
    }

    public void SetMax()
    {
        sanity = 100f;
        Player.Instance.EquipTorch();
    }

    private void FixedUpdate()
    {
        if(eventButton.active == false)
        {
            if (sanity < 50f)
            {
                sanityMeter.GetComponent<Image>().color = new Color(139, 0, 0);
                ShowEvent();
            }
        }
        if(sanity > 50f)
        {
            sanityMeter.GetComponent<Image>().color = new Color(161, 161, 161);
        }
        
    }

    public void EnableTimeScript()
    {
        waitTimeScript.enabled = true;
    }

    public void AfterTimePasses()
    {
        happenedInTheLast5Seconds = false;
        waitTimeScript.enabled = false;
    }

    private void ShowEvent()
    {
        if(!happenedInTheLast5Seconds)
        {
            Debug.Log("After 5 second is false");
            eventButton.SetActive(true);
            happenedInTheLast5Seconds = true;            
        }

    }
}
