# Utilities

This repository contains various utility projects for Dynamics 365.

## Version Checker
Microsoft occasionally updates the revision of Dynamics 365 without notice.  They do this to fix bugs and patch security holes, and while it's great that they do this, occasionally the patches cause inadvertent issues.

With this solution, you can create a Flow to perform any action (e.g. send an email) whenever the revision number has changed!  For example:

   ![](./docs/VersionChecker_Flow_1.png "Version Checker - Flow - Part 1")

   ![](./docs/VersionChecker_Flow_2.png "Version Checker - Flow - Part 2")

As an added bonus, the solution will also track the history of version changes!

   ![](./docs/VersionChecker_Log.png "Version Checker - Log")

### Configuration

Please refer to the [installation](./docs/INSTALL.md) documentation.