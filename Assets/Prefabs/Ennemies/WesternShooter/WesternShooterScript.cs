using UnityEngine;

public class WesternShooterScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f; // Vitesse de déplacement

    private Vector2 startPos; // Point de départ du personnage
    private SpriteRenderer sp; // Sprite du personnage
    private bool lookRight = true, isShooting = false; // Contrôleurs booléens pour savoir si l'ennemi se dirige vers la droite et s'il est en train de tirer

    [SerializeField]
    private float shootDistance = 5f, shootForce = 500f; // Distance et force du tir d'une balle

    [SerializeField]
    private LayerMask shootMask; // Filtre de layer pour la détection d'un collider

    [SerializeField]
    private GameObject bulletShape; // Prefab d'une balle

    private Animator animator; // Animator du personnage
    private AudioSource audioSource; // AudioSource du personnage

    // Initialisation de certaines variables
    void Start()
    {
        startPos = transform.position;
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    /* On met à jour la position du perso à chaque image s'il n'est pas en train de tirer.
    * Et si la distance entre le perso et son point de départ est inférieur à 0.5,
    * alors on change l'orientation du sprite du perso.*/
    void Update()
    {
        if (!isShooting)
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, startPos) < .5f && !lookRight)
            {
                FlipCharacter();
            }
        }

        /* À chaque image, on lance un rayon dans la direction dans laquelle se dirige le perso.
         * On récupère le premier élément touché, et s'il s'agit du joueur on passe en mode
         * tir via le contrôleur booléen, et on joue l'animation qui est liée au tir.
         * S'il n'y a aucun son de joué, alors on joue le son de tir.
         * Si par contre le hit ne touche rien, alors on déclare à false le contrôleur booléen du tir,
         * et on ne joue pas/plus l'anmation de tir.*/
        Vector2 rayDirection = lookRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, shootDistance, shootMask);

        if(hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                isShooting = true;
                animator.SetBool("IsShooting?", true);

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
        else
        {
            isShooting = false;
            animator.SetBool("IsShooting?", false);
        }
    }

    /* Si l'ennemi rentre dans la collision du collider de son point de retour,
     * alors on inverse son orientation et on le fait partir dans l'autre sens vers son point de départ.*/
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ReturnPoint"))
        {
            FlipCharacter();
        }
    }

    /* On change l'orientation du sprite via la variable flipX et on inverse la speed pour le déplacement retour*/
    private void FlipCharacter()
    {
        sp.flipX = !sp.flipX;
        speed = -speed;
        lookRight = !lookRight;
    }

    /* L'ennemi instancie un objet "Bullet" et détermine sa direction en fonction de la direction
     * de l'ennemi, puis on ajoute une force à la balle pour qu'elle se déplace vite.*/
    public void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletShape, transform.position, Quaternion.identity);
        Vector2 bulletDirection = lookRight ? Vector2.right : Vector2.left;
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * shootForce);
    }
}
