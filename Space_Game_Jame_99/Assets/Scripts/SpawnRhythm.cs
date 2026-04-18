using UnityEngine;

public class SpawnRhythm : MonoBehaviour
{
    private float[] currentBeatTimes;
    private int[] currentEmplacements;
    
    private float[] beatTimes1 = { 2.37f,3.21f,4.00f,4.21f,4.40f,5.15f,6.00f,6.22f,7f,7.19f,7.38f,8.22f,9.01f,9.22f,10.15f,10.29f,11f,12.45f,13.10f,13.24f,15.19f,15.33f,16.03f,17.42f,18.12f,18.26f,20.22f,20.37f,21.07f,23.03f,23.17f,23.31f,25.24f,25.38f,26.08f,28.03f,28.17f,28.31f,30.26f,30.40f,31.10f,31.35f,32.10f,32.22f,33.07f,33.21f,33.35f,34.15f,34.35f,35.03f,35.28f,35.42f,36.12f,36.37f,37.12f,37.24f,38.10f,38.24f,38.38f,39.19f,39.38f,40.07f,40.33f,41.03f,41.17f,41.42f,42.17f,42.29f,43.12f,43.26f,43.40f,44.21f,44.40f,45.08f,45.35f,46.05f,46.19f,47.00f,47.19f,47.31f,48.15f,48.29f,49f,49.24f,50f,50.12f,50.38f,51.08f,51.22f,52.03f,52.22f,52.35f,53.17f,53.31f,54.01f,54.26f,55.01f,55.14f,55.42f,56.12f,56.26f,57.07f,57.26f,57.38f,58.21f,58.35f,59.05f,59.29f,60.05f,60.17f,61f,61.29f,62.12f,62.38f,63.24f,64.08f,64.37f,65.19f,66.05f,66.31f,67.14f,68f,68.28f,69.12f,69.37f,70.22f};
    private float[] beatTimes2; 
    private int[] emplacementsManquants1 = {3,4,3,2,3,4,3,2,3,4,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2};
    private int[] emplacementsManquants2; 
    [SerializeField] private int lvPlayed = 1; 
    [SerializeField] private float visualOffset = 0f; // À régler dans l'inspecteur
    [SerializeField] private GameManager gameManager; 

    private bool hasStartedMusicLog = false;

    private int index = 0;
    private SpawnManagement spawnManagement; 
    private AudioSource music;
    private bool finPiste = false; 

    void Start()
    {
        spawnManagement = GetComponent<SpawnManagement>();
        music = spawnManagement.musicSource; // On récupère la musique du chef
        index = 0;

        switch (lvPlayed)
        {
            case 1:
                currentBeatTimes = beatTimes1;
                currentEmplacements = emplacementsManquants1;
                break; 

            case 2: 
                currentBeatTimes = beatTimes2;
                currentEmplacements = emplacementsManquants2;
                break; 

        }
    }

    void Update()
    {// --- DEBUG TEMPS 0 ---
        if (music.isPlaying && !hasStartedMusicLog)
        {
            Debug.Log($"<color=white>DÉMARRAGE MUSIQUE détecté à music.time : </color>{music.time}");
            hasStartedMusicLog = true;
        }
        // ---------------------

        if (currentBeatTimes == null || index >= currentBeatTimes.Length || music == null) return;  //pas de piste trouvée ou index plus grand que nombre de beats ou pas de muique

        if (!finPiste && (index >= currentEmplacements.Length || (!music.isPlaying && index >= 3))) //fin des apparitions ou de la musique
        {
            Debug.Log("ginito pipo la"+music.isPlaying);
            finPiste = true; 
            gameManager.isMusicFinished = true;
            return; 
        }

        float speed = spawnManagement.globalSpeed; 
        float travelTime = spawnManagement.GetTravelTime(currentEmplacements[index], speed); 

        // Calcul du moment exact du spawn
        float targetSpawnTime = currentBeatTimes[index] - travelTime + visualOffset;

        if (music.isPlaying && music.time >= targetSpawnTime) //musique.isPlaying est pas nécessaire je crois pt
        {
            spawnManagement.SpawnMeteorsExceptChemin(currentEmplacements[index]);
            index++;

        }

    }
}