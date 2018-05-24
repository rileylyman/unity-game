using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour {

    protected Collider2D playerCollider;
    protected bool isInteracting;

    protected Interactable current;

    protected void Start() {
        isInteracting = false;
        playerCollider = GetComponent<Collider2D>();
    }

    protected void Update() {
        HandleInput();

        if (isInteracting && current != null) {
            Debug.Log(2);
            current.Interact();
            isInteracting = false;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Interactable") {
            Debug.Log("1");
            current = collision.gameObject.GetComponent<Interactable>();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Interactable" 
                && collision.gameObject.GetComponent<Interactable>() == current) {
            current = null;
        } 
    }

    protected void HandleInput() {
        if (Input.GetButtonDown("Use")) {
            isInteracting = true;
        } else if (Input.GetButtonUp("Use")) {
            isInteracting = false;
        }
    }
}
