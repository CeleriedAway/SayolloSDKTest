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
    public static class WebTools
    {
        public static async Task<byte[]> LoadData(string uri)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                await www.SendWebRequest();
                return www.downloadHandler.data;
            }
        }
        
        public static async Task<string> LoadString(string uri)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                await www.SendWebRequest();
                return www.downloadHandler.text;
            }
        }
        
        public static async Task<Texture2D> LoadImage(string uri)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(uri))
            {
                await www.SendWebRequest();
                return ((DownloadHandlerTexture)www.downloadHandler).texture;
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
    }
}
