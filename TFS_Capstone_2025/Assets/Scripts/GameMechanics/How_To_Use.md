How to Use:
1. Attach the Script to a GameObject:
	Drag and drop the HealthSystem.cs script onto any GameObject you want to have health (e.g., player, enemy, NPC, etc.).

2. Configure Health Values:
	In the Unity Inspector, set the maxHealth value to your desired amount. The currentHealth will initialize to this value when the 	game starts.

3. Taking Damage:
	Call TakeDamage(float amount) on the health system from another script to deal damage (e.g., when the object collides with a 	weapon 	or enemy).

	healthSystem.TakeDamage(20f); // Deals 20 damage

4. Healing:
	Call Heal(float amount) to heal the object.
	
	healthSystem.Heal(15f); // Heals 15 health

5. Event Hooks:
	You can hook up the onHealthChanged and onDeath events to trigger actions such as UI updates, death animations, etc.
	For example, create a new method in another script:

public void UpdateHealthBar()
{
    float healthPercentage = healthSystem.GetHealthPercentage();
    healthBar.fillAmount = healthPercentage;
}

	Then, drag this method into the Unity Inspector to bind to the onHealthChanged event of the HealthSystem component.

