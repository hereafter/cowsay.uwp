using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Default = new CowsayService();
        }

        public string Eyes { get; set; } = "oo";
        public string FileName { get; set; } = "default.cow";
        public string Tongue { get; set; } = "  ";
        public int LineCharactersCount { get; set; } = 40;

        public string Say(string message)
        {   
            return string.Empty;
        }

    }
}
