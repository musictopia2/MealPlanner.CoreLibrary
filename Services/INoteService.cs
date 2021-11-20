namespace MealPlanner.CoreLibrary.Services;
public interface INoteService
{
    Task<BasicList<NoteModel>> GetNotesAsync();
    Task SaveNotesAsync(BasicList<NoteModel> notes);
}