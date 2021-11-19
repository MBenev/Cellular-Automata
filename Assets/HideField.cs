using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideField : MonoBehaviour
{
    public Toggle randomSeed;
    bool temp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideFieldMethod()
    {
        temp = randomSeed.isOn;
        if (!temp)
        {
            gameObject.SetActive(true);
        }
        else if (temp)
            gameObject.SetActive(false);
    }
}
