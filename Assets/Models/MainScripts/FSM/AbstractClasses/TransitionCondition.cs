using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

[System.Serializable]
public abstract class TransitionCondition
{
    public abstract bool IsMet();
    public virtual void Reset() {}
    public virtual void OnStart() {}
    public virtual void DrawGizmos() {}
}
