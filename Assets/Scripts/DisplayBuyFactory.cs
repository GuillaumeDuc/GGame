using UnityEngine;
using UnityEngine.UI;

public class DisplayBuyFactory : DisplayUI
{
    public GameObject scrollViewContent;
    public GameObject unitContentPrefab;
    public GameObject unitFieldPrefab;

    private bool added = false;

    public override void Display(Planet p)
    {
        string displayS = "Factory\n";
        this.gameObject.GetComponent<Text>().text = displayS;
        // Only add them one time in UI
        if (!added)
        {
            AddAllFactories(p);
            added = !added;
        }
    }

    void AddAllFactories(Planet p)
    {
        // Up factories
        p.factories.ForEach(factory =>
        {
            AddFactoryInView(factory);
        });
    }

    private void AddFactoryInView(Factory factory)
    {
        if (factory != null)
        {
            GameObject go = Instantiate(unitContentPrefab);
            // Unit image
            Image img = go.GetComponentInChildren<Image>();
            img.sprite = img.sprite ?? factory.sprite;
            // Unit description
            DisplayFactoryDescription(go.transform.GetChild(0).GetChild(0).GetChild(0).gameObject, factory);
            // Unit buy button
            Button button = go.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => OnClickUp(factory));
            button.GetComponentInChildren<Text>().text = "Next Level";
            go.transform.SetParent(scrollViewContent.transform, false);
        }
    }

    private void OnClickUp(Factory factory)
    {
        Store.player.selectedPlanet.LevelUpFactory(factory);
        ResetContent();
        Store.UpdateUI();
    }

    private void DisplayFactoryDescription(GameObject content, Factory factory)
    {
        SetLabel(content, "Name", factory.name);
        SetLabel(content, "Level", "" + factory.currentLv);
        SetLabel(content, "Resources Produced", factory.GetResources().ToString());
        SetLabel(content, "Resources cost", factory.GetResourcesNeededLvUp().ToString());
    }

    private void SetLabel(GameObject content, string label, string text)
    {
        GameObject go = Instantiate(unitFieldPrefab);
        go.transform.SetParent(content.transform, false);
        // Label
        go.transform.GetChild(0).GetComponent<Text>().text = label;
        // Text
        go.transform.GetChild(1).GetComponent<Text>().text = text;
    }

    private void ResetContent()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        added = !added;
    }
}
