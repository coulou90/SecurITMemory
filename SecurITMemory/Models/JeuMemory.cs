// ============================================================
// FICHIER : JeuMemory.cs
// AUTEURS : Souleymane Coulibaly & Benor Henry DA
// PROJET  : SecurIT Memory - Salon de l'Innovation Tech
// DATE    : Avril 2026
// DESCRIPTION : Gestionnaire principal du jeu Memory.
//               Couche métier pure - aucune dépendance WinForms.
//               Utilise System.Timers.Timer (pas WinForms.Timer)
//               pour respecter la séparation des couches.
//               Communique avec l'UI via des événements C#.
// ============================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Timers;

namespace SecurITMemory.Models
{
    /// <summary>
    /// Gestionnaire principal du jeu Memory - COUCHE MÉTIER PURE.
    /// 
    /// Responsabilités :
    /// - Initialiser et mélanger les cartes (algorithme Fisher-Yates)
    /// - Gérer les clics et la logique de vérification des paires
    /// - Déclencher le timer de délai (1.5s) pour les non-paires
    /// - Bloquer les clics pendant le délai (anti-corruption)
    /// - Gérer le chronomètre de jeu
    /// - Notifier l'UI via des événements (TempsChange, Victoire, etc.)
    /// 
    /// Ce que cette classe ne fait PAS :
    /// - Toucher aux contrôles WinForms (PictureBox, Label, etc.)
    /// - Connaître l'existence de FormulaireJeu
    /// </summary>
    public class JeuMemory
    {
        private List<Carte> _cartes;
        private List<Carte> _cartesRevelees;
        private Random _random;
        private int _nbPaires;
        private int _tailleGrille;
        private string _theme;

        private System.Timers.Timer _timerDelai;
        private System.Timers.Timer _timerChronometre;

        private int _nbEssais;
        private int _nbPairesTrouvees;
        private DateTime _debutPartie;
        private TimeSpan _tempsEcoule;

        private bool _jeuEnCours;
        private bool _attenteRetournement;

        public IReadOnlyList<Carte> Cartes { get { return _cartes.AsReadOnly(); } }
        public int NbEssais { get { return _nbEssais; } }
        public int NbPairesTrouvees { get { return _nbPairesTrouvees; } }
        public TimeSpan TempsEcoule { get { return _tempsEcoule; } }
        public bool JeuEnCours { get { return _jeuEnCours; } }
        public bool AttenteRetournement { get { return _attenteRetournement; } }
        public int TailleGrille { get { return _tailleGrille; } }

        public event EventHandler<TimeSpan> TempsChange;
        public event EventHandler<int> EssaisChange;
        public event EventHandler Victoire;
        public event EventHandler PaireTrouvee;
        public event EventHandler PaireNonTrouvee;

        public JeuMemory(int tailleGrille = 4, string theme = "Cybersecurite")
        {
            _tailleGrille = tailleGrille;
            _theme = theme;
            _random = new Random();
            _cartes = new List<Carte>();
            _cartesRevelees = new List<Carte>();
            _nbPaires = (tailleGrille * tailleGrille) / 2;
            InitialiserTimers();
        }

        private void InitialiserTimers()
        {
            _timerDelai = new System.Timers.Timer(1500);
            _timerDelai.AutoReset = false;
            _timerDelai.Elapsed += TimerDelai_Elapsed;

            _timerChronometre = new System.Timers.Timer(1000);
            _timerChronometre.AutoReset = true;
            _timerChronometre.Elapsed += TimerChronometre_Elapsed;
        }

        public void InitialiserPartie()
        {
            _timerDelai.Stop();
            _timerChronometre.Stop();

            _cartes.Clear();
            _cartesRevelees.Clear();
            _nbEssais = 0;
            _nbPairesTrouvees = 0;
            _tempsEcoule = TimeSpan.Zero;
            _jeuEnCours = true;
            _attenteRetournement = false;
            _debutPartie = DateTime.Now;

            List<Image> images = ChargerImagesTheme(_theme);
            Image dosCarte = ChargerDosCarte();

            for (int i = 0; i < _nbPaires; i++)
            {
                Image imageFace = images[i % images.Count];
                _cartes.Add(new Carte(i, imageFace, dosCarte));
                _cartes.Add(new Carte(i, imageFace, dosCarte));
            }

            MelangerCartes();
        }

