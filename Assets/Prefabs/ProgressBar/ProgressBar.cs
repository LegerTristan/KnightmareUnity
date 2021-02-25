using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    Image bar;
    Text txt;

    public Color AlerteColor = Color.red;
    Color startColor;

    public float alerte = 25f;

    private float val;
    public float Val
    {
        get
        {
            return val;
        }

        set
        {
            val = value;
            val = Mathf.Clamp(val, 0, 100);
            UpdateValue();
        }
    }

	void Awake () {

        bar = transform.Find("Bar").GetComponent<Image>();
        txt = bar.transform.Find("Text").GetComponent<Text>();
        startColor = bar.color;
        Val = 100;        

	}
	
	
	void UpdateValue() {

        txt.text = (int)val + "%";
        bar.fillAmount = val / 100;

        if(val<=alerte)
        {
            bar.color = AlerteColor;
        }
        else
        {
            bar.color = startColor;
        }
	}
    
}
