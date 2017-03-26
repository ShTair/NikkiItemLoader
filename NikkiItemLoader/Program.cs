﻿using NikkiItemLoader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NikkiItemLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Run(args[0]).Wait();
        }

        private static async Task Run(string path)
        {
            var items = new ItemContainer();
            var header = new List<string>();

            Console.Write("Loading...");
            using (var reader = File.OpenText(path))
            {
                for (int i = 0; i < 10; i++) header.Add(await reader.ReadLineAsync());

                items.LoadCsv(reader);
            }
            Console.WriteLine(" Done");

            var p = new ItemLoadParameter
            {
                IdConverter = strs => int.Parse(strs[1]),
                ItemConverter = NormalConverter,
            };

            Console.Write("シューズ...");
            p.PostProcess = item => { item.Name = item.Name.Replace("(シューズ)", ""); };
            items.Load("https://miraclenikki.gamerch.com/%E3%82%B7%E3%83%A5%E3%83%BC%E3%82%BA", 60000, p);
            Console.WriteLine(" Done");

            Console.Write("Saving...");
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(true)))
            {
                foreach (var line in header) await writer.WriteLineAsync(line);

                items.SaveCsv(writer);
            }
            Console.WriteLine(" Done");
        }

        private static void NormalConverter(IList<string> strs, Item item)
        {
            item.Kind = strs[0];
            item.Name = strs[2].Replace("（", "(").Replace("）", ")");
            item.Rarity = strs[3].Substring(1);
            item.P11 = strs[4].ToUpper();
            item.P12 = strs[5].ToUpper();
            item.P21 = strs[6].ToUpper();
            item.P22 = strs[7].ToUpper();
            item.P31 = strs[8].ToUpper();
            item.P32 = strs[9].ToUpper();
            item.P41 = strs[10].ToUpper();
            item.P42 = strs[11].ToUpper();
            item.P51 = strs[12].ToUpper();
            item.P52 = strs[13].ToUpper();
            item.Tags = (strs[14] + " " + strs[15]).Trim();

            if (string.IsNullOrWhiteSpace(item.P11 + item.P12)
                || string.IsNullOrWhiteSpace(item.P21 + item.P22)
                || string.IsNullOrWhiteSpace(item.P31 + item.P32)
                || string.IsNullOrWhiteSpace(item.P41 + item.P42)
                || string.IsNullOrWhiteSpace(item.P51 + item.P52))
            {
                item.Name = "";
            }
        }

        private static void PartConverter(IList<string> strs, Item item)
        {
            item.Kind = strs[1];
            item.Name = strs[3].Replace("（", "(").Replace("）", ")");
            item.Rarity = strs[4].Substring(1);
            item.P11 = strs[5].ToUpper();
            item.P12 = strs[6].ToUpper();
            item.P21 = strs[7].ToUpper();
            item.P22 = strs[8].ToUpper();
            item.P31 = strs[9].ToUpper();
            item.P32 = strs[10].ToUpper();
            item.P41 = strs[11].ToUpper();
            item.P42 = strs[12].ToUpper();
            item.P51 = strs[13].ToUpper();
            item.P52 = strs[14].ToUpper();
            item.Tags = (strs[15] + " " + strs[16]).Trim();

            if (string.IsNullOrWhiteSpace(item.P11 + item.P12)
                || string.IsNullOrWhiteSpace(item.P21 + item.P22)
                || string.IsNullOrWhiteSpace(item.P31 + item.P32)
                || string.IsNullOrWhiteSpace(item.P41 + item.P42)
                || string.IsNullOrWhiteSpace(item.P51 + item.P52))
            {
                item.Name = "";
            }
        }
    }
}
