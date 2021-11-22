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
    bool equipped = true;
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

    public void LowerSanity()
    {
        SanityScript.sanity -= 0.05f;
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
        if (!equipped && panel.active == false)
        {
            LowerSanity();
        }
        if (equipped && panel.active == false)
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
        rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
    }

}
