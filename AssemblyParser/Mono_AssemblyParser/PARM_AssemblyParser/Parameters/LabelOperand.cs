﻿using P_ARM_AssemblyParser.Parsers;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace P_ARM_AssemblyParser.Parameters
{
    public class LabelOperand : IOperand
    {
        public short GetDefaultValue()
        {
            return 0;
        }

        public int GetMaxLength()
        {
            return 8;
        }

        public string GetPattern()
        {
            return @"[\._\w\d]+";
        }

        public bool HasToBeParsed()
        {
            return true;
        }

        public bool IsOptional()
        {
            return false;
        }

        public short Parse(string text)
        {
            Dictionary<string, short> labelsLines = SFileParser.CurrentFileLabelsLines;

            string label = Regex.Match(text, GetPattern()).Value;
            if (!labelsLines.ContainsKey(label))
                throw new KeyNotFoundException();

            return labelsLines[label];
        }
    }
}
