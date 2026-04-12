using UnityEngine;

public class SpawnManagement : MonoBehaviour
{
    [SerializeField] private GameObject[] meteors; 

    [SerializeField] private Transform chemin1, chemin2; 
    private Transform[] pointsCh1, pointsCh2; //Listes des points des chemins 



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialisation des points présents dans les chemins 
        pointsCh1 = new Transform[chemin1.childCount]; 
        for(int i = 0; i < chemin1.childCount; i++)
        {
            pointsCh1[i] = chemin1.GetChild(i); 
        }


        pointsCh2 = new Transform[chemin2.childCount]; 
        for(int i = 0; i < chemin2.childCount; i++)
        {
            pointsCh2[i] = chemin2.GetChild(i); 
        }


        SpawnMeteor(0, pointsCh1); 
        SpawnMeteor(1, pointsCh2); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnMeteor(int meteorX, Transform[] pointsChX)
    {
        GameObject mete = Instantiate(meteors[meteorX], pointsChX[0].transform.position, meteors[meteorX].transform.rotation);  //Instantie le météore X dnas la lsite au point 0 du chemin donné
        FollowPath sc = mete.GetComponent<FollowPath>(); // Récupère le script Follow Path
        sc.points = pointsChX; //Lui donne les points qu'il doit suivre
    }

    //Créer fonction spawnant un meteor dans un chemin aléatoire (mais risque qu'emplacement l'un sur l'autre donc à voir)

    //Créer fonction spawnant un nombre défini de météorite en même temps mais les emplacement sont aléatoires  (passer par tableau de path donc tableau de tableau de transform ?)
    /*private void SpawnXMeteor(int nbMete)
    {
        for(int i = 0; i < nbMete; i++)
        {
            
        }
    }*/
}
