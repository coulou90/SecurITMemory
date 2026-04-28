// ============================================================
// FICHIER : Carte.cs
// AUTEURS : Souleymane Coulibaly & Benor Henry DA
// PROJET  : SecurIT Memory - Salon de l'Innovation Tech
// DATE    : Avril 2026
// DESCRIPTION : Classe métier représentant une carte du jeu.
//               Encapsulation stricte - aucune dépendance WinForms.
//               La carte gère son état, pas son affichage.
// ============================================================

using System.Drawing;

namespace SecurITMemory.Models
{
    /// <summary>
    /// Représente une carte du jeu Memory.
    /// Principe POO : encapsulation stricte avec propriétés get/set contrôlés.
    /// Cette classe ne connaît pas l'interface graphique (pas de PictureBox).
    /// C'est FormulaireJeu qui gère l'affichage via un Dictionary Carte/PictureBox.
    /// </summary>
    public class Carte
    {
        // ==========================================
        // CHAMPS PRIVÉS (Encapsulation stricte)
        // Aucun champ public : respect du principe d'encapsulation POO
        // ==========================================
        private int _id;
        private Image _imageFace;
        private Image _imageDos;
        private EtatCarte _etat;

        // ==========================================
        // PROPRIÉTÉS PUBLIQUES
        // private set : seule la classe peut modifier ses propres données
        // ==========================================

        /// <summary>
        /// Identifiant de la paire. Deux cartes avec le même ID forment une paire valide.
        /// </summary>
        public int ID
        {
            get { return _id; }
            private set { _id = value; }
        }

        /// <summary>
        /// Image affichée quand la carte est retournée (icône cybersécurité).
        /// </summary>
        public Image ImageFace
        {
            get { return _imageFace; }
            private set { _imageFace = value; }
        }

        /// <summary>
        /// Image affichée quand la carte est cachée (dos uniforme SecurIT).
        /// </summary>
        public Image ImageDos
        {
            get { return _imageDos; }
            private set { _imageDos = value; }
        }

        /// <summary>
        /// État actuel de la carte : Cachee, Revelee ou Trouvee.
        /// Modifiable uniquement via les méthodes de comportement.
        /// </summary>
        public EtatCarte Etat
        {
            get { return _etat; }
            private set { _etat = value; }
        }

        // ==========================================
        // CONSTRUCTEUR
        // ==========================================

        /// <summary>
        /// Crée une nouvelle carte avec son identifiant et ses images.
        /// L'état initial est toujours Cachee.
        /// </summary>
        /// <param name="id">Identifiant de la paire (partagé avec la carte jumelle)</param>
        /// <param name="imageFace">Image de l'icône cybersécurité</param>
        /// <param name="imageDos">Image du dos uniforme</param>
        public Carte(int id, Image imageFace, Image imageDos)
        {
            ID = id;
            ImageFace = imageFace;
            ImageDos = imageDos;
            Etat = EtatCarte.Cachee; // Toutes les cartes commencent cachées
        }

        // ==========================================
        // MÉTHODES DE COMPORTEMENT
        // Chaque méthode vérifie l'état avant d'agir (transitions valides uniquement)
        // ==========================================

        /// <summary>
        /// Retourne la carte face visible.
        /// Transition valide : Cachee → Revelee uniquement.
        /// </summary>
        public void Retourner()
        {
            if (Etat == EtatCarte.Cachee)
                Etat = EtatCarte.Revelee;
        }

        /// <summary>
        /// Cache la carte face verso.
        /// Transition valide : Revelee → Cachee uniquement.
        /// Une carte Trouvee ne peut pas être recachée.
        /// </summary>
        public void Cacher()
        {
            if (Etat == EtatCarte.Revelee)
                Etat = EtatCarte.Cachee;
        }

        /// <summary>
        /// Marque la carte comme trouvée (paire validée).
        /// La carte reste visible définitivement.
        /// </summary>
        public void MarquerTrouvee()
        {
            Etat = EtatCarte.Trouvee;
        }

        /// <summary>
        /// Vérifie si cette carte forme une paire avec une autre carte.
        /// Deux conditions : même ID ET instances différentes (pas la même carte).
        /// </summary>
        /// <param name="autre">Carte à comparer</param>
        /// <returns>True si les deux cartes forment une paire valide</returns>
        public bool EstPaireAvec(Carte autre)
        {
            if (autre == null) return false;
            return this.ID == autre.ID && this != autre;
        }

        /// <summary>
        /// Réinitialise la carte pour une nouvelle partie.
        /// Remet l'état à Cachee.
        /// </summary>
        public void Reinitialiser()
        {
            Etat = EtatCarte.Cachee;
        }
    }
}