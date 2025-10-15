using SClarkC971PA.Models;
using SClarkC971PA.Services;

namespace SClarkC971PA.Views;

public partial class TermCourseList : ContentPage
{
    private int _selectedTermId = 0;

    public TermCourseList(Term term)
    {
		InitializeComponent();
        _selectedTermId = term.TermId;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Term term = new Term();
        term = await Services.DatabaseService.LookupTerm(_selectedTermId);
        TermNameTitle.Text = term.TermTitle.ToString() + " Courses";
        TermStartLbl.Text = term.TermStartDate.ToString("d");
        TermEndLbl.Text = term.TermEndDate.ToString("d");
        CourseCollectionView.ItemsSource = await DatabaseService.GetCourses(_selectedTermId);
    }
    private  async void EditTermBtn_Clicked(object sender, EventArgs e)
    {
        Term term = new Term();
        term = await Services.DatabaseService.LookupTerm(_selectedTermId);
        await Navigation.PushAsync(new TermEdit(term));
    }
    private async void AddCourseBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CourseEdit(_selectedTermId));
    }
    private async void DeleteTermBtn_Clicked(object sender, EventArgs e)
    {
        await DatabaseService.RemoveTerm(_selectedTermId);
        //await Navigation.PopToRootAsync();
        await Navigation.PopAsync();
    }
    private async void CourseCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            Course course = (Course)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new CourseEdit(course));
        }
    }
}