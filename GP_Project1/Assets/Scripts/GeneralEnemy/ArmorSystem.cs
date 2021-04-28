using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSystem : MonoBehaviour
{
    [SerializeField]
    [Range(0, float.MaxValue)]
    private float maxArmor;

    private float currentArmor;

    void Awake()
    {
        currentArmor = maxArmor;
    }

    public float CalculateDamage(float dmg)
    {
        if (currentArmor == 0) return dmg;
        float damageReturn = Mathf.Abs(currentArmor - dmg);
        currentArmor =Mathf.Clamp(currentArmor - dmg,0,maxArmor);
        return damageReturn;
    }

    
}
