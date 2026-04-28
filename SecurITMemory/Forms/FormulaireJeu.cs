// ============================================================
// FICHIER : FormulaireJeu.cs
// AUTEURS : Souleymane Coulibaly & Benor Henry DA
// PROJET  : SecurIT Memory - Salon de l'Innovation Tech
// DATE    : Avril 2026
// DESCRIPTION : Formulaire principal du jeu - COUCHE UI.
//               Génère la grille dynamique de PictureBox.
//               Possède le mapping Carte<->PictureBox via Dictionary.
//               S'abonne aux événements de JeuMemory pour
//               mettre à jour l'affichage (pattern Observer).
//               Utilise Invoke() pour la sécurité cross-thread
//               car System.Timers.Timer s'exécute sur thread secondaire.
// ============================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SecurITMemory.Models;

namespace SecurITMemory.Forms
{
    /// <summary>
    /// Formulaire principal du jeu Memory - COUCHE INTERFACE.
    /// 
    /// Responsabilités :
    /// - Construire la grille dynamique de PictureBox
    /// - Maintenir le mapping Dictionary Carte/PictureBox
    /// - Mettre à jour l'affichage selon l'état des cartes
    /// - Gérer les clics utilisateur et les transmettre à JeuMemory
    /// - Afficher le chronomètre et le compteur d'essais
    /// - Afficher l'écran de victoire
    /// 
    /// Pattern utilisé : Observer
    /// FormulaireJeu s'abonne aux événements de JeuMemory
    /// et réagit aux changements d'état sans polling.
    /// 
    /// Sécurité cross-thread :
    /// Tous les handlers d'événements utilisent this.Invoke()
    /// car System.Timers.Timer s'exécute sur un thread secondaire.
    /// </summary>
    public partial class FormulaireJeu : Form
    {
        private JeuMemory _jeu;
        private int _tailleGrille;
        private string _theme;
        private TableLayoutPanel _grille;
        private Label _lblChronometre;
        private Label _lblEssais;
        private Panel _panelInfos;
        private Dictionary<Carte, PictureBox> _mapping;

        public FormulaireJeu(int tailleGrille, string theme)
        {
            _tailleGrille = tailleGrille;
            _theme = theme;
            _mapping = new Dictionary<Carte, PictureBox>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = $"SecurIT Memory - {_tailleGrille}x{_tailleGrille}";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(20, 20, 40);

            // Panel infos en haut
            _panelInfos = new Panel();
            _panelInfos.Dock = DockStyle.Top;
            _panelInfos.Height = 70;
            _panelInfos.BackColor = Color.FromArgb(30, 30, 50);

            _lblChronometre = new Label();
            _lblChronometre.Text = "⏱ 00:00";
            _lblChronometre.Font = new Font("Consolas", 18, FontStyle.Bold);
            _lblChronometre.ForeColor = Color.Cyan;
            _lblChronometre.Location = new Point(20, 15);
            _lblChronometre.AutoSize = true;
            _panelInfos.Controls.Add(_lblChronometre);

            _lblEssais = new Label();
            _lblEssais.Text = "🎯 Essais: 0";
            _lblEssais.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblEssais.ForeColor = Color.Orange;
            _lblEssais.Location = new Point(300, 18);
            _lblEssais.AutoSize = true;
            _panelInfos.Controls.Add(_lblEssais);

            Button btnAbandon = new Button();
            btnAbandon.Text = "Abandonner";
            btnAbandon.Size = new Size(130, 40);
            btnAbandon.Location = new Point(700, 15);
            btnAbandon.BackColor = Color.FromArgb(180, 40, 40);
            btnAbandon.ForeColor = Color.White;
            btnAbandon.FlatStyle = FlatStyle.Flat;
            btnAbandon.Cursor = Cursors.Hand;
            btnAbandon.Click += (s, e) => AbandonnerPartie();
            _panelInfos.Controls.Add(btnAbandon);

            // Grille
            _grille = new TableLayoutPanel();
            _grille.Dock = DockStyle.Fill;
            _grille.BackColor = Color.FromArgb(20, 20, 40);
            _grille.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            // ORDRE CRITIQUE : Fill en premier, Top ensuite
            this.Controls.Add(_grille);
            this.Controls.Add(_panelInfos);

            this.ResumeLayout(false);

            // Initialiser le jeu après que l'UI est construite
            this.Load += (s, e) => InitialiserJeu();
        }

