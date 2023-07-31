using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class Payment : MonoBehaviour {
    public GameObject header, body, bodycontent, loading;
    public GameObject ordervalue, payvalue, paymentSystem, payment, paymentSuccess;
    private double ovalue, pvalue;
    private PaymentType tp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnEnable()
    {
        Utils.SetUIElement(header, body, bodycontent);
    }

    public void NextWith(PaymentType type, double ovalue)
    {
        gameObject.SetActive(true);
        this.ovalue = ovalue;
        tp = type;
        this.ordervalue.GetComponent<Text>().text = ovalue.ToString();
        this.payvalue.GetComponent<Text>().text = ovalue.ToString();
    }

    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void PayBtnClick()
    {
        StartCoroutine(PayBtnClicked());
    }
    IEnumerator PayBtnClicked() {
        this.pvalue = double.Parse(payvalue.GetComponent<Text>().text);
        string v = tp == PaymentType.Cacao ? "카카오 페이" : tp == PaymentType.Neighbour ? "네이버 페이" : "신용카드 결제";
        WWWForm wf = new WWWForm();
        wf.AddField("payment_type", v);
        wf.AddField("username", Global.username);
        wf.AddField("amount", this.pvalue + "");
        var w = UnityWebRequest.Post(Global.URL_PAYMENTCREATE, wf);
        if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
        yield return w.SendWebRequest();

        if (ProductDetail.sItem != null && ProductDetail.sItem.id == -1)
        {
            Chatting.GetInstance().SendPayMessage();
            paymentSystem.SetActive(false);
            payment.SetActive(false);
            paymentSuccess.SetActive(true);
            yield break;
        }
//        StartCoroutine(Utils.ShowMessage("결제금액 : " + pvalue + "\n결제방식 : " + v));
        paymentSystem.SetActive(false);
        StartCoroutine(AddOrderRequest());
    }

    IEnumerator AddOrderRequest()
    {
        yield return new WaitForEndOfFrame();
        WWWForm wf = new WWWForm();
        Dictionary<string, string> postHeader = wf.headers;
        postHeader.Add("Content-type", "application/json");
        string[] type = new string[] { "카카오 페이", "네이버 페이", "신용카드 결제" };
        wf.AddField("pay_type", type[(int)tp]);
        wf.AddField("username", Global.username);
        wf.AddField("user_email", Global.useremail);
        if (ProductDetail.sItem!=null) wf.AddField("product_name", ProductDetail.sItem.title);
        if (ProductDetail.sItem != null) wf.AddField("product_id", ProductDetail.sItem.id);
        wf.AddField("user_id", Global.userid);
        wf.AddField("pay_status", 1);
        loading.SetActive(true);
        using (var w = UnityWebRequest.Post(Global.URL_PRODUCTORDER, wf))
        {
            if (!string.IsNullOrEmpty(Global.token)) w.SetRequestHeader("Auth_Token", Global.token);
            yield return w.SendWebRequest();
            if (w.isNetworkError)
            {
                StartCoroutine(Utils.ShowMessage("서버접속이 원할하지 않습니다.", MessageType.FAILD));
            } else
            {
                if (ProductDetail.sItem != null)
                {
                    paymentSuccess.GetComponent<PaymentSuccess>().SetPrice(ProductDetail.sItem.price);
                    paymentSuccess.SetActive(true);
                }
                gameObject.SetActive(false);
            }
        }
        loading.SetActive(false);
    }
}
