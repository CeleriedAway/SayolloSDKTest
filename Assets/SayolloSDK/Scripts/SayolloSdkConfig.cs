using UnityEngine;

namespace SayolloSDK
{
    [CreateAssetMenu(fileName = "New SayolloSdkConfig", menuName = "SayolloSDK/Create SayolloSdkConfig", order = 1)]
    public class SayolloSdkConfig : ScriptableObject
    {
        public string VideoUri;
        public string PurchaseItemUrl;
        public string UserInfoUrl;
        public GameObject VideoAdCanvasPrefab;
        public PurchaseViewController PurchaseAdCanvasPrefab;
    }
}
