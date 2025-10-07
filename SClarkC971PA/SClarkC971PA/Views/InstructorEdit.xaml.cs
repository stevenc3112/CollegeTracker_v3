using SClarkC971PA.Models;
using SClarkC971PA.Services;
using System.Threading.Tasks;

namespace SClarkC971PA.Views;

public partial class InstructorEdit : ContentPage
{
    private Instructor _instructor = new Instructor();
    private bool _isEditing = false;
    private int _associatedCourseId = 0;
	public InstructorEdit(int associatedCourseId, Instructor instructor)
	{
		InitializeComponent();
        _associatedCourseId = associatedCourseId;
        _instructor = instructor;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _instructor = await DatabaseService.LookupInstructor(_associatedCourseId);

        if (_instructor != null)
        {
            _isEditing = true;

            InstructorNameEntry.Text = _instructor.InstructorName;

            InstructorPhoneEntry.Text = _instructor.InstructorPhone;
            InstructorPhoneEntry.IsVisible = true;

            InstructorEmailEntry.Text = _instructor.InstructorEmail;
            InstructorEmailEntry.IsVisible = true;
        }
        else
        {
            _instructor = new Instructor();
        }
    }
    private async void SaveInstructorBtn_Clicked(object sender, EventArgs e)
    {
        _instructor.AssociatedCourseId = _associatedCourseId;
        _instructor.InstructorName = InstructorNameEntry.Text;
        _instructor.InstructorPhone = InstructorPhoneEntry.Text;
        _instructor.InstructorEmail = InstructorEmailEntry.Text;

        if (_isEditing)
        {
            await DatabaseService.UpdateInstructor(_instructor);
        }
        else
        {
            await DatabaseService.AddInstructor(_instructor);
        }
        await Navigation.PopAsync();
    }

    private async void DeleteAllBtn_Clicked(object sender, EventArgs e)
    {
        if (_isEditing)
        {
            await DatabaseService.RemoveInstructor(_instructor.InstructorId);
            await Navigation.PopAsync();
        }
        else
        {
            await Navigation.PopAsync();
        }
    }
}