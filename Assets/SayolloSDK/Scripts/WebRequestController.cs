using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SayolloSDK
{
    public class WebRequestController
    {
        public IEnumerator GetVideo(string uri, string fileName = "", Action<string> onSuccess = null, Action<string> onError = null)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(www.error);
                }
                else
                {
                    if (fileName == string.Empty)
                        fileName = uri.Split('/').Last();
                    string filePath = $"{Application.dataPath}/SayolloSDK/Resources/{fileName}";
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    File.WriteAllBytes(filePath, www.downloadHandler.data);
                    onSuccess?.Invoke(filePath);
                }
            }
        }

        public IEnumerator GetRequest(string uri, Action<string> onSuccess = null, Action<string> onError = null)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(www.error);
                }
                else
                {
                    onSuccess?.Invoke(www.downloadHandler.text);
                }
            }
        }

        public IEnumerator EmptyPost(string uri, Action<string> onSuccess = null, Action<string> onError = null)
        {
            using (UnityWebRequest www = new UnityWebRequest(uri, "POST"))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("{}");
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(www.error);
                }
                else
                {
                    onSuccess?.Invoke(www.downloadHandler.text);
                }
            }
        }

        public IEnumerator PostUserData(string url, UserData user, Action<string> onSuccess = null, Action<string> onError = null)
        {
            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JsonUtility.ToJson(user));
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(www.error);
                }
                else
                {
                    onSuccess?.Invoke(www.downloadHandler.text);
                }
            }
        }

        public IEnumerator DownloadImage(string mediaUrl, Action<Texture2D> onSuccess = null, Action<string> onError = null)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(mediaUrl))
            {
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(www.error);
                }
                else
                {
                    onSuccess?.Invoke(((DownloadHandlerTexture)www.downloadHandler).texture);
                }
            }
        }
    }
}
