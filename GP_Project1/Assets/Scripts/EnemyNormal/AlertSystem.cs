using UnityEngine;

public class AlertSystem : MonoBehaviour
{
    EnemyBase _parent;

    [SerializeField]
    private float timeAlert = 10;

    bool isAlert=false;

    [SerializeField]
    private float alertSpeed = 4, alertJumpCheckTime = 1;

    private void Awake()
    {
        _parent = GetComponent<EnemyBase>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActiveAndEnabled) return;
        if(collision.CompareTag("Biter"))
        {
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
        _parent.SetAlert(alertSpeed, alertJumpCheckTime);
    }

    public void CancelWorkings()
    {
        CancelInvoke(nameof(CalmParent));
    }
}
