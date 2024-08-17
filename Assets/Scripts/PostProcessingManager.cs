using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class PostProcessingManager : MonoBehaviour
{
    private static PostProcessingManager instance;
    private PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;
    private DepthOfField depthOfField;
    private ChromaticAberration chromaticAberration;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            postProcessVolume = GetComponent<PostProcessVolume>();
            postProcessVolume.profile.TryGetSettings(out colorGrading);
            postProcessVolume.profile.TryGetSettings(out depthOfField);
            postProcessVolume.profile.TryGetSettings(out chromaticAberration);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void EnableEffects(float duration)
    {
        instance.StartCoroutine(instance.FadeEffects(true, duration));
    }

    public static void DisableEffects(float duration)
    {
        instance.StartCoroutine(instance.FadeEffects(false, duration));
    }

    private IEnumerator FadeEffects(bool fadeIn, float duration)
    {
        float elapsedTime = 0f;
        Color startTint = colorGrading.colorFilter.value;
        Color targetTint = fadeIn ? new Color(0.5f, 1f, 1f, 1f) : Color.white; // Blue-teal tint
        float startBlur = depthOfField.focusDistance.value;
        float targetBlur = fadeIn ? 0.1f : 5f;
        float startAberration = chromaticAberration.intensity.value;
        float targetAberration = fadeIn ? 1f : 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            colorGrading.colorFilter.value = Color.Lerp(startTint, targetTint, t);
            depthOfField.focusDistance.value = Mathf.Lerp(startBlur, targetBlur, t);
            chromaticAberration.intensity.value = Mathf.Lerp(startAberration, targetAberration, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        colorGrading.colorFilter.value = targetTint;
        depthOfField.focusDistance.value = targetBlur;
        chromaticAberration.intensity.value = targetAberration;
    }
}