using UnityEngine;

/// <summary>
/// Controls toggling the visibility of an informational text box popup in VR.
/// </summary>
public class InfoBoxToggle : MonoBehaviour
{
    /// <summary>
    /// The informational text box panel GameObject to show or hide.
    /// </summary>
    public GameObject infoBoxPanel;

    // Tracks the current visibility state of the info box.
    private bool isVisible = false;

    // Start is called before the first frame update.
    private void Start()
    {
        if (infoBoxPanel != null)
        {
            infoBoxPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Toggles the active state of the info box panel between open and closed.
    /// </summary>
    public void ToggleInfoBox()
    {
        if (infoBoxPanel == null) return;

        isVisible = !isVisible;
        infoBoxPanel.SetActive(isVisible);
    }
}
