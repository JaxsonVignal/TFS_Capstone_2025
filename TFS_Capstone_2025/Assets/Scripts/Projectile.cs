using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; 
    public float lifeTime = 5f; 
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Projectile hit the player");
           
        }

        Destroy(gameObject);
    }
}
