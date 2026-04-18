using UnityEngine;

public class SpawnRhythm : MonoBehaviour
{
    private float[] currentBeatTimes;
    private int[] currentEmplacements;
    
    private float[] beatTimes1 = { 2.880f, 3.520f, 4.040f, 4.520f, 4.960f, 5.400f, 6.040f, 6.560f, 7.040f, 7.480f, 7.920f, 8.560f, 9.080f, 9.560f, 10.400f, 10.720f, 11.040f, 12.960f, 13.280f, 13.600f, 15.480f, 15.800f, 16.120f, 18.000f, 18.320f, 18.640f, 20.560f, 20.880f, 21.200f, 23.120f, 23.440f, 23.760f, 25.600f, 25.920f, 26.240f, 28.120f, 28.440f, 28.760f, 30.640f, 30.960f, 31.280f, 31.840f, 32.280f, 32.560f, 33.200f, 33.520f, 33.840f, 34.400f, 34.840f, 35.120f, 35.680f, 36.000f, 36.320f, 36.880f, 37.320f, 37.600f, 38.280f, 38.600f, 38.920f, 39.480f, 39.920f, 40.200f, 40.800f, 41.120f, 41.440f, 42.000f, 42.440f, 42.720f, 43.320f, 43.640f, 43.960f, 44.520f, 44.960f, 45.240f, 45.840f, 46.160f, 46.480f, 47.040f, 47.480f, 47.760f, 48.400f, 48.720f, 49.040f, 49.600f, 50.040f, 50.320f, 50.920f, 51.240f, 51.560f, 52.120f, 52.560f, 52.840f, 53.440f, 53.760f, 54.080f, 54.640f, 55.080f, 55.360f, 56.000f, 56.320f, 56.640f, 57.200f, 57.640f, 57.920f, 58.520f, 58.840f, 59.160f, 59.720f, 60.160f, 60.440f, 61.040f, 61.720f, 62.320f, 62.920f, 63.600f, 64.240f, 64.880f, 65.480f, 66.160f, 66.760f, 67.360f, 68.040f, 68.680f, 69.320f, 69.880f, 70.560f, 71.160f, 71.760f, 72.440f, 73.080f, 73.720f, 74.320f, 75.000f, 75.600f, 76.200f, 76.880f, 77.520f, 78.160f, 78.760f, 79.360f, 80.040f, 80.640f, 81.320f, 81.640f, 81.960f, 82.520f, 82.960f, 83.240f, 83.840f, 84.160f, 84.480f, 85.040f, 85.480f, 85.760f, 86.360f, 86.680f, 87.000f, 87.560f, 88.000f, 88.280f, 88.920f, 89.240f, 89.560f, 90.120f, 90.560f, 90.840f, 91.440f, 91.760f, 92.080f, 92.640f, 93.080f, 93.360f, 93.960f, 94.280f, 94.600f, 95.160f, 95.600f, 95.880f, 96.480f, 96.800f, 97.120f, 97.680f, 98.120f, 98.400f, 99.040f, 99.360f, 99.680f, 100.240f, 100.680f, 100.960f, 101.600f, 102.200f, 102.880f, 103.480f, 104.080f, 104.760f, 105.400f, 106.040f, 106.640f, 107.240f, 107.920f, 108.520f, 109.120f, 109.800f, 110.440f, 111.080f, 111.760f, 112.360f, 113.040f, 113.640f, 114.240f, 114.920f, 115.560f, 116.200f, 116.760f, 117.360f, 118.040f, 118.640f, 119.240f, 119.920f, 120.560f, 121.200f, 121.760f, 122.360f, 123.040f, 123.640f, 124.240f, 124.920f, 125.560f, 126.200f, 127.520f, 128.120f, 128.800f, 129.400f, 130.000f, 130.680f, 131.320f, 131.960f, 132.600f, 133.200f, 133.880f, 134.480f, 135.080f, 135.760f, 136.400f, 137.040f, 137.640f, 138.240f, 138.920f, 139.520f, 140.120f, 140.800f, 141.440f, 142.040f, 142.360f, 142.680f, 143.240f, 143.680f, 143.960f, 144.520f, 144.840f, 145.160f, 145.720f, 146.160f, 146.440f, 147.080f, 147.400f, 147.720f, 148.280f, 148.720f, 149.000f };

private int[] emplacementsManquants1 = { 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 4, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 1, 2, 3, 4, 3, 2, 1, 2, 3, 4, 3, 2, 1, 2, 1, 2, 3, 4, 3, 2, 1, 2, 3, 4, 3, 2, 1, 2, 1, 2, 3, 4, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 1, 2, 3, 4, 3, 2, 1, 2, 1, 2, 3, 4, 3, 2, 1, 2, 1, 2, 3, 4, 3, 2, 1, 2, 1, 2, 3, 4, 3, 2, 1, 2, 1, 2, 3, 4, 3, 2, 1, 2, 1, 2, 3, 4, 3, 2, 1, 2, 1, 2, 3, 4, 3, 2, 1, 2, 1, 2, 3, 4, 3, 2, 1, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2, 3, 2, 3, 4, 3, 2 };

    private float[] beatTimes2; 
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