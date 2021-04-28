using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : MonoBehaviour
{
    EnemyBehaviour enemyBehaviour;
    BiterBehaviour biterBehaviour;
    void Start()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        if (!enemyBehaviour) enemyBehaviour = gameObject.AddComponent<EnemyBehaviour>();
        biterBehaviour = GetComponent<BiterBehaviour>();
        if (!biterBehaviour) biterBehaviour = gameObject.AddComponent<BiterBehaviour>();


    }

    public void ChangeToBiter()
    {
        enemyBehaviour.DeActivate();
        biterBehaviour.Activate();

        tag = "Biter";
    }

    public void ChangeToEnemy()
    {
        biterBehaviour.DeActivate();
        enemyBehaviour.Activate();

        tag = "Enemy";
    }
}
