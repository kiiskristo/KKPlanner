using AMMA.Data.Services;
using AMMA.Data.Utils;
using AMMA.Data.ViewModel;
using Moq;

namespace AMMA.Tests;

public class TermsUnitTest
{

    private readonly TermDatabaseService _termsDataService;
    private readonly CourseDatabaseService _coursesDataService;
    
    public TermsUnitTest()
    {
        DatabaseUtility.Instance.InitializeDatabaseAsync(":memory:").Wait();
        _termsDataService = new TermDatabaseService();
        _coursesDataService = new CourseDatabaseService();
    }
    
    [Fact]
    public async Task OnEditCourse_NavigateToEdit_WithCorrectCourseId()
    {
        // Arrange
        var mockNavigationUtility = new Mock<INavigationUtility>();
        var courseId = 4;
        var expectedRoute = $"//terms/courseDetail?courseId={courseId}";

        // Setup ActionSheet to return "Edit" when called
        mockNavigationUtility.Setup(x => x.ActionSheet(It.IsAny<string>(), It.IsAny<List<string>>()))
            .ReturnsAsync("Edit");

        // Track the route passed to NavigateTo
        string actualRoute = string.Empty;
        mockNavigationUtility.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()))
            .Callback<string, bool>((route, _) => actualRoute = route);
        
        var viewModel = new TermsViewModel(_termsDataService, _coursesDataService, mockNavigationUtility.Object);
        
        await viewModel.EditCourseCommand.ExecuteAsync(courseId);

        // Assert
        mockNavigationUtility.Verify(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        Assert.Equal(expectedRoute, actualRoute);
    }

    [Fact]
    public async Task OnEdiTerm_EditTerm_CheckTerm()
    {
        // Arrange
        var mockNavigationUtility = new Mock<INavigationUtility>();
        var termId = 2;
        var expectedRoute = $"//terms/detail?termId={termId}";
        var newTitle = "Testing";
        var newEndDate = DateTime.Now.AddDays(10);

        // Setup ActionSheet to return "Edit" when called
        mockNavigationUtility.Setup(x => x.ActionSheet(It.IsAny<string>(), It.IsAny<List<string>>()))
            .ReturnsAsync("Edit");

        // Track the route passed to NavigateTo
        string actualRoute = string.Empty;
        mockNavigationUtility.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()))
            .Callback<string, bool>((route, _) => actualRoute = route);
        
        var viewModel = new TermsViewModel(_termsDataService, _coursesDataService, mockNavigationUtility.Object);
        
        await viewModel.EditTermCommand.ExecuteAsync(termId);

        // Assert
        mockNavigationUtility.Verify(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        Assert.Equal(expectedRoute, actualRoute);

        // Further interactions with TermDetailViewModel
        var editViewModel = new TermDetailViewModel(_termsDataService, _coursesDataService, mockNavigationUtility.Object);
        editViewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "termId", termId.ToString() } });
        
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        
        // Simulate user changes and save
        editViewModel.Title = newTitle;
        editViewModel.CurrentTerm.EndDate = newEndDate;
        await editViewModel.SaveCommand.ExecuteAsync(null);

        // Assert that navigate back was called
        mockNavigationUtility.Verify(x => x.NavigateBack(null), Times.Once);
        
        viewModel.LoadTermsAsync();
        
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        
        var term = viewModel.Terms.FirstOrDefault(c => c.Id == termId);
        
        // Assert
        Assert.Equal(newTitle, term?.Title);
        Assert.Equal(newEndDate, term?.EndDate);
    }
    
    [Fact]
    public async Task DeleteTerm_CheckTermRemoved()
    {
        // Arrange
        var mockNavigationUtility = new Mock<INavigationUtility>();
        var termId = 1;

        // Assuming your TermsViewModel initially loads some terms
        var viewModel = new TermsViewModel(_termsDataService, _coursesDataService, mockNavigationUtility.Object);
        viewModel.LoadTermsAsync();
        await Task.Delay(100); // Ensure async operations complete

        // Setup mock to simulate user selecting "Delete" from the action sheet
        mockNavigationUtility
            .Setup(x => x.ActionSheet(It.IsAny<string>(), It.IsAny<List<string>>()))
            .ReturnsAsync("Delete");

        var initialCount = viewModel.Terms.Count;

        // Act
        // Invoke OnEditTerm, which internally calls DeleteTerm if "Delete" is selected
        await viewModel.EditTermCommand.ExecuteAsync(termId);

        // Wait a bit for async deletion to complete and re-fetch terms
        viewModel.LoadTermsAsync();
        await Task.Delay(100);

        // Assert
        var termAfterDeletion = viewModel.Terms.FirstOrDefault(c => c.Id == termId);
        Assert.Null(termAfterDeletion); // Ensure the term is removed
        Assert.True(viewModel.Terms.Count < initialCount); // Check if the count of terms decreased
    }


    [Fact]
    public async Task DeleteCourse_CheckCourseRemoved()
    {
        // Arrange
        var mockNavigationUtility = new Mock<INavigationUtility>();
        var courseId = 1;

        // Assuming your ViewModel initially loads some terms and their courses
        var viewModel = new TermsViewModel(_termsDataService, _coursesDataService, mockNavigationUtility.Object);
        viewModel.LoadTermsAsync();
        await Task.Delay(100); // Ensure async operations complete

        // Setup mock to simulate user selecting "Delete" from the action sheet for a course
        mockNavigationUtility
            .Setup(x => x.ActionSheet(It.IsAny<string>(), It.IsAny<List<string>>()))
            .ReturnsAsync("Delete");

        // Find the term that contains the course to delete, and get initial course count
        var termContainingCourse = viewModel.Terms.FirstOrDefault(term => term.Courses.Any(c => c.Id == courseId));
        var initialCourseCount = termContainingCourse?.Courses.Count ?? 0;

        // Act
        // Invoke OnEditCourse, which internally calls your method to delete the course if "Delete" is selected
        await viewModel.EditCourseCommand.ExecuteAsync(courseId);

        // Wait a bit for async deletion to complete and re-fetch terms to refresh courses
        viewModel.LoadTermsAsync();
        await Task.Delay(100);

        // Assert
        var updatedTermContainingCourse = viewModel.Terms.FirstOrDefault(term => term.Id == termContainingCourse?.Id);
        var courseAfterDeletion = updatedTermContainingCourse?.Courses.FirstOrDefault(c => c.Id == courseId);
        Assert.Null(courseAfterDeletion); // Ensure the course is removed
        Assert.True(updatedTermContainingCourse?.Courses.Count < initialCourseCount); // Check if the count of courses decreased
    }
}