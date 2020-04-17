using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

namespace NScript.Helper
{
   public class RichHighlighter
    {
    Hashtable Keywords = new Hashtable();
    public void KeywordsAdd(string key)
    {
            Keywords.Add(key, "1");
    }
    /// <summary>  
    /// C#语法高亮着色器  
    /// </summary>  
    /// <param name="start">起始行号</param>  
    public void RichHighlight(int start,RichTextBox richTextBox1)
    {
        //richTextBox1.Text = textBox4.Text;  
        string[] ln = richTextBox1.Text.Split('\n');
        int pos = 0;
        int lnum = 0;
        foreach (string lv in ln)
        {
            if (lnum >= start)
            {
                string ts = lv.Replace("(", " ").Replace(")", " ");
                ts = ts.Replace("[", " ").Replace("]", " ");
                ts = ts.Replace("{", " ").Replace("}", " ");
                ts = ts.Replace(".", " ").Replace("=", " ").Replace(";", " ");
                if (lv.Trim().StartsWith("'"))
                {
                    richTextBox1.Select(pos, lv.Length);
                    richTextBox1.SelectionFont = new Font("宋体", 9, (FontStyle.Regular));
                    richTextBox1.SelectionColor = Color.Gray;
                    pos += lv.Length + 1;
                    continue;
                }
                if (lv.Trim().StartsWith("#"))
                {
                    richTextBox1.Select(pos, lv.Length);
                    richTextBox1.SelectionFont = new Font("宋体", 9, (FontStyle.Regular));
                    richTextBox1.SelectionColor = Color.Green;
                    pos += lv.Length + 1;
                    continue;
                }
                ArrayList marks = new ArrayList();
                string smark = "";
                string last = "";
                bool inmark = false;
                for (int i = 0; i < ts.Length; i++)
                {
                    if (ts.Substring(i, 1) == "\"" && last != "\\")
                    {
                        if (inmark)
                        {
                            marks.Add(smark + "," + i);
                            smark = "";
                            inmark = false;
                        }
                        else
                        {
                            smark += i;
                            inmark = true;
                        }
                    }
                    last = ts.Substring(i, 1);
                }
                if (inmark)
                {
                    marks.Add(smark + "," + ts.Length);
                }
                string[] ta = ts.Split(' ');
                int x = 0;
                foreach (string tv in ta)
                {
                    if (tv.Length < 2)
                    {
                        x += tv.Length + 1;
                        continue;
                    }
                    else
                    {
                        bool find = false;
                        foreach (string px in marks)
                        {
                            string[] pa = px.Split(',');
                            if (x >= Int32.Parse(pa[0]) && x < Int32.Parse(pa[1]))
                            {
                                find = true;
                                break;
                            }
                        }
                        if (!find)
                        {
                            if (Keywords[tv] != null)
                            {
                                richTextBox1.Select(pos + x, tv.Length);
                                richTextBox1.SelectionFont = new Font("宋体", 9, (FontStyle.Regular));
                                richTextBox1.SelectionColor = Color.Blue;
                            }
                        }
                        x += tv.Length + 1;
                    }
                }
                foreach (string px in marks)
                {
                    string[] pa = px.Split(',');
                    richTextBox1.Select(pos + Int32.Parse(pa[0]), Int32.Parse(pa[1]) - Int32.Parse(pa[0]) + 1);
                    richTextBox1.SelectionFont = new Font("宋体", 9, (FontStyle.Regular));
                    richTextBox1.SelectionColor = Color.DarkRed;
                }
            }
            pos += lv.Length + 1;
            lnum++;
        }
    }
}
    public abstract class friend
    { 
    
    }
    public abstract class girlfriend : friend
    {

    }
    public sealed class mygirlfriend : girlfriend
    {

        public void DoWork()
        {
            return;
        }
    }

}