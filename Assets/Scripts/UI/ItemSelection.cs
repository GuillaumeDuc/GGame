using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    public GameObject itemAddListPrefab;
    public GameObject content;
    public void setItem(string name, int quantity, UnityEngine.Events.UnityAction<string, string> OnChangeSelection)
    {
        GameObject itemGO = Instantiate(itemAddListPrefab);
        itemGO.transform.SetParent(content.transform);
        itemGO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        // Name
        itemGO.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
        // Quantity
        itemGO.transform.GetChild(1).gameObject.GetComponent<Text>().text = "" + quantity;
        // Selection
        InputField inputField = itemGO.transform.GetChild(2).gameObject.GetComponent<InputField>();
        inputField.onValueChanged.AddListener((amount) => InputChanged(
            name,
            quantity,
            amount,
            inputField,
            OnChangeSelection
            ));
    }

    public void InputChanged(string name, int quantity, string amount, InputField inputField, UnityEngine.Events.UnityAction<string, string> OnChangeSelection)
    {
        if (amount.Length == 0)
        {
            inputField.text = "0";
        }
        else if ((System.Int32.Parse(amount)) > quantity)
        {
            inputField.text = "" + quantity;
        }
        OnChangeSelection(name, inputField.text);
    }

    public void ResetContent()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
