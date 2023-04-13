using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarUI : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Update()
    {
        float f = (float)GameManager.Instance.Energy / (float)GameManager.MaxEnergy;
        image.fillAmount = f;
    }
}