        public void MelangerCartes()
        {
            int n = _cartes.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                Carte temp = _cartes[i];
                _cartes[i] = _cartes[j];
                _cartes[j] = temp;
            }
        }

        public bool CliquerCarte(Carte carte)
        {
            if (!_jeuEnCours || _attenteRetournement) return false;
            if (carte == null || carte.Etat != EtatCarte.Cachee) return false;

            carte.Retourner();
            _cartesRevelees.Add(carte);

            if (_cartesRevelees.Count == 2)
            {
                _nbEssais++;
                EssaisChange?.Invoke(this, _nbEssais);
                VerifierPaire();
            }

            return true;
        }

        private void VerifierPaire()
        {
            Carte carte1 = _cartesRevelees[0];
            Carte carte2 = _cartesRevelees[1];

            if (carte1.EstPaireAvec(carte2))
            {
                carte1.MarquerTrouvee();
                carte2.MarquerTrouvee();
                _nbPairesTrouvees++;
                _cartesRevelees.Clear();
                PaireTrouvee?.Invoke(this, EventArgs.Empty);

                if (EstVictoire())
                    TerminerPartie();
            }
            else
            {
                _attenteRetournement = true;
                PaireNonTrouvee?.Invoke(this, EventArgs.Empty);
                _timerDelai.Start();
            }
        }

        public bool EstVictoire()
        {
            return _nbPairesTrouvees >= _nbPaires;
        }

        private void TerminerPartie()
        {
            _jeuEnCours = false;
            _timerChronometre.Stop();
            Victoire?.Invoke(this, EventArgs.Empty);
        }

        private void TimerDelai_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var carte in _cartesRevelees)
                carte.Cacher();

            _cartesRevelees.Clear();
            _attenteRetournement = false;
        }

        private void TimerChronometre_Elapsed(object sender, ElapsedEventArgs e)
        {
            _tempsEcoule = DateTime.Now - _debutPartie;
            TempsChange?.Invoke(this, _tempsEcoule);
        }

        public void DemarrerChronometre()
        {
            _timerChronometre.Start();
        }

        public void ArreterPartie()
        {
            _jeuEnCours = false;
            _timerChronometre.Stop();
            _timerDelai.Stop();
        }

        private Image ChargerImageEnSecurite(string chemin, string fallbackTexte = null)
        {
            if (!File.Exists(chemin))
                return CreerImageDefaut(fallbackTexte ?? Path.GetFileNameWithoutExtension(chemin));
            try
            {
                using (var fs = new FileStream(chemin, FileMode.Open, FileAccess.Read))
                using (var img = Image.FromStream(fs))
                {
                    return new Bitmap(img);
                }
            }
            catch
            {
                return CreerImageDefaut(fallbackTexte ?? Path.GetFileNameWithoutExtension(chemin));
            }
        }

        private List<Image> ChargerImagesTheme(string theme)
        {
            List<Image> images = new List<Image>();
            string cheminResources = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Resources");

            Dictionary<string, string[]> themes = new Dictionary<string, string[]>
    {
        { "Cybersecurite", new[] { "cadenas", "parefeu", "virus", "antivirus",
            "cle", "phishing", "vpn", "bug", "terminal", "database",
            "cloud", "reseau", "biometrie", "2fa", "crypto" } },
        { "Materiel", new[] { "cpu", "ram", "ssd", "carte_mere", "gpu" } },
        { "Logiciel", new[] { "os", "ide", "navigateur", "antivirus", "firewall" } }
    };

            string[] nomsFichiers = themes.ContainsKey(theme)
                ? themes[theme]
                : themes["Cybersecurite"];

            foreach (string nom in nomsFichiers)
            {
                string chemin = Path.Combine(cheminResources, $"{nom}.png");
                images.Add(ChargerImageEnSecurite(chemin, nom));
            }

            return images;
        }

        private Image ChargerDosCarte()
        {
            string chemin = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Resources", "dos_carte.png");
            return ChargerImageEnSecurite(chemin, "dos_carte");
        }

        private Image CreerImageDefaut(string texte)
        {
            Bitmap bmp = new Bitmap(128, 128);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.DarkBlue);
                g.DrawString(texte, new Font("Arial", 12), Brushes.White, 10, 50);
            }
            return bmp;
        }
    }
}