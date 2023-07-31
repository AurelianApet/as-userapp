using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaqUIItem : MonoBehaviour {
    public GameObject description, faq, modifybtn, deletebtn;
    private FaqItem item;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValues(FaqItem item, GameObject faq)
    {
        gameObject.SetActive(true);
        this.item = item;
        this.faq = faq;
        description.GetComponent<Text>().text = item.answer;
        description.GetComponent<Text>().fontSize = (int)(140f / 2880 * Screen.width);
        modifybtn.SetActive(Global.isAdmin);
        deletebtn.SetActive(Global.isAdmin);
    }

    public void ModifyBtnClick()
    {
        faq.GetComponent<Faq>().Modify(item);
    }

    public void DeleteBtnClick()
    {
        faq.GetComponent<Faq>().Delete(item.id);
    }
}
