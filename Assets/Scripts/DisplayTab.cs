using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DisplayTab : MonoBehaviour
{
    public GameObject planetTab, buyTab;
    public Button planetButton, buyButton;

    public Dictionary<Button, GameObject> tabDict;
    void Start()
    {
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
