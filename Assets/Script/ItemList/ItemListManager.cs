using System.Collections.Generic;
using UnityEngine;

public class ItemListManager : MonoBehaviour {
    /***** NEEDS AN OVEHAUL *****/
    public ObjectPoolScript itemButtonPool;
    public ObjectPoolScript itemEquipPool;
    public InvenGridManager invenManager;
    public LoadItemDatabase itemDB;
    public SortAndFilterManager sortManager;

    public float iconSize;
    
    public List<ItemClass> startItemList; // created and initialized on LoadSaveItems
    public List<GameObject> currentButtonList;
    public List<ItemClass> currentItemList;

    private Transform contentPanel;

    private void Start()
    {
        contentPanel = this.transform;
        //lists are initialize on SortAndFilterManager
    }

    //*** rework the add item to list
    // make a proper add item to list with sort and filter in mind

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && invenManager.selectedButton != null) //right click to return item to list if item is from list
        {
            invenManager.RefrechColor(false);
            invenManager.selectedButton.GetComponent<CanvasGroup>().alpha = 1f;
            invenManager.selectedButton = null;
            itemEquipPool.ReturnObject(ItemScript.selectedItem);
            ItemScript.ResetSelectedItem();
        }
    }

    public void AddSelectedItemToList()//used on scrollview pointerclick trigger
    {
        if (invenManager.selectedButton == null && ItemScript.selectedItem != null) //add item to list if item is not from list
        {
            ItemClass item = ItemScript.selectedItem.GetComponent<ItemScript>().item;
            sortManager.AddItemToList(item);
            itemEquipPool.ReturnObject(ItemScript.selectedItem);
            ItemScript.ResetSelectedItem();
        }
    }

    public void PopulateList(List<ItemClass> passedItemlist)
    {
        if (currentButtonList.Count > 0)
        {
            for (int i = currentButtonList.Count - 1; i >= 0; i--)//removes all buttons
            {
                RemoveButton(currentButtonList[i]);
            }
        }
        for (int j = 0; j < passedItemlist.Count; j++)//populates list
        {
            AddButton(passedItemlist[j]);
        }
    }

    public void AddButton(ItemClass addItem)
    {
        GameObject newButton = itemButtonPool.GetObject();
        newButton.transform.SetParent(contentPanel);
        newButton.GetComponent<RectTransform>().localScale = Vector3.one;
        newButton.GetComponent<ItemButtonScript>().SetUpButton(addItem, this);
        currentButtonList.Add(newButton);
    }

    public void RemoveButton(GameObject buttonObj)
    {
        buttonObj.GetComponent<CanvasGroup>().alpha = 1f;
        currentButtonList.Remove(buttonObj);
        itemButtonPool.ReturnObject(buttonObj);
    }

    public void RevomeItemFromList(ItemClass itemToRemove)
    {//used to remove from list when placing item on grid or deleting item
        for (int i = currentItemList.Count - 1; i >= 0; i--)
        {
            if (currentItemList[i] == itemToRemove)
            {
                currentItemList.RemoveAt(i);
                break;//temporary for now
            }
        }
    }

}
