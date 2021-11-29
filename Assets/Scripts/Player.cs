using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public TextMeshProUGUI text;
    public float sanity, maxSanity;
    public GameObject torch;
    public GameObject panel;
    public GameObject eventButton;
    public int invertedControls = 1;
    public bool equipped = true;
    public bool isInLight;

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

    //void IsLighted()
    //{
    //    isInLight = true;
    //}

    Rigidbody rigidBody;
    Vector3 velocity;
    [SerializeField] private int collected;

    public void LowerSanity()
    {
        if(eventButton.active == false)
        {
            SanityScript.sanity -= 0.05f;
        }
    }

    public void IncreaseSanity()
    {
        SanityScript.sanity += 0.05f;
    }

    public void AddCollected()
    {
        collected++;
    }

    public void ClearCollected()
    {
        collected = 0;
    }
    
    public int GetCollectiblesAmount()
    {
        return collected;
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        //range = this.li
    }

    public void EquipTorch()
    {
        torch.SetActive(true);
        equipped = true;
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
        if ((!isInLight && !equipped) && panel.active == false)
        {
            LowerSanity();
        }
        if ((isInLight || equipped) && panel.active == false)
        {
            IncreaseSanity();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (equipped)
            {
                torch.SetActive(false);
                equipped = false;
            }
            else if (!equipped)
            {
                torch.SetActive(true);
                equipped = true;
            }
        }
    }


    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + velocity * invertedControls * Time.fixedDeltaTime);
    }

}
