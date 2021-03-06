﻿using P_ARM_AssemblyParser.Instructions;
using P_ARM_AssemblyParser.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace P_ARM_AssemblyParser.Parsers
{
    public static class SFileParser
    {
        public static Dictionary<string, short> CurrentFileLabelsLines { get; private set; }

        public static string ParseFile(string filePath)
        {
            List<string> allLines = System.IO.File.ReadAllLines(filePath).ToList();
            CurrentFileLabelsLines = GetLabelsLines(allLines);

            InstructionParser parser;
            short convertedIntruction;
            string convertedFile = "", line;

            for (int i = 0; i < allLines.Count; i++)
            {
                line = allLines[i];

                try
                {
                    parser = new InstructionParser(line);
                    convertedIntruction = parser.ParseInstruction();
                    convertedFile += convertedIntruction.ToString("X2") + (i == allLines.Count - 1 ? "" : " ");
                }
                catch (Exception) {}
            }

            return convertedFile.ToLower();
        }

        private static Dictionary<string, short> GetLabelsLines(List<string> wholeFile)
        {
            Dictionary<string, short> labelsLines = new Dictionary<string, short>();
            string pattern = new LabelOperand().GetPattern();

            Match match;
            short numLine = 1;
            InstructionParser parser;
            foreach (string line in wholeFile)
            {
                try
                {
                    match = Regex.Match(line, @"^((\t*\s*\t*)|(\s*\t*\s*))" + pattern + ":", InstructionParser.Options);
                    if (match.Success)
                        labelsLines.Add(Regex.Match(match.Value, pattern + ":").Value.Replace(":", "").ToUpper(), numLine);
                    else
                    {
                        parser = new InstructionParser(line);
                        try
                        {
                            parser.ParseInstruction();
                            numLine++;
                        }
                        catch (InstructionException) {}
                    }
                }
                catch (Exception) {}
            }

            return labelsLines;
        }
    }
}
