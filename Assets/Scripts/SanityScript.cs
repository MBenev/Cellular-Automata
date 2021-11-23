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

    private void Start()
    {
        sanityMeter = GetComponent<Image>();
        sanity = maxSanity;
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
                eventButton.SetActive(true);
            }
        }
        if(sanity > 50f)
        {
            sanityMeter.GetComponent<Image>().color = new Color(161, 161, 161);
        }
    }
}
