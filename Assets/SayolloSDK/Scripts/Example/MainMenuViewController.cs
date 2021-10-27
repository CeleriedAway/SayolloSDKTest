using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SayolloSDK
{
    public class MainMenuViewController : MonoBehaviour
    {
        [SerializeField]
        private Button showVideoButton;

        [SerializeField]
        private Button showItemButton;

        private void Awake()
        {
            showVideoButton.onClick.AddListener(ShowVideo);
            showItemButton.onClick.AddListener(ShowItem);
        }

        private void ShowVideo()
        {
            VideoAd videoAd = SayolloSDK.Instance.GetVideoAd();
            videoAd.Show();
        }

        private void ShowItem()
        {
            PurchaseAd purchaseAd = SayolloSDK.Instance.GetPurchaseAd();
            purchaseAd.Show();
        }
    }
}
