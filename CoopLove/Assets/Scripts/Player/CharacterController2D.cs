using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    // Better Jumping
    [SerializeField] private float m_fallMultiplier = 2.5f;

    [SerializeField] private float m_lowJumpMultiplier = 2f;

    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround = 0;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck = null;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck = null;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider = null;                // A collider that will be disabled when crouching

    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private bool m_Grounded;            // Whether or not the player is grounded.
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public bool UseBoxColliderCheck = true;

    [SerializeField] private float m_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    [SerializeField] private float m_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded

    // Box
    [SerializeField] private float m_GroundedRadiusBoxX = .2f; // Radius of the overlap circle to determine if the player can stand up

    [SerializeField] private float m_GroundedRadiusBoxY = .2f; // Radius of the overlap circle to determine if grounded

    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;
    private Vector2 m_PreviousPosition;
    private Vector2 m_CurrentPosition;
    private Vector2 m_NextMovement = Vector2.zero;
    private Vector2 Velocity;

    private void Awake()
    {
        if (m_GroundCheck == null || m_CeilingCheck == null)
            Debug.LogError("Ground or ceiling check null!");

        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void OnDrawGizmos()
    {
        // Draw ceiling check
        Gizmos.DrawWireSphere(m_CeilingCheck.position, m_CeilingRadius);

        // Draw ground check
        if (!UseBoxColliderCheck)
            Gizmos.DrawWireSphere(m_GroundCheck.position, m_GroundedRadius);
        else
            Gizmos.DrawWireCube(m_GroundCheck.position, new Vector3(m_GroundedRadiusBoxX, m_GroundedRadiusBoxY, 0));
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders;
        if (UseBoxColliderCheck)
            colliders = Physics2D.OverlapBoxAll(m_GroundCheck.position, new Vector2(m_GroundedRadiusBoxX, m_GroundedRadiusBoxY), m_WhatIsGround);
        else
            colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, m_GroundedRadius, m_WhatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump, bool jumpButtonPressed)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, m_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }

        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }

        // Better Jumping
        // faster fall
        if (m_Rigidbody2D.velocity.y < 0)
        {
            // -1 to account for Unity Gravity system
            m_Rigidbody2D.gravityScale = m_fallMultiplier;
        }
        else if (m_Rigidbody2D.velocity.y > 0 && !jumpButtonPressed)
        {
            m_Rigidbody2D.gravityScale = m_lowJumpMultiplier;
        }
        else
        {
            m_Rigidbody2D.gravityScale = 1f;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}