using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingTab : MonoBehaviour {
    public GameObject header, content;
    public GameObject headerItem, noItem;
    private GameObject[] categoryObjs;
    public int selectedCategory;
    Dictionary<int, string> categories = new Dictionary<int, string>();
    List<int> keys = new List<int>();
    private List<object> contents = new List<object>();
	// Use this for initialization
	void Start () {
    }

    Vector2 pre;
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            pre = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 cur = Input.mousePosition;
            if (cur.x-pre.x>Utils.Conv(1800))
            {
                SelectedCategory(selectedCategory - 1);
            }
            if (cur.x - pre.x < -Utils.Conv(1800))
            {
                SelectedCategory(selectedCategory + 1);
            }
        }
	}

    public void change()
    {
        RectTransform h = header.transform.parent.parent.gameObject.GetComponent<RectTransform>();
        h.anchorMin = new Vector2(0, 1);
        h.anchorMax = new Vector2(1, 1);
        h.offsetMax = new Vector2(0, 0);
        h.offsetMin = new Vector2(0, -430.0f * Screen.width / 2880);

        RectTransform g = content.transform.parent.parent.gameObject.GetComponent<RectTransform>();
        g.anchorMin = new Vector2(0, 0);
        g.anchorMax = new Vector2(1, 1);
        g.offsetMax = new Vector2(0, -430.0f * Screen.width / 2880);
        g.offsetMin = new Vector2(0, 0);
    }

    public void Clear(bool ok = true)
    {
        int cnt = header.transform.childCount;
        if (ok)
        {
            for (int i = 0; i < cnt; i++) Destroy(header.transform.GetChild(i).gameObject);
        }
        cnt = content.transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        categories.Clear();
        contents.Clear();
    }

    public void SelectedCategory(int id)
    {
        if (id >= categories.Count || id < 0) return;
        if (id!=selectedCategory) SendMessageUpwards("SelectedItemChange", id);
        selectedCategory = id;
        RectTransform a = header.GetComponent<RectTransform>();
        float H = 800f / 2880 * Screen.width;
        a.anchoredPosition = new Vector2(-H * id, 0);
        if (categoryObjs != null)
        {
            for (int i = 0; i < categoryObjs.Length; i++)
            {
                if (categoryObjs[i] != null)
                    categoryObjs[i].GetComponent<ScrollHeadItem>().Selected(i == id);
            }
        }
    }

    public void AddCategory(Dictionary<int, string> str)
    {
        categories.Clear();
        foreach (int key in str.Keys) categories.Add(key, str[key]);
        categoryObjs = new GameObject[str.Count];
        float H = 800f / 2880 * Screen.width;
        int i = 0;
        foreach(int key in categories.Keys)
        {
            GameObject a = Instantiate(headerItem);
            categoryObjs[i] = a;
            a.transform.SetParent(header.transform);
            RectTransform b = a.GetComponent<RectTransform>();
            b.offsetMin = Vector2.zero;
            b.offsetMax = new Vector2(H, 0);
            b.anchoredPosition = new Vector2(H/2 + i * (H + 10), 0);
            b.localScale = new Vector3(1, 1, 1);
            a.GetComponent<ScrollHeadItem>().Selected(selectedCategory == i);
            a.GetComponent<ScrollHeadItem>().SetValue(key, str[key], i);
            i++;
        }
        RectTransform c = header.GetComponent<RectTransform>();
        c.offsetMin = Vector2.zero;
        c.offsetMax = new Vector2((H+10) * str.Count + H, 0);
        header.GetComponent<RectTransform>().anchoredPosition = new Vector2(-H * selectedCategory, 0);
    }

    public void AddContent(GameObject item, int h, int k)
    {
        item.transform.SetParent(content.transform);
        RectTransform b = item.GetComponent<RectTransform>();
        b.offsetMin = Vector2.zero;
        b.offsetMax = new Vector2(0, h);
        b.anchoredPosition = new Vector2(0, k * -(h + 10));
        b.localScale = new Vector3(1, 1, 1);
        RectTransform c = content.GetComponent<RectTransform>();
        c.offsetMax = Vector2.zero;
        c.offsetMin = new Vector2(0, -(k+1)*(h+10));
    }
}
