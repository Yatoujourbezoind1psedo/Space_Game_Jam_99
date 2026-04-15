using UnityEngine;

public class SpawnRhythm : MonoBehaviour
{
    //LE SCRIPT EST EN COMPLEMENT AVEC SpawnManagement DONC ATTACHER SUR LE MËME OBJET

    [SerializeField] private float[] beatTimes = {1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f}; //ce sont les beats
    [SerializeField] private int[] emplacementsManquants = {1, 2, 3, 4, 4, 3, 2, 1};
    private int index = 0;
    private SpawnManagement spawnManagement; 
    [SerializeField] private AudioSource music; 
    
    void Start()
    {
        spawnManagement = GetComponent<SpawnManagement>();
        index = 0; 
    }

    void Update()
    {

        if (index >= beatTimes.Length) return ; 

        if (index < beatTimes.Length && music.time >= beatTimes[index])
        {
            spawnManagement.SpawnMeteorsExceptChemin(emplacementsManquants[index]); ;
            index++;
        }

        //ajouter condition de victoire
    }


}
