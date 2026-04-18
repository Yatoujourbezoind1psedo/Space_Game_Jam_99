using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KatyRandomSounds : MonoBehaviour
{
    [Header("Configuration Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] poolDeSons; // Glisse tes fichiers .mp3/.wav ici

    [Header("Réglages du Délai")]
    [SerializeField] private float delaiMin = 5f;  // Minimum 5 secondes entre deux sons
    [SerializeField] private float delaiMax = 15f; // Maximum 15 secondes

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        // On lance la boucle de sons aléatoires
        if (poolDeSons.Length > 0)
        {
            StartCoroutine(RoutineSonsAleatoires());
        }
        else
        {
            Debug.LogWarning("Attention : La pool de sons de Katy est vide !");
        }
    }

    IEnumerator RoutineSonsAleatoires()
    {
        while (true)
        {
            // 1. On attend un temps aléatoire
            float tempsAttente = Random.Range(delaiMin, delaiMax);
            yield return new WaitForSeconds(tempsAttente);

            // 2. On vérifie si Katy n'est pas déjà en train de parler
            if (!audioSource.isPlaying)
            {
                JouerSonAleatoire();
            }
        }
    }

    public void JouerSonAleatoire()
    {
        if (poolDeSons.Length == 0) return;

        // Choix d'un index au hasard
        int randomIndex = Random.Range(0, poolDeSons.Length);
        
        // Joue le son
        audioSource.clip = poolDeSons[randomIndex];
        audioSource.Play();

        Debug.Log("<color=pink>Katy joue le son : </color>" + poolDeSons[randomIndex].name);
    }
}