
using SClarkC971PA.Services;
using SClarkC971PA.Models;
namespace SClarkC971PA.Views;

public partial class Report : ContentPage
{
	private int _currentUserId = 0;
	private string _assessmentType = "";
	private string _assessmentStatus = "";
	private IEnumerable<Assessment> _originalReport;
	private IEnumerable<Assessment> _reportSearchResult;
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
                _originalReport = await DatabaseService.GetPerformanceAssessmentReport(_currentUserId, _assessmentStatus);
				_reportSearchResult = Enumerable.Empty<Assessment>();
                AssessmentCollectionView.ItemsSource = _originalReport;
				ReportErrorLbl.Text = "Report run " + DateTime.Now.ToString("MM/dd/yyyy 'at' hh:mm tt");
				ReportErrorLbl.IsVisible = true;
            }
			else if (_assessmentType == "Objective")
			{
                _originalReport = await DatabaseService.GetObjectiveAssessmentReport(_currentUserId, _assessmentStatus);
                _reportSearchResult = Enumerable.Empty<Assessment>();
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

  //  private void ReportSearchBar_SearchButtonPressed(object sender, EventArgs e)
  //  {
		//if (string.IsNullOrEmpty(ReportSearchBar.Text))
		//{
		//	AssessmentCollectionView.ItemsSource = _originalReport;
		//}
		//else
		//{
		//	string searchKeyword = ReportSearchBar.Text.ToString();
		//	foreach (Assessment assessment in _originalReport)
		//	{
		//		if (Int32.TryParse(searchKeyword, out int assessmentId))
		//		{
		//			if (assessment.AssessmentId == assessmentId && !_reportSearchResult.Contains(assessment))
		//			{
		//				_reportSearchResult.Append(assessment);
		//			}
		//		}
		//		if (DateTime.TryParse(searchKeyword, out DateTime searchDate))
		//		{
		//			if (!_reportSearchResult.Contains(assessment) && assessment.AssessmentStartDate == searchDate || assessment.AssessmentEndDate == searchDate)
		//			{
		//				_reportSearchResult.Append(assessment);
		//			}
		//		}
		//		if (!_reportSearchResult.Contains(assessment) && assessment.AssessmentStatus.ToString().ToLower().Contains(searchKeyword.ToLower()))
		//		{
		//			_reportSearchResult.Append(assessment);
		//		}
		//		if (!_reportSearchResult.Contains(assessment) && assessment.AssessmentName.ToString().ToLower().Contains(searchKeyword.ToLower()))
		//		{
		//			_reportSearchResult.Append(assessment);
		//		}
		//		AssessmentCollectionView.ItemsSource = _reportSearchResult;
  //          }
		//}
  //  }

    private void ReportSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(ReportSearchBar.Text))
        {
            AssessmentCollectionView.ItemsSource = _originalReport;
        }
        else
        {
            string searchKeyword = ReportSearchBar.Text.ToString();
            foreach (Assessment assessment in _originalReport)
            {
                if (Int32.TryParse(searchKeyword, out int assessmentId))
                {
                    if (assessment.AssessmentId == assessmentId && !_reportSearchResult.Contains(assessment))
                    {
                        _reportSearchResult.Append(assessment);
                    }
                }
                if (DateTime.TryParse(searchKeyword, out DateTime searchDate))
                {
                    if (!_reportSearchResult.Contains(assessment) && assessment.AssessmentStartDate == searchDate || assessment.AssessmentEndDate == searchDate)
                    {
                        _reportSearchResult.Append(assessment);
                    }
                }
                if (!_reportSearchResult.Contains(assessment) && assessment.AssessmentStatus.ToString().ToLower().Contains(searchKeyword.ToLower()))
                {
                    _reportSearchResult.Append(assessment);
                }
                if (!_reportSearchResult.Contains(assessment) && assessment.AssessmentName.ToString().ToLower().Contains(searchKeyword.ToLower()))
                {
                    _reportSearchResult.Append(assessment);
                }
                AssessmentCollectionView.ItemsSource = _reportSearchResult;
            }
        }
    }
}