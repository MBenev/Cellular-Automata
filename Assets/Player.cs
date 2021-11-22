using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private static Player _instance;
    //public Text text;
    public TextMeshProUGUI text;
    //private float timeToAppear = 0.1f;
    //private float timeWhenDisappear;
    public static Player Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Player is null");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    Rigidbody rigidBody;
    Vector3 velocity;
    [SerializeField] private int collected;

    public void AddCollected()
    {
        collected++;
    }

    public void ClearCollected()
    {
        collected = 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized * 10;

        //CheckForCollectible();
        if (collected == 3)
        {
            text.text = "Door is now open!";
        }

    }


    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
    }

}
