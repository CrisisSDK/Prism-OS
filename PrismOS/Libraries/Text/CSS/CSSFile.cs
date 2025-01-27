﻿using System.Collections.Generic;
using Cosmos.Core;
using System;

namespace PrismOS.Libraries.Text.CSS
{
    public class CSSFile : IDisposable
    {
        public CSSFile(string Contents)
        {
            Properties = new();
            string Builder1 = "";
            string Builder2 = "";

            for (int I = 0; I < Contents.Length; I++)
            {
                if (CheckSkip(Contents[I]))
                {
                    continue;
                }
                if (Contents[I] == '{')
                {
                    Properties.Add(Builder1, new());

                    while (Contents[I++] != '}')
                    {
                        if (CheckSkip(Contents[I]))
                        {
                            continue;
                        }
                        if (Contents[I] == ':')
                        {
                            Properties[Builder1].Add(new(Builder2, ""));
                            while (Contents[I++] != ';')
                            {
                                if (CheckSkip(Contents[I]))
                                {
                                    continue;
                                }

                                Properties[Builder1][^1].Value += Contents[I];
                            }
                            Builder2 = "";
                        }

                        Builder2 += Contents[I];
                    }

                    Builder1 = "";
                    Builder2 = "";
                }

                Builder1 += Contents[I];
            }
        }

        private static bool CheckSkip(char C)
        {
            if (C == ' ' || C == '\r' || C == '\n' || C == '\t')
            {
                return true;
            }
            return false;
        }

        public Dictionary<string, List<CSSProperty>> Properties { get; set; }

        public void Dispose()
        {
            GCImplementation.Free(Properties);
            GC.SuppressFinalize(this);
        }
    }
}