using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Val_heim
{
    class Utils
    {
        public static bool SILENT_MODE = false;

        public static void ToChat(string text)
        {
            if (!SILENT_MODE)
            {
                Chat.instance.SendText(Talker.Type.Whisper, text);
            }
        }

        public static bool FromChat(string text)
        {
            Chat cin = Chat.instance;
            if (cin.m_input.text.Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                cin.m_input.text = "";
                return true;
            }
            return false;
        }

        public static string FromChatRegex(Regex rule)
        {
            Chat cin = Chat.instance;
            foreach (Match match in rule.Matches(cin.m_input.text))
            {
                cin.m_input.text = "";
                return match.Groups[1].Value;
            }
            return "";
        }
    }
}
