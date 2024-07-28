# Email Sender Project

This project includes a library for sending emails and a console application to use the library.

## Prerequisites

- .NET 8.0 SDK or later

## Getting Started

1. Clone the repository:

    ```sh
    git clone https://github.com/yourusername/yourrepository.git
    cd yourrepository
    ```

2. Restore the NuGet packages:

    ```sh
    dotnet restore
    ```

3. Build the solution:

    ```sh
    dotnet build
    ```

4. Run the console application:

    ```sh
    cd EmailSenderConsole
    dotnet run
    ```

## Configuration

Ensure you have an `appsettings.json` file in the `EmailSenderConsole` project with the following structure:

```json
{
  "EmailSendingSettings": {
    "Server": "smtp.gmail.com",
    "Port": "465",
    "Username": "your-email@gmail.com",
    "Password": "your-email-password"
  }
}
