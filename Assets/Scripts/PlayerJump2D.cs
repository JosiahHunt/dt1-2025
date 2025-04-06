using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump2D : MonoBehaviour
{
    [Header("The key that makes the player jump.")]
    public KeyCode JumpKey = KeyCode.Space;

    [Header("How much force to apply when the player jumps.")]
    [Range(0, 20)]
    public float JumpForce = 2f;

    [Header("Fired when the player jumps.")]
    public UnityEvent OnJump = new UnityEvent();

    [Header("Fired while the player is falling.")]
    public UnityEvent OnFalling = new UnityEvent();

    [Header("Fired while the player is grounded.")]
    public UnityEvent OnGrounded = new UnityEvent();

    [Header("Fired when the player lands on the ground.")]
    public UnityEvent OnLanded = new UnityEvent();

    private Collider2D _playerCollider;
    private Rigidbody2D _playerRb;

    private bool _isGrounded = true;

    // An arbitrary multiplier that's added to the jump force
    // so that we can work with nicer numbers.
    private const float JUMP_FORCE_MULTIPLIER = 100f;

    // How close we have to be to the ground to be able to jump.
    private const float FLOOR_CHECK_DISTANCE = 0.2f;

    private LayerMask _floorMask;
    private Vector2 lastPos = Vector2.zero;


    private void Awake()
    {
        _playerCollider = GetComponent<Collider2D>();
        _playerRb = GetComponent<Rigidbody2D>();
        _floorMask = LayerMask.GetMask("Floor");
    }

    public void Update()
    {
        if (Input.GetKeyDown(JumpKey))
        {
            AttemptJump();
        }
        if (transform.position.y < lastPos.y)
        {
            OnFalling?.Invoke();
        }
        if (CanJump())
        {
            OnGrounded?.Invoke();
            if (!_isGrounded)
            {
                OnLanded?.Invoke();
            }
        }


        _isGrounded = CanJump();
    }

    public void LateUpdate()
    {
        lastPos = transform.position;
        
    }

    public void AttemptJump()
    {
        if (CanJump())
        {
            Jump();
        }
    }

    private bool CanJump()
    {
        // This is the base of the player's collider.
        Vector2 foot = transform.position + Vector3.down * _playerCollider.bounds.extents.y + (Vector3)_playerCollider.offset;
        return Physics2D.Raycast(foot, Vector2.down, FLOOR_CHECK_DISTANCE, _floorMask);
    }

    public void Jump()
    {
        _playerRb.AddForce(Vector2.up * JumpForce * JUMP_FORCE_MULTIPLIER);
        OnJump?.Invoke();
    }
}
