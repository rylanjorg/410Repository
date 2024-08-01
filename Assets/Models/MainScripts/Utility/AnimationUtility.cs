using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtility 
{
    public enum AnimationCycleDuration
    {
        OneFrame = 1,
        TwoFrames = 2,
        ThreeFrames = 3,
        FourFrames = 4,
    }


    public static bool AnimCycle_ShouldUpdateVariable(int currentFrame, AnimationCycleDuration cycleDuration)
    {
        int cycleFrameCount = (int)cycleDuration;

        // Calculate the target frame based on the AnimationCycleDuration and average FPS
        float averageFPS = FrameRateManager.Instance.AverageFPS / 30.0f;
        int frameRatio = Mathf.Clamp(Mathf.RoundToInt(averageFPS), 1, 10);  // Adjust the maximum value as needed

        int targetFrame = cycleFrameCount * frameRatio;

        int normalizedFrame = (currentFrame % targetFrame) + 1;

        if (targetFrame == 0)
        {
            //Debug.LogError("Target frame is 0. afps: " + targetFrame);
            return false;
        }
        else
        {
            //Debug.Log($"Normalized Frame: {normalizedFrame}, Target Frame: {targetFrame}, Current Frame: {currentFrame}");
            return normalizedFrame == targetFrame;
        }
    }
}