        private void InitialiserJeu()
        {
            if (_jeu != null)
            {
                _jeu.ArreterPartie();
                _jeu = null;
            }

            _mapping.Clear();
            _jeu = new JeuMemory(_tailleGrille, _theme);

            _jeu.TempsChange += (s, temps) =>
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                    this.Invoke(new Action(() =>
                        _lblChronometre.Text = $"⏱ {temps:mm\\:ss}"));
            };

            _jeu.EssaisChange += (s, essais) =>
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                    this.Invoke(new Action(() =>
                        _lblEssais.Text = $"🎯 Essais: {essais}"));
            };

            _jeu.PaireTrouvee += (s, e) =>
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                    this.Invoke(new Action(() => MettreAJourToutesLesCartes()));
            };

            _jeu.PaireNonTrouvee += (s, e) =>
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                    this.Invoke(new Action(() => MettreAJourToutesLesCartes()));
            };

            _jeu.Victoire += (s, e) =>
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                    this.Invoke(new Action(() => AfficherVictoire()));
            };

            ConfigurerGrille();
            _jeu.InitialiserPartie();
            GenererCartes();
            _jeu.DemarrerChronometre();
        }

        private void ConfigurerGrille()
        {
            _grille.Controls.Clear();
            _grille.RowStyles.Clear();
            _grille.ColumnStyles.Clear();
            _grille.RowCount = _tailleGrille;
            _grille.ColumnCount = _tailleGrille;

            for (int i = 0; i < _tailleGrille; i++)
            {
                _grille.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / _tailleGrille));
                _grille.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / _tailleGrille));
            }
        }

        private void GenererCartes()
        {
            _grille.Controls.Clear();
            _mapping.Clear();
            _grille.SuspendLayout();

            int index = 0;
            for (int row = 0; row < _tailleGrille; row++)
            {
                for (int col = 0; col < _tailleGrille; col++)
                {
                    if (index >= _jeu.Cartes.Count) break;

                    Carte carte = _jeu.Cartes[index];

                    PictureBox pb = new PictureBox();
                    pb.Dock = DockStyle.Fill;
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Margin = new Padding(4);
                    pb.BackColor = Color.FromArgb(40, 40, 60);
                    pb.BorderStyle = BorderStyle.FixedSingle;
                    pb.Cursor = Cursors.Hand;
                    pb.Image = carte.ImageDos;

                    _mapping[carte] = pb;

                    Carte carteCapturee = carte;
                    pb.Click += (s, e) => OnCarteClick(carteCapturee);

                    _grille.Controls.Add(pb, col, row);
                    index++;
                }
            }

            _grille.ResumeLayout();
        }

        private void OnCarteClick(Carte carte)
        {
            _jeu.CliquerCarte(carte);
            MettreAJourAffichage(carte);
        }

        private void MettreAJourAffichage(Carte carte)
        {
            if (!_mapping.ContainsKey(carte)) return;

            PictureBox pb = _mapping[carte];

            switch (carte.Etat)
            {
                case EtatCarte.Cachee:
                    pb.Image = carte.ImageDos;
                    pb.BackColor = Color.FromArgb(40, 40, 60);
                    pb.Enabled = true;
                    pb.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case EtatCarte.Revelee:
                    pb.Image = carte.ImageFace;
                    pb.BackColor = Color.FromArgb(60, 60, 80);
                    break;

                case EtatCarte.Trouvee:
                    pb.Image = carte.ImageFace;
                    pb.BackColor = Color.FromArgb(20, 80, 40);
                    pb.Enabled = false;
                    pb.BorderStyle = BorderStyle.Fixed3D;
                    break;
            }
        }

        private void MettreAJourToutesLesCartes()
        {
            foreach (var carte in _jeu.Cartes)
                MettreAJourAffichage(carte);
        }

        private void AfficherVictoire()
        {
            _jeu.ArreterPartie();

            string message = $"🎉 FÉLICITATIONS ! 🎉\n\n" +
                             $"Temps : {_jeu.TempsEcoule:mm\\:ss}\n" +
                             $"Essais : {_jeu.NbEssais}\n\n" +
                             $"Voulez-vous rejouer ?";

            DialogResult result = MessageBox.Show(
                message, "Victoire !",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
                InitialiserJeu();
            else
                this.Close();
        }

        private void AbandonnerPartie()
        {
            _jeu.ArreterPartie();
            this.Close();
        }
    }
}