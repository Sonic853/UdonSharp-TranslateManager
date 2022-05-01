
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UdonLab
{
    public class UITextExporter : EditorWindow
    {
        // 判断当前编辑器语言
        static bool isChinese = true;
        private bool canMakePo = true;
        [MenuItem("Translation/Export UI Text to po")]
        static void Init()
        {
            UITextExporter window = GetWindow<UITextExporter>();
            window.titleContent = new GUIContent(isChinese ? "导出UI文本工具" : "Export UI Text");
            window.Show();
        }
        private string originalLanguage;
        private string translateLanguage;
        private int popupLanguageIndex = 0;
        private int popupTranslationIndex = 0;
        private string[] popupLanguageList = {
            "English",
            "Simplified Chinese",
            "Japanese",
            "Korean",
            "French",
            "German",
            "Italian",
            "Spanish",
            "Portuguese",
            "Russian",
            "Traditional Chinese",
            };
        private GameObject obj;
        private Text[] texts;
        private TextMeshPro[] textMeshPro;
        private TextMeshProUGUI[] textMeshPro_ugui;
        [MenuItem("Translation/Switch Language(CHS,EN)")]
        static void SwitchLanguage()
        {
            isChinese = !isChinese;
        }
        private void OnGUI()
        {
            minSize = new Vector2(400, 300);
            // language = EditorGUILayout.TextField(isChinese ? "原语言" : "Original Language", language);
            popupLanguageIndex = EditorGUILayout.Popup(isChinese ? "原语言" : "Original Language", popupLanguageIndex, popupLanguageList);
            originalLanguage = popupLanguageList[popupLanguageIndex];
            popupTranslationIndex = EditorGUILayout.Popup(isChinese ? "翻译语言" : "Translation Language", popupTranslationIndex, popupLanguageList);
            translateLanguage = popupLanguageList[popupTranslationIndex];
            GUILayout.Label(isChinese ? "请选择要导出文本的父物体" : "Please select the parent object of the text you want to export");
            obj = EditorGUILayout.ObjectField((isChinese ? "父物体" : "parent"), obj, typeof(GameObject), true) as GameObject;
            if (GUILayout.Button(isChinese ? "读取" : "Read"))
            {
                if (obj == null)
                {
                    Debug.LogError(isChinese ? "请选择要导出文本的父物体" : "Please select the parent object of the text you want to export");
                }
                if (obj != null)
                {
                    Debug.Log(isChinese ? "已选择父物体：" + obj.name : "Parent object: " + obj.name);
                    // Debug.Log(isChinese ? "已选择父物体：" + obj.name : "Parent object: " + obj.name);
                    texts = obj.GetComponentsInChildren<Text>(true);
                    textMeshPro = obj.GetComponentsInChildren<TextMeshPro>(true);
                    textMeshPro_ugui = obj.GetComponentsInChildren<TextMeshProUGUI>(true);
                    Debug.Log((isChinese ? "读取文本" : "Text read") + ": ");
                    Debug.Log("Texts: " + texts.Length);
                    Debug.Log("TextMeshPro: " + textMeshPro.Length);
                    Debug.Log("TextMeshProUGUI: " + textMeshPro_ugui.Length);
                    if (texts.Length == 0 && textMeshPro.Length == 0 && textMeshPro_ugui.Length == 0)
                    {
                        Debug.LogError(isChinese ? "该父物体没有文本" : "This parent object has no text");
                    }
                    else
                    {
                        Debug.Log(isChinese ? "已读取文本" : "Text read");
                        canMakePo = false;
                    }
                }
            }
            EditorGUI.BeginDisabledGroup(canMakePo);
            if (GUILayout.Button(isChinese ? "导出" : "Export"))
            {
                Export();
            }
            EditorGUI.BeginDisabledGroup(canMakePo);
            if (GUILayout.Button(isChinese ? "导出原文本" : "Export Original Language"))
            {
                Export(true);
            }
        }
        private void Export(bool original = false)
        {
            List<string> textList = new List<string>();
            textList.Add("# Translation of Unity Object - " + obj.name + " - Original Language: " + originalLanguage);
            textList.Add("# This file is generated by UdonLab's UITextExporter");
            textList.Add("msgid \"\"");
            textList.Add("msgstr \"\"");
            DateTime date = new DateTime();
            textList.Add("\"PO-Revision-Date: " + date.ToString("yyyy-MM-dd HH:mm:ss+zzzz") + "\\n\"");
            textList.Add("\"MIME-Version: 1.0\\n\"");
            textList.Add("\"Content-Type: text/plain; charset=UTF-8\\n\"");
            textList.Add("\"Content-Transfer-Encoding: 8bit\\n\"");
            textList.Add("\"X-Generator: Unity UITextExporter\\n\"");
            textList.Add("\"Language: " + (original ? Language(originalLanguage) : Language(translateLanguage)) + "\\n\"");
            textList.Add("\"Project-Id-Version: Unity Object - " + obj.name + "\\n\"");
            // textList.Add("\"Last-Translator: " + Application.productName + "\\n\"");
            textList.Add("");
            // for (int i = 0; i < texts.Length; i++)
            // {
            //     string head = texts[i].name;
            //     GameObject parent = texts[i].gameObject;
            //     while (parent != obj)
            //     {
            //         head = parent.name + "/" + head;
            //         parent = parent.transform.parent.gameObject;
            //     }
            //     textList.Add("#: " + head + " Text");
            //     textList.Add("msgctxt \"" + texts[i].name + "\"");
            //     // 如果出现换行符，则替换为\\n\"\n\"
            //     string text = texts[i].text.Replace("\n", "\\n\"\n\"");
            //     textList.Add("msgid \"" + text + "\"");
            //     textList.Add("msgstr \"\"");
            //     textList.Add("");
            // }
            // for (int i = 0; i < textMeshPro.Length; i++)
            // {
            //     string head = textMeshPro[i].name;
            //     GameObject parent = textMeshPro[i].gameObject;
            //     while (parent != obj)
            //     {
            //         head = parent.name + "/" + head;
            //         parent = parent.transform.parent.gameObject;
            //     }
            //     textList.Add("#: " + head + " TextMeshPro");
            //     textList.Add("msgctxt \"" + textMeshPro[i].name + "\"");
            //     // 如果出现换行符，则替换为\\n\"\n\"
            //     string text = textMeshPro[i].text.Replace("\n", "\\n\"\n\"");
            //     textList.Add("msgid \"" + text + "\"");
            //     textList.Add("msgstr \"\"");
            //     textList.Add("");
            // }
            // for (int i = 0; i < textMeshPro_ugui.Length; i++)
            // {
            //     string head = textMeshPro_ugui[i].name;
            //     GameObject parent = textMeshPro_ugui[i].gameObject;
            //     while (parent != obj)
            //     {
            //         head = parent.name + "/" + head;
            //         parent = parent.transform.parent.gameObject;
            //     }
            //     textList.Add("#: " + head + " TextMeshProUGUI");
            //     textList.Add("msgctxt \"" + textMeshPro_ugui[i].name + "\"");
            //     // 如果出现换行符，则替换为\\n\"\n\"
            //     string text = textMeshPro_ugui[i].text.Replace("\n", "\\n\"\n\"");
            //     textList.Add("msgid \"" + text + "\"");
            //     textList.Add("msgstr \"\"");
            //     textList.Add("");
            // }
            List<string> heads = new List<string>();
            List<string> msgctxt = new List<string>();
            List<string> msgid = new List<string>();
            // List<string> msgstr = new List<string>();
            for (int i = 0; i < texts.Length; i++)
            {
                string head = texts[i].name;
                GameObject parent = texts[i].gameObject;
                while (parent != obj)
                {
                    head = parent.name + "/" + head;
                    parent = parent.transform.parent.gameObject;
                }
                if(obj.name != head)
                {
                    head = obj.name + "/" + head;
                }
                // 如果出现换行符，则替换为\\n\"\n\"
                string text = texts[i].text.Replace("\n", "\\n\"\n\"");
                int index = msgid.IndexOf(text);
                if (index != -1)
                {
                    int headIndex = int.Parse(heads[index].Substring(heads[index].Length - 1, 1));
                    headIndex++;
                    heads[index] = heads[index] + " " + head + " Text:" + headIndex.ToString();
                }
                else
                {
                    heads.Add(head + " Text:0");
                    msgctxt.Add(texts[i].name);
                    msgid.Add(text);
                }
            }
            for (int i = 0; i < textMeshPro.Length; i++)
            {
                string head = textMeshPro[i].name;
                GameObject parent = textMeshPro[i].gameObject;
                while (parent != obj)
                {
                    head = parent.name + "/" + head;
                    parent = parent.transform.parent.gameObject;
                }
                if(obj.name != head)
                {
                    head = obj.name + "/" + head;
                }
                // 如果出现换行符，则替换为\\n\"\n\"
                string text = textMeshPro[i].text.Replace("\n", "\\n\"\n\"");
                int index = msgid.IndexOf(text);
                if (index != -1)
                {
                    int headIndex = int.Parse(heads[index].Substring(heads[index].Length - 1, 1));
                    headIndex++;
                    heads[index] = heads[index] + " " + head + " TextMeshPro:" + headIndex.ToString();
                }
                else
                {
                    heads.Add(head + " TextMeshPro:0");
                    msgctxt.Add(textMeshPro[i].name);
                    msgid.Add(text);
                }
            }
            for (int i = 0; i < textMeshPro_ugui.Length; i++)
            {
                string head = textMeshPro_ugui[i].name;
                GameObject parent = textMeshPro_ugui[i].gameObject;
                while (parent != obj)
                {
                    head = parent.name + "/" + head;
                    parent = parent.transform.parent.gameObject;
                }
                if(obj.name != head)
                {
                    head = obj.name + "/" + head;
                }
                // 如果出现换行符，则替换为\\n\"\n\"
                string text = textMeshPro_ugui[i].text.Replace("\n", "\\n\"\n\"");
                int index = msgid.IndexOf(text);
                if (index != -1)
                {
                    int headIndex = int.Parse(heads[index].Substring(heads[index].Length - 1, 1));
                    headIndex++;
                    heads[index] = heads[index] + " " + head + " TextMeshProUGUI:" + headIndex.ToString();
                }
                else
                {
                    heads.Add(head + " TextMeshProUGUI:0");
                    msgctxt.Add(textMeshPro_ugui[i].name);
                    msgid.Add(text);
                }
            }
            for (int i = 0; i < msgid.Count; i++)
            {
                textList.Add("#: " + heads[i]);
                textList.Add("msgctxt \"" + msgctxt[i] + "\"");
                textList.Add("msgid \"" + msgid[i] + "\"");
                textList.Add("msgstr \"" + (original ? msgid[i] : "") + "\"");
                textList.Add("");
            }
            string path = EditorUtility.SaveFilePanel(isChinese ? "导出文本" : "Export text", "", obj.name + "_" + (original ? Language(originalLanguage) : Language(translateLanguage)), "po.txt");
            if (path != "")
            {
                System.IO.File.WriteAllLines(path, textList.ToArray());
                Debug.Log(isChinese ? "导出成功" : "Export success");
            }
        }
        // http://docs.translatehouse.org/projects/localization-guide/en/latest/l10n/pluralforms.html
        string Plural_Forms(string language)
        {
            switch (language)
            {
                case "Simplified Chinese":
                case "Japanese":
                case "Korean":
                case "Aymará":
                case "Tibetan":
                case "Chiga":
                case "Dzongkha":
                case "Indonesian":
                case "Lojban":
                case "Georgian":
                case "Khmer":
                case "Lao":
                case "Malay":
                case "Burmese":
                case "Yakut":
                case "Sundanese":
                case "Thai":
                case "Tatar":
                case "Uyghur":
                case "Vietnamese":
                case "Wolof":
                case "Chinese":
                case "Traditional Chinese":
                    return "nplurals=1; plural=0;";
                case "Acholi":
                case "Akan":
                case "Amharic":
                case "Mapudungun":
                case "Breton":
                case "Persian":
                case "Filipino":
                case "French":
                case "Gun":
                case "Lingala":
                case "Mauritian Creole":
                case "Malagasy":
                case "Maori":
                case "Occitan":
                case "Brazilian Portuguese":
                case "Tajik":
                case "Tigrinya":
                case "Turkish":
                case "Uzbek":
                case "Walloon":
                    return "nplurals=2; plural=(n > 1);";
                case "English":
                case "Afrikaans":
                case "Aragonese":
                case "Angika":
                case "Assamese":
                case "Asturian":
                case "Azerbaijani":
                case "Bulgarian":
                case "Bengali":
                case "Bodo":
                case "Catalan":
                case "Danish":
                case "German":
                case "Dogri":
                case "Greek":
                case "Esperanto":
                case "Spanish":
                case "Argentinean Spanish":
                case "Estonian":
                case "Basque":
                case "Fulah":
                case "Finnish":
                case "Faroese":
                case "Friulian":
                case "Frisian":
                case "Galician":
                case "Gujarati":
                case "Hausa":
                case "Hebrew":
                case "Hindi":
                case "Chhattisgarhi":
                case "Hungarian":
                case "Armenian":
                case "Interlingua":
                case "Italian":
                case "Kazakh":
                case "Greenlandic":
                case "Kannada":
                case "Kurdish":
                case "Kyrgyz":
                case "Letzeburgesch":
                case "Maithili":
                case "Malayalam":
                case "Mongolian":
                case "Manipuri":
                case "Marathi":
                case "Nahuatl":
                case "Neapolitan":
                case "Norwegian Bokmal":
                case "Nepali":
                case "Dutch":
                case "Norwegian Nynorsk":
                case "Northern Sotho":
                case "Oriya":
                case "Punjabi":
                case "Papiamento":
                case "Piemontese":
                case "Pashto":
                case "Portuguese":
                case "Romansh":
                case "Kinyarwanda":
                case "Santali":
                case "Scots":
                case "Sindhi":
                case "Northern Sami":
                case "Sinhala":
                case "Somali":
                case "Songhay":
                case "Albanian":
                case "Swedish":
                case "Swahili":
                case "Tamil":
                case "Telugu":
                case "Turkmen":
                case "Urdu":
                case "Yoruba":
                    return "nplurals=2; plural=(n != 1);";
                case "Russian":
                case "Ukrainian":
                case "Belarusian":
                case "Bosnian":
                case "Croatian":
                case "Serbian":
                    return "nplurals=3; plural=(n%10==1 && n%100!=11 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2);";
                case "Arabic":
                    return "nplurals=6; plural=(n==0 ? 0 : n==1 ? 1 : n==2 ? 2 : n%100>=3 && n%100<=10 ? 3 : n%100>=11 ? 4 : 5);";
                case "Czech":
                case "Slovak":
                    return "nplurals=3; plural=(n==1) ? 0 : (n>=2 && n<=4) ? 1 : 2;";
                case "Kashubian":
                    return "nplurals=3; plural=(n==1) ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2;";
                case "Welsh":
                    return "nplurals=4; plural=(n==1) ? 0 : (n==2) ? 1 : (n != 8 && n != 11) ? 2 : 3;";
                case "Irish":
                    return "nplurals=5; plural=n==1 ? 0 : n==2 ? 1 : (n>2 && n<7) ? 2 :(n>6 && n<11) ? 3 : 4;";
                case "Scottish Gaelic":
                    return "nplurals=4; plural=(n==1 || n==11) ? 0 : (n==2 || n==12) ? 1 : (n > 2 && n < 20) ? 2 : 3;";
                case "Icelandic":
                    return "nplurals=2; plural=(n%10!=1 || n%100==11);";
                case "Javanese":
                    return "nplurals=2; plural=(n != 0);";
                case "Cornish":
                    return "nplurals=4; plural=(n==1) ? 0 : (n==2) ? 1 : (n == 3) ? 2 : 3;";
                case "Lithuanian":
                    return "nplurals=3; plural=(n%10==1 && n%100!=11 ? 0 : n%10>=2 && (n%100<10 || n%100>=20) ? 1 : 2);";
                case "Latvian":
                    return "nplurals=3; plural=(n%10==1 && n%100!=11 ? 0 : n != 0 ? 1 : 2);";
                case "Montenegro":
                    return "nplurals=3; plural=n%10==1 && n%100!=11 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2;";
                case "Macedonian":
                    return "nplurals=2; plural= n==1 || n%10==1 ? 0 : 1;";
                case "Mandinka":
                    return "nplurals=3; plural=(n==0 ? 0 : n==1 ? 1 : 2);";
                case "Maltese":
                    return "nplurals=4; plural=(n==1 ? 0 : n==0 || ( n%100>1 && n%100<11) ? 1 : (n%100>10 && n%100<20 ) ? 2 : 3);";
                case "Polish":
                    return "nplurals=3; plural=(n==1 ? 0 : n%10>=2 && n%10<=4 && (n%100<10 || n%100>=20) ? 1 : 2);";
                case "Romanian":
                    return "nplurals=3; plural=(n==1 ? 0 : (n==0 || (n%100 > 0 && n%100 < 20)) ? 1 : 2);";
                case "Slovenian":
                    return "nplurals=4; plural=(n%100==1 ? 0 : n%100==2 ? 1 : n%100==3 || n%100==4 ? 2 : 3);";
            }
            return "";
        }
        string Language(string language)
        {
            switch (language)
            {
                case "English":
                    return "en_US";
                case "Chinese":
                case "Simplified Chinese":
                    return "zh_CN";
                case "Traditional Chinese":
                    return "zh_TW";
                case "Japanese":
                    return "ja_JP";
                case "Korean":
                    return "ko_KR";
                case "French":
                    return "fr_FR";
                case "German":
                    return "de_DE";
                case "Italian":
                    return "it_IT";
                case "Spanish":
                    return "es_ES";
                case "Portuguese":
                    return "pt_PT";
                case "Russian":
                    return "ru_RU";
            }
            return "";
        }
    }
}