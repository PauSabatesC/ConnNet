- Development methodology:
    - TDD //at the beginning it was a full TDD project, but due to unclear requirements with a lot of architectural changes, it's okay to unit test only for now.

- Principles:
    - SOLID
    - DRY

- Design patters used:
    - dependency injection
    - facade pattern: in order to pass socket interface to library.
    - adapter pattern: used to mock sealed libraries from microsoft(like System.Net.Sockets)
	- strategy pattern(fot different read variations)
	- observer pattern(for receiving update when server reads data)//not used yet, and implementation with locks for multithreading instead is used

---

How to develop:

	- Clone the repo
	- Create a new branch for the desired issue from Develop
	- dotnet restore
	- dotnet build
	- dotnet test
	- Code!