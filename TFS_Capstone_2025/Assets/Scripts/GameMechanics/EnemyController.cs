using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthSystem))]
public class EnemyController : MonoBehaviour
{
    //TODO: #CodeReview: To Ryan. I get the sense that the coupling between the healthSystem, healthSlider, and this controller is a bit of a mess. I would love if you would rip it to shreds with examples of how to better organize this. Thank you good sir. Though I'm frightened you'll tell me it's over complicated and show me some dirt simple way to do it instead :) 
    
    private HealthSystem healthSystem; //system for tracking health and damage
    private GameObject healthSlider; //healthSlider Prefab to instantiate 
    [SerializeField] private GameObject healthSliderAttachPoint; //location to attach healthSlider prefab - could replace with tag and find in children
    private Slider uiHealthSliderBar; //portion of health slider to update with current health
    [SerializeField] private string healthSliderTag = "SliderBar";

    // Start is called before the first frame update
    void Start()
    {
        if (healthSliderAttachPoint == null)
        {
            Debug.LogError("Assign health slider attach point to enemy controller for " + this.name); 
        }

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDeath.AddListener(OnEnemyDeath);
        healthSystem.onHealthChanged.AddListener(OnHealthChanged);

        //instantiate generic slider prefab as health slider
        //create it at the position of the attach point (is this redundant when making it a child of the attach point?) and its rotation
        //make it a child of the attach point
        healthSlider = Instantiate(GameManager.Instance.sliderPrefab, healthSliderAttachPoint.transform.position, transform.rotation, healthSliderAttachPoint.transform);
        uiHealthSliderBar = GetComponentInChildren<Slider>();
        if (uiHealthSliderBar == null)
        {
            Debug.LogError("cannot access slider value for health bar on enemy " + this.name);
        }
        Debug.Log("Current health % = " + healthSystem.GetHealthPercentage());
        Debug.Log("Current healthbar value = " + uiHealthSliderBar.value);
        uiHealthSliderBar.value = healthSystem.GetHealthPercentage(); //init value to 100%

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

    void OnHealthChanged()
    {
        uiHealthSliderBar.value = healthSystem.GetHealthPercentage();
    }

    //helper to find slider value portion of health slider
    GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }

            // Recursively search through the child's children
            GameObject result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

}
