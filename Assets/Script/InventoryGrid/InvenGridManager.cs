using UnityEngine;
using UnityEngine.UI;

public class InvenGridManager : MonoBehaviour {

    public GameObject[,] slotGrid;
    public GameObject highlightedSlot;
    public Transform dropParent;
    [HideInInspector]
    public IntVector2 gridSize;

    public ItemListManager listManager;
    public GameObject selectedButton;

    private IntVector2 totalOffset, checkSize, checkStartPos;
    private IntVector2 otherItemPos, otherItemSize; //*3

    private int checkState;
    private bool isOverEdge = false;

    public ItemOverlayScript overlayScript;

    /* to do list
     * make the ColorChangeLoop on swap items take arrguements fron the other item, not hte private variables *1
     * transfer the CheckArea() and SlotCheck() into inside RefreshColor() *2
     * have *3 be local variables of CheckArea(). SwapItem() uses the variable, may need to rewrite.
     */
    private void Start()
    {
        ItemButtonScript.invenManager = this;
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (highlightedSlot != null && ItemScript.selectedItem != null && !isOverEdge)
            {
                switch (checkState)
                {
                    case 0: //store on empty slots
                        StoreItem(ItemScript.selectedItem);
                        ColorChangeLoop(SlotColorHighlights.Blue, ItemScript.selectedItemSize, totalOffset);
                        ItemScript.ResetSelectedItem();
                        RemoveSelectedButton();
                        break;
                    case 1: //swap items
                        ItemScript.SetSelectedItem(SwapItem(ItemScript.selectedItem));
                        SlotSectorScript.sectorScript.PosOffset();
                        ColorChangeLoop(SlotColorHighlights.Gray, otherItemSize, otherItemPos); //*1
                        RefrechColor(true);
                        RemoveSelectedButton();
                        break;
                }
            }// retrieve items
            else if (highlightedSlot != null && ItemScript.selectedItem == null && highlightedSlot.GetComponent<SlotScript>().isOccupied == true)
            {
                ColorChangeLoop(SlotColorHighlights.Gray, highlightedSlot.GetComponent<SlotScript>().storedItemSize, highlightedSlot.GetComponent<SlotScript>().storedItemStartPos);
                ItemScript.SetSelectedItem(GetItem(highlightedSlot));
                SlotSectorScript.sectorScript.PosOffset();
                RefrechColor(true);
            }
        }
    }

    private void CheckArea(IntVector2 itemSize) //*2
    {
        IntVector2 halfOffset;
        IntVector2 overCheck;
        halfOffset.x = (itemSize.x - (itemSize.x % 2 == 0 ? 0 : 1)) / 2;
        halfOffset.y = (itemSize.y - (itemSize.y % 2 == 0 ? 0 : 1)) / 2;
        totalOffset = highlightedSlot.GetComponent<SlotScript>().gridPos - (halfOffset + SlotSectorScript.posOffset);
        checkStartPos = totalOffset;
        checkSize = itemSize;
        overCheck = totalOffset + itemSize;
        isOverEdge = false;
        //checks if item to stores is outside grid
        if (overCheck.x > gridSize.x)
        {
            checkSize.x = gridSize.x - totalOffset.x;
            isOverEdge = true;
        }
        if (totalOffset.x < 0)
        {
            checkSize.x = itemSize.x + totalOffset.x;
            checkStartPos.x = 0;
            isOverEdge = true;
        }
        if (overCheck.y > gridSize.y)
        {
            checkSize.y = gridSize.y - totalOffset.y;
            isOverEdge = true;
        }
        if (totalOffset.y < 0)
        {
            checkSize.y = itemSize.y + totalOffset.y;
            checkStartPos.y = 0;
            isOverEdge = true;
        }
    }
    private int SlotCheck(IntVector2 itemSize)//*2
    {
        GameObject obj = null;
        SlotScript instanceScript;
        if (!isOverEdge)
        {
            for (int y = 0; y < itemSize.y; y++)
            {
                for (int x = 0; x < itemSize.x; x++)
                {
                    instanceScript = slotGrid[checkStartPos.x + x, checkStartPos.y + y].GetComponent<SlotScript>();
                    if (instanceScript.isOccupied)
                    {
                        if (obj == null)
                        {
                            obj = instanceScript.storedItemObject;
                            otherItemPos = instanceScript.storedItemStartPos;
                            otherItemSize = obj.GetComponent<ItemScript>().item.Size;
                        }
                        else if (obj != instanceScript.storedItemObject)
                            return 2; // if cheack Area has 1+ item occupied
                    }
                }
            }
            if (obj == null)
                return 0; // if checkArea is not accupied
            else
                return 1; // if checkArea only has 1 item occupied
        }
        return 2; // check areaArea is over the grid
    }
    public void RefrechColor(bool enter)
    {
        if (enter)
        {
            CheckArea(ItemScript.selectedItemSize);
            checkState = SlotCheck(checkSize);
            switch (checkState)
            {
                case 0: ColorChangeLoop(SlotColorHighlights.Green, checkSize, checkStartPos); break; //no item in area
                case 1:
                    ColorChangeLoop(SlotColorHighlights.Yellow, otherItemSize, otherItemPos); //1 item on area and can swap
                    ColorChangeLoop(SlotColorHighlights.Green, checkSize, checkStartPos);
                    break;
                case 2: ColorChangeLoop(SlotColorHighlights.Red, checkSize, checkStartPos); break; //invalid position (more then 2 items in area or area is outside grid)
            }
        }
        else //on pointer exit. resets colors
        {
            isOverEdge = false;
            //checkArea(); //commented out for performance. may cause bugs if not included
            
            ColorChangeLoop2(checkSize, checkStartPos);
            if (checkState == 1)
            {
                ColorChangeLoop(SlotColorHighlights.Blue2, otherItemSize, otherItemPos);
            }
        }
    }
    public void ColorChangeLoop(Color32 color, IntVector2 size, IntVector2 startPos)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = color;
            }
        }
    }
    public void ColorChangeLoop2(IntVector2 size, IntVector2 startPos)
    {
        GameObject slot;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                slot = slotGrid[startPos.x + x, startPos.y + y];
                if (slot.GetComponent<SlotScript>().isOccupied != false)
                {
                    slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = SlotColorHighlights.Blue2;
                }
                else
                {
                    slotGrid[startPos.x + x, startPos.y + y].GetComponent<Image>().color = SlotColorHighlights.Gray;
                }
            }
        }
    }
    private void StoreItem(GameObject item)
    {
        SlotScript instanceScript;
        IntVector2 itemSizeL = item.GetComponent<ItemScript>().item.Size;
        for (int y = 0; y < itemSizeL.y; y++)
        {
            for (int x = 0; x < itemSizeL.x; x++)
            {
                //set each slot parameters
                instanceScript = slotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<SlotScript>();
                instanceScript.storedItemObject = item;
                instanceScript.storedItemClass = item.GetComponent<ItemScript>().item;
                instanceScript.storedItemSize = itemSizeL;
                instanceScript.storedItemStartPos = totalOffset;
                instanceScript.isOccupied = true;
                slotGrid[totalOffset.x + x, totalOffset.y + y].GetComponent<Image>().color = SlotColorHighlights.Gray;
            }
        }//set dropped parameters
        item.transform.SetParent(dropParent);
        item.GetComponent<RectTransform>().pivot = Vector2.zero;
        item.transform.position = slotGrid[totalOffset.x, totalOffset.y].transform.position;
        item.GetComponent<CanvasGroup>().alpha = 1f;
        overlayScript.UpdateOverlay(highlightedSlot.GetComponent<SlotScript>().storedItemClass);
    }
    private GameObject GetItem(GameObject slotObject)
    {
        SlotScript slotObjectScript = slotObject.GetComponent<SlotScript>();
        GameObject retItem = slotObjectScript.storedItemObject;
        IntVector2 tempItemPos = slotObjectScript.storedItemStartPos;
        IntVector2 itemSizeL = retItem.GetComponent<ItemScript>().item.Size;

        SlotScript instanceScript;
        for (int y = 0; y < itemSizeL.y; y++)
        {
            for (int x = 0; x < itemSizeL.x; x++)
            {
                //reset each slot parameters
                instanceScript = slotGrid[tempItemPos.x + x, tempItemPos.y + y].GetComponent<SlotScript>();
                instanceScript.storedItemObject = null;
                instanceScript.storedItemSize = IntVector2.Zero;
                instanceScript.storedItemStartPos = IntVector2.Zero;
                instanceScript.storedItemClass = null;
                instanceScript.isOccupied = false;
            }
        }//returned item set item parameters
        retItem.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retItem.GetComponent<CanvasGroup>().alpha = 0.5f;
        retItem.transform.position = Input.mousePosition;
        overlayScript.UpdateOverlay(null);
        return retItem;
    }
    private GameObject SwapItem(GameObject item)
    {
        GameObject tempItem;
        tempItem = GetItem(slotGrid[otherItemPos.x, otherItemPos.y]);
        StoreItem(item);
        return tempItem;
    }

    public void RemoveSelectedButton()
    {
        if (selectedButton != null)
        {
            listManager.RevomeItemFromList(selectedButton.GetComponent<ItemButtonScript>().item);
            listManager.RemoveButton(selectedButton);
            listManager.sortManager.SortAndFilterList();
            selectedButton = null;

        }
    }
    
}

public struct SlotColorHighlights
{
    public static Color32 Green
    { get { return new Color32(127, 223, 127, 255); } }
    public static Color32 Yellow
    { get { return new Color32(223, 223, 63, 255); } }
    public static Color32 Red
    { get { return new Color32(223, 127, 127, 255); } }
    public static Color32 Blue
    { get { return new Color32(159, 159, 223, 255); } }
    public static Color32 Blue2
    { get { return new Color32(191, 191, 223, 255); } }
    public static Color32 Gray
    { get { return new Color32(223, 223, 223, 255); } }
}
