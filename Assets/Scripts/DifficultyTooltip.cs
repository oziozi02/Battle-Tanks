using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DifficultyTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea] public string tooltipText;
    public GameObject tooltipPanel; // shared panel, assigned in Inspector
    public TextMeshProUGUI tooltipTextField;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipPanel.SetActive(true);
        tooltipTextField.text = tooltipText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.SetActive(false);
    }
}
