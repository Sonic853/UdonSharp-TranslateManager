
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonLab
{
    public class TranslatePoManager : UdonSharpBehaviour
    {
        [Header("------------------------------------------------------------------")]
        [Header("■■■ 翻译文本管理器（Po版） TranslatePoManager(Po ver) ■■■")]
        [Header("------------------------------------------------------------------")]
        [Header("默认语言，0为原始语言")]
        [Header("Default language, 0 is the original language")]
        [SerializeField] int defaultLanguage = 0;
        [Header("开始时是否自动翻译")]
        [Header("Whether to automatically translate at start")]
        [SerializeField] bool startAutoTranslate = false;
        [Header("原文本")]
        [Header("Original text")]
        [SerializeField] TranslatePo originalPo;
        [Header("翻译文本，第一个先放原文本")]
        [Header("Translate text, put original text first")]
        [SerializeField] TranslatePo[] translatePos;
        [Header("是否从父物体获取需要翻译的UI")]
        [Header("Whether to get the UI to translate from the parent object")]
        [SerializeField] bool getFromParent = false;
        [Header("需要翻译的父物体")]
        [Header("The parent object to translate")]
        [SerializeField] GameObject parentObject;
        [Header("需要翻译的文本（勾选从父物体获取时不可用）")]
        [Header("The text to translate (when get from parent object is not available)")]
        [SerializeField] Text[] texts;
        [SerializeField] TextMeshPro[] textMeshTexts;
        [SerializeField] TextMeshProUGUI[] textMeshUGUITexts;
        public void updateUI()
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (defaultLanguage == 0)
                {
                    texts[i].text = originalPo._getOriginalText(texts[i].text);
                }
                else
                {
                    texts[i].text = translatePos[defaultLanguage]._getText(texts[i].text);
                }
            }
            for (int i = 0; i < textMeshTexts.Length; i++)
            {
                if (defaultLanguage == 0)
                {
                    textMeshTexts[i].text = originalPo._getOriginalText(textMeshTexts[i].text);
                }
                else
                {
                    textMeshTexts[i].text = translatePos[defaultLanguage]._getText(textMeshTexts[i].text);
                }
            }
            for (int i = 0; i < textMeshUGUITexts.Length; i++)
            {
                if (defaultLanguage == 0)
                {
                    textMeshUGUITexts[i].text = originalPo._getOriginalText(textMeshUGUITexts[i].text);
                }
                else
                {
                    textMeshUGUITexts[i].text = translatePos[defaultLanguage]._getText(textMeshUGUITexts[i].text);
                }
            }
        }
        void Start()
        {
            for (int i = 0; i < translatePos.Length; i++)
            {
                originalPo.ReadPoFile();
                translatePos[i].ReadPoFile();
            }
            if (getFromParent)
            {
                texts = parentObject.GetComponentsInChildren<Text>(true);
                textMeshTexts = parentObject.GetComponentsInChildren<TextMeshPro>(true);
                textMeshUGUITexts = parentObject.GetComponentsInChildren<TextMeshProUGUI>(true);
            }
            if (startAutoTranslate)
            {
                updateUI();
            }
        }
    }
}
