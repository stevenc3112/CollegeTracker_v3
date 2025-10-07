using SClarkC971PA.Models;
using SClarkC971PA.Services;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SClarkC971PA.Views;

public partial class TermEdit : ContentPage
{
    private Term _selectedTerm;
    private int _selectedTermId;
    private bool _editingTerm = false;
	public TermEdit()
	{
		InitializeComponent();
        EditAddTermTitle.Text = "Add Term";
    }
    public TermEdit(Term term)
    {
        InitializeComponent();
        EditAddTermTitle.Text = "Edit Term";
        _selectedTermId = term.TermId;
        TermTitleEntry.Text = term.TermTitle;
        StartDateDpkr.Date = term.TermStartDate;
        EndDateDpkr.Date = term.TermEndDate;
        _editingTerm = true;

    }
    protected override async void OnAppearing()
    {   
        base.OnAppearing();
    }
    private async void SaveTermBtn_Clicked(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(TermTitleEntry.Text) && StartDateDpkr.Date != null && EndDateDpkr.Date != null)
        {
            if (Validation.ValidateDates(StartDateDpkr.Date, EndDateDpkr.Date))
            {
                var termTitle = TermTitleEntry.Text;
                var termStart = StartDateDpkr.Date;
                var termEnd = EndDateDpkr.Date;

                if (_editingTerm)
                {
                    await DatabaseService.UpdateTerm(_selectedTermId, termTitle, termStart, termEnd);
                    await Navigation.PopAsync();
                }
                else
                {
                    DatabaseService.AddTerm(termTitle, termStart, termEnd);
                    await Navigation.PopAsync();
                }
            }
            else
            {
                TermErrorLbl.Text = "Start date must be before end date.";
                TermErrorLbl.IsVisible = true;
            }
        }
        else {
            TermErrorLbl.Text = "Entry cannot include null values.";
            TermErrorLbl.IsVisible = true;
        }
    }
}