using UnityEngine;

public class SpawnManagement : MonoBehaviour
{
    [SerializeField] private GameObject[] meteors; 
    [SerializeField] public float globalSpeed = 10f; // La vitesse unique
    [SerializeField] public AudioSource musicSource; // La musique à glisser ici !

    [SerializeField] private Transform chemin1, chemin2, chemin3, chemin4; 
    private Transform[] pointsCh1, pointsCh2, pointsCh3, pointsCh4; 

    void Start()
    {
        pointsCh1 = GetPoints(chemin1);
        pointsCh2 = GetPoints(chemin2);
        pointsCh3 = GetPoints(chemin3);
        pointsCh4 = GetPoints(chemin4);
    }

    private Transform[] GetPoints(Transform parent) {
        Transform[] pts = new Transform[parent.childCount];
        for(int i = 0; i < parent.childCount; i++) pts[i] = parent.GetChild(i);
        return pts;
    }

    public void SpawnMeteor(int meteorX, Transform[] pointsChX)
    {
        if(meteors == null || meteors.Length == 0) return;

        GameObject mete = Instantiate(meteors[meteorX], pointsChX[0].position, meteors[meteorX].transform.rotation);
        FollowPath sc = mete.GetComponent<FollowPath>();
        
        sc.points = pointsChX;
        sc.speed = globalSpeed;
        // On donne la musique à la météorite pour son debug
        sc.SetMusicSource(musicSource); 
    }

    public void SpawnMeteorsExceptChemin(int oubli)
    {
        Transform[][] listes = { pointsCh1, pointsCh2, pointsCh3, pointsCh4 };
        for (int i = 0; i < listes.Length; i++)
        {
            if ((i + 1) != oubli) SpawnMeteor(0, listes[i]);
        }
    }

    public float GetTravelTime(int cheminIndex, float speed)
    {
        Transform[] points = null;
        switch(cheminIndex) {
            case 1: points = pointsCh1; break;
            case 2: points = pointsCh2; break;
            case 3: points = pointsCh3; break;
            case 4: points = pointsCh4; break;
        }

        if (points == null || points.Length < 3) return 0f;

        float distanceTotale = 0f;
        // Distance jusqu'au point 3 (index 2)
        for (int i = 0; i < 2; i++)
        {
            distanceTotale += Vector3.Distance(points[i].position, points[i+1].position);
        }
        return distanceTotale / speed;
    }
}