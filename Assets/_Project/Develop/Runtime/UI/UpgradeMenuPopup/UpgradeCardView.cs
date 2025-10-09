using Assets._Project.Develop.Runtime.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup
{
    public class UpgradeCardView : MonoBehaviour, IView
    {
        [SerializeField] private GameObject _outline;
        [SerializeField] private Image _upgradeIcon;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _description;

        public void Select() => _outline.SetActive(true);
        public void Deselect() => _outline.SetActive(false);

        public void SetIcon(Sprite icon) => _upgradeIcon.sprite = icon;
        public void SetLabel(string text) => _label.text = text;
        public void SetCost(string text) => _cost.text = text;
        public void SetDescription(string text) => _description.text = text;
    }
}