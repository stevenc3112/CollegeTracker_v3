using Microsoft.Maui.Hosting;
using Plugin.LocalNotification;
using SClarkC971PA.Models;
using SClarkC971PA.Services;
using System.Threading.Tasks;
namespace SClarkC971PA.Views;



public partial class TermList : ContentPage
{
    private int _currentUserId;
    public TermList(int userId)
	{
        _currentUserId = userId;
		InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //Services.Settings.FirstRun = false;
        if (Services.Settings.FirstRun)
        {
            await DatabaseService.LoadSampleData(_currentUserId);
            await DatabaseService.LoadDropdowns();
            Services.Settings.FirstRun = false;
        }

        await RefreshTermCollectionView();
        await ShowCourseNotifications();
        await ShowAssessmentNotifications();
    }
    private async Task RefreshTermCollectionView()
    {
        TermCollectionView.ItemsSource = await DatabaseService.GetTerms(_currentUserId);
    }
    private async void TermCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            Term term = (Term)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new TermCourseList(term, _currentUserId));
        }
    }

    private async void AddTermBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TermEdit(_currentUserId));
    }
    //Notification Id creator that uses hashes and other numbers to avoid notificationid collisions.
    private static int CreateNotifyId(params int[] parts)
    {
        unchecked
        {
            int h = 17;
            foreach (var p in parts) h = h * 31 + p;
            return Math.Abs(h);
        }
    }
    private async Task ShowCourseNotifications()
    {
        //test for enabled notifications
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }
        var today = DateTime.Today.Date;
        var courseList = await DatabaseService.GetCourses();
        int index = 0;

        foreach (Course courseRecord in courseList)
        {
            if (courseRecord.CourseNotify)
            {   
                var start = courseRecord.CourseStartDate.Date;
                var end = courseRecord.CourseEndDate.Date;
                NotificationRequest? notiReq = null;

                if (start == today)
                {
                    notiReq = new NotificationRequest
                    {
                        NotificationId = CreateNotifyId(1, courseRecord.CourseId, today.GetHashCode()),
                        Title = "Course Notification",
                        Subtitle = "This is my notification subtitle.",
                        Description = $"{courseRecord.CourseName} begins today!",
                        ReturningData = "Hello course notification!",
                        BadgeNumber = 42,
                        Schedule = new NotificationRequestSchedule()
                        {
                            NotifyTime = DateTime.Now.AddSeconds(5 + index),
                        }

                    };
                }
                else if (end == today)
                {
                    notiReq = new NotificationRequest
                    {
                        NotificationId = CreateNotifyId(2, courseRecord.CourseId, today.GetHashCode()),
                        Title = "Course Notification",
                        Subtitle = "This is my notification subtitle.",
                        Description = $"{courseRecord.CourseName} ends today!",
                        ReturningData = "Hello course notification!",
                        BadgeNumber = 42,
                        Schedule = new NotificationRequestSchedule()
                        {
                            NotifyTime = DateTime.Now.AddSeconds(5 + index),
                        }
                    };
                }
                if (notiReq != null)
                {
                    index++;
                    await LocalNotificationCenter.Current.Show(notiReq);
                }
            }
        }
    }
    private async Task ShowAssessmentNotifications()
    {
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }
        var today = DateTime.Today.Date;
        int index = 0;
        var assessmentList = await DatabaseService.GetAssessments();

        foreach (Assessment assessmentRecord in assessmentList)
        {
            if (assessmentRecord.AssessmentNotify == true)
            {
                var start = assessmentRecord.AssessmentStartDate.Date;
                var end = assessmentRecord.AssessmentEndDate.Date;
                NotificationRequest? notiReq = null;

                if (assessmentRecord.AssessmentStartDate == DateTime.Today)
                {
                    notiReq = new NotificationRequest
                    {
                        NotificationId = CreateNotifyId(3, assessmentRecord.AssessmentId, today.GetHashCode()),
                        Title = "Assessment Notification",
                        Subtitle = "This is my notification subtitle.",
                        Description = $"{assessmentRecord.AssessmentName} begins today!",
                        ReturningData = "Hello assessment notification!",
                        BadgeNumber = 42,
                        Schedule = new NotificationRequestSchedule()
                        {
                            NotifyTime = DateTime.Now.AddSeconds(5 + index),
                        }

                    };
                }
                else if (assessmentRecord.AssessmentEndDate == DateTime.Today)
                {

                    notiReq = new NotificationRequest
                    {
                        NotificationId = CreateNotifyId(4, assessmentRecord.AssessmentId, today.GetHashCode()),
                        Title = "Assessment Notification",
                        Subtitle = "This is my notification subtitle.",
                        Description = $"{assessmentRecord.AssessmentName} is due today!",
                        ReturningData = "Hello assessment notification!",
                        BadgeNumber = 42,
                        Schedule = new NotificationRequestSchedule()
                        {
                            NotifyTime = DateTime.Now.AddSeconds(5 + index),
                        }
                    };
                }
                if (notiReq != null)
                {
                    index++;
                    await LocalNotificationCenter.Current.Show(notiReq);
                }
            }
        }
    }

    private async void ReportBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Report(_currentUserId));
    }
}