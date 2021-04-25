using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Code retrieved and edited from https://www.youtube.com/watch?v=ktybkQZp2A4&ab_channel=KeeGamedev
public class GrayScale : MonoBehaviour
{
    public Image image;
    private float duration = 1f;

    public void StartGrayScaleRoutine()
    {
        StartCoroutine(GrayScaleRoutine(duration, true));
    }

    public void Reset()
    {
        StartCoroutine(GrayScaleRoutine(duration, false));
    }

    private IEnumerator GrayScaleRoutine(float duration, bool isGrayScale)
    {
        float time = 0;
        while(duration > time)
        {
            float durationFrame = Time.deltaTime;
            float ratio = time / duration;
            float grayAmount = isGrayScale ? ratio : 1 - ratio;
            SetGrayScale(grayAmount);
            time += durationFrame;
            yield return null;
        }

        SetGrayScale(isGrayScale ? 1 : 0);
    }

    public void SetGrayScale(float amount = 1)
    {
        image.material.SetFloat("_GrayscaleAmount", amount);
    }
}
