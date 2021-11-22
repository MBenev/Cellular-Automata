using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityScript : MonoBehaviour
{
    Image sanityMeter;
    float maxSanity = 100f;
    public static float sanity;

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
}
