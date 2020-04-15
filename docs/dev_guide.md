- Development methodology:
    - TDD

- Principles:
    - SOLID
    - DRY

- Design patters used:
    - dependency injection
    - facade pattern: in order to pass socket interface to library.
    - adapter pattern: used to mock sealed libraries from microsoft(like System.Net.Sockets)
	- strategy pattern(fot different read variations)

---

How to develop:

	- Clone the repo
	- Create a new branch for the desired issue from Develop
	- dotnet restore
	- dotnet build
	- dotnet test
	- Code!