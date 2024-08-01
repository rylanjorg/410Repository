using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
public class VFXListHolder : MonoBehaviour
{
    [TabGroup("tab1","VFXEvents", TextColor = "purple")] [SerializeField] public List<VisualEffectStruct> vfxStructs = new List<VisualEffectStruct>();
    [TabGroup("tab1","VFXEvents", TextColor = "purple")] [SerializeField] [ReadOnly] public VFXSpawner vfxSpawner;// Start is called before the first frame update
    
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
