using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemButtonScript : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler{

    public Button buttonComponent;
    public Text nameText;
    public Image iconImage;
    public Text LvlText;
    public Text QualityText;
    public Image QualityColor;

    public ItemClass item;
    private ItemListManager listManager;
    public ObjectPoolScript itemEquipPool;

    public static InvenGridManager invenManager;
    public static ItemOverlayScript overlayScript;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0))// still spawns when drag scroll
        {
            if (ItemScript.selectedItem == null)
            {
                SpawnStoredItem(); //swap item when no selectedButton and selectedItem
            }
            listManager.AddSelectedItemToList();
            if (ItemScript.selectedItem != null && invenManager.selectedButton != this.gameObject) // reset selected button when item is from list
            {
                invenManager.selectedButton.GetComponent<CanvasGroup>().alpha = 1f;
                invenManager.selectedButton = null;
                listManager.itemEquipPool.ReturnObject(ItemScript.selectedItem);
                SpawnStoredItem();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        overlayScript.UpdateOverlay(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overlayScript.UpdateOverlay(null);
    }

    private void SpawnStoredItem()
    {
        GameObject newItem = itemEquipPool.GetObject();
        newItem.GetComponent<ItemScript>().SetItemObject(item);

        ItemScript.SetSelectedItem(newItem);
        invenManager.selectedButton = this.gameObject;

        GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    public void SetUpButton(ItemClass passedItem, ItemListManager passedListManager)
    {
        listManager = passedListManager;
        item = passedItem;
        ItemClass.SetItemValues(passedItem);
        nameText.text = passedItem.TypeName;
        LvlText.text = "Lvl: " + passedItem.Level.ToString();
        QualityText.text = passedItem.GetQualityStr();
        GetComponent<LayoutElement>().preferredHeight = transform.parent.GetComponent<RectTransform>().rect.width / 4;
        iconImage.sprite = passedItem.Icon;
        itemEquipPool = passedListManager.itemEquipPool;
        switch (item.qualityInt)
        {
            case 0: QualityColor.color = Color.gray; break;
            case 1: QualityColor.color = Color.white; break;
            case 2: QualityColor.color = new Color(0.5f, 0.5f, 1f, 1f); break;
            case 3: QualityColor.color = Color.yellow; break;
            default: break;
        }

        float iconSize = listManager.iconSize;
        RectTransform rect = iconImage.GetComponent<RectTransform>();
        if (passedItem.Size.y >= passedItem.Size.x)
        {
            rect.sizeDelta = new Vector2(iconSize / IntVector2.Slope(passedItem.Size), iconSize);
        }
        else
        {
            rect.sizeDelta = new Vector2(iconSize, iconSize * IntVector2.Slope(passedItem.Size));
        }
    }

    
}
