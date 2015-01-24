﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

//this script determines what role you are and then sets up your mechanics accordingly
public class GameManager : MonoBehaviour
{
	public bool OculusGame = false;
    public enum role
    {
        spy, handler,
    }
    public role myRole;
    public List<Camera> secCams = new List<Camera>();
    public GameObject theSpy;
    private int camIndex = 0;
    public GameObject OculusPlayer;
    public GameObject NonOcPlayer;
    public Transform spawnPoint;
    // Use this for initialization
    void Start()
    {
        if (Network.isServer) //I'm the spy, spawn me.
        {
            myRole = role.spy;
			if(OculusGame)
			{
				theSpy = Network.Instantiate (OculusPlayer, spawnPoint.position, spawnPoint.rotation, 0) as GameObject;
			}
			else
			{
				theSpy = Network.Instantiate (NonOcPlayer, spawnPoint.position, spawnPoint.rotation, 0) as GameObject;
			}
			foreach (Camera cam in secCams)
			{
				cam.enabled = false;
				cam.GetComponent<AudioListener>().enabled = false;
			}
		}
        else  //I'm the handler, set up the cameras
        {
            myRole = role.handler;
			//start with the cameras off.
			foreach (Camera cam in secCams)
			{
				cam.enabled = false;
				cam.GetComponent<AudioListener>().enabled = false;
			}
			//turn the first one on.
			secCams[0].enabled = true;
			secCams[0].GetComponent<AudioListener>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //the mechanics methods are running out of the update function based on the role determined, so as not to bloat the update function.
        //im a handler, these are my mechanics
        if (myRole == role.handler)
        {
            HandlerMechanics();
        }

        //im the spy, these are my mechanics
        if (myRole == role.spy)
        {
            SpyMechanics();
        }
    }

    //HACKER SECTION
    void HandlerMechanics()
    {
		//double check the player is turned off.
		//TurnOffPlayer ();

        //press tab to cycle through the cameras
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            secCams[camIndex].enabled = false;
            secCams[camIndex].GetComponent<AudioListener>().enabled = false;
            //cycle through as many cameras as we have.
            if (camIndex < secCams.Count - 1)
            {
                camIndex++;
            }
            else
            {
                camIndex = 0;
            }
            secCams[camIndex].enabled = true;
            secCams[camIndex].GetComponent<AudioListener>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && secCams.Count - 1 >= 0 && secCams[0] != null)
        {
            SetCam(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && secCams.Count - 1 >= 1 && secCams[1] != null)
        {
            SetCam(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && secCams.Count - 1 >= 2 && secCams[2] != null)
        {
            SetCam(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && secCams.Count - 1 >= 3 && secCams[3] != null)
        {
            SetCam(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && secCams.Count - 1 >= 4 && secCams[4] != null)
        {
            SetCam(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && secCams.Count - 1 >= 5 && secCams[5] != null)
        {
            SetCam(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && secCams.Count - 1 >= 6 && secCams[6] != null)
        {
            SetCam(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) && secCams.Count - 1 >= 7 && secCams[7] != null)
        {
            SetCam(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) && secCams.Count - 1 >= 8 && secCams[8] != null)
        {
            SetCam(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) && secCams.Count - 1 >= 9 && secCams[9] != null)
        {
            SetCam(9);
        }
    }

    //sets the camera to the one we want.
    void SetCam(int CamNumber)
    {
        secCams[camIndex].enabled = false;
        secCams[camIndex].GetComponent<AudioListener>().enabled = false;
        camIndex = CamNumber;
        secCams[camIndex].enabled = true;
        secCams[camIndex].GetComponent<AudioListener>().enabled = true;
    }

	//fallback function
	void TurnOffPlayer()
	{
		if (theSpy == null) //set theSpy if it's null, just to avoid errors, especially with the next part.
		{
			theSpy = GameObject.Find("Player(Clone)");

			//turn off the Oculus prefab components if we're playing an oculus game
			if(OculusGame)
			{
				theSpy.SetActive(false);
				theSpy.GetComponent<OVRGamepadController>().enabled = false;
				theSpy.GetComponent<OVRPlayerController>().enabled = false;
				theSpy.GetComponent<OVRMainMenu>().enabled = false;
				theSpy.GetComponentInChildren<OVRCameraRig>().enabled = false;
				theSpy.GetComponentInChildren<OVRManager>().enabled = false;
				theSpy.GetComponentInChildren<OVRScreenFade>().enabled = false;
			}
			else //or the fps controller components if we're not
			{
				theSpy.GetComponent<MouseLook>().enabled = false;
				theSpy.GetComponent<CharacterMotor>().enabled = false;
				theSpy.GetComponent<FPSInputController>().enabled = false;
			}
			//turn off the player's cameras
			foreach (Camera cam in theSpy.GetComponentsInChildren<Camera>())
			{
				cam.enabled = false;
			}
		}
	}

    //SPY SECTION
    void SpyMechanics()
    {

    }




    //Networking section
    void OnPlayerConnected(NetworkPlayer connectingPlayer)
    {
        //We don't accept your kind here (during the game, anyway)
        Network.CloseConnection(connectingPlayer, true);
        Debug.Log("I just kicked: " + connectingPlayer.ipAddress);
    }

    //Client disconnection - just go back to intro scene
    void OnDisconnectedFromServer(NetworkDisconnection disconnect)
    {
        Application.LoadLevel("1-Splash");
    }
}
