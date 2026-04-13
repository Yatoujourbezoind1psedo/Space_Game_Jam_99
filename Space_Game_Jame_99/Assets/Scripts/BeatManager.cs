//https://youtu.be/gIjajeyjRfE?si=JYEevX-JuxWgN5mr

//ATTENTION : fonctionne que pour les beats, si on veut truc en musique voir koreographer (46 balles sadj) ou MIDI jsp quoi 
using UnityEngine;
using UnityEngine.Events; 

public class BeatManager : MonoBehaviour
{
    [SerializeField] private float _bpm; //nombre de beats par minute
    [SerializeField] private AudioSource _audioSource; //Le tiret devant permet d'avoir visibilité juste dans la classe
    [SerializeField] private Intervals[] _intervals; //liste des événements à déclencher à tel ou tel inteval (beat normal, demi-beat ou quart de beat)

    [SerializeField] private GameManager gameManager; 

    private void Update()
    {
        if (gameManager.isGameRunning)
        {
            foreach (Intervals interval in _intervals) //A chaque frame, boucle sur tous les intervals
            {
                //audioSource.timeSample = nombre d'échantillons audio joués
                //audiosource.clip.frequency = nombre d'échantillons par seconde ; timeSamples / frequency = temps en secondes 
                // getIntervalLenght = nombre d'intervalles écoulés 
                float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm)));
                interval.CheckForNewInterval(sampledTime);
            }
        }
        
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float _steps; //permet d'avoir des moitiés de beat ou des quarts de beats (subdivision du beat) 
    [SerializeField] private UnityEvent _trigger; //pour activer facilement d'autres événements 
    private int _lastInterval; //Conserve le dernier beat détecté  

    //Récupère temps d'un beat avec subdivisions (ex : 120 bpm = 0.5 sec par beat, 2 steps = 0.25 sec)
    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * _steps); //interval = nombre secondes dans une minute / (nombre de beats par minute * la mesure qu'on veut (genre que ça arrive tous les demi intervales ou pas par exemple)(apparemment appelé note length))
    }

    public void CheckForNewInterval (float interval)
    {
        if(Mathf.FloorToInt(interval) != _lastInterval) //on arrondit au int inférieur pour check tous les nombres entiers sion les deux peuvent être considérés comme différent parce qu'on travaille en fps donc float per second alors problème de décimal alors que c'est pas le cas
        //donc on est passé à un nouvel interval 
        {
            _lastInterval = Mathf.FloorToInt(interval); //conserve le dernier interval 
            _trigger.Invoke(); //action en rythme 
        }
    }
}
