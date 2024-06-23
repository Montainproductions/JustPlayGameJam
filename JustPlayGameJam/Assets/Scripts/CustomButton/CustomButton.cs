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

    private int whichTransition  = 0;

    public void UpdateColors(Color startColor, Color enteredColor, Color selectedColor)
    {
        //Debug.Log(startColor + " " + enteredColor + " " + selectedColor);
        this.startColor = startColor;
        this.enteredColor = enteredColor;
        this.downColor = selectedColor;

        if(whichTransition == 2 || whichTransition == 1)
        {
            StartCoroutine(Transition(transform.localScale, enteredColor, 0.18f, 1));
        }
        else
        {
            StartCoroutine(Transition(transform.localScale, startColor, 0.18f, 0));
        }
    }


    IEnumerator Transition(Vector3 newSize, Color newColor, float transitionTime, int newTransition)
    {
        whichTransition = newTransition;
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
        //whichTransition = 1;
        StartCoroutine(Transition(enteredVector, enteredColor, 0.1f, 1));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //whichTransition = 0;
        StartCoroutine(Transition(startVector, startColor, 0.1f, 0));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //whichTransition = 2;
        StartCoroutine(Transition(downVector, downColor, 0.2f, 1));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //whichTransition = 2;
        //StartCoroutine(Transition(startVector, startColor, 0.02f));
        OnEvent.Invoke();
        Debug.Log("UI Button Clicked");
    }
}
