using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    Image healthImage;

    [SerializeField]
    private Color fullHealth, noHealth;

    public void UpdateHealthUI(float val,float maxhealth)
    {
        healthImage.color = Color.Lerp(fullHealth, noHealth, Mathf.Clamp01(val/maxhealth));
    }
}
