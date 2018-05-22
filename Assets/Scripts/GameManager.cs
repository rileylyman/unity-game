using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject player;

    [HideInInspector]
    public Texture2D defaultCursor;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one game manager at a time is not allowed.");
            return;
        }
        instance = this;
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
