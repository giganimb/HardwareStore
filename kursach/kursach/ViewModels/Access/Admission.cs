using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace kursach.ViewModels.Access
{
    public class Admission
    {
        public static void GiveAccess(string answer)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("Accsess.txt", FileMode.Create)))
            {
                writer.Write(answer);
            }
        }


        public static bool GetAccess()
        {
            using (BinaryReader reader = new BinaryReader(File.Open("Accsess.txt", FileMode.Open)))
            {
                string temp = reader.ReadString();
                if (temp == "YES")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
