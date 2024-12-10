using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class EnemyController : MonoBehaviour
{
    private HealthSystem healthSystem;
    private GameObject healthSlider;
    [SerializeField] private GameObject healthSliderAttachPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        if (healthSliderAttachPoint == null)
        {
            Debug.LogError("Assign health slider attach point to enemy controller for " + this.name); 
        }

        //instantiate generic slider prefab as health slider
        //create it at the position of the attach point (is this redundant when making it a child of the attach point?) and its rotation
        //make it a child of the attach point
        healthSlider = Instantiate(GameManager.Instance.sliderPrefab, healthSliderAttachPoint.transform.position, transform.rotation, healthSliderAttachPoint.transform); 

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDeath.AddListener(OnEnemyDeath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Example method to simulate taking damage
    public void SimulateDamage()
    {
        healthSystem.TakeDamage(10f);  // Simulate taking 10 damage
    }

    //TODO: This might get called when ANY enemy dies, and therefore called once per enemy still alive. Should listen for this event in a level or combat manager instead in the future. Leaving here for early testing in VGP145
    void OnEnemyDeath()
    {
        // Handle enemy death (e.g., play death animation, destroy enemy)
        Debug.Log("Enemy has died!");
    }

}
