using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class RepairDetail : MonoBehaviour {
    public GameObject date, address, sellpos, sellername, request;
    public GameObject playbutton, background, username;
    public RawImage rawImage;
    public VideoPlayer vp;
    private string imagepath;
    private RepairingItem item;
    public GameObject header, body, bodycontent, loading;
    // Use this for initialization
    void Start () {
	}

    void OnDisable()
    {
        if (vp != null && vp.isPlaying) vp.Stop();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent);
    }

    public void SetValues(RepairingItem item)
    {
        date.GetComponent<Text>().text = "구입날짜 : " + item.date;
        address.GetComponent<Text>().text = "연락처 : " + item.user_address;
        sellpos.GetComponent<Text>().text = "구입처 : " + item.seller_address;
        sellername.GetComponent<Text>().text = "수취인 이름: " + item.sellername;
        request.GetComponent<Text>().fontSize = (int)(140 / 2880f * Screen.width);
        request.GetComponent<Text>().text = "요청사항 : " + item.description;
        imagepath = item.image;
        rawImage.gameObject.SetActive(false);
        playbutton.gameObject.SetActive(false);
        vp.gameObject.SetActive(false);
        StartCoroutine(Show());
        this.item = item;
    }

    IEnumerator Show()
    {
        if (Utils.IsImage(imagepath))
        {
            WWW www = new WWW(imagepath);
            loading.SetActive(true);
            yield return www;
            loading.SetActive(false);
            if (string.IsNullOrEmpty(www.error))
            {
                rawImage.texture = www.texture;
                rawImage.gameObject.SetActive(true);
            } else
            {
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다."));
            }
        } else if (Utils.IsVideo(imagepath))
        {
            vp.gameObject.SetActive(true);
            if (vp != null && vp.isPlaying) vp.Stop();
            vp.url = imagepath;
        }
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void PlayBtnClick()
    {
        if (vp != null && !vp.isPlaying) vp.Play();
    }
}
