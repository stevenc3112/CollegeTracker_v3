using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using SClarkC971PA.Models;
using SClarkC971PA.Services;

using System.Threading.Tasks;
namespace SClarkC971PA.Views;

public partial class AssessmentEdit : ContentPage
{
    private bool _isEditing;
    private int _associatedCourseId = 0;
    private Assessment _assessment;

    private bool _assessmentCountExceeded = false;
	public AssessmentEdit(int courseId)
	{
        InitializeComponent();
        _associatedCourseId = courseId;
        EditAddAssessmentTitle.Text = "Add Assessment";
        _isEditing = false;

    }
    public AssessmentEdit(int courseId, Assessment assessment)
    {
        InitializeComponent();
        _associatedCourseId = courseId;
        _assessment = assessment;
        _isEditing = true;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_isEditing)
        {
            EditAddAssessmentTitle.Text = "Edit Assessment";
            DeleteAssessmentBtn.IsVisible = true;

            AssessmentNameEntry.Text = _assessment.AssessmentName;
            AssessmentStartDateDpkr.Date = _assessment.AssessmentStartDate;
            AssessmentEndDateDpkr.Date = _assessment.AssessmentEndDate;
            NotifyAssessmentSwitch.IsToggled = _assessment.AssessmentNotify;
            AssessmentStatusPkr.SelectedItem = _assessment.AssessmentStatus;

            if (_assessment is Performance performanceAssessment)
            {
                //AssessmentNameEntry.Text = performanceAssessment.AssessmentName;
                //AssessmentStartDateDpkr.Date = performanceAssessment.AssessmentStartDate;
                //AssessmentEndDateDpkr.Date = performanceAssessment.AssessmentEndDate;
                //NotifyAssessmentSwitch.IsToggled = performanceAssessment.AssessmentNotify;

                AssessmentTypePkr.SelectedItem = "Performance";
                PAssessmentFeedbackEdt.Text = performanceAssessment.PAssessmentFeedback;
                PAssessmentFeedbackEdt.IsVisible = true;
                PAssessmentFeedbackEdtLbl.IsVisible = true;

                OAssessmentScoreEntry.IsVisible = false;
                OAssessmentScoreEntryLbl.IsVisible = false;
            }
            else if (_assessment is Objective objectiveAssessment)
            {
                //AssessmentNameEntry.Text = objectiveAssessment.AssessmentName;
                //AssessmentStartDateDpkr.Date = objectiveAssessment.AssessmentStartDate;
                //AssessmentEndDateDpkr.Date = objectiveAssessment.AssessmentEndDate;
                //NotifyAssessmentSwitch.IsToggled = objectiveAssessment.AssessmentNotify;

                AssessmentTypePkr.SelectedItem = "Objective";
                OAssessmentScoreEntry.Text = objectiveAssessment.OAssessmentScore.ToString();
                OAssessmentScoreEntry.IsVisible = true;
                OAssessmentScoreEntryLbl.IsVisible = true;

                PAssessmentFeedbackEdt.IsVisible = false;
                PAssessmentFeedbackEdt.IsVisible = false;
            }
        }
        else 
        {
            AssessmentNameEntry.Text = "";
            AssessmentStartDateDpkr.Date = DateTime.Today;
            AssessmentEndDateDpkr.Date = DateTime.Today.AddDays(1);
            NotifyAssessmentSwitch.IsToggled = false;

            AssessmentTypePkr.SelectedItem = "Objective";
            OAssessmentScoreEntry.Text = "";
            OAssessmentScoreEntry.IsVisible = true;
            OAssessmentScoreEntryLbl.IsVisible = true;
            PAssessmentFeedbackEdtLbl.IsVisible = false;
        }
    }

    private void AssessmentTypePkr_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (AssessmentTypePkr.SelectedIndex == 1)
        {
            PAssessmentFeedbackEdt.IsVisible = true;
            PAssessmentFeedbackEdtLbl.IsVisible = true;

            OAssessmentScoreEntry.IsVisible = false;
            OAssessmentScoreEntryLbl.IsVisible = false;
        }
        else if (AssessmentTypePkr.SelectedIndex == 0)
        {
            OAssessmentScoreEntry.IsVisible = true;
            OAssessmentScoreEntryLbl.IsVisible = true;

            PAssessmentFeedbackEdt.IsVisible = false;
            PAssessmentFeedbackEdtLbl.IsVisible = false;
        }
    }
    private async void DeleteAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        if (_isEditing) 
        {
            if (_assessment is Performance)
            {
                await DatabaseService.RemoveAssessment(_assessment.AssessmentId, "Performance");
                await Navigation.PopAsync();
            }
            else if (_assessment is Objective)
            {
                await DatabaseService.RemoveAssessment(_assessment.AssessmentId, "Objective");
                await Navigation.PopAsync();
            }

        }
    }
    private async void SaveAssessmentBtn_Clicked(object sender, EventArgs e)
    {
        Assessment newAssessment;

        if (!String.IsNullOrEmpty(AssessmentNameEntry.Text)
            && AssessmentTypePkr.SelectedItem != null
            && AssessmentStartDateDpkr.Date != null
            && AssessmentEndDateDpkr.Date != null
            && NotifyAssessmentSwitch.IsToggled != null
            && AssessmentStatusPkr.SelectedItem != null)
        {
            if (Validation.ValidateDates(AssessmentStartDateDpkr.Date, AssessmentEndDateDpkr.Date))
            {
                if (AssessmentTypePkr.SelectedIndex == 1)
                {
                    if (_isEditing)
                    {
                        Performance performance = new Performance();
                        //DatabaseService.RemoveAssessment(_assessment.AssessmentId);
                        if (_assessment is Performance)
                        {
                            performance = (Performance)_assessment;
                        }

                        performance.AssessmentId = _assessment.AssessmentId;
                        performance.AssociatedCourseId = _associatedCourseId;
                        performance.AssessmentName = AssessmentNameEntry.Text;
                        performance.AssessmentStartDate = AssessmentStartDateDpkr.Date;
                        performance.AssessmentEndDate = AssessmentEndDateDpkr.Date;
                        performance.AssessmentNotify = NotifyAssessmentSwitch.IsToggled;
                        performance.AssessmentStatus = AssessmentStatusPkr.SelectedItem.ToString();
                        performance.PAssessmentFeedback = PAssessmentFeedbackEdt.Text;

                        await DatabaseService.UpdateAssessment(performance);
                        await Navigation.PopAsync();
                    }
                    else if (!_isEditing)
                    {
                        Performance performance = new Performance();
                        
                        performance.AssociatedCourseId = _associatedCourseId;
                        performance.AssessmentName = AssessmentNameEntry.Text;
                        performance.AssessmentStartDate = AssessmentStartDateDpkr.Date;
                        performance.AssessmentEndDate = AssessmentEndDateDpkr.Date;
                        performance.AssessmentNotify = NotifyAssessmentSwitch.IsToggled;
                        performance.AssessmentStatus = AssessmentStatusPkr.SelectedItem.ToString();
                        performance.PAssessmentFeedback = PAssessmentFeedbackEdt.Text;

                        await DatabaseService.AddAssessment(performance);
                        await Navigation.PopAsync();
                    }
                }//If assessment is objective
                else if (AssessmentTypePkr.SelectedIndex == 0)
                {
                    bool scoreIsInt = int.TryParse(OAssessmentScoreEntry.Text, out int scoreInt);

                    if (scoreIsInt || string.IsNullOrEmpty(OAssessmentScoreEntry.Text))
                    {
                        AssessmentErrorLbl.IsVisible = false;

                        //update existing objective assessment
                        if (_isEditing)
                        {
                            Objective objective = new Objective();
                            //DatabaseService.RemoveAssessment(_assessment.AssessmentId);
                            if (_assessment is Objective)
                            {
                                objective = (Objective)_assessment;
                            }
                            objective.AssessmentId = _assessment.AssessmentId;
                            objective.AssociatedCourseId = _associatedCourseId;
                            objective.AssessmentName = AssessmentNameEntry.Text;
                            objective.AssessmentStartDate = AssessmentStartDateDpkr.Date;
                            objective.AssessmentEndDate = AssessmentEndDateDpkr.Date;
                            objective.AssessmentNotify = NotifyAssessmentSwitch.IsToggled;
                            objective.AssessmentStatus = AssessmentStatusPkr.SelectedItem.ToString();
                            objective.OAssessmentScore = scoreInt;

                            await DatabaseService.UpdateAssessment(objective);
                            await Navigation.PopAsync();
                        }//Add new objecitve assessment
                        else if (!_isEditing)
                        {
                            Objective objective = new Objective();
                            objective.AssociatedCourseId = _associatedCourseId;
                            objective.AssessmentName = AssessmentNameEntry.Text;
                            objective.AssessmentStartDate = AssessmentStartDateDpkr.Date;
                            objective.AssessmentEndDate = AssessmentEndDateDpkr.Date;
                            objective.AssessmentNotify = NotifyAssessmentSwitch.IsToggled;
                            objective.AssessmentStatus = AssessmentStatusPkr.SelectedItem.ToString();
                            objective.OAssessmentScore = scoreInt;

                            await DatabaseService.AddAssessment(objective);
                            await Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        AssessmentErrorLbl.Text = "Score must be a number.";
                        AssessmentErrorLbl.IsVisible = true;
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