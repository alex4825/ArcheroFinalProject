using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.CommonViews
{
    public class IconTextView : SimpleView
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _icon;

        public void SetText(string text) => _text.text = text;
        public void SetIcon(Sprite icon) => _icon.sprite = icon;
    }
}
