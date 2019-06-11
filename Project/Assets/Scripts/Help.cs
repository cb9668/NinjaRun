using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour {

    public Button backButton;

	// Use this for initialization
	void Start () {
        GameManager.instance.mainText = GameObject.Find("mainText").GetComponent<Text>();

        if (backButton)
        {
            backButton.onClick.AddListener(GameManager.instance.LoadStart);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
