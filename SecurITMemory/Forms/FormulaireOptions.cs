// ============================================================
// FICHIER : FormulaireOptions.cs
// AUTEURS : Souleymane Coulibaly & Benor Henry DA
// PROJET  : SecurIT Memory - Salon de l'Innovation Tech
// DATE    : Avril 2026
// DESCRIPTION : Formulaire des options de partie.
//               Permet de choisir la taille de grille et le thème.
//               Retourne les choix via propriétés publiques
//               après confirmation avec DialogResult.OK.
// ============================================================
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SecurITMemory.Forms
{
    /// <summary>
    /// Formulaire des options - COUCHE INTERFACE.
    /// 
    /// Responsabilités :
    /// - Afficher les choix de taille de grille (4x4 / 6x6)
    /// - Afficher les choix de thème (Cybersécurité, Matériel, Logiciel)
    /// - Pré-sélectionner les valeurs actuelles au chargement
    /// - Retourner les nouveaux choix via TailleGrille et Theme
    /// 
    /// Utilisation depuis MenuPrincipal :
    /// FormulaireOptions options = new FormulaireOptions(tailleActuelle, themeActuel);
    /// if (options.ShowDialog() == DialogResult.OK)
    /// {
    ///     _tailleGrille = options.TailleGrille;
    ///     _theme = options.Theme;
    /// }
    /// 
    /// Sécurité :
    /// - Fallback sur index 0 si thème inconnu
    /// - Opérateur ?. pour éviter NullReferenceException
    /// </summary>
    public partial class FormulaireOptions : Form
    {
        // ==========================================
        // PROPRIÉTÉS PUBLIQUES
        // ==========================================
        public int TailleGrille { get; private set; }
        public string Theme { get; private set; }

        // ==========================================
        // CHAMPS PRIVÉS
        // ==========================================
        private RadioButton _rb4x4;
        private RadioButton _rb6x6;
        private ComboBox _cmbTheme;

        private readonly string[] _themesDisponibles =
        {
            "Cybersecurite",
            "Materiel",
            "Logiciel"
        };

        // ==========================================
        // CONSTRUCTEUR
        // ==========================================
        public FormulaireOptions(int tailleActuelle, string themeActuel)
        {
            TailleGrille = tailleActuelle;
            Theme = themeActuel;
            InitializeComponent();
        }

        // ==========================================
        // CONSTRUCTION DE L'INTERFACE
        // ==========================================
        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Options - SecurIT Memory";
            this.Size = new Size(400, 360);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(20, 20, 40);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Titre
            Label lblTitre = new Label();
            lblTitre.Text = "⚙ OPTIONS";
            lblTitre.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitre.ForeColor = Color.Cyan;
            lblTitre.Dock = DockStyle.Top;
            lblTitre.Height = 60;
            lblTitre.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitre);

            // Groupe Taille
            GroupBox grpTaille = new GroupBox();
            grpTaille.Text = "Taille de la grille";
            grpTaille.ForeColor = Color.White;
            grpTaille.Location = new Point(30, 80);
            grpTaille.Size = new Size(320, 100);
            this.Controls.Add(grpTaille);

            _rb4x4 = new RadioButton();
            _rb4x4.Text = "4 × 4  (8 paires — Facile)";
            _rb4x4.ForeColor = Color.White;
            _rb4x4.Location = new Point(20, 30);
            _rb4x4.AutoSize = true;
            _rb4x4.Checked = (TailleGrille == 4);
            grpTaille.Controls.Add(_rb4x4);

            _rb6x6 = new RadioButton();
            _rb6x6.Text = "6 × 6  (18 paires — Difficile)";
            _rb6x6.ForeColor = Color.White;
            _rb6x6.Location = new Point(20, 62);
            _rb6x6.AutoSize = true;
            _rb6x6.Checked = (TailleGrille == 6);
            grpTaille.Controls.Add(_rb6x6);

            // Groupe Thème
            GroupBox grpTheme = new GroupBox();
            grpTheme.Text = "Thème des cartes";
            grpTheme.ForeColor = Color.White;
            grpTheme.Location = new Point(30, 195);
            grpTheme.Size = new Size(320, 80);
            this.Controls.Add(grpTheme);

            _cmbTheme = new ComboBox();
            _cmbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbTheme.Items.AddRange(_themesDisponibles);
            _cmbTheme.Location = new Point(20, 35);
            _cmbTheme.Size = new Size(280, 25);

            if (_cmbTheme.Items.Contains(Theme))
                _cmbTheme.SelectedItem = Theme;
            else
                _cmbTheme.SelectedIndex = 0;

            grpTheme.Controls.Add(_cmbTheme);

            // Bouton OK
            Button btnOK = new Button();
            btnOK.Text = "✓  OK";
            btnOK.Location = new Point(80, 295);
            btnOK.Size = new Size(100, 38);
            btnOK.BackColor = Color.Cyan;
            btnOK.ForeColor = Color.Black;
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnOK.Cursor = Cursors.Hand;
            btnOK.Click += BtnOK_Click;
            this.Controls.Add(btnOK);

            // Bouton Annuler
            Button btnAnnuler = new Button();
            btnAnnuler.Text = "✕  Annuler";
            btnAnnuler.DialogResult = DialogResult.Cancel;
            btnAnnuler.Location = new Point(210, 295);
            btnAnnuler.Size = new Size(100, 38);
            btnAnnuler.BackColor = Color.FromArgb(40, 40, 60);
            btnAnnuler.ForeColor = Color.LightCoral;
            btnAnnuler.FlatStyle = FlatStyle.Flat;
            btnAnnuler.FlatAppearance.BorderColor = Color.LightCoral;
            btnAnnuler.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAnnuler.Cursor = Cursors.Hand;
            this.Controls.Add(btnAnnuler);

            this.AcceptButton = btnOK;
            this.CancelButton = btnAnnuler;

            this.ResumeLayout(false);
        }

        // ==========================================
        // HANDLER
        // ==========================================
        private void BtnOK_Click(object sender, EventArgs e)
        {
            TailleGrille = _rb4x4.Checked ? 4 : 6;
            Theme = _cmbTheme.SelectedItem?.ToString() ?? "Cybersecurite";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}