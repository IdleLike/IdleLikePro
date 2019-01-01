using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentText : MonoBehaviour
{

	void Awake ()
    {
        var m_Text = GetComponent<Text>();
        var m_Height = m_Text.preferredHeight;
        var m_Rect = GetComponent<RectTransform>().rect;
        GetComponent<RectTransform>().sizeDelta = new Vector2(m_Rect.width,m_Height);
    }
}
