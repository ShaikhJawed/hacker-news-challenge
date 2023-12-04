# hacker-news-challenge

This project is a SPA built using Dot Net Core Web API and Angular  that interacts with a third-party news API to fetch top news stories, allows users to filter news based on search criteria, and implements pagination for better user experience.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Tests](#tests)
- [Contributing](#contributing)
- [License](#license)

## Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/ShaikhJawed/hacker-news-challenge.git

2. **Restore Packages:**
Open the solution in Visual Studio or your preferred IDE, and restore the NuGet packages.

## Usage
1. **Run the Application:**

Build and run the application.
The application will interact with the third-party API to fetch top news stories.

2. **Endpoints:**

/api/news/top - Get top news stories.
/api/news/search?query={searchTerm} - Search news based on a keyword.
/api/news/page?pageNumber={pageNumber}&pageSize={pageSize} - Get paginated news.

3. **Examples:**

Get top news: GET /api/news/top
Search news: GET /api/news/search?query=keyword
Get paginated news: GET /api/news/page?pageNumber=1&pageSize=10

## Contributing
Feel free to contribute to this project. If you find any issues or have suggestions, please open an issue or create a pull request.
