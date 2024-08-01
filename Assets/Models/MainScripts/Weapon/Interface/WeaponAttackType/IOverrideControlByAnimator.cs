using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IOverrideControlByAnimator
{
    Animator Animator { get; set; }
    void AssignAnimationIDs();
}
