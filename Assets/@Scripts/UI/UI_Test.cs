using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Test : MonoBehaviour
{
    public GameObject rect;
    void Start()
    {
        rect.GetComponent<RectTransform>().position = new Vector3(0, -300f, 0); // ��ȥ ���� �ʱ� ����

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
