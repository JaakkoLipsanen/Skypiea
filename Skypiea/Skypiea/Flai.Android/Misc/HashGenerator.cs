using System.Text;

namespace Flai.Misc
{
    public static class HashGenerator
    {
        // okay.. i didnt think this completely through, why dont I just use string.GetHashCode..? oh well..
        // this is a bit slower than it (~20-30%) but also produces less collisions (100%-300%). however the string.GetHashCode doesn't produce *that much* either
        public static int GenerateHash(string str)
        {
            return HashGenerator.GenerateHash(str, 0);
        }

        // http://stackoverflow.com/a/2351171/925777
        public static int GenerateHash(string str, int seed)
        {
            const int MagicNumber = 97; // 37 was suggested, but my limited test showed 97 to be better (and it makes sense kind of, since characters are usually in ~50-130 range. 130 - 50 > 97.. dunno :P)
            int h = str.Length ^ seed; // no idea if this seed makes any sense :D but hey, it's a seed!!
            for (int i = str.Length - 1; i >= 0; i--)
            {
                h = MagicNumber * h + str[i];
            }

            return h; // or, h % ARRAY_SIZE;
        }

        public static int GenerateHash(StringBuilder str)
        {
            return HashGenerator.GenerateHash(str, 0);
        }

        public static int GenerateHash(StringBuilder str, int seed)
        {
            const int MagicNumber = 97; // 37 was suggested, but my limited test showed 97 to be better (and it makes sense kind of, since characters are usually in ~50-130 range. 130 - 50 > 97.. dunno :P)
            int h = str.Length ^ seed; // no idea if this seed makes any sense :D but hey, it's a seed!!
            for (int i = str.Length - 1; i >= 0; i--)
            {
                h = MagicNumber * h + str[i];
            }

            return h; // or, h % ARRAY_SIZE;
        }
    }
}
