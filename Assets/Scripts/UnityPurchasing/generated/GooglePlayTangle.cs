// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("YfDFBsUXLnUnT6sOTMFXUL7CMvEJengM+9OCXMPYUGtKf688AmVZgzyqwFEKDngIyB6JsuhJtK8E6CCqebN7qCeglTMUdOI99ARTWGKelvoUorKD7OJsagcbZ2QnpODcqjzPQZwfER4unB8UHJwfHx61phvDGUkadn3zRHhREWWYqye3oIT7OwD8PcqxZioyvkNBOwOrOQODf/sU98ILege2gLE4MVuTVSOfn2YPsmYkeMFvfxQdIaV5HkIu+0MUeHehI6u5PyjjLrnBfXchO2ZTVoeXoTysL5yfYzhfg2ce+MABfd0O1rIY5v4wMZ1PLpwfPC4TGBc0mFaY6RMfHx8bHh2a91gs/fPB6NRDX/PwambQhiO9LxXwBRwoCBhH0xwdHx4f");
        private static int[] order = new int[] { 10,2,5,11,6,5,7,7,9,9,11,13,13,13,14 };
        private static int key = 30;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
