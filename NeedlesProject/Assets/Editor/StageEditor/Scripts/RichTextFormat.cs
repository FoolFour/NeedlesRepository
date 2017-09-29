using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Format
{
    //俺式リッチテキストフォーマット
    public static class RichText
    {
        public static string Success(string text)
        {
            return "<color=lime>" + text + "</color>";
        }

        public static string Failed(string text)
        {
            return "<color=orange>" + text + "</color>";
        }

        public static string Info(string text)
        {
            return "<color=blue>" + text + "</color>";
        }
    }
}