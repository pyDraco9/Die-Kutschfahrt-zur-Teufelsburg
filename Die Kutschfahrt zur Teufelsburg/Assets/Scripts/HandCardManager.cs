using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCardManager : MonoBehaviour
{
    [SerializeField] Transform HandCardPostionLeft;
    [SerializeField] Transform HandCardPostionRight;
    [SerializeField] GameObject HandCardPanel;

    [ContextMenu("手牌动画对其")]
    public void HandCardAnimation()
    {
        float space = HandCardPostionRight.position.x - HandCardPostionLeft.position.x;
        float cardWidth = 2.2f / (1 + (float)HandCardPanel.transform.childCount / 10);
        float cardArea = HandCardPanel.transform.childCount * cardWidth;
        float spawn = (space - cardArea) / 2 + HandCardPostionLeft.position.x + cardWidth /2 ;

        for (int i = 0; i < HandCardPanel.transform.childCount; i++)
        {
            HandCardPanel.transform.GetChild(i).transform.DOMove(new Vector3(spawn + i * cardWidth, HandCardPostionLeft.position.y, 0), 0.3f);
            HandCardPanel.transform.GetChild(i).GetComponent<Card>().index = i;
            HandCardPanel.transform.GetChild(i).transform.SetSiblingIndex(i);
        }
    }
}
