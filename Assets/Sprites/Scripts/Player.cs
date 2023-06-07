using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int velocidad;
    public float velocidadSalto = 3;
    private Rigidbody2D rb;
    public bool betterJump = false;
    public float fallMultiplier = 0.5f;
    public float lowJumpMultiplier = 1f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private bool isJumping = false;
    private bool isFalling = false;
    public bool sePuedeMover = true;
    public Vector2 velocidadRebote;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
            if ((Input.GetKey("d") || Input.GetKey("right")))
            {
                rb.velocity = new Vector2(velocidad, rb.velocity.y);
                spriteRenderer.flipX = false;
                if (CheckGround.isGrounded)
                {
                    animator.SetBool("Run", true);
                }
            }
            else if ((Input.GetKey("a") || Input.GetKey("left")))
            {
                rb.velocity = new Vector2(-velocidad, rb.velocity.y);
                spriteRenderer.flipX = true;
                if (CheckGround.isGrounded)
                {
                    animator.SetBool("Run", true);
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetBool("Run", false);
            }

            if (Input.GetKey("space"))
            {
                isJumping = true;
                isFalling = false;
                rb.velocity = new Vector2(rb.velocity.x, velocidadSalto);
                animator.SetBool("Jump", true);
            }
            else if (!CheckGround.isGrounded)
            {
                animator.SetBool("Jump", false);
            }

            if (rb.velocity.y < 0 && !CheckGround.isGrounded)
            {
                isJumping = false;
                isFalling = true;
                animator.SetBool("Fall", true);
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else
            {
                isFalling = false;
                animator.SetBool("Fall", false);
            }

            if (rb.velocity.y > 0 && !Input.GetKey("space"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        
       
    }

    public void Rebote(Vector2 puntoGolpe)
    {
        rb.velocity = Vector2.zero; // Resetea la velocidad
        rb.velocity = new Vector2(velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);
    }


}
