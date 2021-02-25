using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPointScript : MonoBehaviour
{
    [SerializeField]
    private GameObject particle; // Particule du point de fin de niveau

    private AudioSource audioSource; // Source audio du point de fin de niveau

    /* Récupération de la source audio*/
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /* Si le personnage du joueur entre dans le collider du point de fin de niveau,
     * on désactive les contrôles et on joue l'animation de victoire puis on désactive la musique de fond
     * du niveau.
     * Enfin, on active la source audio du point et on joue le son de victoire, en plus d'activer les
     * les particules, puis on lance la coroutine. */
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<KnightScript>().enabled = false;
            other.GetComponent<Animator>().SetTrigger("Win");
            GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().enabled = false;
            audioSource.Play();
            particle.SetActive(true);
            StartCoroutine(loadNewLevelAfterClip());
        }
    }

    /* La coroutine met à jour le dernier niveau réalisé par le joueur lorsqu'il voudra reprendre le jeu.
     * Puis une fois la musique terminée, on charge la scène suivante (menu dans notre cas car il n'y en a pas d'autre).*/
    IEnumerator loadNewLevelAfterClip()
    {
        PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(audioSource.clip.length);
        SceneManager.LoadScene("Menu");
    }
}
