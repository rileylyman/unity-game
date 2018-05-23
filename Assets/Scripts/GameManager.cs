﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject player;
    public Canvas menu;

    [HideInInspector]
    public PlayerEngine playerEngine;

    [HideInInspector]
    public Texture2D defaultCursor;


    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one game manager at a time is not allowed.");
            return;
        }
        instance = this;
        menu.enabled = false;
    }

    private void Start() {
        playerEngine = player.GetComponent<PlayerEngine>();
    }

    private void Update() {
        if (Input.GetButtonDown("Menu")) {
            menu.enabled = !menu.enabled;
            if (menu.enabled) { Pause(); }
            else { Unpause(); }
        }
    }

    private void Pause() {

    }

    private void Unpause() {

    }

    public Vector3 PlayerPosition() {
        return player.transform.position;
    }

    public void SetMouseGraphic(Sprite graphic) {
        Cursor.SetCursor(graphic.texture, new Vector2(0, 0), CursorMode.ForceSoftware);
    }

    public void ResetMouseGraphic() {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
} 
