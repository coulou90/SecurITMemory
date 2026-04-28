// ============================================================
// FICHIER : Program.cs
// AUTEURS : Souleymane Coulibaly & Benor Henry DA
// PROJET  : SecurIT Memory - Salon de l'Innovation Tech
// DATE    : Avril 2026
// DESCRIPTION : Point d'entrée de l'application WinForms.
//               Configure le rendu visuel et lance le menu principal.
// ============================================================

using System;
using System.Windows.Forms;
using SecurITMemory.Forms;

namespace SecurITMemory
{
    /// <summary>
    /// Classe de démarrage de l'application SecurIT Memory.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// [STAThread] obligatoire pour les applications WinForms.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Active le rendu visuel moderne (styles Windows)
            Application.EnableVisualStyles();

            // Compatibilité du rendu texte avec les contrôles visuels
            Application.SetCompatibleTextRenderingDefault(false);

            // Lancement du menu principal comme fenêtre de démarrage
            Application.Run(new MenuPrincipal());
        }
    }
}