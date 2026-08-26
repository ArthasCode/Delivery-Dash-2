using UnityEngine;

public class Delivery : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    bool isPackage = false;
    [SerializeField]float destroyTime = 0.1f;
        void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Package"))
        {
            if(!isPackage){
                Debug.Log("Picked it!");
                isPackage = true;
                GetComponent<ParticleSystem>().Play();
                Destroy(collision.gameObject, destroyTime);
            } else{
                Debug.Log("I've already got a package to deliver!");
            }
        }

        if(collision.CompareTag("Costumer"))
        {
            if(isPackage == true){
            Debug.Log("Just in time!");
            GetComponent<ParticleSystem>().Stop();
            isPackage = false;
            Destroy(collision.gameObject, 0.5f);
            } else
            {
                Debug.Log("I need ice creaaaaam");
            }
        }
    }
}
