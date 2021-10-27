using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace SayolloSDK
{
    public class PurchaseAd
    {
        public SayolloSDK.AdStatus Status { get; private set; }

        public UnityEvent onDismissed = new UnityEvent();
        public UnityEvent onFinished = new UnityEvent();

        private WebRequestController webRequestController;
        private ItemData itemData;
        private string purchaseUri;
        private string userInfoUri;
        private bool showOnLoad;
        private PurchaseViewController canvasPrefab;
        private PurchaseViewController canvas;

        public PurchaseAd(WebRequestController requestController, PurchaseViewController purchaseAdCanvasPrefab, string purchUri, string userUri)
        {
            webRequestController = requestController;
            canvasPrefab = purchaseAdCanvasPrefab;
            purchaseUri = purchUri;
            userInfoUri = userUri;
        }
        public void Show()
        {
            if (Status == SayolloSDK.AdStatus.Ready)
            {
                ShowSavedPurchase(itemData);
            }
            else
            {
                showOnLoad = true;
                Prepare();
            }
        }
        public void Prepare()
        {
            Status = SayolloSDK.AdStatus.Loading;
            SayolloSDK.Instance.StartCoroutine
            (
                webRequestController.EmptyPost(purchaseUri,
                onSuccess: HandlePurchaseLoaded,
                onError: HandlePurchaseLoadError)
            );
        }
        private void HandlePurchaseLoaded(string result)
        {
            result = result.Replace("'", "\"");
            ItemData itemData = JsonUtility.FromJson<ItemData>(result);
            Status = SayolloSDK.AdStatus.Ready;
            if (showOnLoad)
            {
                showOnLoad = false;
                ShowSavedPurchase(itemData);
            }
        }

        private void ShowSavedPurchase(ItemData itemData)
        {
            canvas = GameObject.Instantiate(canvasPrefab);
            canvas.SetItem(itemData);
            SayolloSDK.Instance.StartCoroutine(
                webRequestController.DownloadImage(itemData.item_image, HandleImageDownloaded)
            );
            canvas.onPurchase.AddListener(OnPurchase);
            canvas.onClose.AddListener(OnClose);
        }

        private void OnClose()
        {
            Status = SayolloSDK.AdStatus.Cancelled;
            GameObject.Destroy(canvas);
            onDismissed.Invoke();
        }

        private void OnPurchase(UserData data)
        {
            SayolloSDK.Instance.StartCoroutine(
                webRequestController.PostUserData(userInfoUri,
                data,
                HandleSuccessBuy,
                HandleErrorBuy)
            );
        }
        private void HandleSuccessBuy(string result)
        {
            Status = SayolloSDK.AdStatus.Finished;
            canvas.ShowSuccess();
            onFinished.Invoke();
        }
        private void HandleErrorBuy(string error)
        {
            canvas.ShowFail();
        }
        private void HandleImageDownloaded(Texture2D itemTexture)
        {
            if (canvas != null)
            {
                canvas.SetImage(itemTexture);
            }
        }
        private void HandlePurchaseLoadError(string error)
        {
            Status = SayolloSDK.AdStatus.Cancelled;
        }
        public void Cancel(bool invokeEvent = true)
        {
            GameObject.Destroy(canvas.gameObject);
            SayolloSDK.Instance.StopAllCoroutines();
            Status = SayolloSDK.AdStatus.Cancelled;
            if (invokeEvent)
                onDismissed.Invoke();
            onDismissed.RemoveAllListeners();
            onFinished.RemoveAllListeners();
        }
    }
}
