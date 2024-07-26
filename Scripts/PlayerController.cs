using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3000f;
    [SerializeField] float moveDrag = 15f;
    [SerializeField] float stillDrag = 75f;

    [SerializeField] GameObject playerInteractionPopup;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _anim;

    private float currentSpeed = 0.0f;
    private Vector2 moveInput = Vector2.zero;
    private Vector2 calculatedMovement = Vector2.zero;
    private bool CanInteract = false;
    private Collider2D currentCollision = null;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        TryInteract();

        moveInput = GetInput();

        currentSpeed = _rb.velocity.magnitude;
        _anim.SetFloat("speed", currentSpeed);

        FlipSprite();
    }

    private void FixedUpdate()
    {
        if (Managers.Dialogue.DialogueIsPlaying)
        {
            return;
        }

        SetDrag();

        calculatedMovement = Vector2.ClampMagnitude(moveInput, 1);

        _rb.AddForce((calculatedMovement * moveSpeed) * Time.fixedDeltaTime);
    }



    public void TeleportPlayerTo(Transform target)
    {
        _rb.position = target.position;
    }


    private void TryInteract()
    {
        if (CanInteract && Input.GetKeyDown(KeyCode.E))
        {
            var interactable = currentCollision.GetComponent<IInteractable>();
            interactable.Interact();
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void SetDrag()
    {
        if (Mathf.Approximately(moveInput.x, 0.0f) && Mathf.Approximately(moveInput.y, 0.0f))
        {
            _rb.drag = stillDrag;
        }
        else
        {
            _rb.drag = moveDrag;
        }
    }

    private void FlipSprite()
    {
        if (moveInput.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            playerInteractionPopup.SetActive(true);
            currentCollision = collision;
            CanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            playerInteractionPopup.SetActive(false);
            currentCollision = null;
            CanInteract = false;
        }
    }
}
