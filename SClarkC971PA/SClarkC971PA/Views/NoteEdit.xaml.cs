using System.Threading.Tasks;
using SClarkC971PA.Models;
using SClarkC971PA.Services;

namespace SClarkC971PA.Views;

public partial class NoteEdit : ContentPage
{
    private int _associatedCourseId;
    private int _currentNoteId;
    private Note _note = new Note();
    private bool _isEditingNote = false;
    public NoteEdit(int associatedCourseId)
    {
        InitializeComponent();
        _associatedCourseId = associatedCourseId;
        AddEditNotesTitle.Text = "Add Note";
    }
    public NoteEdit(int associatedCourseId, Note note)
	{
		InitializeComponent();
        _note = note;
        _currentNoteId = note.NoteId;
		_associatedCourseId = associatedCourseId;
        NoteTitleEntry.Text = note.NoteTitle;
        NoteBodyEditor.Text = note.NoteBody;
        AddEditNotesTitle.Text = "Edit Note";
        DeleteNoteBtn.IsVisible = true;
        _isEditingNote = true;
    }

    private async void CancelNoteBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void SaveNoteBtn_Clicked(object sender, EventArgs e)
    {

        if (!String.IsNullOrEmpty(NoteTitleEntry.Text) && !String.IsNullOrEmpty(NoteBodyEditor.Text))
        {
            _note.AssociatedCourseId = _associatedCourseId;
            _note.NoteTitle = NoteTitleEntry.Text;
            _note.NoteBody = NoteBodyEditor.Text;
            if (_isEditingNote)
            {
                await DatabaseService.UpdateNote(_note);
            }
            else
            {
                await DatabaseService.AddNote(_note);
            }
            await Navigation.PopAsync();

        }
        else 
        {
            NoteErrorLbl.Text = "Note entry cannot contain empty values.";
            NoteErrorLbl.IsVisible = true;
        }
    }
    private async void ShareNoteBtn_Clicked(object sender, EventArgs e)
    {
        var text = NoteBodyEditor.Text;
        await Share.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = NoteTitleEntry.Text
        });
    }

    private async void DeleteNoteBtn_Clicked(object sender, EventArgs e)
    {
        await DatabaseService.RemoveNote(_currentNoteId);
        await Navigation.PopAsync();
    }
}