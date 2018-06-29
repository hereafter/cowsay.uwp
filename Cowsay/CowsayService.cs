using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace Cowsay
{
    public static class KnownEyes
    {
        public const string Borg = "==";
        public const string Dead = "xx";
        public const string Greedy = "$$";
        public const string Paranoid = "@@";
        public const string Stoned = "**";
        public const string Tired = "--";
        public const string Wired = "OO";
        public const string Young = "..";
    }

    public static class KnownTougues
    {
        public const string Dead = "U ";
        public const string Stoned = "U ";
    }

    public class CowsayService
    {
        public static CowsayService Default { get; set; }

        static CowsayService()
        {
            var args=Environment.GetCommandLineArgs();
            Default = new CowsayService(args);
        }

        public CowsayService(string[] args=null)
        {    
        }

        public string Eyes { get; set; } = "oo";
        public string FileName { get; set; } = "default.cow";
        public string Tongue { get; set; } = "  ";
        public string Thoughts { get; set; } = "\\";
        public int LineCharacterCapacity { get; set; } = 40;
        public string Message { get; set; }

        public async Task<string> SayAsync(string message=null)
        {
            if (message != null) this.Message = message;
            message = this.Message;
            var lines = this.GetMessageLines(message);
            var text = string.Empty;

            text += this.CreateBalloon(lines);

            var template = await this.LoadTemplateAsync(this.FileName);
            text += this.FillTemplate(template);
            return text;
        }

        private int GetMaxLineLength(string[] lines)
        {
            return lines.Max(s => s.Length);
        }

        private string[] GetMessageLines(string message)
        {
            if (string.IsNullOrEmpty(message)) return new string[] { };

            var list = new List<string>();
            var lines = message.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            foreach(var l in lines)
            {
                var tmp = l;
                while (tmp.Length > LineCharacterCapacity)
                {
                    list.Add(tmp.Substring(0, LineCharacterCapacity));
                    tmp = tmp.Substring(LineCharacterCapacity);
                }
                list.Add(tmp);
            }

            return list.ToArray();
        }

        
        private string CreateBalloon(string[] lines)
        {
            var sb = new StringBuilder();
            var maxLineLength = this.GetMaxLineLength(lines);
            var maxLineLength2 = maxLineLength+2;

            var specials = new string[] {
                " ",    //0
                "-",    //1
                "/",    //2
                "\\",   //3
                "|",    //4
                "<",    //5
                ">",    //6
            };

            void fillBorder()
            {
                sb.Append(specials[0]);
                for (int i = 0; i < maxLineLength2; i++) sb.Append(specials[1]);
                sb.AppendLine(specials[0]);
            }


            fillBorder();
            var size = lines.Length;

            for (int i = 0; i < size; i++)
            {
                if (size < 2)
                {
                    sb.Append(specials[5]);
                }
                else
                {
                    if (i == 0)
                    {
                        sb.Append(specials[2]);
                    }
                    else if (i == lines.Length - 1)
                    {
                        sb.Append(specials[3]);
                    }
                    else
                    {
                        sb.Append(specials[4]);
                    }
                }
                sb.Append(specials[0]);
                var line = lines[i];
                sb.Append(line);

                var padding = maxLineLength - line.Length;
                for(int t=0; t<padding; t++) sb.Append(specials[0]);

                sb.Append(specials[0]);
                if (size < 2)
                {
                    sb.Append(specials[6]);
                }
                else
                {
                    if (i == 0)
                    {
                        sb.Append(specials[3]);
                    }
                    else if (i == lines.Length - 1)
                    {
                        sb.Append(specials[2]);
                    }
                    else
                    {
                        sb.Append(specials[4]);
                    }
                    
                }
                sb.AppendLine();
            }

            fillBorder();
            return sb.ToString();
        }


        private async Task<string> LoadTemplateAsync(string fileName)
        {
            if(!fileName.EndsWith(".cow", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".cow";
            }

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Assets\\cows", fileName);

            var template=await File.ReadAllTextAsync(filePath);

            var tag = "$the_cow";
            var index = template.IndexOf(tag);
            if(index>=0)
            {
                tag = "\n";
                index = template.IndexOf(tag, index);
                if (index >= 0) template = template.Substring(index+tag.Length);
            }
            
            index=template.LastIndexOf("EOC");
            if (index >= 0) template = template.Substring(0, index);

            return template;
        }

        private string FillTemplate(string template)
        {
            template = template.Replace("$eyes", this.Eyes);
            template = template.Replace("$tongue", this.Tongue);
            template = template.Replace("$thoughts", this.Thoughts);
            return template;
        }
    }
}
