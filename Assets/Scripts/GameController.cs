using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int growCounter;
    public Text growCounterText;
    public Text winText;
    public Image endImage;
    public Image loco;

	// Use this for initialization
	void Start () {

        growCounter = 1;
        growCounterText.text = growCounter.ToString();
        winText.enabled = false;
        endImage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (growCounter <= 0)
        {
            winText.enabled = true;
            winText.text = "YOU HAVE LOST.";
            endImage.enabled = true;
            growCounterText.enabled = false;
            loco.enabled = false;
        }
	
	}

    public void UpdateGrowCounter(int value)
    {
        growCounter += value;
        growCounterText.text = growCounter.ToString();
    }

    public void Finish()
    {
        winText.enabled = true;
        winText.text = "YOU HAVE WON AND COLLECTED " + growCounter.ToString() + " LOCOS!";
        endImage.enabled = true;
        growCounterText.enabled = false;
        loco.enabled = false;
    }
}
