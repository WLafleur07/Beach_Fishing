using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewDimensions : MonoBehaviour
{
    
    void Start()
    {
        const float ORIGINAL_HEIGHT = 960.0f;
        const float LARGE_HEIGHT = 3000.0f;

        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform myRect = gameObject.GetComponent<RectTransform>();

        if (canvas.pixelRect.height > ORIGINAL_HEIGHT && canvas.pixelRect.height < LARGE_HEIGHT)
        {
            float scaleY = ((canvas.pixelRect.height / ORIGINAL_HEIGHT) + (canvas.scaleFactor / 2f)) / canvas.scaleFactor;
            myRect.offsetMin = new Vector2(myRect.offsetMin.x, myRect.offsetMin.y * scaleY);
        }
        else if (canvas.pixelRect.height >= LARGE_HEIGHT)
        {
            float scaleY = ((canvas.pixelRect.height / ORIGINAL_HEIGHT) + (canvas.scaleFactor + (canvas.scaleFactor / 5))) / canvas.scaleFactor;
            myRect.offsetMin = new Vector2(myRect.offsetMin.x, myRect.offsetMin.y * scaleY);
        }
    }
}
