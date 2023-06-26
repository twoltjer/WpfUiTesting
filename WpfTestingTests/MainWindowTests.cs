namespace WpfTestingTests;

public class MainWindowTests
{
    [Fact]
    [Trait("Category", "UI")]
    public async Task TestButtonClick_WhenButtonIsClickedAndMessageBoxIsClosed_ThenButtonBackgroundIsChanged()
    {
        var solutionDir = "../../../../";
        var appPath = Path.Combine(solutionDir, "WpfTestApplication/bin/Debug/net7.0-windows/WpfTestApplication.exe");
        var app = Application.Launch(appPath);
        try
        {
            using var automation = new UIA3Automation();

            // Wait a half second for the app to start
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            // Get the main window of the app
            var mainWindow = app.GetMainWindow(automation);
            
            // Wait for a half second
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            // Find the button by automation id and click it
            var button = mainWindow.FindAllDescendants(
                cf => cf.ByAutomationId("MyButton")
                ).Single();
            button.Should().NotBeNull();
            button.Click();

            // After clicking the button, a MessageBox will pop up. 
            // We need to close it to interact with the button again.

            // Wait a half second for the MessageBox to appear
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            // Find the MessageBox window
            var msgBox = app.GetMainWindow(automation);
            // TODO: Verify message box by window title

            // Find the OK button on the MessageBox and click it
            var msgBoxOkButton = msgBox.FindAllDescendants(cf => cf.ByText("OK"));
            msgBoxOkButton.Should().NotBeNull();
            // Todo: figure out why this is not clickable, but first we need to get the first button click working.
            // msgBoxOkButton.Click();

            // Compare screenshot of the main window with a reference image
            var testResourcesDir = Path.Combine(solutionDir, "WpfTestingTests", "TestResources");
            var screenshotsDir = Path.Combine(testResourcesDir, "Screenshots");
            var mainWindowScreenshotsDir = Path.Combine(screenshotsDir, "MainWindow");
            var referenceImagePath = Path.Combine(
                mainWindowScreenshotsDir,
                "TestButtonClick_WhenButtonIsClickedAndMessageBoxIsClosed_ThenButtonBackgroundIsChanged.png"
                );
            var screenshot = mainWindow.Capture();
            // Compare bitmaps
            {
                using var referenceImage = new Bitmap(referenceImagePath);
                // Compare dimensions
                screenshot.Width.Should().Be(referenceImage.Width);
                screenshot.Height.Should().Be(referenceImage.Height);
                // Compare pixels
                for (var x = 0; x < screenshot.Width; x++)
                {
                    for (var y = 0; y < screenshot.Height; y++)
                    {
                        var screenshotPixel = screenshot.GetPixel(x, y);
                        var referencePixel = referenceImage.GetPixel(x, y);
                        screenshotPixel.Should().Be(referencePixel);
                    }
                }
            }
        }
        finally
        {
            app.Close();
        }
    }
}