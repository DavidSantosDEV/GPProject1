using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    VisualsBehaviour myUI;
    ArmorSystem myArmor=null;
    EnemyAnimation myAnim;
    EnemyBase _parent;

    [SerializeField][Range(0,400)]
    private float maxHealth=100;

    private float currentHealth=0;

    [SerializeField][Range(1,short.MaxValue)]
    private short maxBiteCount=2;

    private short biteCount;

    bool isdead = false;


    void Awake()
    {
        _parent = GetComponent<EnemyBase>();
        myArmor = GetComponent<ArmorSystem>();
        myUI = GetComponent<VisualsBehaviour>();
        myAnim = GetComponent<EnemyAnimation>();

        currentHealth = maxHealth;

        myUI.UpdateHealthUI(currentHealth, maxHealth);
    }

    public void TakeDamage(float dmg)
    {
        if (isdead) return;
        float damageTake=dmg;

        if (myArmor)
        {
            damageTake = myArmor.CalculateDamage(dmg);
            if (damageTake == 0) return;
        }

        biteCount++; //its right?
        Debug.Log("Bite count:" + biteCount + " Armor: " + myArmor.Armor);

        currentHealth = Mathf.Clamp(currentHealth - damageTake, 0, maxHealth);

        if (currentHealth == 0) Die();

        if (biteCount == maxBiteCount) _parent.ChangeToBiter();

        myUI.UpdateHealthUI(currentHealth, maxHealth);
    }

    private void Die()
    {
        isdead = true;
        _parent.OnDeath();
        myAnim.SetIsDead(true);
    }

}

