using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isWallJumping;
    [SerializeField] public bool movementEnabled;

    private float horizontal;
    private Rigidbody rb;

    private float jumpTimeCounter;

    private GameManager gameManager;

    [SerializeField] private Transform groundCheck;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (movementEnabled)
        {
            CheckIfGrounded();
            GetMovementInput();
            Jump();
            AdvancedJumpPhysics();
        }
    }

    private void FixedUpdate()
    {
        //Movement
        Move();

        //Physics
        ControlDrag();
    }

    private void GetMovementInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

    }

    private void Move()
    {
        if (isGrounded)
        {
            if (horizontal > 0)
            {
                rb.AddForce(transform.right.normalized * gameManager.movementMultiplier, ForceMode.Acceleration);
            }
            else if (horizontal < 0)
            {
                rb.AddForce(-transform.right.normalized * gameManager.movementMultiplier, ForceMode.Acceleration);
            }
        }
        else
        {
            if (horizontal > 0)
            {
                rb.AddForce(transform.right.normalized * gameManager.airMovementMultiplier, ForceMode.Acceleration);
            }
            else if (horizontal < 0)
            {
                rb.AddForce(-transform.right.normalized * gameManager.airMovementMultiplier, ForceMode.Acceleration);
            }
        }
    }

    //Wall Jump Method
    private void OnCollisionStay(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (!isGrounded && contact.normal.y < gameManager.minWallJumpAngle)
            {
                if (Input.GetButton("Jump"))
                {
                    rb.velocity = new Vector3(0, 0, 0); //Reset horizontal velocity
                    rb.AddForce(contact.normal * gameManager.wallJumpHorizontalForce, ForceMode.Impulse);
                    rb.AddForce(Vector3.up * gameManager.wallJumpVerticalForce, ForceMode.Impulse);
                    
                    //Jump sound
                    AudioManager.Instance.Play("Jump");
                    break;
                }
                //Debug.DrawRay(contact.point, contact.normal, Color.green, 1.25f);
            }
        }
    }
    
    private void Jump()
    {
        //Add continous force till the maximum duration is reached
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = gameManager.jumpDuration;
            rb.velocity = new Vector3(rb.velocity.x, gameManager.jumpForce, 0);
            
            //Jump sound
            AudioManager.Instance.Play("Jump");
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, gameManager.jumpForce, 0);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    private void ControlDrag()
    {
        //Control ground and air drag separately for smoother falling and sliding.
        
        Vector3 vel = rb.velocity;
        

        if (!isGrounded)
        {
            vel.y *= 1.0f - gameManager.verticalDrag;
            rb.velocity = vel;
            
        }
        else
        {
            vel.x *= 1.0f - gameManager.horizontalDrag;
            rb.velocity = vel;
        }
    }

    private void AdvancedJumpPhysics()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (gameManager.fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && isJumping)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (gameManager.lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    
    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, gameManager.groundedCheckDistance, gameManager.environmentMask);
    }

    public void StartLevelTransition(int doorNumber)
    {
        StartCoroutine(WalkThroughTheDoor(doorNumber));
    }

    private IEnumerator WalkThroughTheDoor(int doorNumber)
    {
        if (doorNumber == 1)
        {
            movementEnabled = false;
            yield return new WaitForSeconds(2f);
            transform.DOMove(new Vector3(-25f, 0.5f, -13.5f), gameManager.levelTransitionDuration / 2);
            yield return new WaitForSeconds(gameManager.levelTransitionDuration / 2);
            transform.DOMove(new Vector3(-29f, 0.5f, -22.5f), gameManager.levelTransitionDuration / 2);
            yield return new WaitForSeconds(gameManager.levelTransitionDuration / 2);
            movementEnabled = true;
        }
        else if (doorNumber == 2)
        {
            movementEnabled = false;
            yield return new WaitForSeconds(2f);
            transform.DOMove(new Vector3(-76f, 0.5f, -15.5f), gameManager.levelTransitionDuration/2);
            yield return new WaitForSeconds(gameManager.levelTransitionDuration / 2);
            transform.DOMove(new Vector3(-80f, 0.5f, -22.5f), gameManager.levelTransitionDuration/2);
            yield return new WaitForSeconds(gameManager.levelTransitionDuration / 2);
            movementEnabled = true;
        }
    }

    public void StartBackgroundMusic()
    {
        AudioManager.Instance.Play("Music");
    }
}
