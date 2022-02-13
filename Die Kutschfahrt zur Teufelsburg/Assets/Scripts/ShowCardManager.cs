using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCardManager : MonoBehaviour
{
    [SerializeField] Transform ShowCardPostionLeft;
    [SerializeField] Transform ShowCardPostionRight;
    [SerializeField] GameObject ShowCardPanel;

    [ContextMenu("展示牌动画对其")]
    public void ShowCardAnimation()
    {
        float space = ShowCardPostionRight.position.x - ShowCardPostionLeft.position.x;
        float cardWidth = 2.2f / (1 + (float)ShowCardPanel.transform.childCount / 10);
        float cardArea = ShowCardPanel.transform.childCount * cardWidth;
        float spawn = (space - cardArea) / 2 + ShowCardPostionLeft.position.x + cardWidth / 2;

        for (int i = 0; i < ShowCardPanel.transform.childCount; i++)
        {
            ShowCardPanel.transform.GetChild(i).transform.DOMove(new Vector3(spawn + i * cardWidth, ShowCardPostionLeft.position.y, 0), 0.3f);
            ShowCardPanel.transform.GetChild(i).GetComponent<Card>().index = i;
            ShowCardPanel.transform.GetChild(i).transform.SetSiblingIndex(i);
        }
    }
}
