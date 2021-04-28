using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private Rigidbody2D myBody;
    private EnemyAnimation myAnimMaster;

    [Header("Wall detection")]
    [SerializeField]
    private Transform[] wallVectorsDetection;
    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private float wallDetectRange=1;

    [Header("Ground Detection")]
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Transform[] feetTransforms = null;
    private Collider2D[] groundCheckColliders = new Collider2D[1];

    private bool isGrounded = false;


    [Header("Jumping Detection")]
    [SerializeField]
    private Transform jumpVectorDetection = null;
    [SerializeField]
    private float jumpDetectionRange = 1f;
    [SerializeField]
    private LayerMask jumpDetectionLayerMask;
    [SerializeField]
    private float timeBetweenJumpChecks = 2;
    [SerializeField]
    private float jumpForce = 2, /*Bad game*/ resetJumpTime = 0.5f;

    [Header("Platform detection")]
    [SerializeField]
    private Transform platformsEdgeVectorDetection = null;
    [SerializeField]
    private float platformsEdgeDetectionRange = 1f;

    [Header("Movement")]
    [SerializeField]
    private float enemySpeed = 3;

    //Original Values
    private float originalJumpDetectSpeed;
    private float originalSpeed;

    private bool isJumping = false;

    //bool isDead = false;

    [Header("Enemy Fall")]
    bool canFall = false;
    [SerializeField]
    float canFallToggleTime = 20;

    private AlertSystem myAlertSystem;
    private EnemySocial myEnemySocial;
    private BiterBehaviour myBiterBehaviour;

    //Aux var
    private RaycastHit2D[] results = new RaycastHit2D[1];

    private void StartMyRoutines()
    {
        //StartCoroutine(nameof(CheckWallCycle));
        StartCoroutine(nameof(CheckForJump));
        StartCoroutine(nameof(CheckForWall));
    }

    void Awake()
    {
        originalSpeed = enemySpeed;
        originalJumpDetectSpeed = timeBetweenJumpChecks;

        myBody = GetComponent<Rigidbody2D>();
        if (!myBody) myBody = gameObject.AddComponent<Rigidbody2D>();
        myAnimMaster = GetComponent<EnemyAnimation>();

        myEnemySocial = GetComponent<EnemySocial>();
        if (!myEnemySocial) myEnemySocial = gameObject.AddComponent<EnemySocial>();
        myAlertSystem = GetComponent<AlertSystem>();
        if (!myAlertSystem) gameObject.AddComponent<AlertSystem>();

        InvokeRepeating(nameof(ToggleFall), canFallToggleTime, canFallToggleTime);
        StartMyRoutines();
    }

    void ToggleFall()
    {
        canFall = !canFall;
    }

    private void FixedUpdate()
    {
        float speed= isJumping ? 0 : enemySpeed;


        myBody.velocity = new Vector2(transform.right.x * speed, myBody.velocity.y);
        isGrounded = CheckForGround();
        if ((!CheckForPlatforms() && !canFall) && Mathf.Abs(myBody.velocity.y)<0.01f)
        {
            FlipCharacter();
        }
        
        myAnimMaster.SetIsGrounded(isGrounded);
        myAnimMaster.UpdateMovement(myBody.velocity.x);
    }

   

    private void FlipCharacter()
    {
        Vector3 target = transform.localEulerAngles;
        target.y += 180;
        transform.localEulerAngles = target;
        //transform.localEulerAngles = new Vector3( transform.rotation.x,transform.rotation.y+180,transform.rotation.z);
    }

    private void Jump() //Should Biters jump??
    {
        if (!isJumping && !canFall)
        {
            isJumping = true;
            myBody.AddForce(Vector2.up * jumpForce);
            Invoke(nameof(ResetJump), resetJumpTime);
        }
        
    }

    private void ResetJump()
    {
        isJumping = false;
    }

    #region Checks

    private bool CheckForPlatforms()
    {
        Debug.DrawLine(platformsEdgeVectorDetection.position,
                platformsEdgeVectorDetection.position + platformsEdgeVectorDetection.right * platformsEdgeDetectionRange);

        return Physics2D.LinecastNonAlloc(
                platformsEdgeVectorDetection.position,
                platformsEdgeVectorDetection.position + platformsEdgeVectorDetection.right * platformsEdgeDetectionRange,
                results,
                groundLayer) > 0;
    }

    private IEnumerator CheckForWall()
    {
        while (isActiveAndEnabled)
        {
            for (int i = 0; i < wallVectorsDetection.Length; i++)
            {
                Debug.DrawLine(wallVectorsDetection[i].position,
                    wallVectorsDetection[i].position + wallVectorsDetection[i].right * wallDetectRange);

                if (Physics2D.LinecastNonAlloc(wallVectorsDetection[i].position,
                    wallVectorsDetection[i].position + wallVectorsDetection[i].right * wallDetectRange,
                    results, wallLayer) > 0)
                {
                    //FlipCharacter();
                    
                    Debug.Log("WALL");
                    FlipCharacter();
                }
            }
            
            yield return new WaitForSeconds(0.3f);
        }
        
    }


    private IEnumerator CheckForJump()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(timeBetweenJumpChecks);
            Debug.DrawLine(jumpVectorDetection.position,
                jumpVectorDetection.position + jumpVectorDetection.up * jumpDetectionRange,Color.red,3);
            if (isGrounded && Physics2D.LinecastNonAlloc(jumpVectorDetection.position,
                jumpVectorDetection.position + jumpVectorDetection.up * jumpDetectionRange, results, jumpDetectionLayerMask) > 0)
            {
                Debug.Log("Wanna jump");
                Jump();
            }

        }
    }

    private bool CheckForGround()
    {
        if (myBody.velocity.y > 0.01f) return false;
        for(int i=0; i < feetTransforms.Length; i++)
        {
            if (Physics2D.OverlapPointNonAlloc(feetTransforms[i].position, groundCheckColliders, groundLayer) > 0)
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    public void OnDeath()
    {
        enabled = false;
        //isDead = true;
    }

    #region Alert Stuff

    public void SetAlert(float SpeedIncrease, float newTimeIntervalJump)
    {
        FlipCharacter();
        SetSpeed(SpeedIncrease);
        SetTimeBetweenChecks(newTimeIntervalJump);
    }

    public void SetNormal()
    {
        ReturnOriginalSpeed();
        ReturnOriginalCheckTime();
    }

    private void SetTimeBetweenChecks(float newTime)
    {
        timeBetweenJumpChecks = newTime;
    }

    private void SetSpeed(float newSpeed)
    {
        enemySpeed = newSpeed;
    }
    private void ReturnOriginalSpeed()
    {
        enemySpeed = originalSpeed;
    }

    private void ReturnOriginalCheckTime()
    {
        timeBetweenJumpChecks = originalJumpDetectSpeed;
    }

    #endregion

    #region Change Mode

    public void ChangeToBiter()
    {
        myAnimMaster.ChangeToBiter();
        tag = "Biter";
    }

    public void ChangeToNormal()
    {
        myAnimMaster.ChangeToNormal();
        tag = "Enemy";
    }

    #endregion
}
