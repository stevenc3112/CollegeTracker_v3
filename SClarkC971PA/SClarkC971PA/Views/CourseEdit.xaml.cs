using Microsoft.Maui.Layouts;
using SClarkC971PA.Models;
using SClarkC971PA.Services;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SClarkC971PA.Views;

public partial class CourseEdit : ContentPage
{
    private Course _course = new Course();
    private Instructor _instructor = new Instructor();
    private bool _isEditing = false;
    //Constructor for Add View
    public CourseEdit(int associatedTermId)
	{
		InitializeComponent();
        EditAddCourseTitle.Text = "Add Course";
        _course.AssociatedTermId = associatedTermId;
        CourseNameEntry.IsEnabled = true;
        CourseStartDpkr.IsEnabled = true;
        CourseEndDpkr.IsEnabled = true;
        CourseEndDpkr.Date = DateTime.Today.AddMonths(1);
        CourseStatusPkr.IsEnabled = true;
        NotifyStartEndSwitch.IsEnabled = true;
        CancelBtn.IsVisible = true;
        SaveCourseBtn.IsVisible = true;
        InstructorNameLbl.Text = "You can add an instructor after saving a course.";
        SwitchToEditMode();
    }
    //Constructor for details and edit view
    public CourseEdit(Course course)
    {
        InitializeComponent();
        EditAddCourseTitle.Text = "Edit Course";
        _course = course;
        _isEditing = true;
        SwitchToViewMode();

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isEditing)
        {
            AssessmentCollectionView.ItemsSource = await DatabaseService.GetAssessments(_course.CourseId);
            CourseNotesBtn.IsVisible = true;
            CourseAssessmentsBtn.IsVisible = true;
            _instructor = await DatabaseService.LookupInstructor(_course.CourseId);
            if (_instructor != null) 
            {
                InstructorNameLbl.Text = _instructor.InstructorName;

                InstructorPhoneLbl.Text = _instructor.InstructorPhone;
                InstructorPhoneLbl.IsVisible = true;

                InstructorEmailLbl.Text = _instructor.InstructorEmail;
                InstructorEmailLbl.IsVisible = true;
            }
            else
            {
                InstructorNameLbl.Text = "Instructor info will display here";

                InstructorPhoneLbl.Text = "";
                InstructorPhoneLbl.IsVisible = false;

                InstructorEmailLbl.Text = "";
                InstructorEmailLbl.IsVisible = false;
            }
        }
    }

    private async void  CancelBtn_Clicked(object sender, EventArgs e)
    {
        SwitchToViewMode();
    }

    private void EditCourseBtn_Clicked(object sender, EventArgs e)
    {
        SwitchToEditMode();
    }

    private async void CourseInstructorBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InstructorEdit(_course.CourseId, _instructor));
    }

    private async void CourseNotesBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CourseNotesList(_course.CourseId));
    }

    private async void CourseAssessmentsBtn_Clicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new AssessmentEdit(_course.CourseId));
    }
    private async void AssessmentCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            Assessment assessment = (Assessment)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new AssessmentEdit(_course.CourseId, assessment));
        }
    }

    private async void SaveCourseBtn_Clicked(object sender, EventArgs e)
    {
            
        if (!String.IsNullOrEmpty(CourseNameEntry.Text)
             && CourseStartDpkr.Date != null
             && CourseEndDpkr.Date != null
             && CourseStatusPkr.SelectedItem != null
             && NotifyStartEndSwitch.IsToggled != null)
        {
            if (Validation.ValidateDates(CourseStartDpkr.Date, CourseEndDpkr.Date))
            {
                _course.CourseName = CourseNameEntry.Text;
                _course.CourseStartDate = CourseStartDpkr.Date;
                _course.CourseEndDate = CourseEndDpkr.Date;
                _course.CourseStatus = CourseStatusPkr.SelectedItem.ToString();
                _course.CourseNotify = NotifyStartEndSwitch.IsToggled;
                if (_isEditing)
                {
                    await DatabaseService.UpdateCourse(_course);
                }
                else
                {
                    await DatabaseService.AddCourse(_course);
                    _isEditing = true;
                }
                SwitchToViewMode();
            }
            else
            {
                CourseErrorLbl.Text = "Start date must be before end date.";
                CourseErrorLbl.IsVisible = true;
            }
        }
        else
        {
            CourseErrorLbl.Text = "Entry cannot include null values.";
            CourseErrorLbl.IsVisible = true;
        }
    }
    private async void DeleteCourseBtn_Clicked(object sender, EventArgs e)
    {
        await DatabaseService.RemoveCourse(_course.CourseId);
        await Navigation.PopAsync();
    }

    private void SwitchToViewMode()
    {
        CourseErrorLbl.IsVisible = false;
        CourseNameEntry.Text = _course.CourseName;
        CourseStartDpkr.Date = _course.CourseStartDate;
        CourseEndDpkr.Date = _course.CourseEndDate;
        CourseStatusPkr.SelectedItem = _course.CourseStatus;
        NotifyStartEndSwitch.IsToggled = _course.CourseNotify;

        EditCourseBtn.IsVisible = true;
        DeleteCourseBtn.IsVisible = false;
        CourseAssessmentsBtn.IsVisible = true;
        CourseInstructorBtn.IsVisible = true;
        CourseNotesBtn.IsVisible = true;

        EditAddCourseTitle.Text = "Course Details";
        CourseNameEntry.IsEnabled = false;
        CourseStartDpkr.IsEnabled = false;
        CourseEndDpkr.IsEnabled = false;
        CourseStatusPkr.IsEnabled = false;
        NotifyStartEndSwitch.IsEnabled = false;

        InstructorInfoGrid.IsVisible = true;

        AssessmentCollectionViewBorder.IsVisible = true;

        CancelBtn.IsVisible = false;
        SaveCourseBtn.IsVisible = false;
    }

    private void SwitchToEditMode()
    {
        if (_isEditing)
        {
            DeleteCourseBtn.IsVisible = true;

        }
        else
        {
            DeleteCourseBtn.IsVisible = false;
        }
            EditCourseBtn.IsVisible = false;
        CourseAssessmentsBtn.IsVisible = false;
        CourseInstructorBtn.IsVisible = false;
        CourseNotesBtn.IsVisible = false;

        EditAddCourseTitle.Text = "Edit Course";
        CourseNameEntry.IsEnabled = true;
        CourseStartDpkr.IsEnabled = true;
        CourseEndDpkr.IsEnabled = true;
        CourseStatusPkr.IsEnabled = true;
        NotifyStartEndSwitch.IsEnabled = true;

        InstructorInfoGrid.IsVisible = false;

        AssessmentCollectionViewBorder.IsVisible = false;

        CancelBtn.IsVisible = true;
        SaveCourseBtn.IsVisible = true;
    }
}