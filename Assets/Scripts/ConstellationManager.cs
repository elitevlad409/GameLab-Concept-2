using UnityEngine;
using System.Collections.Generic;

public class ConstellationManager : MonoBehaviour
{
    public List<GameObject> stars; // List of star GameObjects in this constellation
    public float detectionTime = 3f; // Time to hover over constellation
    public LineRenderer lineRenderer; // LineRenderer component for drawing lines

    private CircleCollider2D telescopeCollider;
    private float hoverTime = 0f;
    private bool isDetected = false;
    private List<int> tracedStars = new List<int>(); // Tracks stars clicked in sequence

    void Start()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer not assigned in Inspector!");
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = new Color(1, 1, 1, 0.3f); // Faint line
        lineRenderer.endColor = new Color(1, 1, 1, 0.3f);

        if (!GetComponent<CircleCollider2D>())
        {
            Debug.LogError("No CircleCollider2D found on this GameObject!");
        }

        // Debug the stars list
        Debug.Log($"Constellation {gameObject.name} has {stars.Count} stars assigned:");
        for (int i = 0; i < stars.Count; i++)
        {
            if (stars[i] == null)
            {
                Debug.LogError($"Star at index {i} is null!");
            }
            else
            {
                Debug.Log($"Star {i}: {stars[i].name} at position {stars[i].transform.position}");
            }
        }
    }

    void Update()
    {
        if (isDetected && telescopeCollider != null)
        {
            hoverTime += Time.deltaTime;
            Debug.Log($"Hovering: {hoverTime}/{detectionTime} seconds");

            if (hoverTime >= detectionTime && lineRenderer.positionCount == 0)
            {
                Debug.Log("Showing faint lines for all stars");
                ShowFaintLines();
            }
        }

        // Handle mouse clicks for tracing
        if (isDetected && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"Mouse clicked at world position: {mousePos}");
            TraceStar(mousePos);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Collision detected with: {other.gameObject.name}, Tag: {other.tag}");
        if (other.CompareTag("Telescope"))
        {
            telescopeCollider = other as CircleCollider2D;
            isDetected = true;
            hoverTime = 0f;
            Debug.Log("Telescope entered constellation area");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Telescope"))
        {
            telescopeCollider = null;
            isDetected = false;
            hoverTime = 0f;
            lineRenderer.positionCount = 0;
            tracedStars.Clear();
            Debug.Log("Telescope exited constellation area");
        }
    }

    void ShowFaintLines()
    {
        lineRenderer.positionCount = stars.Count;
        for (int i = 0; i < stars.Count; i++)
        {
            if (stars[i] != null)
            {
                lineRenderer.SetPosition(i, stars[i].transform.position);
                Debug.Log($"Setting LineRenderer position {i} to {stars[i].transform.position}");
            }
            else
            {
                Debug.LogError($"Star at index {i} is null, skipping!");
            }
        }
        lineRenderer.startColor = new Color(1, 1, 1, 0.3f); // Faint
        lineRenderer.endColor = new Color(1, 1, 1, 0.3f);
    }

    void TraceStar(Vector2 mousePos)
    {
        for (int i = 0; i < stars.Count; i++)
        {
            if (stars[i] == null) continue;

            float distance = Vector2.Distance(mousePos, stars[i].transform.position);
            float detectionRadius = 1f; // Increased radius for easier clicking
            if (distance < detectionRadius)
            {
                Debug.Log($"Star {i} ({stars[i].name}) clicked at position: {stars[i].transform.position}, Distance: {distance}");
                
                if (!tracedStars.Contains(i))
                {
                    tracedStars.Add(i);
                    UpdateTracedLines();
                    break;
                }
                else
                {
                    Debug.Log($"Star {i} already traced, ignoring click");
                }
            }
            else
            {
                Debug.Log($"Star {i} ({stars[i].name}) not clicked, Distance: {distance} (too far)");
            }
        }
    }

    void UpdateTracedLines()
    {
        Debug.Log($"Updating traced lines, traced stars count: {tracedStars.Count}");
        if (tracedStars.Count == 1)
        {
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, stars[tracedStars[0]].transform.position);
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }
        else if (tracedStars.Count > 1)
        {
            lineRenderer.positionCount = tracedStars.Count;
            for (int i = 0; i < tracedStars.Count; i++)
            {
                lineRenderer.SetPosition(i, stars[tracedStars[i]].transform.position);
            }
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;

            if (tracedStars.Count == stars.Count)
            {
                Debug.Log("Constellation fully traced!");
            }
        }
    }
}