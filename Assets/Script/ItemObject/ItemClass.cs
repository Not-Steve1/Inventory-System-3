using System;
using UnityEngine;

[System.Serializable]
public class ItemClass {


    public int GlobalID;
    [HideInInspector] public int CategoryID;
    [HideInInspector] public string CategoryName;
    [HideInInspector] public int TypeID;
    public string TypeName;
    [Range(1, 100)] public int Level;
    [Range(0, 3)] public int qualityInt;
    [HideInInspector] public IntVector2 Size;
    [HideInInspector] public Sprite Icon;
    [HideInInspector] public string SerialID;

    private enum QualityEnum { Broken, Normal, Magic, Rare}
    public string GetQualityStr()
    {
        return Enum.GetName(typeof(QualityEnum), qualityInt);
    }
    
    public static void SetItemValues(ItemClass item, int ID, int lvl, int quality)
    {
        item.GlobalID = ID;
        item.Level = lvl;
        item.qualityInt = quality;
        GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<LoadItemDatabase>().PassItemData(ref item);
    }
    public static void SetItemValues(ItemClass item)
    {
        GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<LoadItemDatabase>().PassItemData(ref item);
    }


    public ItemClass(ItemClass passedItem)//create new item by copying passedITem properties
    {
        GlobalID = passedItem.GlobalID;
        Level = passedItem.Level;
        qualityInt = passedItem.qualityInt;
    }
    public ItemClass() { }//creates error if this is not put. dont know why

}
