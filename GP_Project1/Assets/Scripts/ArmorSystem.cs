using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSystem : MonoBehaviour
{
    [SerializeField]
    [Range(0, float.MaxValue)]
    private float maxArmor;

    private float currentArmor;

    public float Armor
    {
        get => currentArmor;
        private set => currentArmor = value;
    }

    void Awake()
    {
        currentArmor = maxArmor;
    }

    public float CalculateDamage(float dmg)
    {
        if (currentArmor == 0) return dmg;
        if (currentArmor - dmg > 0)
        {
            currentArmor = Mathf.Clamp(currentArmor - dmg, 0, maxArmor);
            return 0;
        }
        else
        {
            float dam = Mathf.Abs(currentArmor - dmg);
            currentArmor = 0;
            return dam;
        }
        //float damageReturn = Mathf.Abs(currentArmor - dmg);
        
        //return damageReturn;
    }

    
}
