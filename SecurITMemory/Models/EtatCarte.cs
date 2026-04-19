namespace SecurITMemory.Models
{
    /// <summary>
    /// Énumération des états possibles d'une carte
    /// </summary>
    public enum EtatCarte
    {
        Cachee,     // Face verso (dos de carte visible)
        Revelee,    // Temporairement retournée (en cours de comparaison)
        Trouvee     // Paire identifiée, reste visible définitivement
    }
}