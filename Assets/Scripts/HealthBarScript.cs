using UnityEngine.UI;
using UnityEngine;
public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    //public void setMaxHealth(float health)
    //{
    //    slider.maxValue = health;
    //    slider.value = health;
    //}

    public void updateHealthValue(float health)
    {
        slider.value = health;
    }
}
