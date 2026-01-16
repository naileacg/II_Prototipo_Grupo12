using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the VR main menu interaction using gaze-based input.
/// The player selects buttons by looking at them for a certain amount of time.
/// </summary>
public class VRMainMenuController : MonoBehaviour
{
    /// <summary>
    /// Image used as a reticle visual feedback (filled over time).
    /// </summary>
    public Image reticuleImage;

    /// <summary>
    /// Time in seconds the player must gaze at a button to activate it.
    /// </summary>
    public float time = 2.0f;
    
    /// <summary>
    /// Accumulated gaze time over the current button.
    /// </summary>
    private float temp = 0f;

    /// <summary>
    /// Button currently being gazed at.
    /// </summary>
    private Button currentButton;

    private void Update()
    {
        RaycastHit hit;

        // Raycast forward from the camera to detect UI buttons
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            Button button = hit.transform.GetComponent<Button>();
            
            if (button != null)
            {
                // If we start gazing at a new button, reset the timer
                if (button != currentButton)
                {
                    currentButton = button;
                    temp = 0f;
                }

                // Increase gaze time
                temp += Time.deltaTime;

                // Update reticle fill feedback
                if (reticuleImage != null)
                    reticuleImage.fillAmount = temp / time;

                // If gaze time is completed, trigger the button action
                if (temp >= time)
                {
                    button.onClick.Invoke();
                    Restart();
                }
            }
            else
            {
                Restart();
            }
        }
        else
        {
            Restart();
        }
    }

    /// <summary>
    /// Resets the gaze timer and clears the current button selection.
    /// </summary>
    void Restart()
    {
        temp = 0f;
        currentButton = null;
    }
}
