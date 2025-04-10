using UnityEngine;
using UnityEngine.UI;
public class CropBarScript : MonoBehaviour
{
    public Slider progSlider;
    public Slider healthSlider;
    public void updateCropValue(float crop)
    {
        progSlider.value = crop;
    }
    public void updateCropHealthValue(float health)
    {
        healthSlider.value = health;
    }
}
