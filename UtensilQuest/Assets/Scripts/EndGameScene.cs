﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameScene : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		this.GetComponent<Text> ().text = PlayerPrefs.GetString ("Reason");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0) || Input.anyKeyDown)
		{
			Application.LoadLevel(0);
		}
	}
}
