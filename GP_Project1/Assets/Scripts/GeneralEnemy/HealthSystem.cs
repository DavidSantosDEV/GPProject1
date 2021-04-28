using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    UIController myUI;
    ArmorSystem myArmor=null;
    EnemyAnimation myAnim;
    EnemyBase _parent;

    [SerializeField][Range(0,400)]
    private float maxHealth=100;

    private float currentHealth=0;

    void Awake()
    {
        _parent = GetComponent<EnemyBase>();
        myArmor = GetComponent<ArmorSystem>();
        myUI = GetComponent<UIController>();
        myAnim = GetComponent<EnemyAnimation>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        float damageTake=dmg;
        if (myArmor)
        {
            damageTake = myArmor.CalculateDamage(dmg);
            if (damageTake == 0) return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damageTake, 0, maxHealth);

        if (currentHealth == 0) Die();

        myUI.UpdateHealthUI(currentHealth, maxHealth);
    }

    private void Die()
    {
        _parent.OnDeath();
        myAnim.SetIsDead(true);
    }

}

