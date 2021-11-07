using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetOverview : MonoBehaviour
{
    public Text nameText;
    public Image planetImage;
    public Text distanceText;
    public Button attackButton, transportButton, stayButton;

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetSprite(Sprite sprite)
    {
        planetImage.sprite = sprite;
    }

    public void SetDistance(float distance)
    {
        distanceText.text = "Distance\n" + distance;
    }

    public void SetAttackButton(System.Action attack)
    {
        attackButton.onClick.AddListener(() => { attack(); });
    }

    public void SetTransportButton(System.Action transport)
    {
        attackButton.onClick.AddListener(() => { transport(); });
    }

    public void SetStayButton(System.Action stay)
    {
        attackButton.onClick.AddListener(() => { stay(); });
    }
}
