- Development methodology:
    - TDD

- Principles:
    - SOLID
    - DRY

- Design patters used:
    - dependency injection
    - facade pattern: in order to pass (socket, rest...) interface to library.
    - adapter pattern: used to mock sealed libraries from microsoft(like System.Net.Sockets)

---

Requirements to run project:

- Add this line to src/ConnNet/obj/Debug/netstandard2.0/ConnNet.AssemblyInfo.cs
> [assembly: InternalsVisibleTo("ConnNet.UnitaryTests")]
