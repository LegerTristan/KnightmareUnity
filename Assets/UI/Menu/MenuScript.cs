using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private Image imgMute; // Composant image qui contrôle le mute du son.

    [SerializeField]
    private Sprite spSound, spMute; // Sprite du haut-parleur avec le son actif, et muté.

    private bool isPlayingSound = true; // Contrôleur booléen qui vérifie si le son est joué ou non.

    /* Récupération du composant Image, et mute ou non du son en fonction de la valeur
     * de la variable "Mute" du PlayerPrefs*/
    private void Start()
    {
        imgMute = GameObject.Find("ImgSoundManagement").GetComponent<Image>();
        bool mute = PlayerPrefs.GetInt("Mute") == 0 ? false : true;
        imgMute.sprite = mute ? spMute : spSound;
    }

    /* On charge la scène "Level_01"*/
    public void StartGame()
    {
        SceneManager.LoadScene("Level_01");
    }

    /* On charge la scène qui suit la dernière scène que le personnage a complété d'après le PlayerPrefs*/
    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel") + 1);
    }

    /* Quitte l'application ou sort du mode jeu de l'éditeur, si on est dans l'éditeur.*/
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    /* On inverse la valeur du contrôleur booléen du son, et on édite le sprite du haut-parleur en
     * fonction de la valeur du booléen.
     * On met également à jour la variable "Mute" du PlayerPrefs en fonction de la valeur de la variable
     * "isPlayingSound"*/
    public void MuteSound()
    {
        isPlayingSound = !isPlayingSound;
        imgMute.sprite = isPlayingSound ? spMute : spSound;
        PlayerPrefs.SetInt("Mute", isPlayingSound ? 1 : 0);
    }
}
