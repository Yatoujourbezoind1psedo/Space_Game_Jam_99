using UnityEngine;

public class DisparitionApresXTemps : MonoBehaviour
{
    private float afficherElement; 
    [SerializeField] private float tempsDisparition = 3f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerPrefs.SetFloat("afficherElements", tempsDisparition);
        afficherElement = PlayerPrefs.GetFloat("afficherElements", tempsDisparition); //Récupère souvent temps disparition, sauf si on est déjà arrivé à une fin
        //Debug.Log(afficherElement); 


        
    }

    // Update is called once per frame
    void Update()
    {
        if (afficherElement > 0)
        {
            afficherElement -= Time.deltaTime;
            //Debug.Log(afficherElement); 
        } 
        else
        {
            this.gameObject.SetActive(false);   
            PlayerPrefs.SetFloat("afficherElements", 0f);
        }

    }
}
