using SClarkC971PA.Models;
using SClarkC971PA.Services;
using System.Threading.Tasks;
namespace SClarkC971PA.Views;

public partial class AssessmentEdit : ContentPage
{
    private int _associatedCourseId = 0;
    private Assessment _assessment = new Assessment();
    private bool _isEditing = false;
    private bool _assessmentCountExceeded = false;
	public AssessmentEdit(int courseId)
	{
		InitializeComponent();
        _associatedCourseId = courseId;
        EditAddAssessmentTitle.Text = "Add Assessment";
    }
    public AssessmentEdit(int courseId, Assessment assessment)
    {
        InitializeComponent();
        _associatedCourseId = courseId;
        _assessment = assessment;
        _isEditing = true;

        AssessmentNameEntry.Text = _assessment.AssessmentName;
        AssessmentTypePkr.SelectedItem = _assessment.AssessmentType;
        AssessmentStartDateDpkr.Date = _assessment.AssessmentStartDate;
        AssessmentEndDateDpkr.Date = _assessment.AssessmentEndDate;
        NotifyAssessmentSwitch.IsToggled = _assessment.AssessmentNotify;

        DeleteAssessmentBtn.IsVisible = true;
        

        EditAddAssessmentTitle.Text = "Edit Assessment";
    }
    private async void DeleteAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        if (_isEditing) 
        { 
            await DatabaseService.RemoveAssessment(_assessment.AssessmentId);
            await Navigation.PopAsync();
            
        }
    }
    private async void SaveAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(AssessmentNameEntry.Text)
            && AssessmentTypePkr.SelectedItem != null
            && AssessmentStartDateDpkr.Date != null
            && AssessmentEndDateDpkr.Date != null
            && NotifyAssessmentSwitch.IsToggled != null)
        {
            if (Validation.ValidateDates(AssessmentStartDateDpkr.Date, AssessmentEndDateDpkr.Date))
            {
                if (await DatabaseService.VerifyAssessmentCountMet(_assessment.AssessmentId, _associatedCourseId, AssessmentTypePkr.SelectedItem.ToString()))
                {
                    AssessmentErrorLbl.Text = "Max number of " + AssessmentTypePkr.SelectedItem.ToString().ToLower() + " assessments reached. Please choose a different type or remove an existing assessment.";
                    AssessmentErrorLbl.IsVisible = true;
                }
                else
                {
                    _assessment.AssociatedCourseId = _associatedCourseId;
                    _assessment.AssessmentName = AssessmentNameEntry.Text;
                    _assessment.AssessmentType = AssessmentTypePkr.SelectedItem.ToString();
                    _assessment.AssessmentStartDate = AssessmentStartDateDpkr.Date;
                    _assessment.AssessmentEndDate = AssessmentEndDateDpkr.Date;
                    _assessment.AssessmentNotify = NotifyAssessmentSwitch.IsToggled;

                    if (_isEditing)
                    {
                        await DatabaseService.UpdateAssessment(_assessment);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DatabaseService.AddAssessment(_assessment);
                        await Navigation.PopAsync();
                    }
                }
            }
            else
            {
                AssessmentErrorLbl.Text = "Start date must be before end date.";
                AssessmentErrorLbl.IsVisible = true;
            }
        }
        else
        {
            AssessmentErrorLbl.Text = "Entry cannot have empty values.";
            AssessmentErrorLbl.IsVisible = true;
        }
    }
}