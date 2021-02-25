using System.Collections;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    [SerializeField]
    private string[] tags; // Liste des tags d'objets que l'on souhaite désactiver lors d'un game over.

    /* On vérifie si la variable "Mute" du PlayerPrefs est égale à 0, le cas échéant, on désactive l'audio source du game over*/
    private void Awake()
    {
        if(PlayerPrefs.GetInt("Mute") == 0)
        {
            GetComponent<AudioSource>().enabled = false;    
        }
    }

    /* On boucle sur chaque tag présent dans la liste tags et on récupère la liste d'objets possédant ce tag.
     * On désactive ensuite tout les objets présents dans cette liste.
     * Enfin, on désactive la musique de fond et on charge le menu principal via une coroutine.*/
    void Start()
    {
        foreach(string tag in tags)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(false);
            }
        }
        GameObject.Find("BackgroundMusic").GetComponent<AudioSource>().enabled = false;
        StartCoroutine(LoadMenu());
    }

    /*Après 3 secondes d'attente, on charge le menu principal.*/
    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
