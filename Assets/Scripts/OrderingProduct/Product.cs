using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour {
    public GameObject header, body, bodycontent, manager;
    private bool IsSpecial;
    public GameObject image, productName, productNo, price, modifybtn, deletebtn;
    private SpecialProductItem item;
    Dictionary<int, string> categories = new Dictionary<int, string>();
    List<int> keys = new List<int>();
    List<string> values = new List<string>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetType(bool isSpecial)
    {
        IsSpecial = isSpecial;
    }

    public void SetValues(SpecialProductItem t, Dictionary<int, string> cats, GameObject detail)
    {
        this.manager = detail;
        item = t;
        modifybtn.SetActive(Global.isAdmin);
        deletebtn.SetActive(Global.isAdmin);
        StartCoroutine(Utils.ShowImage(image, (IsSpecial?Global.URL_IMAGEPATH_SPECIAL:Global.URL_IMAGEPATH_ORDERING) + item.folder + "/" + item.image));
        productName.GetComponent<Text>().fontSize = (int)(130f / 2880 * Screen.width);
        productNo.GetComponent<Text>().fontSize = (int)(130f / 2880 * Screen.width);
        price.GetComponent<Text>().fontSize = (int)(130f / 2880 * Screen.width);
        productName.GetComponent<Text>().text = t.title;
        productNo.GetComponent<Text>().text = t.serial_number;
        price.GetComponent<Text>().text = t.price + "";
        categories.Clear(); keys.Clear(); values.Clear();
        foreach (int s in cats.Keys)
        {
            keys.Add(s); values.Add(cats[s]);
            categories.Add(s, cats[s]);
        }
    }

    public SpecialProductItem GetItem()
    {
        return item;
    }

    public void EditBtnClick()
    {
        GameObject a = !IsSpecial ? manager.GetComponent<OrderingProductList>().modify : manager.GetComponent<SpecialProductList>().modify;
        a.SetActive(true);
        a.GetComponent<ProductModify>().SetType(IsSpecial);
        a.GetComponent<ProductModify>().SetValues(item, manager, categories);
    }

    public void DeleteBtnClick()
    {
        manager.SendMessage("Delete", item, SendMessageOptions.DontRequireReceiver);
    }

    public void DetailBtnClick()
    {
        print("Detail Show");
        manager.SendMessage("DetialShow", item, SendMessageOptions.DontRequireReceiver);
    }
}
