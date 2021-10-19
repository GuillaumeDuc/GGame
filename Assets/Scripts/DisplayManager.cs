using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour
{
    public DisplayUI resources;
    public GameObject planetTab;
    public GameObject buyTab;

    private List<DisplayUI> displayedUI = new List<DisplayUI>();

    void Start()
    {
        // Store update
        Store.UpdateUI = UpdateUI;
        // Get display planet list
        displayedUI = planetTab.GetComponentsInChildren<DisplayUI>().ToList();
        // Get display buy list
        displayedUI.AddRange(buyTab.GetComponentsInChildren<DisplayUI>().ToList());
    }
    public void UpdateUI()
    {
        // Always update resources
        resources.Display(Store.player.selectedPlanet);
        // Update current displayed list
        displayedUI.ForEach(disp => disp.Display(Store.player.selectedPlanet));
    }
}
