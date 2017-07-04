using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashButtonScript : MonoBehaviour {

    public Button button;
    public InvenGridManager invenManager;
    public ItemListManager listManager;

    private void Start()
    {
        button.onClick.AddListener(DestroyItem);
    }
    
    private void DestroyItem()
    {
        if (ItemScript.selectedItem != null)
        {
            invenManager.RemoveSelectedButton();
            listManager.itemEquipPool.ReturnObject(ItemScript.selectedItem);
            ItemScript.ResetSelectedItem();
        }
    }
}
