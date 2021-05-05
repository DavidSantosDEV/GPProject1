using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    EnemyBase _parent;

    [SerializeField]
    private float timeAlert = 10;

    bool isAlert=false;

    [SerializeField]
    private float alertSpeed = 6, alertJumpCheckTime = 0.4f, alertJumpCheckTimeMax =1;

    private void Start()
    {
        _parent = GetComponent<EnemyBase>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        AlertRoutine(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AlertRoutine(collision);
    }

    private void AlertRoutine(Collider2D collision)
    {
        if (!isActiveAndEnabled) return;
        if (collision.CompareTag("Biter"))
        {
            //Debug.Log(isAlert + " Its a biter!");
            if (isAlert)
            {
                CancelInvoke(nameof(CalmParent));
                Invoke(nameof(CalmParent), timeAlert); //Extending the alert time;
            }
            else
            {
                AlertParent();
                Invoke(nameof(CalmParent), timeAlert);
            }

        }
    }

    private void CalmParent()
    {
        isAlert = false;
        _parent.SetNormal();
        //_parent.ReturnOriginalSpeed();
    }

    private void AlertParent()
    {
        isAlert = true;
        _parent.SetAlert(alertSpeed, alertJumpCheckTime,alertJumpCheckTimeMax);
    }

    public void CancelWorkings()
    {
        CancelInvoke(nameof(CalmParent));
        CalmParent();
    }
}
