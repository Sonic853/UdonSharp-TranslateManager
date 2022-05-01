
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonLab
{
    /// <summary>
    /// 读取po文件，并用于翻译
    /// </summary>
    public class TranslatePo : UdonSharpBehaviour
    {
        [SerializeField] private TextAsset poFile;
        [TextArea(3, 10)]
        [HideInInspector] private string[] msgid;
        [TextArea(3, 10)]
        [HideInInspector] private string[] msgstr;
        [HideInInspector] public string language;
        // void Start()
        // {
        //     ReadPoFile();
        // }
        public void ReadPoFile()
        {
            if (poFile == null)
            {
                Debug.LogError("poFile is null");
                return;
            }
            msgid = new string[0];
            msgstr = new string[0];
            string[] lines = poFile.text.Split('\n');
            int msgidIndex = -1;
            int msgstrIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                // Debug.Log(line);
                // msgid \"你好，Lindinia\"
                if (line.StartsWith("msgid \""))
                {
                    // 将\\n替换为\n
                    string text = returnText(line.Substring(7, line.Length - 8));
                    msgid = stringArrayAdd(msgid, text);
                    msgidIndex = msgid.Length - 1;
                    msgstrIndex = -1;
                }
                else if (line.StartsWith("msgstr \""))
                {
                    string text = returnText(line.Substring(8, line.Length - 9));
                    msgstr = stringArrayAdd(msgstr, text);
                    msgstrIndex = msgstr.Length - 1;
                    msgidIndex = -1;
                }
                // "Language: zh_CN\n"
                // 找到符合"Language: 的行，然后获取语言，同时去除后面的换行
                else if (line.StartsWith("\"Language: ") && msgstrIndex == 0)
                {
                    language = line.Substring(11, line.Length - 12);
                    // 找到并去除\n
                    language = language.IndexOf("\\n") == -1 ? language : language.Substring(0, language.IndexOf("\\n"));
                }
                else if (line.StartsWith("\""))
                {
                    if (msgidIndex != -1 && msgidIndex != 0)
                    {
                        string text = returnText(line.Substring(1, line.Length - 2));
                        msgid[msgidIndex] += text;
                    }
                    else if (msgstrIndex != -1 && msgstrIndex != 0)
                    {
                        string text = returnText(line.Substring(1, line.Length - 2));
                        msgstr[msgstrIndex] += text;
                    }
                }
            }
            if (msgid.Length != msgstr.Length)
            {
                Debug.LogError("msgid.Length != msgstr.Length");
                return;
            }
            // for (int i = 0; i < msgid.Length; i++)
            // {
            //     Debug.Log(msgid[i] + "\n\n" + msgstr[i]);
            // }
        }
        string returnText(string text)
        {
            if (text.EndsWith("\""))
            {
                text = text.Substring(0, text.Length - 1);
            }
            if (text.EndsWith("\\n"))
            {
                text = text.Substring(0, text.Length - 2);
                text = text + "\n";
            }
            return text;
        }
        public string _getText(string text)
        {
            for (int i = 0; i < msgid.Length; i++)
            {
                if (msgid[i] == text)
                {
                    return msgstr[i];
                }
            }
            return text;
        }
        public string _getOriginalText(string text)
        {
            return text;
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
        string[] stringArrayAdd(string[] strings, string string_)
        {
            string[] newStrings = new string[strings.Length + 1];
            for (int i = 0; i < strings.Length; i++)
            {
                newStrings[i] = strings[i];
            }
            newStrings[strings.Length] = string_;
            return newStrings;
        }
        // string[] stringArrayRemove(string[] strings, string string_)
        // {
        //     string[] newStrings = new string[strings.Length - 1];
        //     int index = stringIndexOf(strings, string_);
        //     if (index == -1)
        //     {
        //         return strings;
        //     }
        //     int cou = 0;
        //     for (int i = 0; i < strings.Length; i++)
        //     {
        //         if(i == index)
        //         {
        //             cou++;
        //             continue;
        //         }
        //         newStrings[i - cou] = strings[i];
        //     }
        //     return newStrings;
        // }
    }
}
