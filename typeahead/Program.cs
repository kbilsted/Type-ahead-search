using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static typeahead.Timer;

namespace typeahead
{
    class Program
    {
        private const int BackspaceKey = 8;
        private const int ReturnKey = 13;

        static void Main(string[] args)
        {
            var output = @"c:\src\typeahead\dkaddress.csv";

            //RawFileExtractor.ExtractOnlyAdress(@"C:\src\typeahead\10adr.csv", output);

            var addresses = Time(() => FilteredFileReader.ReadLines(output), "load data file");
            var typeaheadcache = Time(() => CacheBuilder.BuildCache(addresses), "build cache");

            ReadEvalLoop(typeaheadcache);

            Console.WriteLine("PRESS KEY");
            Console.ReadKey();
        }

        private static void ReadEvalLoop(Dictionary<string, List<Adress>> typeaheadcache)
        {
            Console.WriteLine("press ctrl-c to escape, and RETURN to clear search and BACKSPACE to delete last keypress");
            Console.Write("\n> ");

            string buffer = "";
            while (true)
            {
                char c = Console.ReadKey(true).KeyChar;

                if ((int)c == ReturnKey)
                {
                    buffer = "";
                    Console.Write("\n> ");
                    continue;
                }

                if ((int)c == BackspaceKey) 
                {
                    if (buffer.Length > 0)
                        buffer = buffer.Substring(0, buffer.Length - 1);
                }
                else
                    buffer += c.ToString();

                SearchAndShow(typeaheadcache, buffer);
            }
        }

        private static void SearchAndShow(Dictionary<string, List<Adress>> typeaheadcache, string buffer)
        {
            Console.WriteLine("Searching '" + buffer + "'");

            var searchbuffer = buffer.Replace(".", "");
            var results = typeaheadcache.ContainsKey(searchbuffer) ? typeaheadcache[searchbuffer] : new List<Adress>();

            foreach (var result in results)
                Console.WriteLine(result.ToString());
            Console.WriteLine("-------");
        }
    }

    class FilteredFileReader
    {
        public static Adress[] ReadLines(string path)
        {
            return File.ReadAllLines(path)
                .Select(x => x.Split(','))
                .Select(x => new Adress()
                {
                    adresseringsvejnavn = string.Intern(x[0]),
                    husnr = string.Intern(x[1]),
                    etage = string.Intern(x[2]),
                    dør = string.Intern(x[3]),
                    supplerendebynavn = string.Intern(x[4]),
                    postnr = string.Intern(x[5]),
                    postnrnavn = string.Intern(x[6]),
                }).ToArray();
        }
    }

    class RawFileExtractor
    {
        public static void ExtractOnlyAdress(string input, string output)
        {
            var result = new List<List<string>>();
            var lines = File.ReadAllLines(input);
            foreach (var l in lines)
            {
                var columsn = l.Split(",");
                var adr = columsn.Where((x, i) => i >= 6 && i <= 12).ToList();
                result.Add(adr);
            }

            File.WriteAllLines(output, result.Select(x => string.Join(",", x)));
        }
    }
}
