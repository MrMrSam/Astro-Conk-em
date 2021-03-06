﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboJuiceScript : MonoBehaviour {

    public Text myText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (ScoreManager.scoreSingleton.getComboNo() > 1)
        {
            myText.transform.localScale = Vector3.Lerp(myText.transform.localScale, Vector3.one * 0.5f * (1 + (ScoreManager.scoreSingleton.getComboNo() * 0.1f)), 8 * Time.deltaTime);
        }
        else
        {
            myText.transform.localScale = Vector3.Lerp(myText.transform.localScale, Vector3.one * 0, 4 * Time.deltaTime);
        }
	}

    public void UpdateComboDisplay(int increase)
    {
        myText.transform.localScale *= 1.25f;

    }

    public void ResetCombo()
    {

    }
}
