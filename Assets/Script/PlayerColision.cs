using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PLayerMovement : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rb;
    private float horizontal;
    [SerializeField]
    private float jumpforce = 1.0f;
    private bool grounded;
    public float velocidad;
    private Animator animator;
    [SerializeField]
    private int playerViewID;

    private float subiendoAnterior;
    [SerializeField]
    public float margen = 0.001f;  // Umbral para detectar el cambio significativo en Y

    private Collider2D objectCollider; // Para controlar las colisiones

    private Vector2 direction;

    


    void Start()//Coger el RigidBody del objeto al que est� asignado el script
    {
        subiendoAnterior = transform.position.y;
        objectCollider = gameObject.GetComponent<Collider2D>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {



        if (photonView.IsMine)
        {
            //Girar al jugador
            if (horizontal < 0)
                transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            else if (horizontal > 0)
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);


            if (horizontal < 0)
            {
                animator.SetBool("Running", true);
            }
            else if (horizontal > 0)

            {
                animator.SetBool("Running", true);
            }
            else if(horizontal == 0)
            {
                animator.SetBool("Running", false);
            }


                horizontal = Input.GetAxis("Horizontal");// Coger el input del teclado, con valores del -1 al 1


            Debug.DrawRay(transform.position, Vector2.down * 2.5f, Color.red);
            RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, 2.5f);
            if (hitGround)
            {
                animator.SetBool("EnSuelo", true);
                grounded = true;
            }
            else
            {
                animator.SetBool("EnSuelo", false);
                grounded = false;
            }

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && grounded)//GetkeyDown quiere decir cuando presionas una tecla, en este caso con KeyCode hemos puesto el espacio
            {
                Jump();
            }

            float subiendoActual = transform.position.y;

            Debug.Log("Posici�n actual: " + subiendoActual + ", Posici�n anterior: " + subiendoAnterior);


            // Comparamos las posiciones con un umbral para evitar problemas de precisi�n
            if (subiendoActual > subiendoAnterior + margen)
            {
                if (objectCollider.enabled) // Verifica si el collider est� activo
                {
                    objectCollider.enabled = false; // Desactiva el collider (sin colisiones)
                    Debug.Log("Est� subiendo, colisi�n desactivada");
                    animator.SetBool("Subiendo", true);
                }
            }
            else if (subiendoActual < subiendoAnterior - margen)
            {
                if (!objectCollider.enabled) // Verifica si el collider est� desactivado
                {
                    objectCollider.enabled = true; // Activa el collider (colisiones activas)
                    Debug.Log("Est� bajando, colisi�n activada");
                    animator.SetBool("Subiendo", false);

                }
            }

            // Actualizar la posici�n anterior solo despu�s de la comparaci�n
            subiendoAnterior = subiendoActual;

           
        }




    }
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            rb.velocity = new Vector2(horizontal * velocidad, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (photonView.IsMine)
        {
            rb.AddForce(new Vector2(0, jumpforce));
        }
    }
}
