# Project Description

## Overview
This project involves the development of a mobile application targeting university students, designed to streamline their academic activities. The app provides functionalities such as:

- Viewing and managing the current timetable.
- Automated reminders for classes and deadlines.
- Organizing and tracking deadlines with filtering and sorting options.

The application is built using the .NET MAUI framework for cross-platform compatibility, allowing seamless execution on Android, iOS, macOS, and Windows platforms.

## Key Features

### Timetable Management
- Fetches the latest timetable from the university's API (`cist.nure.ua`).
- Displays events in a weekly calendar view.
- Provides notifications for upcoming classes and important events.

### Deadline Tracking
- Allows students to create, manage, and organize deadlines.
- Includes filtering options for priorities, completion status, and date ranges.
- Offers reminders for deadlines to avoid missed submissions.

### Customization
- Intuitive user interface with clear navigation.
- Configurable settings for enabling/disabling notifications.
- Easy group selection to fetch specific timetables.

## Architecture
The project follows the MVVM (Model-View-ViewModel) architectural pattern:

- **Model**: Handles data retrieval and storage, including timetable and deadlines.
- **View**: Manages the graphical interface using XAML.
- **ViewModel**: Acts as a mediator between the Model and View, handling business logic and data binding.

## Technologies Used

### Development Framework
- **.NET MAUI**: Selected for its robust support for cross-platform native development.

### Libraries and Tools
- `CommunityToolkit.Mvvm`: Simplifies MVVM pattern implementation.
- `Newtonsoft.Json`: Handles JSON serialization and deserialization.
- `Plugin.LocalNotification`: Manages native notifications across platforms.
- `DevExpress.Maui.Scheduler`: Provides advanced scheduling components for timetable display.
- `Mopups`: Enables popup views for enhanced user interactions.
- `UraniumUI.Material`: Adds modern material design components.

### IDE and Tools
- **Visual Studio for Windows**: Primary development environment.
- Integrated Android emulator for testing.

## Setup and Infrastructure

### Prerequisites
1. Install [Visual Studio](https://visualstudio.microsoft.com/) with the **.NET MAUI workload**.
2. Ensure Android/iOS emulators or devices are configured for testing.

### Installation Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/username/project-repo.git
   ```
2. Navigate to the project directory:
   ```bash
   cd project-repo
   ```
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Build the project:
   ```bash
   dotnet build
   ```

### Running the Application
1. Launch the project in Visual Studio.
2. Select the target platform (Android/iOS/Windows) in the IDE.
3. Click the "Run" button to deploy the application.

## Future Improvements
- **Platform Expansion**: Extend support to iOS and Windows using community-driven efforts.
- **Localization**: Add support for multiple languages.
