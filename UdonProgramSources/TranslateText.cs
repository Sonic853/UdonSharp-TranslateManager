
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonLab
{
    public class TranslateText : UdonSharpBehaviour
    {
        [Header("在此处输入文本（Mode为1和2时有效）")]
        [Header("Enter text here (valid when Mode is 1 and 2)")]
        [TextArea(3,10)]
        public string[] strings;
        [Header("在此处输入文本（Mode为0时有效）")]
        [Header("Enter text here (valid when Mode is 0)")]
        [TextArea(3,10)]
        public string[] texts;
        // [Header("在此处输入文本（Mode为0时有效）")]
        // [Header("Enter text here (valid when Mode is 0)")]
        // [TextArea(3,10)]
        // public string[] textMeshTexts;
        [Header("在此处输入文本（Mode为0时有效）")]
        [Header("Enter text here (valid when Mode is 0)")]
        [TextArea(3,10)]
        public string[] textMeshProTexts;
        [Header("在此处输入文本（Mode为0时有效）")]
        [Header("Enter text here (valid when Mode is 0)")]
        [TextArea(3,10)]
        public string[] textMeshProUGUITexts;
    }
}
