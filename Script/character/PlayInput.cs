using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]

[RequireComponent(typeof(Rigidbody))]

[RequireComponent(typeof(Animator))]

[RequireComponent(typeof(AudioSource))]

public class PlayInput : MonoBehaviour
{

    public static string[] keyName;

    [Header("----- Input -----")]
    public string keyFight;
    public string keyActive;
    public string keyWalk;
    public string keyRun;
    public string keyLock;
    public string lightAttack;
    public string strongAttack; //也是抵挡键
    public string keyJump;
    public string keyDance;


    [Header("----- signal -----")]
    public float vSpeed;
    public float hSpeed;
    public bool isGround;
    public bool isWalk;
    public bool isRun;
    public bool isJump;
    public bool isDance;
    public bool isFight;
    public bool isLightAttack;
    public bool isStrongAttack;
    public bool isBlock;
    public bool isActive;
    public bool isLock;
    public bool isKill;
    public bool closeLadderButtom;
    public bool closeLadderTop;
    public bool getHit;

    [Header("----- parameter -----")]
    public float jumpPower = 6f;
    public float gravityMultiplier = 2f;//重力

    [Header("----- Enable -----")]
    public bool inputEnable = true;




    // Use this for initialization
    void Start()
    {

        if (keyName != null)
        {
            for (int i = 0; i < keyName.Length; i++)
            {
                switch (keyName[i])
                {
                    case "leftshift":
                        keyName[i] = "left shift";
                        break;
                    case "rightshift":
                        keyName[i] = "right shift";
                        break;
                    case "mouse0":
                        keyName[i] = "mouse 0";
                        break;
                    case "mouse1":
                        keyName[i] = "mouse 1";
                        break;
                    case "mouse2":
                        keyName[i] = "mouse 2";
                        break;
                    case "leftalt":
                        keyName[i] = "left alt";
                        break;
                }
            }

            keyFight = keyName[4];
            keyActive = keyName[5];
            keyWalk = keyName[6];
            keyRun = keyName[7];
            keyLock = keyName[8];
            lightAttack = keyName[9];
            strongAttack = keyName[10];
            keyJump = keyName[11];
            keyDance = keyName[12];
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputEnable)
        {
            isWalk = Input.GetKey(keyWalk);

            vSpeed = Input.GetAxis("Vertical");
            hSpeed = Input.GetAxis("Horizontal");






            if (!isFight)
            {
                isDance = Input.GetKeyDown(keyDance);

                if (vSpeed != 0 || hSpeed != 0)
                    isDance = false;
            }
            else
            {
                isRun = Input.GetKey(keyRun);
                isLightAttack = Input.GetKeyDown(lightAttack);
                isStrongAttack = Input.GetKeyDown(strongAttack);
                isBlock = Input.GetKey(strongAttack);

            }


            if (Input.GetKeyDown(keyFight)) isFight = !isFight;





            isActive = Input.GetKeyDown(keyActive);

            isJump = Input.GetKeyDown(keyJump);

        }






    }


}
