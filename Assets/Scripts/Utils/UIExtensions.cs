using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtensions
{
    public static void Fade(this Image self, float seconds)
    {
        self.gameObject.SetActive(true);
        self.StartCoroutine(FadeImage(self, seconds));
    }

    private static IEnumerator FadeImage(Image image, float seconds)
    {
        for (float t = 0.0f; t < seconds; t += UnityEngine.Time.deltaTime)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, t / seconds);
            yield return null;
        }
    }

    public static void Unfade(this Image self, float seconds)
    {
        self.StartCoroutine(UnfadeImage(self, seconds));
    }

    private static IEnumerator UnfadeImage(Image image, float seconds)
    {
        for (float t = seconds; t > 0.0f; t -= UnityEngine.Time.deltaTime)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, t / seconds);
            yield return null;
        }
        image.gameObject.SetActive(false);
    }
}