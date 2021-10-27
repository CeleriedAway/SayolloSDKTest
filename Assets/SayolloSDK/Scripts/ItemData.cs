using System;

namespace SayolloSDK
{
    [Serializable]
    public class ItemData
    {
        public string title;
        public string item_id;
        public string item_name;
        public string item_image;
        public float price;
        public string currency;
        public string currency_sign;
        public string status;
        public int error_code;
    }
}
