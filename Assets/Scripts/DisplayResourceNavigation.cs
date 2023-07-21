using UnityEngine;
using UnityEngine.UI;


public class DisplayResourceNavigation : MonoBehaviour
{
    public GameObject resourceFieldPrefab;
    public GameObject content;
    public void setItem(string name, Resource resource)
    {
        GameObject itemGO = GetItemGO(name);
        if (itemGO == null)
        {
            itemGO = Instantiate(resourceFieldPrefab);
            itemGO.transform.SetParent(content.transform);
            itemGO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            // Name
            Text textName = itemGO.transform.GetChild(0).gameObject.GetComponent<Text>();
            textName.text = name;
            textName.fontSize = 14;
            RectTransform rectTransformName = itemGO.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            rectTransformName.pivot = new Vector2();
            rectTransformName.position = new Vector3();
            rectTransformName.sizeDelta = new Vector2(100, rectTransformName.sizeDelta.y);
            // Quantity
            Text textQuantity = itemGO.transform.GetChild(1).gameObject.GetComponent<Text>();
            textQuantity.text = "" + resource.amount;
            textQuantity.fontSize = 14;
            RectTransform rectTransformQuantity = itemGO.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
            rectTransformQuantity.pivot = new Vector2();
        }
        else
        {
            Text textName = itemGO.transform.GetChild(0).gameObject.GetComponent<Text>();
            textName.text = name;

            Text textQuantity = itemGO.transform.GetChild(1).gameObject.GetComponent<Text>();
            textQuantity.text = "" + resource.amount;
        }
    }

    GameObject GetItemGO(string name)
    {
        GameObject itemGO = null;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            if (content.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text == name)
            {
                itemGO = content.transform.GetChild(i).gameObject;
            }
        }
        return itemGO;
    }

    public void ResetContent()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
