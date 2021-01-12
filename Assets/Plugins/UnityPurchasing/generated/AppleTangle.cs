#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("XO5o11zub8/Mb25tbm5tblxhamVMDQIITA8JHhgFCgUPDRgFAwJMHNlWwZhjYmz+Z91NekIYuVBhtw56GAUKBQ8NGAlMDhVMDQIVTBwNHhgeDQ8YBQ8JTB8YDRgJAQkCGB9CXFr1IEEU24Hg97CfG/eeGr4bXCOt7m1samVG6iTqmw8IaW1c7Z5cRmqsD18bm1ZrQDqHtmNNYrbWH3Uj2fnyFmDIK+c3uHpbX6eoYyGieAW9elx4am85aG9/YS0cHAAJTD4DAxhz6e/pd/VRK1uexfcs4kC43fx+tHP9t3IrPIdpgTIV6EGHWs47IDmADgAJTB8YDQIIDR4ITBgJHgEfTA1rgBFV7+c/TL9UqN3T9iNmB5NHkAAJTCUCD0JdSlxIam85aGd/cS0cJbQa8194Cc0b+KVBbm9tbG3P7m1ZXl1YXF9aNnthX1lcXlxVXl1YXEIsypsrIRNkMlxzam85cU9odFx6pXUemTFiuRMz955Jb9Y54yExYZ0cAAlMPgMDGEwvLVxye2FcWlxYXkwDCkwYBAlMGAQJAkwNHBwABQ8NC+Nk2Eybp8BATAMc2lNtXODbL6NRSgtM5l8Gm2Huo7KHz0OVPwY3CNt30f8uSH5Gq2Nx2iHwMg+kJ+x73Vw0gDZoXuAE3+NxsgkfkwsyCdBj8VGfRyVEdqSSotnVYrUycLqnURgEAx4FGBVdelx4am85aG9/YS0cSI6Hvdscs2MpjUumnQEUgYvZe3tcfWpvOWhmf2YtHBwACUwlAg9CXSkScyAHPPot5agYDmd87y3rX+btaWxv7m1jbFzubWZu7m1tbIj9xWVG6iTqm2FtbWlpbFwOXWdcZWpvORMtxPSVvaYK8EgHfbzP14h3Rq9z0pgf94K+CGOnFSNYtM5SlRSTB6RkR2ptaWlrbm16cgQYGBwfVkNDG+d15bKVJwCZa8dOXG6EdFKUPGW/PgkABQ0CDwlMAwJMGAQFH0wPCR5MLy1c7m1OXGFqZUbqJOqbYW1tbQUKBQ8NGAUDAkwtGRgEAx4FGBVdalxjam85cX9tbZNoaVxvbW2TXHFDXO2vamRHam1paWtublzt2nbt32hqf245P11/XH1qbzloZn9mLRwcYWplRuok6pthbW1paWxv7m1tbDDjH+0Mqnc3ZUP+3pQoJJwMVPJ5mbVaE63rObXL9dVeLpe0uR3yEs0+AghMDwMCCAUYBQMCH0wDCkwZHwlfWjZcDl1nXGVqbzloan9uOT9df+x4R7wFK/gaZZKYB+FCLMqbKyETxLASTlmmSbm1Y7oHuM5IT32bzcAWXO5tGlxiam85cWNtbZNoaG9ubRsbQg0cHAAJQg8DAUMNHBwACQ8NZDJc7m19am85cUxo7m1kXO5taFwIWU95J3k1cd/4m5rw8qM81q00PBwACUwvCR4YBQoFDw0YBQMCTC0ZNctpZRB7LDp9chi/2+dPVyvPuQPHzx3+Kz85rcNDLd+Ul48coYrPIEpcSGpvOWhnf3EtHBwACUwvCR4YQEwPCR4YBQoFDw0YCUwcAwAFDxVqbzlxYmh6aHhHvAUr+BplkpgH4RVMDR8fGQEJH0wNDw8JHBgNAg8JPMbmubaIkLxla1vcGRlN");
        private static int[] order = new int[] { 0,45,23,39,46,48,34,57,18,12,49,15,42,51,39,27,57,31,19,34,41,21,56,59,26,56,48,53,42,56,39,45,45,50,35,57,38,51,53,56,51,59,47,55,58,55,57,55,54,55,53,53,59,57,59,59,59,58,59,59,60 };
        private static int key = 108;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
