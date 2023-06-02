using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public Panel currentPanel = null;
    private List<Panel> panelHistory = new List<Panel>();
    private Panel[] panels;

    public string context = "Panel_Main";

    private void Start()
    {
        SetupPanels();
        SetCurrentFromVoice("Panel_Main");
    }

    private void SetupPanels()
    {
        panels = GetComponentsInChildren<Panel>();

        foreach (Panel panel in panels)
        {
            panel.Setup(this);

            //disable child colliders in each panel
            SetColliders(panel, false);
        }
            

        currentPanel.Show();
        //enable child colliders in main panel
        SetColliders(currentPanel, true);
    }

    private void Update()
    {
        
    }

    public void GoToPrevious()
    {
        if (panelHistory.Count == 0)
        {
            return;
        }

        int lastIndex = panelHistory.Count - 1;
        SetCurrent(panelHistory[lastIndex]);
        panelHistory.RemoveAt(lastIndex);
    }

    public void SetCurrentWithHistory(Panel newPanel)
    {
        panelHistory.Add(currentPanel);
        SetCurrent(newPanel);
    }

    public void SetCurrent(Panel newPanel)
    {
        currentPanel.Hide();
        SetColliders(currentPanel, false);

        currentPanel = newPanel;
        SetColliders(currentPanel, true);

        context = currentPanel.name;
        Debug.Log("CONTEXT: " + context);
        currentPanel.Show();
    }

    public void SetCurrentFromVoice(string newPanel)
    {
        foreach (var panel in panels)
        {
            if (panel.name == newPanel)
            {
                currentPanel.Hide();
                SetColliders(currentPanel, false);

                currentPanel = panel;
                SetColliders(currentPanel, true);

                context = currentPanel.name;
                currentPanel.Show();
            }
        }
    }

    //this method allows you to toggle all of the colliders in any panel on or off depending on parameters
    public void SetColliders(Panel newPanel, bool which)
    {
        var collidersObj = newPanel.GetComponentsInChildren<Collider>();
        for (var index = 0; index < collidersObj.Length; index++)
        {
            var colliderItem = collidersObj[index];
            colliderItem.enabled = which;
        }
    }

    public enum Scene
    {
        FinalScene,
        StartingScene,
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
