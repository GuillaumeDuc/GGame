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
    public Button planetButton, buyButton;

    private List<DisplayUI> displayedUI = new List<DisplayUI>();
    private Dictionary<Button, GameObject> tabDict;

    void Start()
    {
        // Store update
        Store.UpdateUI = UpdateUI;
        // Get display planet list
        displayedUI = planetTab.GetComponentsInChildren<DisplayUI>().ToList();
        // Get display buy list
        displayedUI.AddRange(buyTab.GetComponentsInChildren<DisplayUI>().ToList());
        // Tab listeners
        planetButton.onClick.AddListener(() => { HandleChangeTab(planetButton); });
        buyButton.onClick.AddListener(() => { HandleChangeTab(buyButton); });
        tabDict = new Dictionary<Button, GameObject>() {
            { planetButton, planetTab },
            { buyButton, buyTab }
        };
        // Initialize tab
        HandleChangeTab(planetButton);
    }
    public void UpdateUI()
    {
        // Always update resources
        resources.Display(Store.selectedPlanet);
        // Update current displayed list
        displayedUI.ForEach(disp => disp.Display(Store.selectedPlanet));
    }

    private void HandleChangeTab(Button clicked)
    {
        foreach (var item in tabDict)
        {
            if (item.Key == clicked)
            {
                ColorBlock colorBlock = ColorBlock.defaultColorBlock;
                colorBlock.normalColor = Color.gray;
                colorBlock.selectedColor = Color.gray;
                colorBlock.highlightedColor = new Color(.4f, .4f, .4f);
                colorBlock.pressedColor = new Color(.35f, .35f, .35f);
                item.Key.colors = colorBlock;
                item.Value.SetActive(true);
            }
            else
            {
                item.Key.colors = ColorBlock.defaultColorBlock;
                item.Value.SetActive(false);
            }
        }
    }
}
