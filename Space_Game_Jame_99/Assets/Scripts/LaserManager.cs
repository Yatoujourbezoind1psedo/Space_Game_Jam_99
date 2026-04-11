using UnityEngine;
using UnityEngine.InputSystem;

public class LaserManager : MonoBehaviour
{
    [SerializeField] private GameObject laser; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed) //action du laser
        {
            laser.SetActive(true);
        }
        else //je le laisse ici pour rajouter un son de fin du laser pt
        {
            laser.SetActive(false);
        }
    }
}
