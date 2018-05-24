using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Item EMPTY = null;

    public static Inventory instance;

    private Transform playerTransform;
    private Collider2D playerCollider;

    public Canvas inventoryCanvas;
    private int capacity;
    private int numItems;

    private int toFill;

    private Item[] items;
    private InventorySlot[] slots;
    public LinkedList<GameObject> itemsLink;

    private Item currentlyReplacing;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Two instances of Inventory" +
                " are trying to be singletons at once!");
            return;
        }
        instance = this;
    }

    private void Start() {
        currentlyReplacing = null;
        playerTransform = GetComponent<Transform>();
        playerCollider = GetComponent<Collider2D>();

        inventoryCanvas.enabled = false;
        itemsLink = new LinkedList<GameObject>();
        slots = inventoryCanvas.GetComponentsInChildren<InventorySlot>();
        capacity = slots.Length;
        items = new Item[capacity];
        numItems = 0;
        toFill = 0;
    }

    private void Update() {
        HandleInput();
    }

    public void Store(Item item, int filling = -1) {
        if (filling == -1) { filling = toFill; }
        if (filling >= capacity) {
            return;
        }
        items[filling] = item;
        UpdateToFill(); numItems++;
        UpdateInventoryGui();
    }

    public void Remove(Item item, bool drop) {
        bool isRemoved = false;
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == item) {
                Item toRemove = items[i];
                items[i] = EMPTY;
                isRemoved = true;
                numItems--;

                if (drop) {
                    toRemove.Drop();
                }  
            }
        }
        if (isRemoved) {
            UpdateToFill();
            UpdateInventoryGui();
        }
    }

    private void UpdateInventoryGui() {
        for (int i = 0; i < capacity; i++) {
            slots[i].SetSlotNumber(i); // potentially slow to do every update call
            if (items[i] != EMPTY) {
                slots[i].AddItem(items[i]);
            }
            else {
                slots[i].ClearSlot();
            }
        }
    } 

    private void HandleInput() {
        if (Input.GetButtonDown("ToggleInventory")) { inventoryCanvas.enabled = !inventoryCanvas.enabled; }
        if (Input.GetButtonDown("Use")) {
            if (itemsLink.Count == 0) {
                return;
            }
            else {
                var toPickUp = itemsLink.First.Value.GetComponent<ItemPickup>();
                itemsLink.RemoveFirst();
                toPickUp.PickUp();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Collectable") {
            itemsLink.AddLast(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Collectable") {
            itemsLink.Remove(collision.gameObject);
        }
    }

    private void UpdateToFill() {
        for (int i = 0; i < capacity; i++) {
            if (items[i] == EMPTY) {
                toFill = i;
                return;
            }
        }
        toFill = capacity;
    }

    public void SetReplacing(Item item) {
        currentlyReplacing = item;
    }

    public Item GetReplacementItem() {
        return currentlyReplacing;
    }

    public bool IsReplacing() {
        return currentlyReplacing != null;
    }

    public Item[] GetItemsArray() {
        return items;
    }
}
