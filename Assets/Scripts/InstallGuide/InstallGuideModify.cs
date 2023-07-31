using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InstallGuideModify : MonoBehaviour {
    public GameObject path, desc, title;
    public GameObject header, body, bodycontent, loading, installguide;
    public List<string> uploadfiles = new List<string>();
    private int id;
    private InstallGuideItem dd;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent);
        foreach (Text a in desc.GetComponentsInChildren<Text>())
        {
            a.fontSize = (int)(140f / 2880 * Screen.width);
        }
    }
    public void SetValues(InstallGuideItem a)
    {
        id = a.id;
        path.GetComponent<InputField>().text = "";
        desc.GetComponent<InputField>().text = a.setup_guide;
        title.GetComponent<InputField>().text = a.title;
        uploadfiles.Clear();
        gameObject.SetActive(true);
        dd = a;
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void ModifyBtnCLick()
    {
        StartCoroutine(ModifyResult());
    }

    IEnumerator ModifyResult()
    {
        loading.SetActive(true);
        WWWForm form = new WWWForm();
        Dictionary<string, string> postHeader = form.headers;
        postHeader.Add("Content-type", "application/json");
        if (uploadfiles.Count > 0)
        {
            form.AddField("folder", dd.folder);
            for (int i = 0; i < uploadfiles.Count; i++)
            {
                string path = uploadfiles[i];
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
        form.AddField("setup_guide", desc.GetComponent<InputField>().text);
        form.AddField("title", title.GetComponent<InputField>().text);
        using (var w = UnityWebRequest.Post(Global.URL_INSTALLGUIDEMODIFY + id, form))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            yield return w.SendWebRequest();
            loading.SetActive(false);
            if (w.isNetworkError)
            {
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            }
            else
            {
                StartCoroutine(Utils.ShowMessage("성과적으로 편집되었습니다!", MessageType.SUCCESS));
            }
            gameObject.SetActive(false);
            installguide.SetActive(false);
            installguide.SetActive(true);
        }
    }

    public void AddFileBtnClick()
    {
        string tpath = NativeGallery.PickImageOrVideo();
        if (string.IsNullOrEmpty(tpath)) return;
        string path = tpath;
        uploadfiles.Add(path);
        if (uploadfiles.Count > 1)
            this.path.GetComponent<InputField>().text += ",";
        this.path.GetComponent<InputField>().text += Utils.GetFileNameFromPath(path);
    }
}
