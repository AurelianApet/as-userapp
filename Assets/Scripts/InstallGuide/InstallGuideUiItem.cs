using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class InstallGuideUiItem : MonoBehaviour {
    public GameObject text;
    public GameObject attachment;
    public GameObject rawImage;
    public GameObject playBtn;
    public GameObject firstline;
    public GameObject showscene;
    public GameObject manager;
    public GameObject modifybtn, deletebtn;
    private string attachmentPath;
    private int id;
    private InstallGuideItem item;
    // Use this for initialization
    void Start () {
	}

    void OnEnable()
    {
        rawImage.GetComponent<RawImage>().texture = null;
        text.GetComponent<Text>().fontSize = (int)(140f / 2880 * Screen.width);
        modifybtn.SetActive(Global.isAdmin);
        deletebtn.SetActive(Global.isAdmin);
    }

    // Update is called once per frame
    void Update () {
	}

    public void SetValue(InstallGuideItem a, int index, GameObject parent, GameObject show, GameObject manager)
    {
        showscene = show;
        this.manager = manager;
        firstline.SetActive(index == 0);
        item = a;
        id = a.id;
        transform.SetParent(parent.transform);
        text.GetComponent<Text>().text = a.title;
        RectTransform rt = GetComponent<RectTransform>();
        float H = 490f / 2880 * Screen.width;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = new Vector2(0, H);
        rt.anchoredPosition = new Vector2(0, -(H + 10) * index);
        rt.localScale = new Vector3(1, 1, 1);
        rt = parent.GetComponent<RectTransform>();
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = new Vector2(0, (H + 10) * (index + 1));
    }

    public void OnDeleteBtnClick()
    {
        manager.GetComponent<InstallGuide>().DeleteConfirm(id);
    }

    public void OnModifyBtnClick()
    {
        manager.GetComponent<InstallGuide>().modifyscene.GetComponent<InstallGuideModify>().SetValues(item);
    }

    public void OnClickItem()
    {
        showscene.SetActive(true);
        showscene.GetComponent<InstallGuideIVShow>().SetValues(item, "설치가이드상세보기");
    }
}
