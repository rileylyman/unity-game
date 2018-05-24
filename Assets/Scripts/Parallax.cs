using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    Camera cam;

    public List<GameObject> parts;
    private List<Transform> layers;

    private Vector2 previousPosition;

    private float shiftSpeed = 0.07f;

    private void Start() {
        cam = GetComponent<Camera>();
        previousPosition = transform.position;
        layers = ProcessLayers(parts);
    }

    private void Update() {
        Vector2 currentPosition = transform.position;
        if (currentPosition.x == previousPosition.x) {
            return;
        }
        for (int i = 0; i < layers.Count; i++) {
            int layer = i + 1;
            float dx = currentPosition.x - previousPosition.x;
            Vector2 pos = layers[i].position;
            pos.x -= dx / (layer) * shiftSpeed;
            layers[i].position = pos;
        }
        previousPosition = currentPosition;
    }

    private List<Transform> ProcessLayers(List<GameObject> parts) {
        var ret = new List<Transform>();
        foreach (var part in parts) {
            ret.Add(part.GetComponent<Transform>());
        }
        return ret;
    }
}
