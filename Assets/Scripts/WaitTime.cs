using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTime : MonoBehaviour
{
    public GameObject sanityMeter;

    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnEnable()
    {
        StartCoroutine(Example());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Example()
    {
        yield return new WaitForSecondsRealtime(5);
        //Debug.Log("time passed");
        sanityMeter.GetComponent<SanityScript>().AfterTimePasses();
    }
}
