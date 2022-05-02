﻿namespace PrismOS.Libraries.Graphics.GUI.Elements
{
    public class Textbox : Element
    {
        public Color Background = Color.White, Foreground = Color.Black;
        public string Text = "";

        public override void Update(Canvas Canvas, WindowManager.Window Parent)
        {
            if (Cosmos.System.KeyboardManager.TryReadKey(out var Key))
            {
                Text += Key.KeyChar.ToString();
            }

            Canvas.DrawFilledRectangle(Parent.X + X, Parent.Y + Y, Width, Height, Radius, Background);
            Canvas.DrawString(Parent.X + X, Parent.Y + Y, Text, Foreground);
        }
    }
}