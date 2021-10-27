using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SayolloSDK
{
    public class PurchaseViewController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI itemTitle;

        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TextMeshProUGUI price;

        [SerializeField]
        private TextMeshProUGUI currency;

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

        public UnityEvent<UserData> onPurchase = new UnityEvent<UserData>();
        public UnityEvent onClose = new UnityEvent();

        public void SetItem(ItemData itemData)
        {
            itemTitle.text = itemData.title;
            price.text = $"{itemData.price}{itemData.currency_sign}";
            currency.text = itemData.currency;
        }

        public void Submit()
        {
            SendSubmitted();
        }

        public void ShowFail()
        {
            failPanel.SetActive(true);
        }

        public void ShowSuccess()
        {
            successPanel.SetActive(true);
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
            UserData sentData = new UserData 
            { 
                Email = email.text, 
                CreditCardNumber = cardNumber.text, 
                ExpirationDate = cardExpirationDate.text 
            };
            onPurchase?.Invoke(sentData);
        }

        private void GoToMenu()
        {
            ClosePanel();
        }

        private void ClosePanel()
        {
            onClose?.Invoke();
            Destroy(this.gameObject);
        }
        public void SetImage(Texture2D texture)
        {
            itemImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));
        }
    }
}
