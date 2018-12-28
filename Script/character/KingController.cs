using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : DancerController
{

    public override string changeWeaponName()
    {
        return "Spear";
    }

    public override void UpdateAnimation()
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

        if (pi.isFight)
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
                randomID = Random.Range(1, 8);
                anim.SetInteger("attackID", randomID);
                anim.SetTrigger("attack");
            }


            anim.SetBool("block", pi.isBlock);
            //
        }


      

        if (pi.isActive)
            anim.SetTrigger("playerActive");
        //


        //in Air
        anim.SetFloat("fall", rb.velocity.y);
    }


    


    //以下动画帧方法
    void WeaponSwitch()
    {
        prefabWeapon.SetActive(!pi.isFight);
        fightWeapon.SetActive(pi.isFight);
    }

    void Hit()
    {
        attack = !attack;
        trail.Emit = attack;
        weaponCollider.enabled = attack;
    }

}
