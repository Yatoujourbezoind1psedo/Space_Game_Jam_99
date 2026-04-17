using UnityEngine;

public class SpawnRhythm : MonoBehaviour
{
    private float[] currentBeatTimes;
    private int[] currentEmplacements;
    
    private float[] beatTimes1 = {2.37f,3.21f,4.00f,4.21f,4.40f,5.15f,6.00f,6.22f,7f,7.19f,7.38f,8.22f,9.01f,9.22f,10.15f,10.29f,11f,12.45f,13.10f,13.24f,15.19f,15.33f,16.03f,17.42f,18.12f,18.26f,20.22f,20.37f,21.07f,23.03f,23.17f,23.31f,25.24f,25.38f,26.08f,28.03f,28.17f,28.31f,30.26f,30.40f,31.10f,31.35f,32.10f,32.22f,33.07f,33.21f,33.35f,34.15f,34.35f,35.03f};
    private int[] emplacementsManquants1 = {3,4,3,2,3,4,3,2,3,4,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,2,1,2,3,4,3,2,1,3,2,3,4,3,2,3,2,3,4,3,2,3,2,3,4,3,2};

    [SerializeField] private int lvPlayed = 1; 
    [SerializeField] private float visualOffset = 0f; // À régler dans l'inspecteur

    private bool hasStartedMusicLog = false;

    private int index = 0;
    private SpawnManagement spawnManagement; 
    private AudioSource music;

    void Start()
    {
        spawnManagement = GetComponent<SpawnManagement>();
        music = spawnManagement.musicSource; // On récupère la musique du chef
        index = 0;

        if (lvPlayed == 1)
        {
            currentBeatTimes = beatTimes1;
            currentEmplacements = emplacementsManquants1;
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

    if (currentBeatTimes == null || index >= currentBeatTimes.Length || music == null) return;
        float speed = spawnManagement.globalSpeed; 
        float travelTime = spawnManagement.GetTravelTime(currentEmplacements[index], speed); 

        // Calcul du moment exact du spawn
        float targetSpawnTime = currentBeatTimes[index] - travelTime + visualOffset;

        if (music.isPlaying && music.time >= targetSpawnTime)
        {
            spawnManagement.SpawnMeteorsExceptChemin(currentEmplacements[index]);
            index++;
        }
    }
}