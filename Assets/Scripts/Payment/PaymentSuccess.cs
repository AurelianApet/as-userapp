using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaymentSuccess : MonoBehaviour
{
    public Text price;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OkBtnClick()
    {
        gameObject.SetActive(false);
    }

    public void SetPrice(double val)
    {
        price.text = val+"";
    }
}
