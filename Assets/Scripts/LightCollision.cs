using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.tag == "Player")
    //    {
    //        Player.Instance.isInLight = true;
    //        Debug.Log("lighted");
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.Instance.isInLight = true;
            Debug.Log("lighted");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.Instance.isInLight = false;
            Debug.Log("darkness");
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.collider.tag == "Player")
    //    {
    //        Player.Instance.isInLight = false;
    //        Debug.Log("darkness");
    //    }
    //}
    // Update is called once per frame
    void Update()
    {
        //if(Player.Instance.)
    }
}
