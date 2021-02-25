using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    /* Au bout de 4 secondes, on détruit l'objet.*/
    void Start()
    {
        Destroy(gameObject, 4f);   
    }

    /* Lorsque l'objet entre en collision avec un collider 2D, on vérifie s'il s'agit de celui du joueur.
     * Si c'est le cas, on inflige des dégâts au joueur (5pts).
     * Dans tout les cas, on détruit l'objet après qu'il soit entré en collision avec un autre objet.*/
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<KnightScript>().Hurt(5f);
        }
        Destroy(gameObject);
    }
}
