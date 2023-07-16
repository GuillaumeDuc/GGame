using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DisplayTab : MonoBehaviour
{
    public List<GameObject> tabs;
    public Dictionary<Button, GameObject> tabDict = new Dictionary<Button, GameObject>();
    void Start()
    {
        // Get buttons
        List<Button> buttons = this.gameObject.GetComponentsInChildren<Button>().ToList();
        // Tab listeners
        int i = 0;
        buttons.ForEach(button =>
        {
            AddTab(button, tabs[i]);
            i += 1;
        });
        // Initialize tab
        HandleChangeTab(buttons[0]);
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

    void AddTab(Button button, GameObject tab)
    {
        button.onClick.AddListener(() => { HandleChangeTab(button); });
        tabDict.Add(button, tab);
    }
}
