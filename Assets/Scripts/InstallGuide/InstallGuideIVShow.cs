using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SimpleJSON;
using UnityEngine.Networking;

public class InstallGuideIVShow : MonoBehaviour {
    public RawImage rawimage;
    public Button playbtn, pausebtn;
    public VideoPlayer vp;
    public Texture tx;
    public Text description, title;
    public string imagepath, videopath;
    public GameObject header, body, bodycontent, loading;
    // Use this for initialization
    void Start () {
        vp.errorReceived += Vp_errorReceived;
        vp.loopPointReached += Vp_loopPointReached;
    }

    private void Vp_loopPointReached(VideoPlayer vp)
    {
        playbtn.gameObject.SetActive(true);
        vp.Stop();
    }

    // Update is called once per frame
    Vector2 pos = Vector2.zero;
    void Update () {
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
            curShowId = Mathf.Max(Mathf.Min(paths.Count-1, curShowId), 0);
            if (curShowId<paths.Count) ShowMedia(paths[curShowId]);
        }
	}
    void OnEnable()
    {
        rawimage.texture = null;
        Utils.SetUIElement(header, body, bodycontent);
        description.fontSize = (int)(130f / 2880 * Screen.width);
    }
    public void SetValues(string path, string tit = "동영상보기")
    {
        paths.Clear();
        paths.Add(path);
        curShowId = 0;
        description.text = "";
        ShowMedia(path);
    }

    int curShowId = 0;
    List<string> paths = new List<string>();
    IEnumerator GetDataAndShow(object obj)
    {
        InstallGuideItem item = null;
        RepairingItem item1 = null;
        if (obj is InstallGuideItem) item = (InstallGuideItem)obj;
        if (obj is RepairingItem) item1 = (RepairingItem)obj;
        loading.SetActive(true);
        curShowId = 0;
        print(item1 == null ? Global.URL_INSTALLGUIDEONEGET + "?id=" + item.id : Global.URL_REPAIRANDACCEPONEGET + "?id=" + item1.id);
        var w = UnityWebRequest.Get(item1==null?Global.URL_INSTALLGUIDEONEGET + "?id=" + item.id:Global.URL_REPAIRANDACCEPONEGET+"?id="+item1.id);
        if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
        yield return w.SendWebRequest();
        loading.SetActive(false);
        if (!string.IsNullOrEmpty(w.error))
        {
            StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            yield break;
        }
        JSONNode a = JSON.Parse(w.downloadHandler.text);
        if (!a["status"].AsBool)
        {
            StartCoroutine(Utils.ShowMessage(a["message"], MessageType.FAILD));
            yield break;
        }

        JSONNode node = a["result"];
        if (item != null)
        {
            if (node.ContainsKey("setup_guide")) description.text = node["setup_guide"];
            if (node.ContainsKey("folder") && !string.IsNullOrEmpty(node["folder"]))
            {
                string folder = node["folder"];
                if (a.ContainsKey("others") && a["others"] != null)
                {
                    for (int i = 0; i < a["others"].Count; i++)
                    {
                        paths.Add((item == null ? Global.URL_IMAGEPATH_REPAIR : Global.URL_IMAGEPATH_GUIDE) + folder + "/" + a["others"][i]);
                        print(paths[paths.Count - 1]);
                    }
                }
            }
        }
        if (item1 != null)
        {
            string str = "";
            if (node.ContainsKey("sell_date")) str += "구입날짜 : " + node["sell_date"];
            if (node.ContainsKey("user_address")) str += "\n연락처 : " + node["user_address"];
            if (node.ContainsKey("seller_address")) str += "\n구입처 : " + node["seller_address"];
            if (node.ContainsKey("sellername")) str += "\n수취인이름 : " + node["sellername"];
            if (node.ContainsKey("description")) str += "\n요청사항 : " + node["description"];
            description.text = str;
            if (node.ContainsKey("image"))
            {
                paths.Add(Global.URL_IMAGEPATH_REPAIR + node["image"]);
                print(Global.URL_IMAGEPATH_REPAIR + node["image"]);
            }
        }

        if (paths.Count == 0) yield break;
        ShowMedia(paths[curShowId]);
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
            rawimage.texture = null;
        }
    }

    public void SetValues(InstallGuideItem item, string tit)
    {
        paths.Clear();
        title.text = tit;
        rawimage.texture = null;
        StartCoroutine(GetDataAndShow(item));
    }
    public void SetValues(RepairingItem item, string tit)
    {
        paths.Clear();
        title.text = tit;
        rawimage.texture = null;
        StartCoroutine(GetDataAndShow(item));
    }

    private void Vp_errorReceived(VideoPlayer source, string message)
    {
//        StartCoroutine(Utils.ShowMessage("There is a problem to play this video.", MessageType.FAILD));
    }

    IEnumerator ShowVideo()
    {
        if (vp != null && vp.isPlaying)
        {
            vp.Stop();
        }
        vp.url = videopath;
        vp.Play();
        yield break;
    }

    public void PlayBtnClick()
    {
        playbtn.gameObject.SetActive(false);
        rawimage.texture = tx;
//        pausebtn.gameObject.SetActive(false);
        if (vp != null && !vp.isPlaying)
        {
//            pausebtn.gameObject.SetActive(true);
            vp.Play();
        }
//        if (vp != null && vp.isPlaying)
//        {
//            playbtn.gameObject.SetActive(true);
////            pausebtn.gameObject.SetActive(false);
//            vp.Pause();
//        }
    }
    public void BackBtnClick()
    {
        if (vp != null && vp.isPlaying) vp.Stop();
        rawimage.gameObject.SetActive(false);
        playbtn.gameObject.SetActive(false);
//        pausebtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        rawimage.gameObject.SetActive(false);
        playbtn.gameObject.SetActive(false);
//        pausebtn.gameObject.SetActive(false);
        if (vp != null && vp.isPlaying) vp.Stop();
    }
}
