using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour, IPointerClickHandler
{
    private GameObject invenPanel;
    public static GameObject selectedItem;
    public static IntVector2 selectedItemSize;
    public static bool isDragging = false;
    
    private float slotSize;


    public ItemClass item;

    private void Awake()
    {
        slotSize = GameObject.FindGameObjectWithTag("InvenPanel").GetComponent<InvenGridScript>().slotSize;
    }


    public void SetItemObject(ItemClass passedItem)
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, passedItem.Size.x * slotSize);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, passedItem.Size.y * slotSize);
        item = passedItem;
        GetComponent<Image>().sprite = passedItem.Icon;
    }

    //obsolete
    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelectedItem(this.gameObject);
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        canvas.blocksRaycasts = false;
        canvas.alpha = 0.5f;
    }

    private void Update()
    {
        if (isDragging)
        {
            selectedItem.transform.position = Input.mousePosition;
        }
    }

    public static void SetSelectedItem(GameObject obj)
    {
        selectedItem = obj;
        selectedItemSize = obj.GetComponent<ItemScript>().item.Size;
        isDragging = true;
        obj.transform.SetParent(GameObject.FindGameObjectWithTag("DragParent").transform);
        obj.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public static void ResetSelectedItem()
    {
        selectedItem = null;
        selectedItemSize = IntVector2.Zero;
        isDragging = false;
    }
}
