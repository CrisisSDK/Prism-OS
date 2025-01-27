﻿using PrismOS.Libraries.UI;
using Cosmos.System;

namespace PrismOS.Libraries.Runtime
{
    public class Terminal : Application
    {
        public Window Window = new();
        public Button Button = new();
        public Label Label1 = new();
        public string Input = "";

        public override void OnCreate()
        {
            // Window
            Window.X = 75;
            Window.Y = 75;
            Window.Width = 300;
            Window.Height = 200;
            Window.Text = "Console";

            // Button
            Button.X = (int)(Window.Width - 20);
            Button.Width = 20;
            Button.Height = 20;
            Button.Text = "X";
            Button.OnClickEvents.Add(() => { Window.Windows.Remove(Window); Applications.Remove(this); });

            // Label1
            Label1.X = 1;
            Label1.Y = 20;
            Label1.Width = Window.Width - 2;
            Label1.Height = Window.Height - 21;
            Label1.Text = "";

            WriteLine("Prism OS CLI V1");
            Window.Elements.Add(Button);
            Window.Elements.Add(Label1);
            Window.Windows.Add(Window);
        }

        public override void OnUpdate()
        {

        }

        public override void OnDestroy()
        {
        }

        public override void OnKey(KeyEvent Key)
        {
            switch (Key.Key)
            {
                case ConsoleKeyEx.Backspace:
                    if (Input.Length != 0)
                    {
                        Input = Input[0..(Input.Length - 1)];
                        Label1.Text = Label1.Text[0..(Label1.Text.Length - 1)];
                    }
                    break;
                case ConsoleKeyEx.Enter:
                    WriteLine("");
                    RunCommand(Input);
                    Input = "";
                    break;
                case ConsoleKeyEx.Tab:
                    Input += '\t';
                    Label1.Text += '\t';
                    break;
                default:
                    Input += Key.KeyChar;
                    Label1.Text += Key.KeyChar;
                    break;
            }
        }

        public void RunCommand(string Command)
        {
            switch (Command)
            {
                case "test":
                    WriteLine("this is a testing command!");
                    break;
            }
        }

        public void WriteLine(string T)
        {
            Label1.Text += T + '\n';
        }
    }
}