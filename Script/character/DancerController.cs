using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class DancerController : MonoBehaviour
{
    private GameObject[] weapon;

    public AudioClip getHitSound1;
    public AudioClip getHitSound2;

    private GameObject lockTarget;   //锁定的敌人

    //public Transform attackPoint;   //敌人处决位置

    private Enemy enemy;        //敌人脚本

    protected Rigidbody rb;       //角色刚体

    private Transform camTran;  //主相机

    protected Animator anim;      //动画状态机

    protected PlayInput pi;       //角色按键脚本

    private Vector3 forward;    //前方向

    private CapsuleCollider cc;     //自身胶囊碰撞器

    protected GameObject prefabWeapon;     //预设武器
    protected GameObject fightWeapon;       //战斗武器  

    protected BoxCollider weaponCollider;  //武器的碰撞器
    public bool attack;

    protected bool changeRotation;        //可以旋转

    protected float move;         //角色行走

    private float groundCheckDistance = 0.3f;      //地面检测距离

    private float animWeight;           //动画权重

    private AudioSource au;

    protected int randomID;

    private float walkValue = 1f;

    protected MeleeWeaponTrail trail;

    public Vector3 te = new Vector3(0,-0.05f,0.05f);

    // Use this for initialization
    void Awake()
    {
        anim = this.GetComponent<Animator>();
        pi = this.GetComponent<PlayInput>();
        rb = this.GetComponent<Rigidbody>();
        cc = this.GetComponent<CapsuleCollider>();

        cc.center = new Vector3(0, 1, 0);
        cc.radius = 0.35f;
        cc.height = 2.0f;

        au = this.GetComponent<AudioSource>();
    }

    private void Start()
    {
        camTran = Camera.main.transform;

        weapon = GameObject.FindGameObjectsWithTag("Weapon");

        

        foreach (GameObject g in weapon)
        {
            if (g.name == "Fight" + changeWeaponName())
                fightWeapon = g;

            else if (g.name == "Prefab" + changeWeaponName())
                prefabWeapon = g;

            //else if (g.name == "Scabbard")
            //    g.SetActive(pi.theRole == pi.dancer);

        }

        trail = fightWeapon.GetComponent<MeleeWeaponTrail>();

        trail.Emit = false;

        prefabWeapon.SetActive(true);
        fightWeapon.SetActive(false);

        weaponCollider = fightWeapon.GetComponent<BoxCollider>();

        // weaponCollider.enabled = false;

        changeRotation = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public virtual string changeWeaponName()
    {
        return "Blade";
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (pi.isFight)
            pi.isLock = Lock();
        else
            pi.isLock = false;

        //IKLock();

        Movement(pi.vSpeed, pi.hSpeed);  //角色行走

        if (pi.vSpeed != 0 || pi.hSpeed != 0)
            Rotating(forward);

        //HandleJump();

        CheckGroundStatus();

        FightState();

        UpdateAnimation();          //更新动画

        // if(attack){
            // Vector3 po = transform.position + Vector3.forward * 0.5f + Vector3.up * 0.5f;
            // Collider[] cols = Physics.OverlapSphere(po,0.6f,LayerMask.GetMask("enemy"));
            // if(cols.Length>0)
            //     Instantiate(effect,cols[0].transform);


        // }
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Hand"){
            anim.SetTrigger("getHit");
        }
    }

    void HandleJump()
    {

        if (pi.isGround && !pi.isFight)
        {
            //检测是否能跳
            //如果是jump为ture且是行走混合树，则施加一个力
            HandleGroundedMovement(pi.isJump);
        }
        else
        {
            //在空中施加重力
            HandleAirborneMovement();
        }
    }

    
    void FightState()   //战斗状态
    {
        if (pi.isFight)
        {
            animWeight = 1.0f;
            anim.SetLayerWeight(anim.GetLayerIndex("attack layer"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack layer")), animWeight, 0.4f));
        }
        else
        {
            animWeight = 0f;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("empty"))
                anim.SetLayerWeight(anim.GetLayerIndex("attack layer"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack layer")), animWeight, 0.4f));
        }
    }


    void Rotating(Vector3 targetDirection)  //旋转角度
    {
        if (!pi.isLock && changeRotation)
        {
            //Vector3 targetDirection = new Vector3(horizontal, 0, vertical);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            Quaternion newRotation = Quaternion.Lerp(rb.rotation, targetRotation, 15f * Time.fixedDeltaTime);
            rb.MoveRotation(newRotation);
        }
        else
        {
            transform.forward =
                Vector3.Lerp(transform.forward, Vector3.Scale((lockTarget.transform.position - transform.position), new Vector3(1, 0, 1)), 0.1f);
        }

    }

    void Movement(float vertical, float horizontal)     //移动
    {
        if (pi.isLock)
        {
            move = pi.vSpeed;
            changeRotation = false;
        }
        else
        {
            move = Mathf.Sqrt((vertical * vertical) + (horizontal * horizontal));
            changeRotation = true;
        }

        forward = Vector3.Scale((vertical * camTran.forward + horizontal * camTran.right), new Vector3(1, 0, 1));

        if (!pi.isWalk)
        {
            walkValue = Mathf.Lerp(walkValue, 1f, 0.1f);
        }
        else
        {
            walkValue = Mathf.Lerp(walkValue, 0.5f, 0.1f);
        }
    }


    void CheckGroundStatus()        //检测与地面距离
    {
        RaycastHit hitInfo;

#if UNITY_EDITOR

        // 用来在场景视图中辅助想象地面检查射线

        //在场景中显示地面检查线，从脚上0.1米处往下射m_GroundCheckDistance的距离，预制体默认是0.3  

        //Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));

#endif

        // 0.1的射线是比较小的，基础包中预制体所设置的0.3是比较好的  

        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))

        {//射到了，保存法向量，改变变量，将动画的applyRootMotion置为true，true的含义是应用骨骼节点的位移

            //就是说动画的运动会对实际角色坐标产生影响，用于精确的播放动画  
            pi.isGround = true;//是否在地面上
        }
        else
        {
            pi.isGround = false;//是否在地面上为false
        }
    }

    void HandleGroundedMovement(bool jump)
    {
        //检查是否允许跳条件是正确的

        //m_Animator.GetCurrentAnimatorStateInfo(0)获取当前动画状态信息

        if (jump && anim.GetCurrentAnimatorStateInfo(0).IsName("locomoment"))
        {
            // 跳!
            rb.velocity = new Vector3(rb.velocity.x, pi.jumpPower, rb.velocity.z);

            //保存x、z轴速度，并给以y轴向上的速度

            pi.isGround = false;//是否跳跃为false

        }

    }


    void HandleAirborneMovement()
    {

        //根据乘子引用额外的重力

        Vector3 extraGravityForce = (Physics.gravity * pi.gravityMultiplier) - Physics.gravity;

        rb.AddForce(extraGravityForce);

        //groundCheckDistance = rb.velocity.y < 0 ? 0.3f : 0.01f;
        //上升的时候不判断是否在地面上  

    }


    public virtual void UpdateAnimation()
    {
        //move
        anim.SetFloat("vertical", move);
        //

        //jump
        if (pi.isJump)
        {
            anim.SetTrigger("jump");
        }
        //

        //state
        anim.SetBool("isGround", pi.isGround);

        anim.SetBool("isFight", pi.isFight);

        if (!pi.isFight)
        {
            anim.SetBool("isDance", pi.isDance);

            //bow
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bow"))
            {
                changeRotation = false;
                pi.inputEnable = false;
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                {
                    pi.inputEnable = true;
                    changeRotation = true;
                }
            }


            //
        }
        else
        {
            //Lock State
            if (pi.isLock)
                anim.SetFloat("horizontal", pi.hSpeed);
            else
            {
                anim.SetBool("isRun", pi.isRun && anim.GetFloat("vertical") > 0.3f);
                anim.SetFloat("horizontal", 0);
            }
            //

            //attack
            if (pi.isLightAttack)
            {
                anim.SetTrigger("lightAttack");
            }

            if (pi.isStrongAttack)
            {
                anim.SetTrigger("strongAttack");
            }
            //
        }




        if (pi.isActive)
            anim.SetTrigger("playerActive");
        //



        //in Air
        anim.SetFloat("fall", rb.velocity.y);


    }



    bool Lock()
    {
        Collider[] cols = new Collider[0];
        if (Input.GetKeyDown(pi.keyLock))
        {
            Vector3 boxCenter = camTran.position + camTran.forward * 5.0f + new Vector3(0, 1, 0);
            cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), camTran.rotation, LayerMask.GetMask("enemy"));
            
            if (cols.Length == 0)
            {
                lockTarget = null;
                enemy = null;
            }

            else
            {
                foreach (var col in cols)
                {
                    if (lockTarget == col.gameObject)
                    {
                        if (enemy != null) enemy.isLocked = false;
                        lockTarget = null;
                        enemy = null;
                    }
                    else
                    {
                        lockTarget = col.gameObject;

                        if (enemy == null)
                        {
                            enemy = lockTarget.GetComponent<Enemy>();

                        }
                        else
                        {
                            //此时cs不为null，但却不是自己的cs
                            if (enemy != lockTarget.GetComponent<Enemy>())
                            {
                                enemy = lockTarget.GetComponent<Enemy>();
                            }
                        }

                        enemy.isLocked = true;
                    }
                    break;
                }

            }
        }

        return lockTarget != null && pi.isFight;
    }

    //void IKLock()
    //{
    //        RaycastHit hit;
    //        if(Physics.Raycast(transform.position+Vector3.up * 0.3f,transform.forward,out hit, 3f))
    //        {
    //            if(hit.collider.tag == "enemy" && pi.isFight)
    //            {
    //                if(ee == null && ee != hit.collider.gameObject.GetComponent<Enemy>())
    //                    ee = hit.collider.gameObject.GetComponent<Enemy>();

    //                pi.isKill = true;

    //                if (Input.GetKeyDown(KeyCode.Alpha1))
    //                {
    //                    attackPoint = ee.AttackPoint;
    //                    transform.position = attackPoint.position;
    //                    transform.forward =
    //                        Vector3.Scale((ee.enemyTrans.transform.position - transform.position), new Vector3(1, 0, 1));
    //                }

    //            }
    //            else
    //            {
    //                pi.isKill = false;
    //            }
    //        }

    //}

    // public void OnTriggerEnter(Collider other)
    // {
    //     if (other.tag == "Hand")
    //     {
    //         anim.SetTrigger("getHit");
    //         randomID = Random.Range(1, 3);
    //         switch (randomID)
    //         {
    //             case 1:
    //                 au.PlayOneShot(getHitSound1);
    //                 break;
    //             case 2:
    //                 au.PlayOneShot(getHitSound2);
    //                 break;
    //         }

    //     }
    // }

    

    //以下都是动画帧方法

    void HandleWeapon()
    {
        prefabWeapon.SetActive(!pi.isFight);
        fightWeapon.SetActive(pi.isFight);
    }


    void showCollider()
    {
        rb.constraints = ~RigidbodyConstraints.FreezePosition;
        cc.enabled = true;
    }


    void hideCollider()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        cc.enabled = false;
    }

    void hit()
    {
        attack = !attack;
        trail.Emit = attack;
        // weaponCollider.enabled = attack;
    }

    void FootR()
    {

    }

    void FootL()
    {

    }

    void Land()
    {

    }
}
