using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dev.Scripts.UI
{
    public class LevelUIView : MonoBehaviour
    {
        [SerializeField] private LevelUIButton _button;

        [SerializeField] private TMP_Text _levelIdText;
        [SerializeField] private TMP_Text _levelDescription;
        [SerializeField] private Image _selectionImage;

        public int Id { get; private set; }
        
        public LevelUIButton Button => _button;

        private void Awake()
        {
            _button.Init(this);
        }

        public void Setup(int levelId, string description)
        {
            Id = levelId;
            _levelIdText.text = $"{levelId}";
            _levelDescription.text = $"{description}";
        }
        

        public void SetSelection(bool isOn)
        {
            _selectionImage.enabled = isOn;
        }
    }
}