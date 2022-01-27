using System;
using System.Collections.Generic;
using System.Text;

namespace kursach.Information
{
    public sealed class InformationAboutApp
    {
        private string Version;
        private string Creator;
        private string ReleaseDate;
        private string Contact;

        private static InformationAboutApp Instance;

        public InformationAboutApp()
        {
            Version = "1.0";
            Creator = "giganimb";
            ReleaseDate = "2021-05-24";
            Contact = "giganimb@gmail.com";
        }

        public static InformationAboutApp GetInstance()
        {
            return Instance ?? (Instance = new InformationAboutApp());
        }
        public override string ToString()
        {
            return $" Version: {Version}\n Creator: {Creator}\n ReleaseDate: {ReleaseDate}\n Contact: {Contact}";
        }
    }
}
