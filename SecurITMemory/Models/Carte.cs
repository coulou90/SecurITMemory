using System.Drawing;
using SecurITMemory.Models;

namespace SecurITMemory.Models

{
    /// <summary>
    /// Représente une carte du jeu Memory - COUCHE MÉTIER PURE
    /// Aucune dépendance UI : la carte gère son état, pas son affichage
    /// </summary>
    public class Carte
    {
        // ==========================================
        // CHAMPS PRIVÉS (Encapsulation stricte)
        // ==========================================
        private int _id;
        private Image _imageFace;
        private Image _imageDos;
        private EtatCarte _etat;

        // ==========================================
        // PROPRIÉTÉS PUBLIQUES (get/set contrôlés)
        // ==========================================

        public int ID
        {
            get { return _id; }
            private set { _id = value; }
        }

        public Image ImageFace
        {
            get { return _imageFace; }
            private set { _imageFace = value; }
        }

        public Image ImageDos
        {
            get { return _imageDos; }
            private set { _imageDos = value; }
        }

        public EtatCarte Etat
        {
            get { return _etat; }
            private set { _etat = value; }
        }

        // ==========================================
        // CONSTRUCTEUR
        // ==========================================

        public Carte(int id, Image imageFace, Image imageDos)
        {
            ID = id;
            ImageFace = imageFace;
            ImageDos = imageDos;
            Etat = EtatCarte.Cachee;
        }

        // ==========================================
        // MÉTHODES DE COMPORTEMENT
        // ==========================================

        public void Retourner()
        {
            if (Etat == EtatCarte.Cachee)
                Etat = EtatCarte.Revelee;
        }

        public void Cacher()
        {
            if (Etat == EtatCarte.Revelee)
                Etat = EtatCarte.Cachee;
        }

        public void MarquerTrouvee()
        {
            Etat = EtatCarte.Trouvee;
        }

        public bool EstPaireAvec(Carte autre)
        {
            if (autre == null) return false;
            return this.ID == autre.ID && this != autre;
        }

        public void Reinitialiser()
        {
            Etat = EtatCarte.Cachee;
        }
    }
}