using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Pingu : MonoBehaviourPunCallbacks
{

    // Variables configurables en el Inspector de Unity
    public float speed = 5f; // Velocidad de movimiento
    public float jumpForce = 7f; // Fuerza de salto
    public bool isGrounded; // Verificar si est� en el suelo
    public Transform groundCheck; // Transform para verificar si est� tocando el suelo
    public LayerMask groundLayer; // Capa del suelo para la detecci�n

    // Variables de ataque
    public Transform attackPoint; // Punto desde donde se realizar� el ataque
    public float attackRange = 0.5f; // Alcance del ataque
    public int attackDamage = 10; // Da�o que hace el ataque
    public LayerMask enemyLayers; // Capa de los enemigos para detectar con el ataque
    public float attackRate = 2f; // Velocidad de ataque
    private float nextAttackTime = 0f; // Tiempo entre ataques

    // Variables de salud
    public int maxHealth = 100;
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator anim;
    private bool facingRight = true;

    // M�todo llamado al iniciar el juego
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth; // Inicia con la salud m�xima
    }

    // M�todo llamado una vez por frame
    void Update()
    {
        Move();
        Jump();

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1")) // El bot�n de ataque es "Fire1" (normalmente clic izquierdo o Ctrl)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Move()
    {
        // Obtener entrada del jugador en el eje horizontal (flechas o WASD)
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Control de animaci�n de caminar
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Cambiar direcci�n del personaje si es necesario
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        // Verificar si el personaje est� tocando el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Si presiona espacio y est� en el suelo, el personaje salta
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }

        // Configuraci�n de animaciones seg�n si est� en el suelo o no
        anim.SetBool("isGrounded", isGrounded);
    }

    // M�todo para atacar
    void Attack()
    {
        // Reproducir animaci�n de ataque
        anim.SetTrigger("Attack");

        // Detectar enemigos en el rango del ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        /*Aplicar da�o a cada enemigo detectado
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<enemy>().TakeDamage(attackDamage); // Suponiendo que el enemigo tiene un script con TakeDamage()
        }*/
    }

    // M�todo para recibir da�o
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // M�todo para morir
    void Die()
    {
        anim.SetBool("isDead", true);
        // Desactivar el jugador o reiniciar la escena
        this.enabled = false;
        // Puedes agregar aqu� l�gica adicional para cuando el jugador muera.
    }

    // M�todo para voltear al personaje
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // M�todo para mostrar el �rea de ataque en el Editor de Unity
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}