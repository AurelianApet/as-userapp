using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using SimpleJSON;

public class ProductDetail : MonoBehaviour {
    public GameObject header, body, bodycontent, title;
    public GameObject backWindow, okBtn;
    public GameObject image, productname, productnumber, price, description, loading;
    public GameObject[] titles;
    public GameObject paymentsystem, playbtn;
    public Texture tx;
    public VideoPlayer vp;
    public RawImage rawimage;
    public static SpecialProductItem sItem;
    List<string> paths = new List<string>();
    private SpecialProductItem item;
    int curShowId = 0;
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    Vector2 pos = Vector2.zero;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (Input.mousePosition.x - pos.x > Screen.width / 4)
            {
                curShowId--;
            }
            else if (Input.mousePosition.x - pos.x < -Screen.width / 4)
            {
                curShowId++;
            }
            else return;
            curShowId = Mathf.Max(Mathf.Min(paths.Count - 1, curShowId), 0);
            if (curShowId < paths.Count) ShowMedia(paths[curShowId]);
        }
    }
    public void ShowMedia(string path)
    {
        rawimage.gameObject.SetActive(true);
        if (vp != null && vp.isPlaying) vp.Stop();
        if (Utils.IsImage(path))
        {
            playbtn.gameObject.SetActive(false);
            StartCoroutine(Utils.ShowImage(rawimage.gameObject, path));
        }
        else if (Utils.IsVideo(path))
        {
            playbtn.gameObject.SetActive(true);
            vp.url = path;
            vp.Stop();
            rawimage.texture = null;
        } else
        {
            playbtn.gameObject.SetActive(false);
        }
    }
    public void SetValues(bool isSpecial, SpecialProductItem item, string tit = "")
    {
        curShowId = 0;
        paths.Clear();
        if (tit != "") title.GetComponent<Text>().text = tit;
        Utils.SetUIElement(header, body, bodycontent);
        sItem = this.item = item;
        okBtn.GetComponentInChildren<Text>().text = Global.isAdmin ? "확 인" : "주문하기";
        gameObject.SetActive(true);
        GameObject[] a = new GameObject[] { productname, productnumber, price, description };
        foreach (GameObject b in a) b.GetComponent<Text>().fontSize = (int)(130f / 2880 * Screen.width);
        foreach (GameObject b in titles) b.GetComponent<Text>().fontSize = (int)(130f / 2880 * Screen.width);
        okBtn.GetComponentInChildren<Text>().fontSize = (int)(140f / 2880 * Screen.width);
        productname.GetComponent<Text>().text = "" + item.title;
        productnumber.GetComponent<Text>().text = "" + item.serial_number;
        price.GetComponent<Text>().text = "" + item.price;
        description.GetComponent<Text>().text = "" + item.description;
        StartCoroutine(GetDataAndShow(isSpecial, item));
    }

    IEnumerator GetDataAndShow(bool isSpecial, SpecialProductItem item)
    {
        var w = UnityWebRequest.Get((isSpecial ? Global.URL_SPECIALPRODUCTONE : Global.URL_ORDERINGPRODUCTONE) + "?id=" + item.id);
        if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
        yield return w.SendWebRequest();
        if (string.IsNullOrEmpty(w.error))
        {
            JSONNode a = JSON.Parse(w.downloadHandler.text);
            if (a["status"].AsBool)
            {
                if (a.ContainsKey("others") && a["others"]!=null)
                {
                    int cnt = a["others"].Count;
                    for (int i=0; i< cnt; i++)
                    {
                        paths.Add((isSpecial ? Global.URL_IMAGEPATH_SPECIAL : Global.URL_IMAGEPATH_ORDERING) + item.folder + "/" + a["others"][i]);
                    }
                    if (paths.Count > curShowId) ShowMedia(paths[curShowId]);
                }
            }
        } else
        {
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.WARNING));
        }
    }

    public void BackBtnClick()
    {
        if (vp != null && vp.isPlaying) vp.Stop();
        rawimage.gameObject.SetActive(false);
        playbtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        rawimage.gameObject.SetActive(false);
        playbtn.gameObject.SetActive(false);
        if (vp != null && vp.isPlaying) vp.Stop();
    }
    public void OKBtnClick()
    {
        if (Global.isAdmin) gameObject.SetActive(false);
        else
        {
            if (!Global.isLogin)
            {
                Utils.it.GotoLogin();
            }
            else paymentsystem.SetActive(true);
        }
    }

    public void PlayBtnClick()
    {
        playbtn.gameObject.SetActive(false);
        rawimage.texture = tx;
        if (vp != null && !vp.isPlaying)
        {
            vp.Play();
        }
    }
}


