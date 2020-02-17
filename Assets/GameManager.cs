using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private static  GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public PlayerController[] players;
    public Camera[] cameras;
    public Canvas generalCanvas;

    int activeCamera;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        foreach (var c in cameras)
        {
            c.enabled = false;
        }
        cameras[activeCamera].enabled = true;
        generalCanvas.enabled = false;
    }

    public void WinGame(string playerName)
    {
        foreach (var p in players)
        {
            p.Stop();
        }
        Debug.Log(playerName + " has WON!!!");
        var text = generalCanvas.GetComponentInChildren<Text>();
        text.text = playerName + " has WON!!!";
        generalCanvas.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activeCamera++;
            if(activeCamera >= cameras.Length)
            {
                activeCamera = 0;
            }
            foreach (var c in cameras)
            {
                c.enabled = false;
            }
            cameras[activeCamera].enabled = true;
        }
    }
}
