using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField]
    private Image buttonBackground;

    [SerializeField]
    private Vector3 startVector, enteredVector, downVector;

    [SerializeField]
    private Color startColor, enteredColor, downColor;

    [System.Serializable]
    public class CustomUIEvent : UnityEvent { }
    public CustomUIEvent OnEvent;

    private int whichTransition;

    public void UpdateColors(Color startColor, Color enteredColor, Color selectedColor)
    {
        //Debug.Log(startColor + " " + enteredColor + " " + selectedColor);
        this.startColor = startColor;
        this.enteredColor = enteredColor;
        this.downColor = selectedColor;

        if (whichTransition == 0)
        {
            StartCoroutine(Transition(transform.localScale, this.startColor, 0.1f));
        }else if (whichTransition == 1)
        {
            StartCoroutine(Transition(transform.localScale, this.enteredColor, 0.1f));
        }
        else
        {
            StartCoroutine(Transition(transform.localScale, this.downColor, 0.1f));
        }
    }


    IEnumerator Transition(Vector3 newSize, Color newColor, float transitionTime)
    {
        float timer = 0;
        Vector3 startSize = transform.localScale;
        Color startColor = buttonBackground.color;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            yield return null;
            transform.localScale = Vector3.Lerp(startSize, newSize, timer / transitionTime);
            buttonBackground.color = Color.Lerp(startColor, newColor, timer / transitionTime);
        }
        yield return null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        whichTransition = 1;
        StartCoroutine(Transition(enteredVector, enteredColor, 0.15f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        whichTransition = 0;
        StartCoroutine(Transition(startVector, startColor, 0.15f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        whichTransition = 2;
        StartCoroutine(Transition(downVector, downColor, 0.3f));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        whichTransition = 2;
        //StartCoroutine(Transition(startVector, startColor, 0.02f));
        OnEvent.Invoke();
        Debug.Log("UI Button Clicked");
    }
}
