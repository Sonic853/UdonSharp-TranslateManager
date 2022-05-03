
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonLab
{
    public class TranslateManager : UdonSharpBehaviour
    {
        [Header("------------------------------------------------------------------")]
        [Header("■■■ 翻译文本管理器 TranslateManager ■■■")]
        [Header("------------------------------------------------------------------")]
        [Header("Mode 0：TranslateManager里的Text、textMeshTexts、textMeshUGUITexts")]
        [Header("将与TranslateText里的Text、textMeshTexts、textMeshUGUITexts对应翻译")]
        [Space(10)]
        [Header("Mode 0: Text, textMeshTexts, textMeshUGUITexts in TranslateManager will be")]
        [Header("translated corresponding to Text, textMeshTexts, textMeshUGUITexts in TranslateText")]
        [Space(10)]
        [Header("Mode 1：只使用TranslateText里的strings来从上往下翻译TranslateManager")]
        [Header("的Text、textMeshTexts、textMeshUGUITexts文本")]
        [Space(10)]
        [Header("Mode 1: Translate Text, textMeshTexts, textMeshUGUITexts of")]
        [Header("TranslateManager from top to bottom using only strings in TranslateText")]
        [Space(10)]
        [Header("Mode 2：将拿每个Text、textMeshTexts、textMeshUGUITexts查找匹配")]
        [Header("TranslateText里的strings进行替换")]
        [Space(10)]
        [Header("Mode 2: Will take each Text, textMeshTexts, textMeshUGUITexts")]
        [Header("to find and replace the strings in TranslateText")]
        [Space(10)]
        // [Header("Mode 3：将拿每个Text、textMeshTexts、textMeshUGUITexts查找匹配")]
        // [Header("TranslatePo里的翻译文本进行替换")]
        // [Space(10)]
        // [Header("Mode 3: Will take each Text, textMeshTexts, textMeshUGUITexts")]
        // [Header("to find and replace the strings in TranslatePo")]
        [Range(0, 2)]
        [SerializeField] int mode = 0;
        [Header("默认语言，0为原始语言")]
        [Header("Default language, 0 is the original language")]
        [SerializeField] int defaultLanguage = 0;
        [Header("开始时是否自动翻译")]
        [Header("Whether to automatically translate at start")]
        [SerializeField] bool startAutoTranslate = false;
        [Header("原文本")]
        [Header("Original text")]
        // [Header("原文本（Mode为0到2时有效）")]
        // [Header("Original text(valid when Mode is 0 to 2)")]
        [SerializeField] TranslateText originalText;
        // [Header("原文本（Mode为3时有效）")]
        // [Header("Original text(valid when Mode is 3)")]
        // [SerializeField] TranslatePo originalPo;
        [Header("语言名称")]
        [Header("Language name")]
        [SerializeField] public string[] languageNames;
        [Header("翻译文本，第一个先放原文本")]
        [Header("Translate text, put original text first")]
        // [Header("翻译文本，第一个先放原文本（Mode为0到2时有效）")]
        // [Header("Translate text, put original text first(valid when Mode is 0 to 2)")]
        [SerializeField] TranslateText[] translateTexts;
        // [Header("翻译文本，第一个先放原文本（Mode为3时有效）")]
        // [Header("Translate text, put original text first(valid when Mode is 3)")]
        // [SerializeField] TranslatePo[] translatePos;
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
        // Mode 2 时用到的临时变量
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
            setLanguage((defaultLanguage + 1) % languageNames.Length);
            updateUI();
        }
        public void switchLanguagePrevious()
        {
            setLanguage((defaultLanguage - 1 + languageNames.Length) % languageNames.Length);
            updateUI();
        }
        public string _getText(string text)
        {
            if (defaultLanguage >= translateTexts.Length)
            {
                return text;
            }
            else if (translateTexts[defaultLanguage] == null)
            {
                return text;
            }
            else
            {
                int index = stringIndexOf(originalText.strings, text);
                if (index != -1)
                {
                    if (index < translateTexts[defaultLanguage].strings.Length)
                    {
                        return translateTexts[defaultLanguage].strings[index];
                    }
                    else
                    {
                        return text;
                    }
                }
                else
                {
                    return text;
                }
            }
        }
        public string _(string text)
        {
            return _getText(text);
        }
        int stringIndexOf(string[] strings, string string_)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i] == string_)
                {
                    return i;
                }
            }
            return -1;
        }
        public void updateUI()
        {
            if (defaultLanguage < 0 || defaultLanguage > translateTexts.Length) return;
            switch (mode)
            {
                case 0:
                    {
                        for (int i = 0; i < texts.Length; i++)
                        {
                            if (i < translateTexts[defaultLanguage].texts.Length)
                            {
                                texts[i].text = translateTexts[defaultLanguage].texts[i];
                            }
                        }
                        // for (int i = 0; i < textMeshTexts.Length; i++)
                        // {
                        //     if (i < translateTexts[defaultLanguage].texts.Length)
                        //     {
                        //         textMeshTexts[i].text = translateTexts[defaultLanguage].texts[i];
                        //     }
                        // }
                        for (int i = 0; i < textMeshProTexts.Length; i++)
                        {
                            if (i < translateTexts[defaultLanguage].textMeshProTexts.Length)
                            {
                                textMeshProTexts[i].text = translateTexts[defaultLanguage].textMeshProTexts[i];
                            }
                        }
                        for (int i = 0; i < textMeshProUGUITexts.Length; i++)
                        {
                            if (i < translateTexts[defaultLanguage].textMeshProUGUITexts.Length)
                            {
                                textMeshProUGUITexts[i].text = translateTexts[defaultLanguage].textMeshProUGUITexts[i];
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        int i = 0;
                        int Length = translateTexts[defaultLanguage].strings.Length;
                        for (int j = 0; j < texts.Length; j++)
                        {
                            if (i < Length)
                            {
                                texts[j].text = translateTexts[defaultLanguage].strings[i];
                                i++;
                            }
                        }
                        // for (int j = 0; j < textMeshTexts.Length; j++)
                        // {
                        //     if (i < Length)
                        //     {
                        //         textMeshTexts[j].text = translateTexts[defaultLanguage].strings[i];
                        //         i++;
                        //     }
                        // }
                        for (int j = 0; j < textMeshProTexts.Length; j++)
                        {
                            if (i < Length)
                            {
                                textMeshProTexts[j].text = translateTexts[defaultLanguage].strings[i];
                                i++;
                            }
                        }
                        for (int j = 0; j < textMeshProUGUITexts.Length; j++)
                        {
                            if (i < Length)
                            {
                                textMeshProUGUITexts[j].text = translateTexts[defaultLanguage].strings[i];
                                i++;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < texts.Length; i++)
                        {
                            if (texts[i] != null)
                            {
                                texts[i].text = _getText(_texts[i]);
                            }
                        }
                        // for (int i = 0; i < textMeshTexts.Length; i++)
                        // {
                        //     if (textMeshTexts[i] != null)
                        //     {
                        //         textMeshTexts[i].text = _getText(_textMeshTexts[i]);
                        //     }
                        // }
                        for (int i = 0; i < textMeshProTexts.Length; i++)
                        {
                            if (textMeshProTexts[i] != null)
                            {
                                textMeshProTexts[i].text = _getText(_textMeshProTexts[i]);
                            }
                        }
                        for (int i = 0; i < textMeshProUGUITexts.Length; i++)
                        {
                            if (textMeshProUGUITexts[i] != null)
                            {
                                textMeshProUGUITexts[i].text = _getText(_textMeshProUGUITexts[i]);
                            }
                        }
                        break;
                    }
            }
        }
        public string getLanguageName(int language)
        {
            if (language < 0 || language >= languageNames.Length) return "";
            return languageNames[language];
        }
        public string returnLanguageName()
        {
            return getLanguageName(defaultLanguage);
        }
        void Start()
        {
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
                        // textMeshTextArrayAdd(textMeshTexts, parentObjects[i].GetComponentsInChildren<TextMeshPro>(true));
                        textMeshProTexts = textMeshProArrayAdd(textMeshProTexts, parentObjects[i].GetComponentsInChildren<TextMeshPro>(true));
                        textMeshProUGUITexts = textMeshProUGUIArrayAdd(textMeshProUGUITexts, parentObjects[i].GetComponentsInChildren<TextMeshProUGUI>(true));
                    }
                }
                // texts = parentObject.GetComponentsInChildren<Text>(true);
                // // textMeshTexts = parentObject.GetComponentsInChildren<TextMesh>(true);
                // textMeshProTexts = parentObject.GetComponentsInChildren<TextMeshPro>(true);
                // textMeshProUGUITexts = parentObject.GetComponentsInChildren<TextMeshProUGUI>(true);
            }
            if (mode == 2)
            {
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
