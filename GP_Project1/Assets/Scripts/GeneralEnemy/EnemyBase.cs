using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
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


    [Header("Movement")]
    [SerializeField]
    private float enemySpeed = 3;
    [SerializeField]
    private float jumpForce = 2, /*Bad game*/ resetJumpTime = 0.5f;
    
    private float originalSpeed;
    private bool isJumping = false;

    protected bool isGrounded = false;

    //Aux var
    private RaycastHit2D[] results = new RaycastHit2D[1];

    public virtual void Activate()
    {
        enabled = true;
        StartMyRoutines();
    }

    public virtual void DeActivate()
    {
        enabled = false;
    }

    protected virtual void StartMyRoutines()
    {
        StartCoroutine(nameof(CheckWallCycle));
    }

    void Awake()
    {
        originalSpeed = enemySpeed;

        myBody = GetComponent<Rigidbody2D>();
        if (!myBody) myBody = gameObject.AddComponent<Rigidbody2D>();
        myAnimMaster = GetComponent<EnemyAnimation>();

        //StartMyRoutines(); Not needed since game manager already does that
    }

    protected virtual void Update()
    {
        enemySpeed = isJumping ? 0 : originalSpeed;
    }

    protected virtual void FixedUpdate()
    {
        myBody.velocity = transform.right * enemySpeed;
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

    protected void FlipCharacter()
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

    protected bool CheckForWall()
    {
        for(int i = 0; i < wallVectorsDetection.Length; i++)
        {
            if (Physics2D.LinecastNonAlloc(wallVectorsDetection[i].position,
                wallVectorsDetection[i].position + wallVectorsDetection[i].right * wallDetectRange,
                results, wallLayer) > 0)
                return true;
        }
        return false;
    }
}
