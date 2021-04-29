using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;

    private SpriteRenderer _spriteRenderer=null;

    [SerializeField]
    private Color biterColor= Color.red;

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
        _spriteRenderer.color = biterColor;
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
