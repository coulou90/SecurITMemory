// ============================================================
// FICHIER : MenuPrincipal.cs
// AUTEURS : Souleymane Coulibaly & Benor Henry DA
// PROJET  : SecurIT Memory - Salon de l'Innovation Tech
// DATE    : Avril 2026
// DESCRIPTION : Menu principal de l'application - Point d'entrée UI.
//               Permet de lancer une partie, configurer les options
//               ou quitter l'application.
//               Gère le cycle de vie : Menu -> Jeu -> Menu.
// ============================================================
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecurITMemory.Forms
{
    /// <summary>
    /// Menu principal de l'application SecurIT Memory.
    /// 
    /// Responsabilités :
    /// - Afficher les 3 boutons : Jouer, Options, Quitter
    /// - Stocker les paramètres de partie (taille grille, thème)
    /// - Ouvrir FormulaireJeu avec les paramètres choisis
    /// - Ouvrir FormulaireOptions et récupérer les choix
    /// - Se cacher pendant la partie et réapparaître après
    /// 
    /// Cycle de vie :
    /// MenuPrincipal -> (Hide) -> FormulaireJeu -> (FormClosed) -> (Show)
    /// </summary>
    public partial class MenuPrincipal : Form
    {
        // ==========================================
        // CHAMPS PRIVÉS
        // ==========================================
        private int _tailleGrille = 4;
        private string _theme = "Cybersecurite";

        // ==========================================
        // CONSTRUCTEUR
        // ==========================================
        public MenuPrincipal()
        {
            InitializeComponent();
            ConfigurerInterface();
        }

        // ==========================================
        // CONSTRUCTION DE L'INTERFACE
        // ==========================================
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }

        private void ConfigurerInterface()
        {
            this.Text = "SecurIT Memory - Salon de l'Innovation Tech";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 40);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Layout principal centré automatiquement
            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.RowCount = 5;
            layout.ColumnCount = 1;
            layout.Padding = new Padding(80, 40, 80, 40);
            layout.BackColor = Color.FromArgb(20, 20, 40);

            for (int i = 0; i < 5; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 5));

            // Titre
            Label lblTitre = new Label();
            lblTitre.Text = "🔒 SECURIT MEMORY";
            lblTitre.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            lblTitre.ForeColor = Color.Cyan;
            lblTitre.Dock = DockStyle.Fill;
            lblTitre.TextAlign = ContentAlignment.MiddleCenter;
            layout.Controls.Add(lblTitre, 0, 0);

            // Sous-titre
            Label lblSousTitre = new Label();
            lblSousTitre.Text = "Testez votre mémoire. Protégez vos données.";
            lblSousTitre.Font = new Font("Segoe UI", 11, FontStyle.Italic);
            lblSousTitre.ForeColor = Color.LightGray;
            lblSousTitre.Dock = DockStyle.Fill;
            lblSousTitre.TextAlign = ContentAlignment.MiddleCenter;
            layout.Controls.Add(lblSousTitre, 0, 1);

            // Boutons
            layout.Controls.Add(CreerBouton("▶  JOUER", Color.Cyan, BtnJouer_Click), 0, 2);
            layout.Controls.Add(CreerBouton("⚙  OPTIONS", Color.Orange, BtnOptions_Click), 0, 3);
            layout.Controls.Add(CreerBouton("✕  QUITTER",
                Color.LightCoral, (s, e) => Application.Exit()), 0, 4);

            this.Controls.Add(layout);
        }

        private Button CreerBouton(string texte, Color couleur, EventHandler handler)
        {
            Button btn = new Button();
            btn.Text = texte;
            btn.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            btn.BackColor = Color.FromArgb(40, 40, 60);
            btn.ForeColor = couleur;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = couleur;
            btn.FlatAppearance.BorderSize = 2;
            btn.Cursor = Cursors.Hand;
            btn.Dock = DockStyle.Fill;
            btn.Margin = new Padding(20, 8, 20, 8);

            btn.MouseEnter += (s, e) => {
                btn.BackColor = couleur;
                btn.ForeColor = Color.Black;
            };
            btn.MouseLeave += (s, e) => {
                btn.BackColor = Color.FromArgb(40, 40, 60);
                btn.ForeColor = couleur;
            };

            btn.Click += handler;
            return btn;
        }

        // ==========================================
        // HANDLERS
        // ==========================================
        private void BtnJouer_Click(object sender, EventArgs e)
        {
            FormulaireJeu jeu = new FormulaireJeu(_tailleGrille, _theme);
            this.Hide();
            jeu.FormClosed += (s, args) => this.Show();
            jeu.Show();
        }

        private void BtnOptions_Click(object sender, EventArgs e)
        {
            FormulaireOptions options = new FormulaireOptions(_tailleGrille, _theme);
            if (options.ShowDialog() == DialogResult.OK)
            {
                _tailleGrille = options.TailleGrille;
                _theme = options.Theme;
            }
        }
    }
}