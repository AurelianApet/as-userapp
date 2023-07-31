using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AddProduct : MonoBehaviour {
    public GameObject header, body, bodycontent, dropdownitem;
    public GameObject image, productName, price, productNo, description, category, loading;
    public GameObject[] titles;
    private GameObject backWindow;
    private string imagepath;
    private bool IsSpecial;
    List<string> paths = new List<string>();
    Dictionary<int, string> categories = new Dictionary<int, string>();
    List<int> keys = new List<int>();
    List<string> values = new List<string>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValues(GameObject b, bool ok, Dictionary<int,string> str)
    {
        Utils.SetUIElement(header, body, bodycontent, 7740);
        GameObject[] tmp = new GameObject[] { image, productName, price, productNo, description};
        foreach(GameObject obj in tmp)
        {
            foreach(Text f in obj.GetComponentsInChildren<Text>()) f.fontSize = (int)(135f / 2880 * Screen.width);
        }
        foreach(GameObject obj in titles)
        {
            obj.GetComponent<Text>().fontSize = (int)(135f / 2880 * Screen.width);
        }
        categories.Clear(); keys.Clear(); values.Clear();
        backWindow = b;
        IsSpecial = ok;
        foreach(int s in str.Keys)
        {
            keys.Add(s); values.Add(str[s]);
            categories.Add(s, str[s]);
        }
        values.Add("");
        category.GetComponent<Dropdown>().ClearOptions();
        category.GetComponent<Dropdown>().AddOptions(values);
        if (values.Count > 1) category.GetComponent<Dropdown>().value = 0;
        RectTransform a = dropdownitem.GetComponent<RectTransform>();
        a.anchorMin = new Vector2(0, 1);
        a.anchorMax = new Vector2(1, 1);
        a.offsetMax = Vector2.zero;
        a.offsetMin = new Vector2(0, -400f / 2880 * Screen.width);
        dropdownitem.GetComponentInChildren<Text>().fontSize = (int)(140f / 2880 * Screen.width);

        this.image.GetComponent<InputField>().text="";
        this.productName.GetComponent<InputField>().text="";
        this.price.GetComponent<InputField>().text="";
        this.productNo.GetComponent<InputField>().text="";
        this.description.GetComponent<InputField>().text="";
        this.category.GetComponent<Dropdown>().value=0;
        paths.Clear();
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void AddFileBtnClick()
    {
        string path = NativeGallery.PickImageOrVideo();
        if (string.IsNullOrEmpty(path)) return;
        paths.Add(path);
        if (paths.Count > 1) this.image.GetComponent<InputField>().text += ",";
        this.image.GetComponent<InputField>().text += Utils.GetFileNameFromPath(path);
    }

    public void AddBtnClick()
    {
        if (category.GetComponent<Dropdown>().value == -1)
        {
            StartCoroutine(Utils.ShowMessage("카테고리를 선택하세요.", MessageType.WARNING));
            return;
        }
        string productname = productName.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(productname))
        {
            StartCoroutine(Utils.ShowMessage("상품명을 입력하세요.", MessageType.WARNING));
            productName.GetComponent<InputField>().Select();
            return;
        }
        string productprice = price.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(productprice))
        {
            StartCoroutine(Utils.ShowMessage("가격을 입력하세요.", MessageType.WARNING));
            price.GetComponent<InputField>().Select();
            return;
        }
        string productno = productNo.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(productno))
        {
            StartCoroutine(Utils.ShowMessage("제품번호를 입력하세요.", MessageType.WARNING));
            productNo.GetComponent<InputField>().Select();
            return;
        }
        string desc = description.GetComponent<InputField>().text;
        if (string.IsNullOrEmpty(desc))
        {
            StartCoroutine(Utils.ShowMessage("상품소개를 입력하세요.", MessageType.WARNING));
            description.GetComponent<InputField>().Select();
            return;
        }
        if (paths.Count==0)
        {
            StartCoroutine(Utils.ShowMessage("대표사진을 선택해주세요.", MessageType.WARNING));
            return;
        }
        StartCoroutine(AddNewProduct());
    }

    IEnumerator AddNewProduct()
    {
        loading.SetActive(true);
        string productname = this.productName.GetComponent<InputField>().text;
        double price = double.Parse(this.price.GetComponent<InputField>().text);
        string productnumber = this.productNo.GetComponent<InputField>().text;
        string description = this.description.GetComponent<InputField>().text;
        int category = keys[this.category.GetComponent<Dropdown>().value];

        WWWForm form = new WWWForm();
        Dictionary<string, string> postHeader = form.headers;
        postHeader.Add("Content-type", "application/json");
        if (paths.Count>0)
        {
            form.AddField("folder", System.DateTime.Now.ToFileTime().ToString());
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i];
                var w = new WWW(path);
                yield return w;
                if (Utils.IsImage(path))
                {
                    var t = w.texture;
                    form.AddBinaryData("others[]", t.EncodeToPNG(), "1.png", "image/png");
                }
                else if (Utils.IsVideo(path))
                {
                    form.AddBinaryData("others[]", w.bytes, path, "video/" + Utils.FileType(path));
                }
            }
        }
        form.AddField("title", productname);
        form.AddField("price", price.ToString());
        form.AddField("serial_number", productnumber);
        form.AddField("description", description);
        form.AddField("categorie", category);
        form.AddField("quantity", 1);
        using (var w = UnityWebRequest.Post(IsSpecial ? Global.URL_SPECIALPRODUCTADD : Global.URL_ORDERINGPRODUCTAdd, form))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            yield return w.SendWebRequest();
            loading.SetActive(false);
            if (!string.IsNullOrEmpty(w.error))
            {
                if (Global.Debug) print(w.error);
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            }
            else
            {
                
            }
            gameObject.SetActive(false);
            backWindow.SetActive(false);
            backWindow.SetActive(true);
        }
    }
}
