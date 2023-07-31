using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PaymentType
{
    Cacao, Neighbour, Card
}

public class PaymentSystem : MonoBehaviour {
    public GameObject header, body, bodycontent;
    public GameObject payment;
    public ToggleGroup tg;
    public double ovalue;
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
    public void BackBtnClick()
    {
        gameObject.SetActive(false);
    }
    public void NextBtnClick()
    {
        if (ProductDetail.sItem == null) ovalue = 0;
        else ovalue = ProductDetail.sItem.price;
        GameObject b = payment;
        foreach (Toggle a in tg.ActiveToggles())
        {
            if (a.gameObject.GetComponent<PaymentToggle>().Type == PaymentType.Cacao)
            {
                b.GetComponent<Payment>().NextWith(PaymentType.Cacao, ovalue);
            }
            if (a.gameObject.GetComponent<PaymentToggle>().Type == PaymentType.Neighbour)
            {
                b.GetComponent<Payment>().NextWith(PaymentType.Neighbour, ovalue);
            }
            if (a.gameObject.GetComponent<PaymentToggle>().Type == PaymentType.Card)
            {
                b.GetComponent<Payment>().NextWith(PaymentType.Card, ovalue);
            }
        }
    }
}
