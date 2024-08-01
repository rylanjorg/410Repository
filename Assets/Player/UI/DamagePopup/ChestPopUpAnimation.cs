using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChestPopUpAnimation : MonoBehaviour
{
    public enum PopUpState
    {
        FadeIn,
        Active,
        InActive,
        FadeOut
    }


    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin;
    public PopUpState popUpState;
    public bool doFadeOut;
    public bool doFadeIn;
    // Start is called before the first frame update
    void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
        popUpState = PopUpState.InActive;
        doFadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (popUpState)
        {
            case PopUpState.FadeIn:
                FadeIn();
                break;
            case PopUpState.FadeOut:
                FadeOut();
                break;
            case PopUpState.Active:
                Active();
                break;
            case PopUpState.InActive:
                InActive();
                break;
        }
    }


    void FadeIn()
    {
        tmp.color = new Color(1,1,1,opacityCurve.Evaluate(time));   
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0,heightCurve.Evaluate(time),0);
        time += Time.deltaTime;

        if(time >= 1)
        {
            popUpState = PopUpState.Active;
        }
    }

    void Active()
    {
        if(doFadeOut)
        {
            popUpState = PopUpState.FadeOut;
            doFadeOut = false;
        }
    }

    void InActive()
    {
        if(doFadeIn)
        {
            popUpState = PopUpState.FadeIn;
            doFadeIn = false;
        }
    }

    public void SetFadeOut()
    {
        doFadeOut = true;
    }

    public void SetFadeIn()
    {
        doFadeIn = true;
    }

    void FadeOut()
    {
        tmp.color = new Color(1,1,1,opacityCurve.Evaluate(time));   
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0,heightCurve.Evaluate(time),0);
        time -= Time.deltaTime;

        if(time <= 0)
        {
            popUpState = PopUpState.InActive;
        }
    }




}