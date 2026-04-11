using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float distance = 2; 
    [SerializeField] private int nbCanaux = 4; 
    private float xPlayer, xOrigine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xPlayer = transform.position.x; 
        xOrigine = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame && xPlayer > xOrigine) //gauche + peut pas aller en dessous de point de départ
        {
            transform.position -= new Vector3(distance, 0f, 0f);
            xPlayer -= distance;
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && xPlayer < xOrigine * (distance * (nbCanaux - 1))) //droite + paramétrer pour aller que dans 5 canaux XORIGINE PAS ZERO
        {
            transform.position += new Vector3(distance, 0f, 0f);
            xPlayer += distance; 
        }
    }


}
