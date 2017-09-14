using System;
using System.Collections.Generic;

namespace typeahead
{
    class CacheBuilder
    {
        public static Dictionary<string, List<Adress>> BuildCache(Adress[] addresses)
        {
            var result = new Dictionary<string, List<Adress>>();

            foreach (var a in FindUniqueueAddresses(addresses, x => x.adresseringsvejnavn))
            {
                SplitAndAddSubsAccents(result, a.Value, a.Key);
            }

            foreach (var a in addresses)
            {
                SplitAndAddSubsAccents(result, a, a.adresseringsvejnavn);
                SplitAndAddSubsAccents(result, a, a.postnr);
                SplitAndAddSubsAccents(result, a, a.supplerendebynavn);
            }

            return result;
        }

        private static Dictionary<string, Adress> FindUniqueueAddresses(Adress[] addresses, Func<Adress,string> selector)
        {
            var uniqAddress = new Dictionary<string, Adress>();

            foreach (var a in addresses)
            {
                var field = selector(a);
                if (!uniqAddress.ContainsKey(field))
                    uniqAddress.Add(field, a);
            }

            return uniqAddress;
        }

        static void SplitAndAddSubsAccents(Dictionary<string, List<Adress>> result, Adress a, string searchstring)
        {
            searchstring = searchstring
                .Trim()
                .ToLowerInvariant()
                .Replace(".", "");

            SplitAndAdd(result, a, searchstring);

            if (searchstring.Contains("é") || searchstring.Contains("ü"))
                SplitAndAdd(result, a, searchstring.Replace("é", "e").Replace("ü", "u"));
        }

        private static void SplitAndAdd(Dictionary<string, List<Adress>> result, Adress a, string searchstring)
        {
            for (int i = 1; i < searchstring.Length; i++)
            {
                Add(result, a, searchstring.Substring(0, i));
            }
        }

        private static void Add(Dictionary<string, List<Adress>> result, Adress a, string searchstring)
        {
            if (!result.TryGetValue(searchstring, out List<Adress> hits))
                result.Add(searchstring, hits = new List<Adress>());

            if (hits.Count > 35)
                return;

            hits.Add(a);
        }
    }
}
