using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EnemyBase
{
    private AlertSystem myAlertSystem;
    private EnemySocial myEnemySocial;

    [SerializeField]
    private float timeBetweenJumpChecks = 2;

    bool isAlert=false;

    bool canFall = false;
    float canFallToggleTime = 20;

    
    void Start()
    {
        myEnemySocial = GetComponent<EnemySocial>();
        if (!myEnemySocial) myEnemySocial = gameObject.AddComponent<EnemySocial>();
        myAlertSystem = GetComponent<AlertSystem>();
        if (!myAlertSystem) gameObject.AddComponent<AlertSystem>();

        InvokeRepeating(nameof(ToggleFall), canFallToggleTime, canFallToggleTime);
    }

    void ToggleFall()
    {
        canFall = !canFall;
    }

    public override void Activate()
    {
        base.Activate();
        myAlertSystem.enabled = true;
        myEnemySocial.enabled = true;
    }

    public override void DeActivate()
    {
        base.DeActivate();
        myAlertSystem.enabled = false;
        myEnemySocial.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void StartMyRoutines()
    {
        base.StartMyRoutines();
        StartCoroutine(nameof(CheckForJump));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(CheckForWall() || (!CheckForPlatforms() && !canFall))
        {
            FlipCharacter();
        }
    }

    private bool CheckForPlatforms()
    {
        return false;
    }

    public void SetAlertState()
    {
        isAlert = true;
    }

    private IEnumerator CheckForJump()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(timeBetweenJumpChecks);

            if (isGrounded)
            {
                Jump();
            }

        }   
    }
}
