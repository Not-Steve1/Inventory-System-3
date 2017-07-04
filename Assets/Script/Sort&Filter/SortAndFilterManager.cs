using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SortAndFilterManager : MonoBehaviour {

    public ItemListManager listManager;
    private List<ItemClass> itemList;

    public List<GameObject> categoryButtons;
    private GameObject selectedCatButton;
    public int catFilterInt = 0;

    public Image qualityButtonImage;
    public Text qualityButtonText;
    private int qualityFilterInt = 0;

    private List<ItemClass> filteredList;
    public List<ItemClass> sortedList;
    private int sortTypeInt = 0;

    private void Start()
    {
        selectedCatButton = categoryButtons[0];
        qualityButtonImage.color = Color.gray;
        sortedList = SortList(listManager.startItemList);
        listManager.currentItemList = sortedList;
        listManager.PopulateList(sortedList);
        filteredList = sortedList;
    }

    #region filter list
    private void FilterList(List<ItemClass> list)
    {
        list = FilterByClass(list);
        list = FilterByQuality(list);
        filteredList = list;
        listManager.PopulateList(filteredList);
    }

    private void ClassFilterChange(int type)//used on classfilter buttons
    {
        if (selectedCatButton != categoryButtons[type])
        {
            catFilterInt = type;
            categoryButtons[type].GetComponent<CanvasGroup>().alpha = 1f;
            selectedCatButton.GetComponent<CanvasGroup>().alpha = 0.5f;
            selectedCatButton = categoryButtons[type];
            FilterList(sortedList);
        }
    }

    private List<ItemClass> FilterByClass(List<ItemClass> list)
    {
        if (catFilterInt == 0) return list;
        else return list.FindAll(x => x.CategoryID == catFilterInt - 1);
    }

    private List<ItemClass> FilterByQuality(List<ItemClass> list)
    {
        if (qualityFilterInt == 0) return list;
        else return list.FindAll(x => x.qualityInt >= qualityFilterInt);
    }

    private void QualityButtonClick()//used on quality filter button
    {
        switch (qualityFilterInt)//change button text
        {
            case 0: qualityFilterInt = 1; qualityButtonText.text = "Normal+"; break;
            case 1: qualityFilterInt = 2; qualityButtonText.text = "Magic+"; break;
            case 2: qualityFilterInt = 3; qualityButtonText.text = "Rare"; break;
            case 3: qualityFilterInt = 0; qualityButtonText.text = "All"; break;
            default:break;
        }
        switch (qualityFilterInt)//change button color
        {
            case 0: qualityButtonImage.color = Color.gray; break;
            case 1: qualityButtonImage.color = Color.white; break;
            case 2: qualityButtonImage.color = new Color(0.5f, 0.5f, 1f, 1f); break;
            case 3: qualityButtonImage.color = Color.yellow; break;
            default:break;
        }
        FilterList(sortedList);
    }
    #endregion
    #region sortlist
    public void OnSortTypeChange(int index)//used on sortType dropdown
    {
        sortTypeInt = index;
        sortedList = SortList(listManager.currentItemList);
        listManager.currentItemList = sortedList;
        filteredList = SortList(filteredList);
        listManager.PopulateList(filteredList);
    }
    public List<ItemClass> SortList(List<ItemClass> list)
    {
        switch (sortTypeInt)
        {
            case 0: return list.OrderBy(x => x.GlobalID).ToList();
            case 1: return list.OrderByDescending(x => x.GlobalID).ToList();
            case 2: return list.OrderBy(x => x.Level).ToList();
            case 3: return list.OrderByDescending(x => x.Level).ToList();
            case 4: return list.OrderBy(x => x.qualityInt).ToList();
            case 5: return list.OrderByDescending(x => x.qualityInt).ToList();
            case 6: return list.OrderBy(x => x.TypeName).ToList();
            case 7: return list.OrderByDescending(x => x.TypeName).ToList();
            default: return list;
        }
    }
#endregion
    public void AddItemToList(ItemClass item)
    {
        listManager.currentItemList.Add(item);
        SortAndFilterList();
    }
    //add function add list to currentlist
    public void SortAndFilterList()//rework to use findindex then inset
    {
        sortedList = SortList(listManager.currentItemList);
        listManager.currentItemList = sortedList;
        FilterList(sortedList);
        listManager.PopulateList(filteredList);
    }
}
