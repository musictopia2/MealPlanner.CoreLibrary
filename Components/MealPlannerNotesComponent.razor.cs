namespace MealPlanner.CoreLibrary.Components;
public partial class MealPlannerNotesComponent
{
    [Inject]
    private IToast? Toast { get; set; }
    [Inject]
    private INoteService? DataContext { get; set; }
    private readonly BasicList<string> _headers = new()
    {
        "Date",
        "Breakfast",
        "Lunch",
        "Dinner",
        "Desserts",
        "Snacks"
    };
    private BasicList<NoteModel>? _notes;
    private readonly string _columnWidth = "250px";
    protected override async Task OnInitializedAsync()
    {
        _notes = await DataContext!.GetNotesAsync();
        if (_notes is null)
        {
            throw new CustomBasicException("Failed to get notes.  Rethink");
        }
    }
    private async Task SaveNotesAsync()
    {
        await DataContext!.SaveNotesAsync(_notes!);
        Toast!.ShowSuccessToast("Saved Successfully");
    }
}