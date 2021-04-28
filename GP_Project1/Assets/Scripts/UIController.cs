using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Image healthImage;

    [SerializeField]
    private Color fullHealth=Color.green, noHealth=Color.red;

    public void UpdateHealthUI(float val,float maxhealth)
    {
        if (val <= 0)
        {
            healthImage.enabled = false;
        }
        else
        {
            healthImage.color = Color.Lerp(fullHealth, noHealth, Mathf.Clamp01(val / maxhealth));
        }
        
    }
}
