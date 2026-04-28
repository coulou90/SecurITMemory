// ============================================================
// FICHIER : EtatCarte.cs
// AUTEURS : Souleymane Coulibaly & Benor Henry DA
// PROJET  : SecurIT Memory - Salon de l'Innovation Tech
// DATE    : Avril 2026
// DESCRIPTION : Énumération représentant les 3 états possibles
//               d'une carte dans le jeu Memory.
//               Utilisée par Carte.cs et JeuMemory.cs
// ============================================================

namespace SecurITMemory.Models
{
    /// <summary>
    /// Représente l'état actuel d'une carte dans le jeu Memory.
    /// Une carte ne peut être que dans UN SEUL état à la fois.
    /// </summary>
    public enum EtatCarte
    {
        /// <summary>
        /// La carte est face verso (dos visible).
        /// État initial de toutes les cartes au démarrage.
        /// </summary>
        Cachee,

        /// <summary>
        /// La carte est temporairement retournée face visible.
        /// Maximum 2 cartes peuvent être dans cet état simultanément.
        /// </summary>
        Revelee,

        /// <summary>
        /// La paire a été identifiée et validée.
        /// La carte reste visible définitivement et ne peut plus être cliquée.
        /// </summary>
        Trouvee
    }
}