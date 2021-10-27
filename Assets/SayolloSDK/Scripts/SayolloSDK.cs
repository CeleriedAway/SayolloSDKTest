using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SayolloSDK
{
    public class SayolloSDK : MonoBehaviour
    {
        public enum AdStatus
        {
            Loading,
            Ready,
            InProgress,
            Cancelled,
            Finished
        }

        private static SayolloSDK _instance;
        public static SayolloSDK Instance 
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = (SayolloSDK)FindObjectOfType(typeof(SayolloSDK));
                    if (_instance == null)
                    {
                        _instance = (new GameObject("SayolloSDK", typeof(SayolloSDK))).GetComponent<SayolloSDK>();
                    }

                    _instance.Init();
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        [SerializeField] private GameObject videoAdCanvas;
        [SerializeField] private GameObject purchaseAdCanvas;
        private VideoAd videoAd;
        private PurchaseAd purchAd;
        private WebRequestController webRequestController;
        private SayolloSdkConfig sayolloSdkConfig;

        private void Init()
        {
            sayolloSdkConfig = Resources.Load<SayolloSdkConfig>("SayolloSdkConfig");
            webRequestController = new WebRequestController();
        }

        public VideoAd GetVideoAd()
        {
            if (videoAd != null)
                videoAd.Cancel(false);
            if (purchAd != null)
                purchAd.Cancel(false);
            videoAd = new VideoAd(webRequestController, sayolloSdkConfig.VideoAdCanvasPrefab, sayolloSdkConfig.VideoUri);
            return videoAd;
        }

        public PurchaseAd GetPurchaseAd()
        {
            if (purchAd != null)
                purchAd.Cancel(false);
            if (videoAd != null)
                videoAd.Cancel(false);
            purchAd = new PurchaseAd(webRequestController, sayolloSdkConfig.PurchaseAdCanvasPrefab, sayolloSdkConfig.PurchaseItemUrl, sayolloSdkConfig.UserInfoUrl);
            return purchAd;
        }
    }
}
