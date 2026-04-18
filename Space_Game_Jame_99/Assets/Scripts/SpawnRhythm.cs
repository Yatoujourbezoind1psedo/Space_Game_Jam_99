using UnityEngine;

public class SpawnRhythm : MonoBehaviour
{
    private float[] currentBeatTimes;
    private int[] currentEmplacements;
    
private float[] beatTimes1 = { 2.00f, 2.33f, 2.83f, 3.50f, 4.00f, 4.00f, 4.50f, 4.93f, 5.37f, 6.00f, 6.53f, 6.53f, 7.00f, 7.43f, 7.90f, 8.53f, 9.03f, 9.03f, 9.53f, 9.97f, 10.37f, 10.70f, 11.00f, 12.93f, 13.23f, 13.57f, 15.43f, 15.77f, 16.10f, 17.97f, 18.30f, 18.60f, 20.53f, 20.83f, 21.17f, 23.10f, 23.40f, 23.73f, 25.57f, 25.90f, 26.20f, 28.10f, 28.40f, 28.73f, 30.60f, 30.93f, 31.23f, 31.80f, 31.80f, 32.23f, 32.53f, 33.17f, 33.50f, 33.80f, 34.37f, 34.37f, 34.80f, 35.10f, 35.63f, 35.97f, 36.30f, 36.83f, 36.83f, 37.30f, 37.57f, 38.23f, 38.57f, 38.90f, 39.43f, 39.43f, 39.90f, 40.17f, 40.77f, 41.10f, 41.40f, 41.97f, 41.97f, 42.40f, 42.70f, 43.30f, 43.60f, 43.93f, 44.50f, 44.50f, 44.93f, 45.20f, 45.80f, 46.13f, 46.43f, 47.00f, 47.00f, 47.43f, 47.73f, 48.37f, 48.70f, 49.00f, 49.57f, 49.57f, 50.00f, 50.30f, 50.90f, 51.20f, 51.53f, 52.10f, 52.10f, 52.53f, 52.80f, 53.40f, 53.73f, 54.03f, 54.60f, 54.60f, 55.03f, 55.33f, 55.97f, 56.30f, 56.60f, 57.17f, 57.17f, 57.60f, 57.90f, 58.50f, 58.80f, 59.13f, 59.70f, 59.70f, 60.13f, 60.40f, 61.00f, 61.70f, 62.30f, 62.90f, 62.90f, 63.57f, 64.20f, 64.83f, 65.43f, 66.13f, 66.73f, 66.73f, 67.33f, 68.00f, 68.63f, 69.30f, 69.83f, 70.53f, 71.13f, 71.73f, 71.73f, 72.40f, 73.03f, 73.70f, 74.30f, 74.97f, 75.57f, 75.57f, 76.17f, 76.83f, 77.50f, 78.13f, 78.73f, 79.33f, 80.00f, 80.60f, 80.60f, 81.30f, 81.60f, 81.93f, 82.50f, 82.50f, 82.93f, 83.20f, 83.80f, 84.13f, 84.43f, 85.00f, 85.00f, 85.43f, 85.73f, 86.33f, 86.63f, 86.97f, 87.53f, 87.53f, 87.97f, 88.23f, 88.90f, 89.20f, 89.53f, 90.10f, 90.10f, 90.53f, 90.80f, 91.40f, 91.73f, 92.03f, 92.60f, 92.60f, 93.03f, 93.33f, 93.93f, 94.23f, 94.57f, 95.13f, 95.13f, 95.57f, 95.83f, 96.43f, 96.77f, 97.10f, 97.63f, 97.63f, 98.10f, 98.37f, 99.00f, 99.33f, 99.63f, 100.20f, 100.20f, 100.63f, 100.93f, 101.57f, 102.17f, 102.83f, 103.43f, 103.43f, 104.03f, 104.73f, 105.37f, 106.00f, 106.60f, 107.20f, 107.90f, 108.50f, 108.50f, 109.10f, 109.77f, 110.40f, 111.03f, 111.73f, 112.33f, 113.00f, 113.60f, 113.60f, 114.20f, 114.90f, 115.53f, 116.17f, 116.73f, 117.33f, 118.00f, 118.60f, 118.60f, 119.20f, 119.90f, 120.53f, 121.17f, 121.73f, 122.33f, 123.00f, 123.60f, 123.60f, 124.20f, 124.90f, 125.53f, 126.17f, 127.50f, 128.10f, 128.77f, 129.37f, 129.37f, 129.97f, 130.63f, 131.30f, 131.93f, 132.57f, 133.17f, 133.83f, 134.43f, 134.43f, 135.03f, 135.73f, 136.37f, 137.00f, 137.60f, 138.20f, 138.90f, 139.50f, 139.50f, 140.10f, 140.77f, 141.40f, 142.00f, 142.33f, 142.63f, 143.20f, 143.20f, 143.63f, 143.93f, 144.50f, 144.80f, 145.13f, 145.70f, 145.70f, 146.13f, 146.40f, 147.03f, 147.37f, 147.70f, 148.23f, 148.23f, 148.70f, 148.97f, };
    private float[] beatTimes2; 
private int[] emplacementsManquants1 = { 3, 2, 2, 3, 2, 4, 3, 2, 2, 3, 2, 4, 3, 2, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 1, 2, 3, 2, 4, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 1, 2, 3, 2, 4, 3, 2, 1, 2, 1, 2, 3, 2, 4, 3, 2, 1, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, 3, 2, 3, 2, 4, 3, 2, };
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