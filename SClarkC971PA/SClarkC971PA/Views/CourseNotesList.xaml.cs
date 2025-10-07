using SClarkC971PA.Services;
using SClarkC971PA.Models;
namespace SClarkC971PA.Views;

public partial class CourseNotesList : ContentPage
{
    private int _associatedCourseId;

    public CourseNotesList(int associatedCourseId)
    {
        InitializeComponent();
        _associatedCourseId = associatedCourseId;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        NoteCollectionView.ItemsSource = await DatabaseService.GetNotes(_associatedCourseId);

    }

    private async void AddNoteBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NoteEdit(_associatedCourseId));
    }

    private async void NoteCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            Note note = (Note)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new NoteEdit(_associatedCourseId, note));
        }
    }
}