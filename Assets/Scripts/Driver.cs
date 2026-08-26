using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Driver : MonoBehaviour
{
    [SerializeField]float currentSpeed = 12f;
    [SerializeField]float steerSpeed = 200f;
    [SerializeField]float boosterSpeed = 20f;
    [SerializeField]float usualSpeed = 12f;
    [SerializeField]TMP_Text boosterText;

    private void Start() {
        boosterText.gameObject.SetActive(false);    
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Booster")){
            currentSpeed = boosterSpeed;
            Destroy(collision.gameObject, 0.1f);
            Debug.Log("Wohoo!");
            boosterText.gameObject.SetActive(true);
        
    }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.CompareTag("WorldCollision")){
            currentSpeed = usualSpeed;
            boosterText.gameObject.SetActive(false);
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        float move = 0f;
        float steer = 0f;

        if (Keyboard.current.wKey.isPressed)
        {
            move = 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            move = -1f;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            steer = 1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            steer = -1f;
        }


        float moveAmount = move * currentSpeed * Time.deltaTime;
        float steerAmount = steer * steerSpeed * Time.deltaTime;
        transform.Translate(0, moveAmount, 0);
        transform.Rotate(0, 0, steerAmount);
    }
}
