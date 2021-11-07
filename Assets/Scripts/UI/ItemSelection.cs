using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    public GameObject itemAddListPrefab;
    public GameObject content;
    public void setItem(string name, int quantity)
    {
        GameObject itemGO = Instantiate(itemAddListPrefab);
        itemGO.transform.SetParent(content.transform);
        itemGO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        // Name
        itemGO.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
        // Quantity
        itemGO.transform.GetChild(1).gameObject.GetComponent<Text>().text = "" + quantity;
    }

    public void ResetContent()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
