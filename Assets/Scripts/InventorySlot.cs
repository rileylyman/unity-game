using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    private bool isFilled;
    public Image icon;
    public Button removeButton;
    Item item;

    private int slotNumber;

    private void Start() {
        slotNumber = -1;
        isFilled = false;
    }

    public void AddItem(Item newItem) {
        isFilled = true;

        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        isFilled = false;

        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() {
        if (GameManager.instance.playerEngine.IsPlayerGrounded()) {
            Inventory.instance.Remove(item, true);
        }
    }

    public void OnItemButton() {
        GameManager.instance.tooltipCanvas.enabled = false;
        if (Inventory.instance.IsReplacing() && isFilled) {
            if (icon == null) {
                GameManager.instance.ResetMouseGraphic();
            }
            else {
                GameManager.instance.SetMouseGraphic(icon.sprite);
            }
            Item pickingUp = item;
            Inventory.instance.Remove(item, false);
            Inventory.instance.Store(Inventory.instance.GetReplacementItem(), slotNumber);
            Inventory.instance.SetReplacing(pickingUp);

        }
        else if (Inventory.instance.IsReplacing() && !isFilled) {
            GameManager.instance.ResetMouseGraphic();
            Inventory.instance.Store(Inventory.instance.GetReplacementItem(), slotNumber);
            Inventory.instance.SetReplacing(null);
        }
        else if (isFilled) {
            GameManager.instance.SetMouseGraphic(icon.sprite);
            Inventory.instance.SetReplacing(item);
            Inventory.instance.Remove(item, false);
        }
    }

    public void TooltipEnter() {
        if (item != null) {
            GameManager.instance.tooltipCanvas.enabled = true;
            GameManager.instance.tooltipPosition.position = Input.mousePosition
                + new Vector3(30, -30);
            GameManager.instance.tooltipPosition.GetComponentInChildren<Text>().text = item.name;

            GameManager.instance.tooltipDescriptionPosition.GetComponentInChildren<Text>().text = item.description;
        }
        
    }

    public void TooltipExit() {
        GameManager.instance.tooltipCanvas.enabled = false;
    }

    public bool IsFull() {
        return isFilled;
    }

    public void SetSlotNumber(int value) {
        slotNumber = value;
    }
}
