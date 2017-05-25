
using UnityEngine;
using System.Collections.Generic;
using System;

public class ItemDatabase : MonoBehaviour
{
    public TextAsset file;
    public List<int> GlobalIDList;
    public List<string> TypeNameList;
    public List<IntVector2> SizeList;
    public List<Sprite> IconList;
    


    private void Awake()
    {
        Load(file);
    }

	public class Row
	{
		public int GlobalID;
        public int CategoryID;
        public string CategoryName;
        public int TypeID;
		public string TypeName;
        public IntVector2 Size;
        public Sprite Icon;
        public string SerailID;

    }

	public List<Row> rowList = new List<Row>();
	bool isLoaded = false;

	public bool IsLoaded()
	{
		return isLoaded;
	}

	public List<Row> GetRowList()
	{
		return rowList;
	}

	public void Load(TextAsset csv)
	{
		rowList.Clear();
		string[][] grid = CsvParser2.Parse(csv.text);
		for(int i = 1 ; i < grid.Length ; i++)
		{
			Row row = new Row();
			row.GlobalID = Int32.Parse(grid[i][0]);
            GlobalIDList.Add(row.GlobalID);
            row.CategoryID = Int32.Parse(grid[i][1]);
            row.CategoryName = grid[i][2];
            row.TypeID = Int32.Parse(grid[i][3]);
            row.TypeName = grid[i][4];
            TypeNameList.Add(row.TypeName);
            row.Size = new IntVector2(Int32.Parse(grid[i][5]), Int32.Parse(grid[i][6]));
            SizeList.Add(row.Size);
            row.Icon = Resources.Load<Sprite>("ItemIcons/" + grid[i][4]);
            IconList.Add(row.Icon);
            row.SerailID = grid[i][7];
            rowList.Add(row);
		}
		isLoaded = true;
	}

    public void PassItemData(ref ItemClass item)
    {
        int ID = item.GlobalID;
        item.CategoryID = rowList[ID].CategoryID;
        item.CategoryName = rowList[ID].CategoryName;
        item.TypeID = rowList[ID].TypeID;
        item.TypeName = rowList[ID].TypeName;
        item.Size = rowList[ID].Size;
        item.Icon = rowList[ID].Icon;
        item.SerialID = rowList[ID].SerailID;
    }



	public int NumRows()
	{
		return rowList.Count;
	}

	public Row GetAt(int i)
	{
		if(rowList.Count <= i)
			return null;
		return rowList[i];
	}








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