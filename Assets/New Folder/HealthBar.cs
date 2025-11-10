using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private float smoothSpeed = 5f;
    private float targetValue;



    // Set the slider max value (for new health systems)
    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        targetValue = maxHealth;

        slider.value = maxHealth;
    }

    // Update the slider when health changes
    public void SetHealth(int currentHealth)
    {
        targetValue = currentHealth;

    }

    void Update()
    {
        // Smoothly interpolate towards the target health value
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothSpeed);
    }

}