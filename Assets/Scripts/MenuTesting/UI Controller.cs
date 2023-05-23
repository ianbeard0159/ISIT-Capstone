using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject[] uiElements;
    private GameObject activeUI;

    private void Start()
    {
        Debug.Log("UIController Start()");
        // Hide all UI elements initially
        HideAllUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        int triggerIndex = GetTriggerIndex(other.tag);
        if (triggerIndex >= 0)
        {
            // Hide the currently active UI element (if any)
            HideActiveUI();

            // Show the new UI element corresponding to the trigger index
            ShowUI(triggerIndex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int triggerIndex = GetTriggerIndex(other.tag);
        if (triggerIndex >= 0)
        {
            // Hide the UI element corresponding to the trigger index
            HideUI(triggerIndex);
        }
    }

    private void HideAllUI()
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
    }

    private void HideActiveUI()
    {
        if (activeUI != null)
        {
            activeUI.SetActive(false);
            activeUI = null;
        }
    }


    private int GetTriggerIndex(string tag)
    {
        switch (tag)
        {
            case "UITrigger1":
                return 0;
            case "UITrigger2":
                return 1;
            case "UITrigger3":
                return 2;
            case "UITrigger4":
                return 3;
            case "UITrigger5":
                return 4;
            case "UITrigger6":
                return 5;
            case "UITrigger7":
                return 6;
            case "UITrigger8":
                return 7;
            default:
                return -1;
        }
    }

    private void ShowUI(int index)
    {
        if (index >= 0 && index < uiElements.Length)
        {
            uiElements[index].SetActive(true);
            activeUI = uiElements[index];
        }
    }

    private void HideUI(int index)
    {
        if (index >= 0 && index < uiElements.Length)
        {
            uiElements[index].SetActive(false);
        }
    }
}
