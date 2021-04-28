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


    private int groundedAnimationID;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        groundedAnimationID = Animator.StringToHash("Grounded");
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

}
