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
    public GameObject light;
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
            //Debug.Log("After 5 second is false");            
            happenedInTheLast5Seconds = true;

            var rng = new System.Random();
            int randomEventNumber = rng.Next(0, 3);
            switch(randomEventNumber)
            {
                case 0:
                    Debug.Log("case 0");
                    //EnableTimeScript();
                    eventButton.SetActive(true);
                    break;
                case 1:
                    Debug.Log("case 1");
                    light.SetActive(false);
                    EnableTimeScript();
                    break;
                case 2:
                    Debug.Log("case 2");
                    EnableTimeScript();
                    break;
            }
        }
    }
}
