using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combate : MonoBehaviour
{
    public Transform controladorGolpe;
    public float radioGolpe;
    public float dañoGolpe;
    public Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey("c") || Input.GetMouseButtonDown(0))
        {
            Golpe();
        }
    }

    private void Golpe()
    {
        animator.SetTrigger("Attack");

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach(Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Goblin"))
            {
                colisionador.transform.GetComponent<Goblin>().TomarDaño(dañoGolpe);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }

}
