using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float vida;
    public Player movimientoJugador;
    public float tiempoPerdidaControl;
    public Animator animator;
    public bool isTakingDamage;
    // Start is called before the first frame update
    void Start()
    {
        movimientoJugador = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TomarDaño(float daño, Vector2 posicion)
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            vida -= daño;
            animator.SetTrigger("Golpe");
            StartCoroutine(PerderControl());
            movimientoJugador.Rebote(posicion);
            if (vida <= 0)
            {
                Muerte();
            }
        }
    }

    public void EndDamage()
    {
        isTakingDamage = false;
    }

    private void Muerte()
    {
        animator.SetTrigger("Muerte");
        movimientoJugador.GetComponent<Collider2D>().enabled = false;
    }

        private IEnumerator PerderControl()
    {
        movimientoJugador.sePuedeMover = false;
        yield return new WaitForSeconds(tiempoPerdidaControl);
        movimientoJugador.sePuedeMover = true;
    }
}
