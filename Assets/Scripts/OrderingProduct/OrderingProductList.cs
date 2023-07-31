using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class OrderingProductList : MonoBehaviour
{
    public GameObject header, body, bodycontent, downmenu;
    public ScrollingTab tab;
    public GameObject item, upPanel;
    public GameObject noitem;
    public GameObject deleteWindow, loading, addcategory, addproduct, detail, modify;
    public GameObject addbtn, addback, addbackground;
    private int curSel = 0;
    private int deleteId;
    public bool IsSpecial;
    Dictionary<int, string> categories = new Dictionary<int, string>();
    List<int> keys = new List<int>();
    List<string> values = new List<string>();
    // Use this for initialization
    void Start()
    {
        Global.attachcount = 0;
        PlayerPrefs.SetString("attach_time", Utils.GetCurTime());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        Global.attachcount = 0;
        PlayerPrefs.SetString("attach_time", Utils.GetCurTime());
        RectTransform h = header.GetComponent<RectTransform>();
        h.anchorMin = new Vector2(0, 1);
        h.anchorMax = new Vector2(1, 1);
        h.offsetMax = new Vector2(0, 0);
        h.offsetMin = new Vector2(0, -500.0f * Screen.width / 2880);

        RectTransform g = tab.gameObject.GetComponent<RectTransform>();
        g.anchorMin = new Vector2(0, 0);
        g.anchorMax = new Vector2(1, 1);
        g.offsetMax = new Vector2(0, -500.0f / 2880 * Screen.width);
        g.offsetMin = new Vector2(0, 0);
        tab.change();

        RectTransform r = downmenu.gameObject.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(0, 1);
        r.anchorMax = new Vector2(1, 1);
        r.offsetMax = new Vector2(0, 0);
        r.offsetMin = new Vector2(0, -1470f / 2880 * Screen.width);

        addbtn.SetActive(Global.isAdmin);
        Refresh();
    }
    void Refresh()
    {
        tab.Clear();
        StartCoroutine(GetResults());
    }

    IEnumerator GetResults()
    {
        WWW www1 = new WWW(Global.URL_ORDERINGCATEGORY + 0);
        loading.SetActive(true);
        noitem.SetActive(true);
        yield return www1;
        if (string.IsNullOrEmpty(www1.error))
        {
            JSONNode root = JSON.Parse(www1.text);
            if (root["status"].AsBool)
            {
                tab.SelectedCategory(curSel);
                categories.Clear();
                keys.Clear();
                foreach (string str in root["result"].Keys) keys.Add(int.Parse(str));
                foreach (int str in keys)
                {
                    categories.Add(str, root["result"][str + ""]["info"][0]["name"]);
                }
                tab.AddCategory(categories);
            }
            tab.noItem.SetActive(false);
        }
        WWW www = new WWW(Global.URL_ORDERINGPRODUCTLIST + 0 + "?category=" + keys[curSel]);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
        }
        else
        {
            SpecialProductItemList a = JsonUtility.FromJson<SpecialProductItemList>(www.text);
            if (a.status)
            {
                int H = (int)(1120f/2880*Screen.width);
                tab.noItem.SetActive(a.count == 0);
                for (int i = 0; i < a.count; i++)
                {
                    tab.AddContent(GetBodyItem(a.result[i]), H, i);
                }
            }
        }
        loading.SetActive(false);
    }

    public GameObject GetBodyItem(SpecialProductItem item)
    {
        GameObject obj = Instantiate(this.item);
        obj.GetComponent<Product>().SetType(IsSpecial);
        obj.GetComponent<Product>().SetValues(item, categories, gameObject);
        return obj;
    }

    public void BackBtnClick()
    {
        downmenu.SetActive(false);
        if (downmenu.activeInHierarchy)
        {
            addback.GetComponent<Image>().color = Color.white;
            addbackground.GetComponent<Image>().color = Color.white;
        }
        else
        {
            addback.GetComponent<Image>().color = new Color(177f / 255f, 196f / 255, 210f / 255);
            addbackground.GetComponent<Image>().color = new Color(177f / 255, 196f / 255, 210f / 255);
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void AddBtnClick()
    {
        downmenu.SetActive(!downmenu.activeInHierarchy);
        if (downmenu.activeInHierarchy)
        {
            addback.GetComponent<Image>().color = Color.white;
            addbackground.GetComponent<Image>().color = Color.white;
        }
        else
        {
            addback.GetComponent<Image>().color = new Color(177f/255f, 196f/255, 210f/255);
            addbackground.GetComponent<Image>().color = new Color(177f/255, 196f/255, 210f/255);
        }
    }

    public void AddCategoryBtnClick()
    {
        downmenu.SetActive(false);
        if (downmenu.activeInHierarchy)
        {
            addback.GetComponent<Image>().color = Color.white;
            addbackground.GetComponent<Image>().color = Color.white;
        }
        else
        {
            addback.GetComponent<Image>().color = new Color(177f / 255f, 196f / 255, 210f / 255);
            addbackground.GetComponent<Image>().color = new Color(177f / 255, 196f / 255, 210f / 255);
        }
        GameObject obj = addcategory;
        obj.GetComponent<AddCategory>().SetValues(gameObject, IsSpecial);
        obj.SetActive(true);
    }

    public void AddProductBtnClick()
    {
        downmenu.SetActive(false);
        if (downmenu.activeInHierarchy)
        {
            addback.GetComponent<Image>().color = Color.white;
            addbackground.GetComponent<Image>().color = Color.white;
        }
        else
        {
            addback.GetComponent<Image>().color = new Color(177f / 255f, 196f / 255, 210f / 255);
            addbackground.GetComponent<Image>().color = new Color(177f / 255, 196f / 255, 210f / 255);
        }
        GameObject obj = addproduct;
        obj.GetComponent<AddProduct>().SetValues(gameObject, IsSpecial, categories);
        obj.SetActive(true);
    }

    public void Delete(SpecialProductItem item)
    {
        deleteId = item.id;
        deleteWindow.SetActive(true);
    }

    public void DeleteYesBtnClick()
    {
        StartCoroutine(DeleteItem());
    }

    IEnumerator DeleteItem()
    {
        using (var w = UnityWebRequest.Delete(Global.URL_ORDERINGPRODUCTDELETE + deleteId))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            loading.SetActive(true);
            yield return w.SendWebRequest();
            loading.SetActive(false);
            if (w.isNetworkError)
            {
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 못합니다.", MessageType.FAILD));
            }
            else
            {
                StartCoroutine(Utils.ShowMessage("삭제되었습니다.", MessageType.SUCCESS));
            }
            deleteWindow.SetActive(false);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    public void DeleteNoBtnClick()
    {
        deleteWindow.SetActive(false);
    }


    public void SelectedItemChange(int id)
    {
        curSel = id;
        Refresh();
    }

    public void DetialShow(SpecialProductItem item)
    {
        detail.GetComponent<ProductDetail>().SetValues(false, item, "부품주문상세보기");
    }
}
