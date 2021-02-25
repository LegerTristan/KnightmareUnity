using UnityEngine;

public class Gobelin : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f; // Vitesse de déplacement

    private Vector2 startPos; // Point de départ du personnage
    private SpriteRenderer sp; // Sprite du personnage
    private bool lookRight = true; // Contrôleur booléen pour savoir si l'ennemi se dirige vers la droite

    // Initialisation de certaines variables
    void Start()
    {
        startPos = transform.position;
        sp = GetComponent<SpriteRenderer>();
    }

    /* On met à jour la position du perso à chaque image.
    * Et si la distance entre le perso et son point de départ est inférieur à 0.5,
    * alors on change l'orientation du sprite du perso.*/
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, startPos) < .5f && !lookRight)
        {
            FlipCharacter();
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
}
