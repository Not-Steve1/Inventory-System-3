using UnityEngine;

[System.Serializable]
public class ItemClass {


    public int GlobalID;
    [HideInInspector] public int CategoryID;
    [HideInInspector] public string CategoryName;
    [HideInInspector] public int TypeID;
    public string TypeName;
    public int Level;
    [Range(0, 3)] public int qualityInt;
    [HideInInspector] public IntVector2 Size;
    [HideInInspector] public Sprite Icon;
    public string SerialID;
    public string qualityStr
    {
        get
        {
            switch (qualityInt)
            {
                case 0: return "Broken";
                case 1: return "Normal";
                case 2: return "Magic";
                case 3: return "Rare";
                default: return null;
            }
        }
    }
    
    public static void SetItemValues(ItemClass item, int ID, int lvl, int quality)
    {
        item.GlobalID = ID;
        item.Level = lvl;
        item.qualityInt = quality;
        GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>().PassItemData(ref item);
    }
    public static void SetItemValues(ItemClass item)
    {
        GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<ItemDatabase>().PassItemData(ref item);
    }


    public ItemClass(ItemClass passedItem)//create new item by copying passedITem properties
    {
        GlobalID = passedItem.GlobalID;
        Level = passedItem.Level;
        qualityInt = passedItem.qualityInt;
    }
    public ItemClass() { }//creates error if this is not put. dont know why

}
