using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin;
    // Start is called before the first frame update
    void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tmp.color = new Color(1,1,1,opacityCurve.Evaluate(time));   
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0,heightCurve.Evaluate(time),0);
        time += Time.deltaTime;
    }
}
