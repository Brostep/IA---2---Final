using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    public PlayerController[] players;
    public Camera[] cameras;
    public Canvas generalCanvas;

    private int _activeCamera;

    private void Awake() {
        foreach (var c in cameras) {
            c.enabled = false;
        }

        cameras[_activeCamera].enabled = true;
        generalCanvas.enabled = false;
    }

    public void WinGame(string playerName) {
        foreach (var p in players) {
            p.Stop();
        }

        Debug.Log(playerName + " won!");
        var text = generalCanvas.GetComponentInChildren<Text>();
        text.text = playerName + " won!";
        generalCanvas.enabled = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            _activeCamera++;

            if (_activeCamera >= cameras.Length)
                _activeCamera = 0;
            
            foreach (var c in cameras) {
                c.enabled = false;
            }

            cameras[_activeCamera].enabled = true;
        }
    }
}
