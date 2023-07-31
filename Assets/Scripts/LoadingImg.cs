using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingImg : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	void OnEnable()
    {
        float H = 500f / 2880 * Screen.width;
        RectTransform a = gameObject.GetComponent<RectTransform>();
        a.anchorMin = a.anchorMax = new Vector2(0.5f, 0.5f);
        a.offsetMax = new Vector2(H / 2, H / 2);
        a.offsetMin = new Vector2(-H / 2, -H / 2);
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, -500 * Time.deltaTime);
	}
}
