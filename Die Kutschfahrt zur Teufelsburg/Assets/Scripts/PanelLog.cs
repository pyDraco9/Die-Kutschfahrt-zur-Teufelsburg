using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLog : MonoBehaviour
{
    [SerializeField] Text text;
    public void SetLogText(string _text)
    {
        text.text = _text;
    }
}
