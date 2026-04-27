![WinForms](https://img.shields.io/badge/WinForms-C%23-blue)
![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-purple)
![Statut](https://img.shields.io/badge/Statut-Fonctionnel-green)

> Jeu de cartes Memory sur le thème de la cybersécurité, développé pour le stand SecurIT au **Salon de l'Innovation Tech**.

---

## 👥 Auteurs

| Nom | Rôle |
|-----|------|
| Souleymane Coulibaly | Logique métier, architecture, timers |
| Benor Henry DA | Interface WinForms, grille, menu |

---

## 🎯 Contexte

SecurIT est une jeune startup spécialisée en cybersécurité. L'équipe marketing a besoin d'un mini-jeu interactif pour attirer les visiteurs sur leur stand au Salon de l'Innovation Tech. Ce jeu Memory met en scène des icônes de cybersécurité pour captiver les visiteurs et tester leur mémoire tout en démontrant l'expertise technique de SecurIT.

---

## ✅ Fonctionnalités implémentées

### Fonctionnalités de base
- Menu principal avec 3 boutons : Jouer, Options, Quitter
- Grille de jeu dynamique 4×4 (8 paires) et 6×6 (18 paires)
- Classe `Carte` avec encapsulation stricte (POO)
- Mélange aléatoire Fisher-Yates au démarrage
- Retournement de cartes (maximum 2 simultanément)
- Timer de délai 1,5 seconde pour retourner les non-paires
- Blocage des clics pendant le délai (anti-corruption de logique)
- Chronomètre en temps réel
- Compteur d'essais
- Détection de victoire avec affichage du temps et des essais
- Images thématiques cybersécurité pour les faces
- Dos de carte uniforme SecurIT

### Options
- Choix de la taille de grille (4×4 / 6×6)
- Choix du thème (Cybersécurité, Matériel, Logiciel)

---

## 🏗️ Architecture technique

Le projet suit une **architecture en 3 couches** avec séparation stricte des responsabilités :
SecurITMemory/
│
├── Models/                    ← Couche métier (aucune dépendance UI)
│   ├── EtatCarte.cs          ← Énumération : Cachee, Revelee, Trouvee
│   ├── Carte.cs              ← Classe carte avec encapsulation stricte
│   └── JeuMemory.cs          ← Gestionnaire de jeu, timers, événements
│
├── Forms/                     ← Couche interface WinForms
│   ├── MenuPrincipal.cs      ← Menu de démarrage
│   ├── FormulaireJeu.cs      ← Grille de jeu dynamique
│   └── FormulaireOptions.cs  ← Paramètres de partie
│
├── Resources/                 ← Images PNG cybersécurité
│   ├── dos_carte.png
│   ├── cadenas.png
│   └── ...
│
└── Program.cs                 ← Point d'entrée

### Diagramme de classes simplifié
EtatCarte (enum)
└── Cachee | Revelee | Trouvee
Carte
├── ID : int
├── ImageFace : Image
├── ImageDos : Image
├── Etat : EtatCarte
├── Retourner()
├── Cacher()
├── MarquerTrouvee()
└── EstPaireAvec(Carte)
JeuMemory
├── Cartes : IReadOnlyList<Carte>
├── NbEssais : int
├── TempsEcoule : TimeSpan
├── InitialiserPartie()
├── CliquerCarte(Carte)
├── MelangerCartes()
└── EstVictoire()

---

## 🔄 Flux de jeu

1. **Mélange** — Fisher-Yates au démarrage de chaque partie
2. **Révélation** — Clic sur carte cachée → retournement
3. **Vérification** — Comparaison des IDs des 2 cartes retournées
4. **Délai** — 1,5 seconde si non-paire, clics bloqués pendant ce temps
5. **Victoire** — Toutes les paires trouvées → affichage du score

---

## 🖼️ Captures d'écran

### Menu Principal
![Menu Principal](screenshots/menu.png)

### Grille de jeu 4×4
![Grille 4x4](screenshots/grille_4x4.png)

### Grille de jeu 6×6
![Grille 6x6](screenshots/grille_6x6.png)

---

## 🚀 Installation et exécution

### Prérequis
- Windows 10/11
- Visual Studio 2022
- .NET Framework 4.7.2

### Étapes
1. Cloner le dépôt :
```bash
git clone https://github.com/coulou90/SecurIT-Memory.git
```
2. Ouvrir `SecurITMemory.sln` dans Visual Studio 2022
3. Compiler avec `Ctrl+Shift+B`
4. Lancer avec `F5`

---

## 🧠 Choix techniques

**Pourquoi `System.Timers.Timer` plutôt que `System.Windows.Forms.Timer` ?**
Pour respecter la séparation des couches — `JeuMemory` est une classe métier pure sans dépendance WinForms.

**Pourquoi une énumération pour les états ?**
Trois états mutuellement exclusifs avec des noms sémantiques. Un simple booléen ne peut pas représenter `Trouvee` sans ajouter un second booléen, ce qui crée de la complexité inutile.

**Pourquoi `IReadOnlyList<Carte>` plutôt que `List<Carte>` ?**
Pour empêcher la couche UI de modifier directement la collection de cartes et contourner la logique métier.

**Pourquoi un `Dictionary<Carte, PictureBox>` dans `FormulaireJeu` ?**
Pour que la classe `Carte` reste indépendante de WinForms — c'est `FormulaireJeu` qui possède la relation carte/affichage.

---

## 📋 Grille d'évaluation visée

| Critère | Points | Status |
|---------|--------|--------|
| Fonctionnalités de base | 3/3 | ✅ |
| Conception Orientée Objet | 3/3 | ✅ |
| Interface WinForms | 2/2 | ✅ |
| Qualité Code & Git | 2/2 | ✅ |
| **Total technique** | **10/10** | |
