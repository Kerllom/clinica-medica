using System;
using System.Windows.Forms;
using ClinicaMedica.Forms;

namespace ClinicaMedica
{
    // =================================================================
    // CLASSE PRINCIPAL (MAIN) da aplicação.
    // É por aqui que o programa começa: abre a tela de Login.
    //
    // ATENÇÃO: este arquivo SUBSTITUI o Program.cs que o Visual Studio
    // gerou automaticamente (aquele que abria o "Form1").
    // =================================================================
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Configurações visuais padrão do Windows Forms.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicia o sistema pela tela de Login.
            Application.Run(new FormLogin());
        }
    }
}
