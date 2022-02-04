using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_Preferences
{
    class Program
    {
        private static RWPref rwpref = new RWPref();

        static void Main(string[] args)
        {
            // Assignation dans le fichier %appdata%\MonAppli\Preferences.xml
            Console.WriteLine("Ecriture des données");
            rwpref.ServerUrl = "www.toto.com";
            rwpref.UserLogin = "Titi";
            rwpref.UserPassword = "Mon Password Codé !";
            rwpref.WebAuthToken = "123456789";
            Console.WriteLine("Ecriture des données finie !");

            // là le fichier existe même s'il n'existait pas à l'origine

            // et maintenant on lit le contenu !
            Console.WriteLine("On lite le contenu ! \n");
            Console.WriteLine($"Server URL : {rwpref.ServerUrl}");
            Console.WriteLine($"UserLogin  : {rwpref.UserLogin}");
            Console.WriteLine($"User Pwd   : {rwpref.UserPassword}");
            Console.WriteLine($"WebAuthTok : {rwpref.WebAuthToken}");

            Console.WriteLine("\nVérifier le fichier : %appdata%\\MonAppli\\Preferences.xml\n\n:3");

            Console.ReadLine();
        }
    }
}
