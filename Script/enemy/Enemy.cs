using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private PlayInput pi;

    private SkinnedMeshRenderer sr;

    public Transform player;

    [Header("----- signal -----")]
    public bool isDie;
    public bool isLocked;
    public bool isKilled;
    public bool toPlayer;

    [HideInInspector]
    public int attackID;        //攻击ID

    public Material noLock;     //未锁定材质
    public Material hasLock;    //锁定材质

    private CapsuleCollider cc;     //自身Collider


    public GameObject handAttack;   // 攻击球

    public SphereCollider AttackCollider;

    private Animator anim;

    private Rigidbody rb;

    private bool attack;

    private float attackTime;

    private float vSpeed;
    private float hSpeed;

    public float maxAngle = 110f;

    private float enemyTime;

    private float enemyTurn = 1f;

    // Use this for initialization
    private void Awake()
    {
        sr = transform.Find("Enemy_Model").gameObject.GetComponent<SkinnedMeshRenderer>();
        anim = this.GetComponent<Animator>();
        cc = this.GetComponent<CapsuleCollider>();              //自身碰撞体
        AttackCollider = handAttack.GetComponent<SphereCollider>();    //攻击球碰撞体

        rb = this.GetComponent<Rigidbody>();    //刚体



        attackTime = Random.Range(1, 10);//初始赋值，不然为0，一直打我
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null && pi == null)
        {
            pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayInput>();
            player = pi.transform;
        }

        isLock();

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dead_Front"))
        {
            cc.enabled = false;
        }

        UpdateAnimation();

        if (toPlayer)
            Rotate(player);



        if (Vector3.Distance(player.position, transform.position) < 5f)
        {
            findPlayer();
        }
        else
        {
            vSpeed = Mathf.Lerp(vSpeed, 0, 0.08f);
            hSpeed = Mathf.Lerp(vSpeed, 0, 0.08f);
            if (toPlayer != false)
                toPlayer = false;
        }
    }

    void isLock()
    {
        if (pi.isLock == false) isLocked = false;

        if (isLocked && sr.material != hasLock) sr.material = hasLock;
        if (!isLocked && sr.material != noLock) sr.material = noLock;

    }

    void UpdateAnimation()
    {

        if (!toPlayer && rb.constraints != RigidbodyConstraints.FreezeAll)
            rb.constraints = RigidbodyConstraints.FreezeAll;

        if (toPlayer && rb.constraints != RigidbodyConstraints.FreezeRotation)
            rb.constraints = RigidbodyConstraints.FreezeRotation;


        //attack
        anim.SetInteger("attackID", attackID);
        //


        anim.SetFloat("forward", vSpeed);

        if (toPlayer)
            anim.SetFloat("turn", hSpeed);

        anim.SetBool("seePlayer", toPlayer);
    }

    public void Rotate(Transform player)
    {
        transform.forward =
            Vector3.Lerp(transform.forward, Vector3.Scale((player.position - transform.position), new Vector3(1, 0, 1)), 0.1f);
    }

    public void findPlayer()
    {

        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (angle < (maxAngle * 0.5f))
        {
            toPlayer = true;
            if (Vector3.Distance(transform.position, player.position) > 1.2f)
            {
                vSpeed = Mathf.Lerp(vSpeed, 1, 0.05f);
                hSpeed = Mathf.Lerp(hSpeed, 0, 0.05f);
                if (attackID != 0)   //不然卡在循环出不来
                    attackID = 0;
            }
            else
            {
                vSpeed = Mathf.Lerp(vSpeed, 0, 0.05f);
                fightWithPlayer();
            }
        }

    }


    public void fightWithPlayer()
    {
        enemyTime += Time.deltaTime;
        hSpeed = Mathf.Lerp(hSpeed, enemyTurn, 0.08f);
        if (enemyTime < 5f)
            enemyTurn = 1f;
        else
        {
            enemyTurn = -1f;
            if (enemyTime > 10f)
            {
                enemyTime = 0;
            }
        }

        if (enemyTime > attackTime)
        {
            attackID = Random.Range(1, 4);

        }



        if (attackID != 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("fight"))
        {
            attackTime = Random.Range(1, 10);
            attackID = 0;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            anim.SetTrigger("getHit");
        }
    }


    //动画帧方法
    void FootR()
    {

    }

    void FootL()
    {

    }

    void Hit()
    {
        attack = !attack;
        AttackCollider.enabled = attack;
    }

}
