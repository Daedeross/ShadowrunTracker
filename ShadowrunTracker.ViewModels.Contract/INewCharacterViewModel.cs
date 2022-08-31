namespace ShadowrunTracker.ViewModels
{
    public interface INewCharacterViewModel : IReusableModalViewModel<ICharacterViewModel?>
    {
        string Alias { get; set; }
        bool IsPlayer { get; set; }
        string Player { get; set; }

        int BaseEdge { get; set; }

        int BaseBody { get; set; }
        int BaseAgility { get; set; }
        int BaseReaction { get; set; }
        int BaseStrength { get; set; }
        int BaseCharisma { get; set; }
        int BaseIntuition { get; set; }
        int BaseLogic { get; set; }
        int BaseWillpower { get; set; }

        bool PainEditor { get; set; }
        int PainResistence { get; set; }

        IReadOnlyCollection<IInitiativeScoreViewModel> Initiatives { get; }
    }
}
