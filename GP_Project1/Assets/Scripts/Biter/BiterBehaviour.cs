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

    // Start is called before the first frame update
    void Start()
    {
        myAnimationMaster = GetComponent<EnemyAnimation>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActiveAndEnabled)
        {
            if (collision.CompareTag("Enemy"))
            {
                hurtThisGuy = null;
                hurtThisGuy = collision.GetComponent<HealthSystem>();
                if(hurtThisGuy)
                EnactBite();
            }
        }
    }

    private void EnactBite()
    {
        myAnimationMaster.PlayAttackAnimation();
    }

    public void DoBiteEffect()
    {
        hurtThisGuy.TakeDamage(biteDamage);
    }
}
