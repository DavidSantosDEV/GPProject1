using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiterBehaviour : MonoBehaviour
{
    EnemyAnimation myAnimationMaster;

    //I kinda wanna do this this way but kinda don't
    HealthSystem hurtThisGuy;

    [SerializeField]
    private float biteDamage = 20;

    [SerializeField]
    private float biteCoolDown=2;

    bool canBite = true;

    // Start is called before the first frame update
    void Start()
    {
        myAnimationMaster = GetComponent<EnemyAnimation>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        AttackRoutine(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AttackRoutine(collision);
    }

    private void AttackRoutine(Collider2D collision)
    {
        if (isActiveAndEnabled)
        {
            if (collision.CompareTag("Enemy") && canBite)
            {
                //Debug.Log("I wanna bite");
                hurtThisGuy = null;
                hurtThisGuy = collision.GetComponent<HealthSystem>();
                if (hurtThisGuy)
                    EnactBite();
                canBite = false;
                Invoke(nameof(ResetBite), biteCoolDown);
            }
        }
    }

    private void ResetBite()
    {
        canBite = true;
    }

    private void EnactBite()
    {
        myAnimationMaster.PlayAttackAnimation();
        //DoBiteEffect();
    }

    public void DoBiteEffect()
    {
        hurtThisGuy.TakeDamage(biteDamage);
    }
}
