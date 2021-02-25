using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteSound : MonoBehaviour
{
    private AudioSource[] sources; // Liste des sources audio du niveau

    /* Récupération des sources audios du niveau et mute du son si la variable "Mute" stockée
     * dans les PlayersPrefs est égale à 0*/
    private void Awake()
    {
        sources = FindObjectsOfType<AudioSource>();

        if(PlayerPrefs.GetInt("Mute") == 0)
        {
            MuteAllSound(true);
        }
        else
        {
            MuteAllSound(false);
        }
    }

    /* On boucle sur chaque source audio qu'on possède et on réduit leur volume à 0 si
     * le booléen est à true, sinon on monte le volume à 0.1*/
    public void MuteAllSound(bool mute)
    {
        foreach(AudioSource source in sources)
        {
            source.volume = mute ? 0 : .1f;
        }
    }
}
