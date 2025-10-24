
using SClarkC971PA.Services;
using SClarkC971PA.Models;
using System.Runtime.CompilerServices;
namespace SClarkC971PA.Views;

public partial class Report : ContentPage
{
	private int _currentUserId = 0;
	private string _assessmentType = "";
	private string _assessmentStatus = "";
	private IEnumerable<Assessment> _originalReport;
	public Report(int currentUserId)
	{
		InitializeComponent();
		_currentUserId = currentUserId;
		ReportErrorLbl.Text = "";
		ReportErrorLbl.IsVisible = false;
		AssessmentTypePkr.SelectedIndex = 0;


    }
	protected override async void OnAppearing()
	{
		base.OnAppearing();
        AssessmentStatusPkr.ItemsSource = null;
        var statusTable = await DatabaseService.GetStatusItems();
        AssessmentStatusPkr.ItemsSource = statusTable.Select(s => s.StatusItem).ToList();
        AssessmentStatusPkr.SelectedIndex = 0;

        AssessmentTypePkr.ItemsSource = null;
        var typeTable = await DatabaseService.GetAssessmentTypeItems();
        AssessmentTypePkr.ItemsSource = typeTable.Select(s => s.TypeItem).ToList();
        AssessmentTypePkr.SelectedIndex = 0;
    }
    private async void LoadReportBtn_Clicked(object sender, EventArgs e)
    {
		if (AssessmentTypePkr.SelectedIndex != -1 && AssessmentStatusPkr.SelectedIndex != -1)
		{
			_assessmentType = AssessmentTypePkr.SelectedItem.ToString();
			_assessmentStatus = AssessmentStatusPkr.SelectedItem.ToString();
            if (_assessmentType == "Performance")
            {
                _originalReport = await DatabaseService.GetPerformanceAssessmentReport(_currentUserId, _assessmentStatus);
                AssessmentCollectionView.ItemsSource = _originalReport;
				ReportErrorLbl.Text = "Report run " + DateTime.Now.ToString("MM/dd/yyyy 'at' hh:mm tt");
				ReportErrorLbl.IsVisible = true;
            }
			else if (_assessmentType == "Objective")
			{
                _originalReport = await DatabaseService.GetObjectiveAssessmentReport(_currentUserId, _assessmentStatus);
                AssessmentCollectionView.ItemsSource = _originalReport;
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
    private void ReportSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_originalReport == null)
        {
            AssessmentCollectionView.ItemsSource = null;
			return;
        }

		string searchKeyword = ReportSearchBar?.Text?.Trim() ?? string.Empty;

		if (string.IsNullOrEmpty(searchKeyword))
		{
			AssessmentCollectionView.ItemsSource = _originalReport;
			return;
		}

		bool hasId = int.TryParse(searchKeyword, out int assessmentId);
		bool hasDate = DateTime.TryParse(searchKeyword, out DateTime searchDate);
		string keywordLower = searchKeyword.ToLower();

		var results = new List<Assessment>();

		foreach (Assessment assessment in _originalReport)
		{
			bool match = false;
			if (hasId && assessment.AssessmentId == assessmentId)
			{
				match = true;
			}
			
			if(!match && hasDate)
			{
				var d = searchDate.Date;
				if (assessment.AssessmentStartDate.Date == d || assessment.AssessmentEndDate.Date == d)
				{
					match = true;
                }

            }

			if (!match && (assessment.AssessmentStatus?.ToString() ?? string.Empty).ToLower().Contains(keywordLower))
			{
				match = true;
            }

			if (!match && (assessment.AssessmentName?.ToString() ?? string.Empty).ToLower().Contains(keywordLower))
			{
				match = true;
            }

			if (match)
			{
				results.Add(assessment);
            }

        }
            AssessmentCollectionView.ItemsSource = results;
    }
}