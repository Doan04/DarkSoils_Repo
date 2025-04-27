using UnityEngine;
using UnityEngine.UI;
public class StaminaBarScript : MonoBehaviour
{
    public Slider slider;

    public void updateStaminaValue(float stamina)
    {
        slider.value = stamina;
    }

}
