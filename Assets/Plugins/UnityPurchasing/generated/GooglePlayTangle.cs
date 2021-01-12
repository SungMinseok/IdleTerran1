#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("iXQQ8WosAbe4ma19tz2/UnpHfU9Z58b/gQLix127F8W2waaPy4yPhW3IOkUsxtTlm/P8djTDH8x4R/mCQh9FIQfE4c4Eu447sXVwob9s2xOPU3+O7JyEMfokv8JoNTNmfprP963x1LbPx/oy80D/UCUDTN94toTBWEP3YDR7a3p6ouXanobLCwD9/9Vnt088ki8YdjUZXol1E85LMiBS28h6+drI9f7x0n6wfg/1+fn5/fj79Yuo1c+oFJUYtxvF64MI5V+Grod6+ff4yHr58vp6+fn4c3xdxiFyAqxLXqKUqL9Uxy/0QY8xhwR/fPFkpD9zzh6aEZFwZPKZFDSbUKpn/kuDeYwiIueaGg/UgiafNTVvqxW4XvQyetza86IAifr7+fj5");
        private static int[] order = new int[] { 11,8,6,12,12,12,6,12,11,12,11,12,12,13,14 };
        private static int key = 248;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
