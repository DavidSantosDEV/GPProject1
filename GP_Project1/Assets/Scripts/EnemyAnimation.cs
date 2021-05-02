using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;

    private SpriteRenderer _spriteRenderer=null;

    //[SerializeField]
    private Color biterColor= Color.red; //Unity is having a problem with custom colors for some reason, i can't use x.color = color var, i need to use x.color =new color(etc etc)

    //Hashed id
    private int attackID;
    private int horizontalMovementID;
    private int groundedAnimationID;
    private int deadAnimationID;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        attackID = Animator.StringToHash("Attack");
        horizontalMovementID = Animator.StringToHash("HorizontalMovement");
        groundedAnimationID = Animator.StringToHash("Grounded");
        deadAnimationID = Animator.StringToHash("Dead");
    }

    public void ChangeToNormal()
    {
        _spriteRenderer.color = Color.white;
    }

    public void ChangeToBiter()
    {
        _spriteRenderer.color = new Color(biterColor.r, biterColor.g, biterColor.b);
    }

    public void SetIsGrounded(bool val)
    {
        _animator.SetBool(groundedAnimationID,val);
    }

    public void UpdateMovement(float horizontal)
    {
        //Debug.Log(horizontal);
        _animator.SetFloat(horizontalMovementID, Mathf.Abs(horizontal));
    }

    public void SetIsDead(bool val)
    {
        _animator.SetBool(deadAnimationID, val);
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(attackID);
    }
}
