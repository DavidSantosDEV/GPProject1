using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    EnemyBehaviour parent;

    [SerializeField]
    private float timeAlert = 10;

    public void SettupParent(EnemyBehaviour eb)
    {
        parent = eb;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Biter"))
        {

            Invoke(nameof(CalmParent), timeAlert);
        }
    }

    private void CalmParent()
    {

    }
}
