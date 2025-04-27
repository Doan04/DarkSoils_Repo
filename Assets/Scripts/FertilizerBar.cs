using UnityEngine;
using UnityEngine.UI;   
public class FertilizerBar : MonoBehaviour
{
    public Slider slider;

    public void updateFertValue(float fert)
    {
        slider.value = fert;
    }
}
