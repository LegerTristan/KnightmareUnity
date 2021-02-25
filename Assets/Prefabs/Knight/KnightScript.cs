using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : MonoBehaviour {

    [SerializeField] float speed = 2f, jumpForce = 500f; // Respectivement, la vitesse et la force de saut
    private Rigidbody2D rb; // Rigidbody du chevalier
    private Animator anim; // Animator du chevalier
    private bool lookRight = true; // Contrôleur booléen afin de savoir si le perso se dirige à droite
    private bool wantToJump = false; // Contrôleur booléen pour savoir si le joueur souhaite sauter.
    public bool OnAttack = false; // Contrôleur booléen qui vérifie si le joueur est en train d'attaquer.

    //grounded
    [SerializeField] bool grounded; // Contrôleur booléen afin de savoir si le personnage est au sol
    [SerializeField] float groundRadius = 0.02f; // Rayon du contrôleur booléen du sol
    [SerializeField] Transform groundCheck; // La position du gameObject vide qui contrôle si le joueur est au sol ou non
    [SerializeField] LayerMask theGround; // Le filtre de layer qui permet de choisir lesquelles déterminent si le personnage est au sol.

    [SerializeField] AudioClip sndAttack, sndJump, sndHurt, sndDead, sndWin, sndGoblin, sndPickUp; // Les différents sont liés au personnage
        
    private AudioSource audioS; // L'audio source du chevalier

    public ProgressBar healthBar; // La barre de vie
    public GameObject gameOverCanvas; // Le canvas de gameOver

    // Initialisation des composants du chevalier
	void Awake () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
	}	
	
	void Update () {

        // On vérifie dans un premier temps si le personnage est bien au sol et on édite l'animation en conséquence.
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, theGround);
        anim.SetBool("Grounded", grounded);

        /* On met également à jour la position du personngae via une translation
         * en fonction de l'axe dans lequel avance le personnage (horizontale ou verticale, ou les deux)*/
        float move = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * move * speed * Time.deltaTime);
        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetFloat("Vspeed", rb.velocity.y);

        /* On vérifie que le sprite du personnage est en adéquation avec la direction dans laquelle il se déplace.
         * Si ce n'est pas le cas, on flip le sprite.*/
        if(move>0 && !lookRight)
        {
            Flip();
        }
        else if(move < 0 && lookRight)
        {
            Flip();
        }

        /* Si le joueur presse la touche "LeftAlt" et qu'il n'est pas en train d'attaquer et qu'il est bien au sol,
         * alors on met à jour le contrôleur onAttack et on joue l'animation et le son.*/
        if(Input.GetKeyDown(KeyCode.LeftAlt) && grounded && !OnAttack)
        {
            OnAttack = true;
            anim.SetTrigger("Attack");
            audioS.PlayOneShot(sndAttack);
        }

        /* Si le joueur presse la touche Space et est au sol, alors on indique via le contrôleur booléen, la volonté que le personnage saute.*/
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            wantToJump = true;
        }
    }

    private void FixedUpdate()
    {
        /* Si le joueur a montré une intention de sauter, alors on ajoute une force verticale au chevalier
        * et on joue un son de saut puis on désactive l'intention de sauter via le contrôleur booléen.*/
        if(wantToJump)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            audioS.PlayOneShot(sndJump);
            wantToJump = false;
        }
    }

    /* On inverse la valeur de lookRight et pour effectuer un flip, on va inverser la coordonnée X du vecteur scale*/
    void Flip()
    {
        lookRight = !lookRight;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    /* On joue l'animation de Hurt et le son avec, puis on met à jour la barre de vie.
     * Si la barre de vie est à 0 ou moins, on déclenche le game over.*/
    public void Hurt(float damage)
    {
        anim.SetTrigger("Hurt");
        audioS.PlayOneShot(sndHurt);
        healthBar.Val -= damage;

        if(healthBar.Val <= 0)
        {
            gameOverCanvas.SetActive(true);
        }
    }

    /* On joue l'animation de win et le son qui va avec*/
    public void Win()
    {
        anim.SetTrigger("Win");
        audioS.PlayOneShot(sndWin);
    }

    /* On joue l'animation de mort et le son qui va avec*/
    public void Dead()
    {
        anim.SetTrigger("Dead");
        audioS.PlayOneShot(sndDead);
    }

    /* Cette collision se produit lorsque le joueur rentre dans un perso.
     * S'il s'agit d'un gobelin ou d'un shooter, il y a deux options :
     * Si le joueur est en train d'attaquer, alors l'ennemi est envoyé vers le GameObjectDestroyer pour simuler la mort de l'ennemi.
     * Si le joueur n'est pas en train d'attaquer, alors le joueur est repoussé et subit des dégâts, un son de dégâts est joué.
     * Lorsqu'il s'agit d'une masse, le joueur subit des dégâts dans tout les cas.*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "WesternShooter":
            case "Goblin":
                if (OnAttack)
                {
                    Rigidbody2D rb2D = collision.gameObject.GetComponent<Rigidbody2D>();
                    rb2D.bodyType = RigidbodyType2D.Dynamic;
                    rb2D.AddForce(Vector3.up * 2000);
                    audioS.PlayOneShot(sndGoblin);
                }
                else
                {
                    ApplyKnockback(collision, 10f);
                }
                break;

            case "Mace":
                ApplyKnockback(collision, 20f);
                break;
        }
    }

    /* On calcule le vecteur directeur du knockback, puis on y ajoute un peu de puissance pour repousser le chevalier
     * dans la direction opposé de la zone où il a subit des dégâts.*/
    private void ApplyKnockback(Collision2D collision, float damage)
    {
        Vector2 knockback = collision.transform.position - transform.position;
        rb.AddForce(knockback.normalized * -200f);
        Hurt(damage);
    }

    /* On vérifie si le joueur entre dans la collision d'un objet.
     * Si c'est de l'eau, on déclenche le game over.
     * Si c'est de la vie, alors on augmente la barre de vie.*/
    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Water":
                gameOverCanvas.SetActive(true);
                break;
            case "ItemHeart":
                Mathf.Clamp(healthBar.Val += 10, 0, 100);
                Destroy(collision.gameObject);
                audioS.PlayOneShot(sndPickUp);
                break;

        }
        if (collision.CompareTag("Water"))
        {
            gameOverCanvas.SetActive(true);
        }
    }
}
