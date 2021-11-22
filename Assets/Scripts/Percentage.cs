using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Percentage : MonoBehaviour
{
    Text percentageText;

    private void Start()
    {
        percentageText = GetComponent<Text>();
    }

    public void PercentageUpdate (float value)
    {
        //Debug.Log(value);   
        percentageText.text = Mathf.RoundToInt(value) + "%";
    }

    public string GetPercentage(float value)
    {
        return percentageText.text = Mathf.RoundToInt(value) + "%";
    }
}
