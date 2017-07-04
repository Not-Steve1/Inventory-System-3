using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadItemDatabase : MonoBehaviour {

    public TextAsset dbFile;

    public List<string> TypeNameList = new List<string>();


    private void Awake()
    {
        LoadDb(dbFile);
    }

    public class ItemData
    {
        public int GlobalID;
        public int CategoryID;
        public string CategoryName;
        public int TypeID;
        public string TypeName;
        public IntVector2 Size;
        public Sprite Icon;
    }

    public List<ItemData> dbList = new List<ItemData>();

    private void LoadDb(TextAsset csvFile)
    {
        string[][] grid = CsvReadWrite.LoadTextFile(csvFile);
        for (int i = 1; i < grid.Length; i++)
        {
            ItemData row = new ItemData();
            row.GlobalID = Int32.Parse(grid[i][0]);
            row.CategoryID = Int32.Parse(grid[i][1]);
            row.CategoryName = grid[i][2];
            row.TypeID = Int32.Parse(grid[i][3]);
            row.TypeName = grid[i][4];
            TypeNameList.Add(row.TypeName);
            row.Size = new IntVector2(Int32.Parse(grid[i][5]), Int32.Parse(grid[i][6]));
            row.Icon = Resources.Load<Sprite>("ItemIcons/" + grid[i][4]);
            dbList.Add(row);
        }
    }

    public void PassItemData(ref ItemClass item)
    {
        int ID = item.GlobalID;
        item.CategoryID = dbList[ID].CategoryID;
        item.CategoryName = dbList[ID].CategoryName;
        item.TypeID = dbList[ID].TypeID;
        item.TypeName = dbList[ID].TypeName;
        item.Size = dbList[ID].Size;
        item.Icon = dbList[ID].Icon;
    }



    //create find item funtions later

    //*from CsvParser2*
    //public Row Find_ItemTypeID(string find)
    //{
    //	return rowList.Find(x => x.GlobalID.ToString() == find);
    //}
    //public List<Row> FindAll_ItemTypeID(string find)
    //{
    //	return rowList.FindAll(x => x.GlobalID.ToString() == find);
    //}
    //public Row Find_ItemTypeName(string find)
    //{
    //	return rowList.Find(x => x.TypeName == find);
    //}
    //public List<Row> FindAll_ItemTypeName(string find)
    //{
    //	return rowList.FindAll(x => x.TypeName == find);
    //}
    //public Row Find_SizeX(string find)
    //{
    //	return rowList.Find(x => x.SizeX == find);
    //}
    //public List<Row> FindAll_SizeX(string find)
    //{
    //	return rowList.FindAll(x => x.SizeX == find);
    //}
    //public Row Find_SizeY(string find)
    //{
    //	return rowList.Find(x => x.SizeY == find);
    //}
    //public List<Row> FindAll_SizeY(string find)
    //{
    //	return rowList.FindAll(x => x.SizeY == find);
    //}

}
