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
        public int LineCharacterCapacity { get; set; } = 40;

        public string Message { get; set; }


        public string Say(string message=null)
        {
            if (message != null) this.Message = message;
            message = this.Message;
            var lines = this.GetMessageLines(message);


            return string.Empty;
        }

        private string[] GetMessageLines(string message)
        {
            if (string.IsNullOrEmpty(message)) return new string[] { };

            var list = new List<string>();
            var lines = message.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

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

        private int GetMaxLineLength(string[] lines)
        {
            return lines.Max(s => s.Length);
        }

        private string CreateBalloon(string[] lines)
        {
            var maxLineLength = this.GetMaxLineLength(lines);
            var maxLineLength2 = maxLineLength + 2; //for border space fudge

            var thoughts = "o";
            

            return string.Empty;
        }


        private async Task<string> LoadTemplateAsync(string fileName)
        {
            if(!fileName.EndsWith(".cow", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".cow";
            }

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Assets\\cows", fileName);

            return await File.ReadAllTextAsync(filePath);

        }
    }
}
