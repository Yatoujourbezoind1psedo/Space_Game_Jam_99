using UnityEngine;

public class SpawnRhythm : MonoBehaviour
{
    //LE SCRIPT EST EN COMPLEMENT AVEC SpawnManagement DONC ATTACHER SUR LE MËME OBJET

    [SerializeField] private float[] beatTimesTest = {1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f}; //ce sont les beats
    [SerializeField] private int[] emplacementsManquantsTest = {1, 2, 3, 4, 4, 3, 2, 1};
    private int index = 0;
    private SpawnManagement spawnManagement; 
    [SerializeField] private AudioSource music; 

    [SerializeField] private int lvPlayed; 

    //MUSIQUE 1 
    private int[] emplacementsManquants1 = {2,3,4,3,2,3,4,3,2,3,4,3,2,3,4,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2}; 
    private float[] beatTimes1 = {1,2,4,6}; 

    //MUSIQUE 2
    private int[] emplacementsManquants2 ; 
    private float[] beatTimes2; 

    [SerializeField] private GameManager gameManager;

    
    void Start()
    {
        spawnManagement = GetComponent<SpawnManagement>();
        index = 0; 
    }

    void Update()
    {

        if (index >= beatTimesTest.Length) // si on a fini la piste on arrête  
        {
            gameManager.isGameWinned = true; 
            return; 
        } ; 


        switch (lvPlayed) //si on a pas fini la piste on va chercher au bon endroit
            {
                case 0: //si niveau 0 (juste test)
                    if (index < beatTimesTest.Length && music.time >= beatTimesTest[index])
                    {
                        spawnManagement.SpawnMeteorsExceptChemin(emplacementsManquantsTest[index]); ;
                        index++;
                    }
                    break; 
                    
                case 1: //si niveau 1
                    if (index < beatTimes1.Length && music.time >= beatTimes1[index])
                    {
                        spawnManagement.SpawnMeteorsExceptChemin(emplacementsManquants1[index]); ;
                        index++;
                    }
                    break; 

                case 2: //si niveau 2
                    if (index < beatTimes2.Length && music.time >= beatTimes2[index])
                    {
                        spawnManagement.SpawnMeteorsExceptChemin(emplacementsManquants2[index]); ;
                        index++;
                    }
                    break; 

                default: //même chose que test si valeur du niveau saisi pas bien
                    if (index < beatTimesTest.Length && music.time >= beatTimesTest[index])
                    {
                        spawnManagement.SpawnMeteorsExceptChemin(emplacementsManquantsTest[index]); ;
                        index++;
                    }
                    break; 
            }



    }


}
