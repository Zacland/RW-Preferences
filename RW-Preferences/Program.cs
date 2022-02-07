using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW_Preferences
{
    class Program
    {
        
        static void Main(string[] args)
        {
            WriteConfig();

        // là le fichier existe même s'il n'existait pas à l'origine

            ReadConfig();

            Console.WriteLine("\nVérifier le fichier : %appdata%\\MonAppli\\Preferences.xml\n\n:3");

            Console.ReadLine();
        }

        private static void ReadConfig()
        {
            // et maintenant on lit le contenu !
            Console.WriteLine("On lite le contenu ! \n");
            Console.WriteLine($"Server URL : {Globals.rwpref.ServerUrl}");
            Console.WriteLine($"UserLogin  : {Globals.rwpref.UserLogin}");
            Console.WriteLine($"User Pwd   : {Globals.rwpref.UserPassword}");
            Console.WriteLine($"WebAuthTok : {Globals.rwpref.WebAuthToken}");
        }

        private static void WriteConfig()
        {
            // Assignation dans le fichier %appdata%\MonAppli\Preferences.xml
            Console.WriteLine("Ecriture des données");
            Globals.rwpref.ServerUrl = "www.toto.com";
            Globals.rwpref.UserLogin = "Titi";
            Globals.rwpref.UserPassword = "Mon Password Codé !";
            Globals.rwpref.WebAuthToken = "123456789";
            Console.WriteLine("Ecriture des données finie !");
        }
    }
}
