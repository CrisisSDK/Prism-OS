﻿using System;
using System.Drawing;

namespace PrismProject.Functions.System2
{
    abstract class Convert
    {
        public static byte[] ToByteArray(dynamic Input)
        {
            return System.Text.Encoding.UTF8.GetBytes(Input);
        }

        public abstract string[] ToStringArray();

        public static string[] ToStringArray(byte[] Input)
        {
            string[] NewStringArray = new string[] { };
            int place = 0;
            foreach(byte Set in Input)
            {
                NewStringArray[place++] = Set.ToString();
            }
            return NewStringArray;
        }

        public abstract Color ToColor();

        public static Color ToColor(int Input)
        {
            return Color.FromArgb(Input);
        }

        public static Color ToColor(string Input)
        {
            return Color.FromArgb(System.Convert.ToInt32(Input));
        }

        public abstract Color[] ToColorArray();

        public static Color[] ToColorArray(string[] Input)
        {
            Color[] ColorArray = new Color[] { };
            int place = 0;
            foreach (string RGB in Input)
            {
                Console.WriteLine("changing " + RGB + " to color");
                ColorArray[place++] = Color.FromArgb(System.Convert.ToInt32(RGB));
            }
            return ColorArray;
        }


        public static int KMtoMile(int KM)
        {
            return KM / (int)1.609344;
        }
    }
}
