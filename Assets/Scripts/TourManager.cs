using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Manages room navigation and smooth camera fade transitions between 360 video spheres.
/// </summary>
public class TourManager : MonoBehaviour
{
    /// <summary>
    /// The starting sphere GameObject.
    /// </summary>
    public GameObject startingSphere;

    /// <summary>
    /// List of all room spheres in the tour.
    /// </summary>
    public List<GameObject> allSpheres;

    /// <summary>
    /// The CanvasGroup used for screen fading transitions.
    /// </summary>
    public CanvasGroup fadeCanvasGroup;

    /// <summary>
    /// Duration in seconds for fading out and fading in.
    /// </summary>
    public float fadeDuration = 0.5f;

    // The currently active sphere GameObject.
    private GameObject currentSphere;

    // Tracks if a room transition is currently running to prevent spam clicks.
    private bool isTransitioning = false;

    // Start is called before the first frame update.
    private void Start()
    {
        InitializeTour();
    }

    // Deactivates all spheres except the starting sphere.
    private void InitializeTour()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }

        if (allSpheres == null) return;

        foreach (GameObject sphere in allSpheres)
        {
            if (sphere != null)
            {
                sphere.SetActive(false);
            }
        }

        if (startingSphere != null)
        {
            currentSphere = startingSphere;
            currentSphere.SetActive(true);
            PlaySphereVideo(currentSphere);
        }
    }

    /// <summary>
    /// Initiates a smooth fade transition to the target sphere.
    /// </summary>
    /// <param name="targetSphere">The sphere GameObject to navigate to.</param>
    public void SwitchToSphere(GameObject targetSphere)
    {
        if (targetSphere == null || targetSphere == currentSphere || isTransitioning)
        {
            return;
        }

        StartCoroutine(TransitionRoutine(targetSphere));
    }

    // Coroutine that fades to black, swaps active rooms and videos, then fades back in.
    private IEnumerator TransitionRoutine(GameObject targetSphere)
    {
        isTransitioning = true;

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;

            // Fade out to black.
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        // Switch spheres while screen is black.
        if (currentSphere != null)
        {
            StopSphereVideo(currentSphere);
            currentSphere.SetActive(false);
        }

        currentSphere = targetSphere;
        currentSphere.SetActive(true);
        PlaySphereVideo(currentSphere);

        // Brief pause to allow the new video frame to render cleanly.
        yield return new WaitForSeconds(0.1f);

        // Fade in from black.
        if (fadeCanvasGroup != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / fadeDuration));
                yield return null;
            }
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }

        isTransitioning = false;
    }

    // Plays the VideoPlayer component on the given sphere.
    private void PlaySphereVideo(GameObject sphere)
    {
        VideoPlayer vp = sphere.GetComponent<VideoPlayer>();
        if (vp != null)
        {
            vp.Play();
        }
    }

    // Stops the VideoPlayer component on the given sphere to conserve resources.
    private void StopSphereVideo(GameObject sphere)
    {
        VideoPlayer vp = sphere.GetComponent<VideoPlayer>();
        if (vp != null)
        {
            vp.Stop();
        }
    }
}
