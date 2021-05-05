using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private Rigidbody2D myBody;
    private EnemyAnimation myAnimMaster;

    [SerializeField]
    private LayerMask enemyLayer, biterLayer;
    [SerializeField][TagSelector]
    private string enemyTag, biterTag;

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
    private float timeBetweenJumpChecks = 2,MaxTimeJumpChecks;
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
    private float originalMaxTimeJumpCheck;
    private float originalSpeed;

    private bool isJumping = false;

    //bool isDead = false;

    [Header("Enemy Fall")]
    bool canFall = false;
    [SerializeField]
    float canFallToggleTime = 20;

    bool isAlert = false;
    bool isIdle = false;

   //bool biter=false;

    Collider2D mycol;

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
        mycol = GetComponent<Collider2D>();

        originalSpeed = enemySpeed;
        originalJumpDetectSpeed = timeBetweenJumpChecks;
        originalMaxTimeJumpCheck = MaxTimeJumpChecks;

        myBody = GetComponent<Rigidbody2D>();
        if (!myBody) myBody = gameObject.AddComponent<Rigidbody2D>();
        myAnimMaster = GetComponent<EnemyAnimation>();

        myEnemySocial = GetComponent<EnemySocial>();
        if (!myEnemySocial) myEnemySocial = gameObject.AddComponent<EnemySocial>();
        myAlertSystem = GetComponent<AlertSystem>();
        if (!myAlertSystem) gameObject.AddComponent<AlertSystem>();

        myBiterBehaviour = GetComponent<BiterBehaviour>();
        if (!myBiterBehaviour) myBiterBehaviour = gameObject.AddComponent<BiterBehaviour>();

        InvokeRepeating(nameof(ToggleFall), canFallToggleTime, canFallToggleTime);
        StartMyRoutines();
    }

    void ToggleFall()
    {
        canFall = !canFall;
    }

    private void FixedUpdate()
    {
        float speed= isJumping || isIdle ? 0 : enemySpeed;


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
                    
                    //Debug.Log("WALL");
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
            yield return new WaitForSeconds(Random.Range(timeBetweenJumpChecks,MaxTimeJumpChecks));
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
        GameManager.Instance.EnemyDied(this,myAlertSystem);
        //enabled = false;
    }

    #region Alert Stuff

    public void SetAlert(float SpeedIncrease, float newTimeIntervalJump, float newMaxTimeInterval)
    {
        FlipCharacter();
        SetSpeed(SpeedIncrease);
        SetTimeBetweenChecks(newTimeIntervalJump,newMaxTimeInterval);
        isAlert = true;

        if (isIdle)
        {
            CancelInvoke(nameof(ReturnRoutine));
            StartCoroutine(nameof(CheckForJump));
            isIdle = false;
        }
    }

    public void SetNormal()
    {
        ReturnOriginalSpeed();
        ReturnOriginalCheckTime();
        isAlert = false;
    }

    public bool GetIsAlert()
    {
        return isAlert;
    }

    public bool GetIsIdle()
    {
        return isIdle;
    }

    private void SetTimeBetweenChecks(float newTime, float maxtime)
    {
        timeBetweenJumpChecks = newTime;
        MaxTimeJumpChecks = maxtime;
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
        MaxTimeJumpChecks = originalMaxTimeJumpCheck;
    }

    #endregion

    public void SetIdle(float time)
    {
        isIdle = true;

        StopCoroutine(nameof(CheckForJump));

        Invoke(nameof(ReturnRoutine), time);
    }

    private void ReturnRoutine()
    {
        FlipCharacter();
        StartCoroutine(nameof(CheckForJump));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (tag == biterTag) return;
        if (collision.gameObject.tag == enemyTag)
        {
            Physics2D.IgnoreCollision(collision.collider, mycol);
        }
    }

    #region Change Mode

    public void ChangeToBiter()
    {
        myAnimMaster.ChangeToBiter();

        myBiterBehaviour.enabled = true;

        myAlertSystem.enabled = false;
        myEnemySocial.enabled = false;

        tag = biterTag;
        gameObject.layer = LayerMask.NameToLayer("Biter");//biterLayer.ToString()); 
    }
    //Just to let know, unity just doesnt let me use the layer var AND the Layer.ToString together with nametoLayer
    public void ChangeToNormal()
    {
        myAnimMaster.ChangeToNormal();

        myBiterBehaviour.enabled = false;

        myAlertSystem.enabled = true;
        myEnemySocial.enabled = true;

        tag = enemyTag;
        gameObject.layer = LayerMask.NameToLayer("Enemy");//enemyLayer.ToString());// LayerMask.NameToLayer( enemyLayer); doesnt work cause UNITAY
    }

    #endregion
}
