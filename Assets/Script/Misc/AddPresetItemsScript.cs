using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPresetItemsScript : MonoBehaviour {

    private List<ItemClass> itemsToAdd = new List<ItemClass>();
    private Button button;

    public InvenGridManager gridManager;
    public ItemListManager listManager;
    public SortAndFilterManager sortManager;
    public Transform dropParent;
    public List<GameObject> gridItems;

    private void Start()
    {
        for (int i = 0; i < 17; i++)
        {
            ItemClass item = new ItemClass();
            item.GlobalID = i;
            item.Level = Random.Range(0, 101);
            item.qualityInt = Random.Range(0, 4);
            ItemClass.SetItemValues(item);
            itemsToAdd.Add(item);
        }
        button = GetComponent<Button>();
        button.onClick.AddListener(AddItems);
    }

    private void AddItems()
    {
        listManager.currentItemList.AddRange(itemsToAdd);
        sortManager.SortAndFilterList();
    }

    public void RemoveListButtons()
    {
        listManager.currentItemList.Clear();
        if (listManager.currentButtonList.Count > 0)
        {
            for (int i = listManager.currentButtonList.Count - 1; i >= 0; i--)//removes all buttons
            {
                listManager.RemoveButton(listManager.currentButtonList[i]);
            }
        }
    }

    public void RemoveGridItems()
    {
        foreach (Transform child in dropParent.transform)
        {
            gridItems.Add(child.gameObject);
        }
        for (int i = gridItems.Count - 1; i >= 0; i--)//removes all buttons
        {
            gridItems[i].GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            listManager.itemEquipPool.ReturnObject(gridItems[i]);
        }
        gridItems.Clear();
        //clears all slotGrid properties
        IntVector2 gridSize = gridManager.gridSize;
        GameObject[,] slotGrid = gridManager.slotGrid;
        SlotScript instanceScript;
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                slotGrid[x, y].GetComponent<Image>().color = SlotColorHighlights.Gray;
                instanceScript = slotGrid[x, y].GetComponent<SlotScript>();
                instanceScript.storedItemObject = null;
                instanceScript.storedItemClass = null;
                instanceScript.storedItemSize = IntVector2.Zero;
                instanceScript.storedItemStartPos = IntVector2.Zero;
                instanceScript.isOccupied = false;
            }
        }
    }

}
