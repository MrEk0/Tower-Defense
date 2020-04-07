using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;

    private void OnEnable()
    {
        UIManager.Instance.onUpdatePanelShowed += SetUpdatePrice;
    }

    private void OnDisable()
    {
        UIManager.Instance.onUpdatePanelShowed -= SetUpdatePrice;
    }
    private void SetUpdatePrice(float price)
    {
        if (price == 0)
        {
            buttonText.text = "Max Level";
        }
        else
        {
            buttonText.text = "Update " + price.ToString();
        }
    }
}
