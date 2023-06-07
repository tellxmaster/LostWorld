using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public Animator animator;
    public float velocidad;
    public float fuerzaSalto;
    public SpriteRenderer spriteRenderer;
    public bool esDerecha;
    public float timer;
    public float timeChange;
    public Rigidbody2D rb;
    public float vida;
    public GameObject itemPrefab;
    public LayerMask groundLayer;
    public GameObject player;
    public float detectionRadius;

    private bool isAlive = true;
    private bool estaSaltando;
    private float attackCooldown = 1f; // El intervalo en segundos entre golpes
    private float lastAttackTime;

    void Start()
    {
        timer = timeChange;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAlive)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
            {
                // Follow the player
                animator.SetBool("Run", true);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, velocidad * Time.deltaTime);
                spriteRenderer.flipX = transform.position.x < player.transform.position.x ? false : true;
            }
            else
            {
                // Normal movement
                animator.SetBool("Run", !estaSaltando);

                if (esDerecha == true)
                {
                    transform.position += Vector3.right * velocidad * Time.deltaTime;
                    spriteRenderer.flipX = false;
                }
                else
                {
                    transform.position += Vector3.left * velocidad * Time.deltaTime;
                    spriteRenderer.flipX = true;
                }

                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    timer = timeChange;
                    esDerecha = !esDerecha;
                }

                if (!estaSaltando && IsTouchingGround())
                {
                    Saltar();
                }
            }
        }
    }

    public void TomarDaño(float daño)
    {
        vida -= daño;
        animator.SetTrigger("Damage");
        if (vida <= 0)
        {
            Muerte();
        }
    }

    void Muerte()
    {
        animator.SetBool("Run", false);
        animator.SetTrigger("Death");
        isAlive = false;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false; // Desactiva el collider

        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity); // Crea un nuevo item en la posición del Goblin

        rb.bodyType = RigidbodyType2D.Dynamic; // Cambia a Dynamic
        rb.constraints = RigidbodyConstraints2D.None; // Elimina las restricciones

        Destroy(gameObject, 2f);
    }



    public void Saltar()
    {
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        estaSaltando = true;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && isAlive)
        {
            // Sólo golpea al jugador si ha pasado suficiente tiempo desde el último golpe
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                other.gameObject.GetComponent<HealthController>().TomarDaño(20, other.GetContact(0).normal);
                animator.SetTrigger("Golpe");
                lastAttackTime = Time.time; // Actualiza el momento del último golpe
            }
        }
    }


    private bool IsTouchingGround()
    {
        float extraHeightText = 0.05f;
        RaycastHit2D raycastHit = Physics2D.Raycast(rb.position, Vector2.down, extraHeightText, groundLayer);
        return raycastHit.collider != null;
    }
}
