using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public TextMeshProUGUI text;
    private void Start()
    {
        sanityMeter = GetComponent<Image>();
        sanity = maxSanity;
        waitTimeScript = player.GetComponent<WaitTime>();
    }

    private void Update()
    {
        sanityMeter.fillAmount = sanity / maxSanity;
        if (sanity >= 100)
            sanity = 100;
        else if (sanity <= 0)
            sanity = 0;
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

    public void ResetEffectsText()
    {
        text.text = "No effect";
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
                    text.text = "BSOD";
                    eventButton.SetActive(true);
                    break;
                case 1:
                    Debug.Log("case 1");
                    text.text = "No light from torch";
                    light.SetActive(false);
                    EnableTimeScript();
                    break;
                case 2:
                    Debug.Log("case 2");
                    text.text = "Inverted Controls";
                    Player.Instance.invertedControls = -1;
                    EnableTimeScript();
                    break;
            }
        }
    }
}
