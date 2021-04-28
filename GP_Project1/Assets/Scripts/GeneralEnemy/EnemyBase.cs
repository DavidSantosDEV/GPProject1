using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour //TODO CHANGE ALL PROTECTED TO PRIVATE
{
    protected Rigidbody2D myBody;
    protected EnemyAnimation myAnimMaster;

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

    protected bool isGrounded = false;


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


    [Header("Movement")]
    [SerializeField]
    private float enemySpeed = 3;
    
    
    private float originalSpeed;
    private bool isJumping = false;

    //bool isDead = false;

    bool canFall = false;
    float canFallToggleTime = 20;

    private AlertSystem myAlertSystem;
    private EnemySocial myEnemySocial;
    private BiterBehaviour myBiterBehaviour;

    //Aux var
    private RaycastHit2D[] results = new RaycastHit2D[1];

    protected virtual void StartMyRoutines()
    {
        StartCoroutine(nameof(CheckWallCycle));
        StartCoroutine(nameof(CheckForJump));
    }

    void Awake()
    {
        originalSpeed = enemySpeed;

        myBody = GetComponent<Rigidbody2D>();
        if (!myBody) myBody = gameObject.AddComponent<Rigidbody2D>();
        myAnimMaster = GetComponent<EnemyAnimation>();

        myEnemySocial = GetComponent<EnemySocial>();
        if (!myEnemySocial) myEnemySocial = gameObject.AddComponent<EnemySocial>();
        myAlertSystem = GetComponent<AlertSystem>();
        if (!myAlertSystem) gameObject.AddComponent<AlertSystem>();

        InvokeRepeating(nameof(ToggleFall), canFallToggleTime, canFallToggleTime);
        StartMyRoutines();
        //StartMyRoutines(); Not needed since game manager already does that
    }

    void ToggleFall()
    {
        canFall = !canFall;
    }

    private void Update()
    {
        
        enemySpeed = isJumping ? 0 : originalSpeed;
    }

    private void FixedUpdate()
    {

        myBody.velocity = transform.right * enemySpeed;
        isGrounded = CheckForGround();
        if (isGrounded)
        {
            if (CheckForWall() || (!CheckForPlatforms() && !canFall))
            {
                FlipCharacter();
            }
        }
        myAnimMaster.SetIsGrounded(isGrounded);
        myAnimMaster.UpdateMovement(myBody.velocity.x);
    }

   

    private void FlipCharacter()
    {
        transform.localEulerAngles = new Vector3( transform.rotation.x,transform.rotation.y+180,transform.rotation.z);
    }

    protected void Jump() //Should Biters jump??
    {
        if (!isJumping)
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

        return false;
    }

    private IEnumerator CheckWallCycle()
    {
        while (isActiveAndEnabled)
        {
            bool hitWall = false;

            if (hitWall) FlipCharacter();
            yield return new WaitForSeconds(0.1f);
        }
    }
    protected bool CheckForWall()
    {
        for (int i = 0; i < wallVectorsDetection.Length; i++)
        {
            Debug.DrawLine(wallVectorsDetection[i].position,
                wallVectorsDetection[i].position + wallVectorsDetection[i].right * wallDetectRange);

            if (Physics2D.LinecastNonAlloc(wallVectorsDetection[i].position,
                wallVectorsDetection[i].position + wallVectorsDetection[i].right * wallDetectRange,
                results, wallLayer) > 0)
                return true;
        }
        return false;
    }


    private IEnumerator CheckForJump()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(timeBetweenJumpChecks);

            if (isGrounded && Physics2D.LinecastNonAlloc(jumpVectorDetection.position,
                jumpVectorDetection.position + jumpVectorDetection.right * jumpDetectionRange, results, jumpDetectionLayerMask) > 0)
            {
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
