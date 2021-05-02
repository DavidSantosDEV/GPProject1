using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VisualsBehaviour : MonoBehaviour
{
    [SerializeField]
    Image healthImage;

    [SerializeField]
    private Color fullHealth=Color.green, midHealth=Color.yellow, noHealth=Color.red;

    /*private void Start()
    {
        //Doing image = fullhealth doesnt work cause unity is ret*rded
        healthImage.color = new Color( fullHealth.r,fullHealth.g,fullHealth.b);
    }*/
    /*float var=0;
    private void Update() //Debug only
    {
        var = Mathf.Clamp01(Time.deltaTime + var*0.01f);
        if (var == 1)
        {
            healthImage.color = Color.green;
            var = 0;
        }

        Color target = healthImage.color;

        target= Color.Lerp(target, noHealth,var);
        Debug.Log(target); 
        healthImage.color = new Color(target.r, target.g, target.b);
    }*/

    public void UpdateHealthUI(float val,float maxhealth)
    {
        float ammount = val / maxhealth;

        if (ammount <= 0)
        {
            healthImage.enabled = false;
            return;
        }

        Color toUse=fullHealth;
        if (ammount>0.6f)
        {
            toUse = fullHealth;
        }
        else if((ammount>0.3f) && (ammount<=0.6f))
        {
            toUse = midHealth;
        }
        else
        {
            toUse = noHealth;
        }

        healthImage.color = new Color(toUse.r, toUse.g, toUse.b);

        //Original plan was to use color.lerp, didnt work very well
        /*if (val <= 0) 
        {
            healthImage.enabled = false;
        }
        else
        {
            healthImage.color = Color.Lerp(healthImage.color, noHealth, Mathf.Clamp01(val / maxhealth));
        }*/
        
    }

    public void ResetComponent()
    {
        healthImage.enabled = true;
    }
}
