
using SClarkC971PA.Services;
using SClarkC971PA.Models;
namespace SClarkC971PA.Views;

public partial class Report : ContentPage
{
	private int _currentUserId = 0;
	private string _assessmentType = "";
	private string _assessmentStatus = "";
	public Report(int currentUserId)
	{
		InitializeComponent();
		_currentUserId = currentUserId;
		ReportErrorLbl.Text = "";
		ReportErrorLbl.IsVisible = false;
		AssessmentTypePkr.SelectedIndex = 0;
        AssessmentStatusPkr.SelectedIndex = 0;
	}

    private async void LoadReportBtn_Clicked(object sender, EventArgs e)
    {
		if (AssessmentTypePkr.SelectedIndex != -1 && AssessmentStatusPkr.SelectedIndex != -1)
		{
			_assessmentType = AssessmentTypePkr.SelectedItem.ToString();
			_assessmentStatus = AssessmentStatusPkr.SelectedItem.ToString();
            if (_assessmentType == "Performance")
            {
				AssessmentCollectionView.ItemsSource = await DatabaseService.GetPerformanceAssessmentReport(_currentUserId, _assessmentStatus);
				ReportErrorLbl.Text = "Report run " + DateTime.Now.ToString("MM/dd/yyyy 'at' hh:mm tt");
				ReportErrorLbl.IsVisible = true;
            }
			else if (_assessmentType == "Objective")
			{
                AssessmentCollectionView.ItemsSource = await DatabaseService.GetObjectiveAssessmentReport(_currentUserId, _assessmentStatus);
                ReportErrorLbl.Text = "Report run " + DateTime.Now.ToString("MM/dd/yyyy 'at' hh:mm tt");
                ReportErrorLbl.IsVisible = true;
            }

        }
		else
		{
			ReportErrorLbl.Text = "Assessment Status and Type cannot be empty fields.";
			ReportErrorLbl.IsVisible = true;
		}
    }
}