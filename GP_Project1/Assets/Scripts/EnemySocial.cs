using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySocial : MonoBehaviour
{
    EnemyBase parent;

    [SerializeField]
    private float idleTime;

    private void Start()
    {
        parent = GetComponent<EnemyBase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActiveAndEnabled) return;

        if (collision.CompareTag("Enemy"))
        {
            EnemyBase myFriend = collision.GetComponent<EnemyBase>();
            if (!myFriend.GetIsAlert() && !myFriend.GetIsIdle())
            {
                Debug.Log("Hey, " + collision + ". How are you?");
                parent.SetIdle(idleTime);
                myFriend.SetIdle(idleTime);
            }
        }
    }
}
