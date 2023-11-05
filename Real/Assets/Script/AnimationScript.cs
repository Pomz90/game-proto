using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{

    private Animator anim;
    private Movement move;
    private Collision coll;
    [HideInInspector]
    public SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<Collision>();
        move = GetComponentInParent<Movement>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        anim.SetBool("OnGround", coll.onGround);
        anim.SetBool("CanGrab", move.canGrab);
        // anim.SetBool("Vaulting", coll.IsVault);
        anim.SetBool("IsDashing", move.isDashing);
        anim.SetBool("IsBDashing", move.IsBdash);
        anim.SetBool("IsDashing", move.isDashing);
        anim.SetBool("WallGrab", move.wallGrab);

    }

    public void SetHorizontalMovement(float x,float y)
    {
        anim.SetFloat("HorizontalAxis", Mathf.Abs(x));
        anim.SetFloat("VerticalAxis", y);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void Flip(int side)
    {
        

        bool state = (side == 1) ? false : true;
        if (state)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else 
        
            transform.localScale = new Vector3(1, 1, 1);
        
    }
}
