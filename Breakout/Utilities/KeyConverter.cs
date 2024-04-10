using DIKUArcade.Input;
using System;
using System.IO;

namespace Breakout.Utilities {
    public class KeyConverter {
        private static KeyConverter instance;


        public static KeyboardAction ConvertActionString(string action) {
            switch (action) {
                case "KEY_RELEASE":
                    return KeyboardAction.KeyRelease;
                case "KEY_PRESS":
                    return KeyboardAction.KeyPress;
                default:
                    throw new ArgumentException("KeyboardAction is not a valid action");
            }
        }

        public static KeyboardKey ConvertKeyString(string key) {
            switch(key) {
                case "KEY_A":
                    return KeyboardKey.A;
                case "KEY_D":
                    return KeyboardKey.D;
                case "KEY_LEFT":
                    return KeyboardKey.Left;
                case "KEY_RIGHT":
                    return KeyboardKey.Right;
                default:
                    return KeyboardKey.Unknown;
            }
        }

        public static KeyConverter GetInstance() {
            return KeyConverter.instance ?? 
                (KeyConverter.instance = new KeyConverter());
        }
    }
}