using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int index;
    public CardData cardData;
    public bool isHand;
    public bool isUI;
    [SerializeField] Image image;
    [SerializeField] AudioSource audioSource;
    HandCardManager handCardManager;
    ShowCardManager showCardManager;

    public void InitCardData(CardData _cardData, bool _isHand = true, bool _isUI = false)
    {
        handCardManager = GameObject.Find("CardManager").GetComponent<HandCardManager>();
        showCardManager = GameObject.Find("CardManager").GetComponent<ShowCardManager>();
        
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        transform.position = GameObject.Find("DeckPostion").transform.position;
        cardData = _cardData;
        isHand = _isHand;
        isUI = _isUI;
        if (isHand)
        {
            handCardManager.HandCardAnimation();
        }
        else
        {
            showCardManager.ShowCardAnimation();
        }
        image.overrideSprite = Resources.Load<Sprite>("Images/" + cardData.cardRes);

        audioSource.Play();
    }

    public void InitCardDataMute(CardData _cardData)
    {
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        cardData = _cardData;
        image.overrideSprite = Resources.Load<Sprite>("Images/" + cardData.cardRes);
        isUI = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        if (isUI) return;
        transform.SetSiblingIndex(9999);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f);
        if (isUI) return;
        transform.SetSiblingIndex(index);
    }

    public void onButtonClick_Card()
    {
        GameManager.Instance.localGamePlayer.onButtonClick_Card(this, GetComponent<RectTransform>());
    }
}
