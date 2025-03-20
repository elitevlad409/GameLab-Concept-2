using UnityEngine;
using UnityEngine.UI;

public class TelescopeController : MonoBehaviour
{
    public GameObject telescopeOverlay; // The telescope sprite GameObject
    public float moveSpeed = 5f; // Speed of telescope movement
    public Button toggleButton; // UI Button to show/hide telescope

    private bool isTelescopeActive = false;

    void Start()
    {
        // Ensure telescope is hidden at start
        telescopeOverlay.SetActive(false);
        toggleButton.onClick.AddListener(ToggleTelescope);
    }

    void Update()
    {
        if (isTelescopeActive)
        {
            // Telescope movement with arrow buttons (you can map these to UI buttons later)
            float moveX = Input.GetAxisRaw("Horizontal"); // Left/Right
            float moveY = Input.GetAxisRaw("Vertical");   // Up/Down
            Vector3 movement = new Vector3(moveX, moveY, 0).normalized * moveSpeed * Time.deltaTime;
            telescopeOverlay.transform.position += movement;

            // Optional: Clamp telescope position to screen bounds
            Vector3 clampedPos = telescopeOverlay.transform.position;
            clampedPos.x = Mathf.Clamp(clampedPos.x, -8f, 8f); // Adjust based on your screen size
            clampedPos.y = Mathf.Clamp(clampedPos.y, -4.5f, 4.5f);
            telescopeOverlay.transform.position = clampedPos;
        }
    }

    void ToggleTelescope()
    {
        isTelescopeActive = !isTelescopeActive;
        telescopeOverlay.SetActive(isTelescopeActive);
    }
}