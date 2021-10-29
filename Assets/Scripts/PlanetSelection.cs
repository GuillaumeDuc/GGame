using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlanetSelection : MonoBehaviour
{
    public GameObject planetButtonPrefab, scrollViewContent;

    private List<Planet> planets = new List<Planet>();

    void Start()
    {
        planets = Store.player != null ? Store.player.planets : null;
    }

    void Update()
    {
        // Update when list change
        if (Store.player != null && (planets == null || planets.Count != Store.player.planets.Count))
        {
            planets = Store.player.planets;
            ResetContent();
            UpdateSelection();
        }
    }

    void UpdateSelection()
    {
        planets.ForEach(planet =>
        {
            AddInUI(planet);
        });
    }

    void OnClick(Planet p)
    {
        Store.player.selectedPlanet = p;
        Store.camera.gameObject.transform.position = new Vector3(p.planetGO.gameObject.transform.position.x, 0, 10);
        Store.UpdateUI();
    }

    void AddInUI(Planet p)
    {
        GameObject go = Instantiate(planetButtonPrefab);
        Button button = go.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => OnClick(p));
        button.GetComponentInChildren<Text>().text = p.name;
        go.transform.SetParent(scrollViewContent.transform, false);
    }

    private void ResetContent()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
