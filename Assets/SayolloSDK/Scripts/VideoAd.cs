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
    public class VideoAd
    {
        private WebRequestController webRequestController;
        private string uri;
        private bool showOnLoad;
        private string savedVideoPath;
        private GameObject canvasPrefab;
        private GameObject canvas;

        public UnityEvent onDismissed = new UnityEvent();
        public UnityEvent onFinished = new UnityEvent();

        public SayolloSDK.AdStatus Status { get; private set; }
        public VideoAd(WebRequestController requestController, GameObject videoAdCanvasPrefab, string videoUri)
        {
            webRequestController = requestController;
            canvasPrefab = videoAdCanvasPrefab;
            uri = videoUri;
        }

        public void Show()
        {
            if (Status == SayolloSDK.AdStatus.Ready)
            {
                ShowSavedVideo(savedVideoPath);
            }
            else
            {
                showOnLoad = true;
                Load();
            }
        }

        public void Load()
        {
            Status = SayolloSDK.AdStatus.Loading;
            SayolloSDK.Instance.StartCoroutine
            (
                webRequestController.GetRequest(
                    uri,
                    HandleXmlLoaded,
                    HandleXmlLoadError)
            );
        }

        private void HandleXmlLoaded(string result)
        {
            XDocument document = XDocument.Parse(result);
            var data = document.ToDictionary();
            if (!data.ContainsKey("MediaFile"))
            {
                return;
            }
            DownloadVideo(data["MediaFile"]);
        }

        private void HandleXmlLoadError(string error)
        {
            Status = SayolloSDK.AdStatus.Cancelled;
        }

        private void DownloadVideo(string videoUri)
        {
            SayolloSDK.Instance.StartCoroutine
            (
                webRequestController.GetVideo(videoUri,
                    onSuccess: HandleVideoDownloaded,
                    onError: HandleVideoDownloadError)
            );
        }
        private void HandleVideoDownloaded(string result)
        {
            Status = SayolloSDK.AdStatus.Ready;
            if (showOnLoad)
            {
                showOnLoad = false;
                ShowSavedVideo(result);
            }
        }

        private void HandleVideoDownloadError(string error)
        {
            Status = SayolloSDK.AdStatus.Cancelled;
        }

        private void ShowSavedVideo(string path)
        {
            Status = SayolloSDK.AdStatus.InProgress;
            canvas = GameObject.Instantiate(canvasPrefab);
            VideoPlayer videoPlayer = canvas.GetComponentInChildren<VideoPlayer>();
            videoPlayer.url = path;
            videoPlayer.EnableAudioTrack(0, true);
            videoPlayer.Prepare();
            videoPlayer.Play();
            videoPlayer.loopPointReached += HandleVideoEnded;
        }
        private void HandleVideoEnded(VideoPlayer player)
        {
            GameObject.Destroy(canvas);
            Status = SayolloSDK.AdStatus.Finished;
            onFinished?.Invoke();
        }

        public void Cancel(bool invokeEvent = true)
        {
            GameObject.Destroy(canvas);
            SayolloSDK.Instance.StopAllCoroutines();
            Status = SayolloSDK.AdStatus.Cancelled;
            if (invokeEvent)
                onDismissed?.Invoke();
            onDismissed?.RemoveAllListeners();
            onFinished?.RemoveAllListeners();
        }
    }
}
