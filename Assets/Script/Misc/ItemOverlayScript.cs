using UnityEngine;
using UnityEngine.UI;

public class ItemOverlayScript : MonoBehaviour {

    public Text nameText, lvlText, qualityText, size;
    public Image Icon;
    public Sprite nullSprite;

    private float slotSize;

    private void Start()
    {
        ItemButtonScript.overlayScript = this;
        SlotSectorScript.overlayScript = this;
        UpdateOverlay(null);
        slotSize = GameObject.FindGameObjectWithTag("InvenPanel").GetComponent<InvenGridScript>().slotSize;
    }

    public void UpdateOverlay(ItemClass item)
    {
        if (item != null)
        {
            nameText.text = item.TypeName;
            lvlText.text = "Lvl: " + item.Level;
            qualityText.text = item.GetQualityStr();
            switch (item.qualityInt)
            {
                case 0: qualityText.color = Color.gray; break;
                case 1: qualityText.color = Color.white; break;
                case 2: qualityText.color = new Color(0.5f, 0.5f, 1f, 1f); break;
                case 3: qualityText.color = Color.yellow; break;
                default: break;
            }
            Icon.color = new Color32(255, 255, 255, 255);
            Icon.sprite = item.Icon;
            size.text = item.Size.ToString();
            RectTransform rect = Icon.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.Size.x * slotSize);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.Size.y * slotSize);
        }
        else
        {
            nameText.text = null;
            lvlText.text = null;
            qualityText.text = null;
            size.text = IntVector2.Zero.ToString();
            Icon.sprite = null;
            Icon.color = new Color32(0, 0, 0, 0);
        }
    }

    public void UpdateOverlay2(ItemClass item, bool ID, bool lvl, bool quality)
    {
        if (item != null)
        {
            nameText.text = ID ? item.TypeName : "***";
            lvlText.text = lvl ? "Lvl: " + item.Level : "***";
            qualityText.text = quality ? item.GetQualityStr() : "***";
            Icon.color =new Color32(255, 255, 255, 255);
            Icon.sprite = ID ? item.Icon : nullSprite;
            switch (item.qualityInt)
            {
                case 0: qualityText.color = Color.gray; break;
                case 1: qualityText.color = Color.white; break;
                case 2: qualityText.color = new Color(0.5f, 0.5f, 1f, 1f); break;
                case 3: qualityText.color = Color.yellow; break;
                default: break;
            }
            IntVector2 size = ID ? item.Size : new IntVector2(4, 4);
            RectTransform rect = Icon.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x * slotSize);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y * slotSize);
        }
        else
        {
            nameText.text = null;
            lvlText.text = null;
            qualityText.text = null;
            size.text = IntVector2.Zero.ToString();
            Icon.sprite = null;
            Icon.color = new Color32(0, 0, 0, 0);
        }
    }
}
