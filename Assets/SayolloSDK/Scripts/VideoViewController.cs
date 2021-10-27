using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SayolloSDK
{
    public class VideoViewController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI itemTitle;

        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TextMeshProUGUI currency;

        [SerializeField]
        private TextMeshProUGUI currencySign;

        [SerializeField]
        private TMP_InputField email;

        [SerializeField]
        private TMP_InputField cardNumber;

        [SerializeField]
        private TMP_InputField cardExpirationDate;

        [SerializeField]
        private Button submitButton;

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private GameObject successPanel;

        [SerializeField]
        private GameObject failPanel;

        [SerializeField]
        private Button successToMenuButton;

        [SerializeField]
        private Button failToMenuButton;

        public Action OnClose;

        public void SetItem(ItemData itemData)
        {
            itemTitle.text = itemData.title;
            //itemImage.sprite = itemData.ItemImage;
            currency.text = itemData.currency;
            currencySign.text = itemData.currency_sign;
        }

        public void Submit()
        {
            SendSubmitted();
            ClosePanel();
        }

        private void Awake()
        {
            submitButton.onClick.AddListener(Submit);
            closeButton.onClick.AddListener(ClosePanel);
            successToMenuButton.onClick.AddListener(GoToMenu);
            failToMenuButton.onClick.AddListener(GoToMenu);
        }

        private void SendSubmitted()
        {

        }

        private void GoToMenu()
        {
            ClosePanel();
        }

        private void ClosePanel()
        {
            OnClose?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
