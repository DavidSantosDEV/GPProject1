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

    [SerializeField][Range(1,short.MaxValue)]
    private short maxBiteCount=2;

    private short biteCount;


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

        biteCount++; //its right?

        currentHealth = Mathf.Clamp(currentHealth - damageTake, 0, maxHealth);

        if (currentHealth == 0) Die();

        if (biteCount == maxBiteCount) _parent.ChangeToBiter();

        myUI.UpdateHealthUI(currentHealth, maxHealth);
    }

    private void Die()
    {
        _parent.OnDeath();
        myAnim.SetIsDead(true);
    }

}

