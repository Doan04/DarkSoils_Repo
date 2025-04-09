using UnityEngine;
using UnityEngine.UI;
public class StaminaBarScript : MonoBehaviour
{
    public Slider slider;

    //public void set(float health)
    //{
    //    slider.maxValue = health;
    //    slider.value = health;
    //}

    public void updateStaminaValue(float stamina)
    {
        slider.value = stamina;
    }

}
