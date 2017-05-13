using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNum : MonoBehaviour
{
    private Text textNum;

    private void Awake()
    {
        textNum = GetComponent<Text>();
        Invoke("RemoveSelf", 2f);
    }

    private void Update()
    {
        this.transform.Translate(Vector3.up * Time.deltaTime * 10);
    }

    private void RemoveSelf()
    {
        Destroy(this.gameObject);
    }

    public void SetText(string str)
    {
        textNum.text = str;
    }

}
