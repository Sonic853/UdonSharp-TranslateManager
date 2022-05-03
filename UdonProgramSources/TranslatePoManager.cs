
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
        [Header("需要翻译的父物体（勾选从父物体获取时可用）")]
        [Header("The parent object to translate (not selected when getting from parent object is not available)")]
        [SerializeField] GameObject[] parentObjects;
        [Header("需要翻译的文本（勾选从父物体获取时不可用）")]
        [Header("The text to translate (when get from parent object is not available)")]
        [SerializeField] Text[] texts;
        // [SerializeField] TextMesh[] textMeshTexts;
        [SerializeField] TextMeshPro[] textMeshProTexts;
        [SerializeField] TextMeshProUGUI[] textMeshProUGUITexts;
        [HideInInspector] string[] _texts;
        // [HideInInspector] string[] _textMeshTexts;
        [HideInInspector] string[] _textMeshProTexts;
        [HideInInspector] string[] _textMeshProUGUITexts;
        public void setLanguage(int language)
        {
            defaultLanguage = language;
        }
        public void switchLanguage()
        {
            defaultLanguage = (defaultLanguage + 1) % translatePos.Length;
            updateUI();
        }
        public void switchLanguagePrevious()
        {
            defaultLanguage = (defaultLanguage - 1 + translatePos.Length) % translatePos.Length;
            updateUI();
        }
        public string getLanguageName(int language)
        {
            if (language < 0 || language >= translatePos.Length) return "";
            return translatePos[language].language;
        }
        public string returnLanguageName()
        {
            return getLanguageName(defaultLanguage);
        }
        public string _getText(string text)
        {
            if (defaultLanguage == 0)
            {
                return originalPo._getOriginalText(text);
            }
            else
            {
                return translatePos[defaultLanguage]._getText(text);
            }
        }
        public string _(string text)
        {
            return _getText(text);
        }
        public void updateUI()
        {
            for (int i = 0; i < texts.Length; i++)
            {
                if (defaultLanguage == 0)
                {
                    texts[i].text = originalPo._getOriginalText(_texts[i]);
                }
                else
                {
                    texts[i].text = translatePos[defaultLanguage]._getText(_texts[i]);
                }
            }
            // for (int i = 0; i < textMeshTexts.Length; i++)
            // {
            //     if (defaultLanguage == 0)
            //     {
            //         textMeshTexts[i].text = originalPo._getOriginalText(_textMeshTexts[i]);
            //     }
            //     else
            //     {
            //         textMeshTexts[i].text = translatePos[defaultLanguage]._getText(_textMeshTexts[i]);
            //     }
            // }
            for (int i = 0; i < textMeshProTexts.Length; i++)
            {
                if (defaultLanguage == 0)
                {
                    textMeshProTexts[i].text = originalPo._getOriginalText(_textMeshProTexts[i]);
                }
                else
                {
                    textMeshProTexts[i].text = translatePos[defaultLanguage]._getText(_textMeshProTexts[i]);
                }
            }
            for (int i = 0; i < textMeshProUGUITexts.Length; i++)
            {
                if (defaultLanguage == 0)
                {
                    textMeshProUGUITexts[i].text = originalPo._getOriginalText(_textMeshProUGUITexts[i]);
                }
                else
                {
                    textMeshProUGUITexts[i].text = translatePos[defaultLanguage]._getText(_textMeshProUGUITexts[i]);
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
                // texts = new Text[0];
                // // textMeshTexts = new TextMeshPro[0];
                // textMeshProTexts = new TextMeshPro[0];
                // textMeshProUGUITexts = new TextMeshProUGUI[0];
                for (int i = 0; i < parentObjects.Length; i++)
                {
                    if (parentObjects[i] != null)
                    {
                        texts = textArrayAdd(texts, parentObjects[i].GetComponentsInChildren<Text>(true));
                        // textMeshTexts = textMeshArrayAdd(textMeshTexts, parentObjects[i].GetComponentsInChildren<TextMesh>(true));
                        textMeshProTexts = textMeshProArrayAdd(textMeshProTexts, parentObjects[i].GetComponentsInChildren<TextMeshPro>(true));
                        textMeshProUGUITexts = textMeshProUGUIArrayAdd(textMeshProUGUITexts, parentObjects[i].GetComponentsInChildren<TextMeshProUGUI>(true));
                    }
                }
                // texts = parentObject.GetComponentsInChildren<Text>(true);
                // // textMeshTexts = parentObject.GetComponentsInChildren<TextMesh>(true);
                // textMeshProTexts = parentObject.GetComponentsInChildren<TextMeshPro>(true);
                // textMeshProUGUITexts = parentObject.GetComponentsInChildren<TextMeshProUGUI>(true);
            }
            _texts = new string[texts.Length];
            // _textMeshTexts = new string[textMeshTexts.Length];
            _textMeshProTexts = new string[textMeshProTexts.Length];
            _textMeshProUGUITexts = new string[textMeshProUGUITexts.Length];
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i] != null)
                {
                    _texts[i] = texts[i].text;
                }
                else
                {
                    _texts[i] = "";
                }
            }
            // for (int i = 0; i < textMeshTexts.Length; i++)
            // {
            //     if (textMeshTexts[i] != null)
            //     {
            //         _textMeshTexts[i] = textMeshTexts[i].text;
            //     }
            //     else
            //     {
            //         _textMeshTexts[i] = "";
            //     }
            // }
            for (int i = 0; i < textMeshProTexts.Length; i++)
            {
                if (textMeshProTexts[i] != null)
                {
                    _textMeshProTexts[i] = textMeshProTexts[i].text;
                }
                else
                {
                    _textMeshProTexts[i] = "";
                }
            }
            for (int i = 0; i < textMeshProUGUITexts.Length; i++)
            {
                if (textMeshProUGUITexts[i] != null)
                {
                    _textMeshProUGUITexts[i] = textMeshProUGUITexts[i].text;
                }
                else
                {
                    _textMeshProUGUITexts[i] = "";
                }
            }
            if (startAutoTranslate)
            {
                updateUI();
            }
        }
        Text[] textArrayAdd(Text[] textArray, Text[] newTextArray)
        {
            Text[] temp = new Text[textArray.Length + newTextArray.Length];
            for (int i = 0; i < textArray.Length; i++)
            {
                temp[i] = textArray[i];
            }
            for (int i = 0; i < newTextArray.Length; i++)
            {
                temp[textArray.Length + i] = newTextArray[i];
            }
            return temp;
        }
        // TextMesh[] textMeshArrayAdd(TextMesh[] textMeshArray, TextMesh[] newTextMeshArray)
        TextMeshPro[] textMeshProArrayAdd(TextMeshPro[] textMeshProArray, TextMeshPro[] newTextMeshProArray)
        {
            TextMeshPro[] temp = new TextMeshPro[textMeshProArray.Length + newTextMeshProArray.Length];
            for (int i = 0; i < textMeshProArray.Length; i++)
            {
                temp[i] = textMeshProArray[i];
            }
            for (int i = 0; i < newTextMeshProArray.Length; i++)
            {
                temp[textMeshProArray.Length + i] = newTextMeshProArray[i];
            }
            return temp;
        }
        TextMeshProUGUI[] textMeshProUGUIArrayAdd(TextMeshProUGUI[] textMeshProUGUIArray, TextMeshProUGUI[] newTextMeshProUGUIArray)
        {
            TextMeshProUGUI[] temp = new TextMeshProUGUI[textMeshProUGUIArray.Length + newTextMeshProUGUIArray.Length];
            for (int i = 0; i < textMeshProUGUIArray.Length; i++)
            {
                temp[i] = textMeshProUGUIArray[i];
            }
            for (int i = 0; i < newTextMeshProUGUIArray.Length; i++)
            {
                temp[textMeshProUGUIArray.Length + i] = newTextMeshProUGUIArray[i];
            }
            return temp;
        }
    }
}
