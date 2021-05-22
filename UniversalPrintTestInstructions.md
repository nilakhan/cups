Microsoft Universal Print Printer Attributes Test Tool

PRE-REQUISITES
1.      Azure AD tenant:
        o       Tenant must have Universal Print license.  Please refer to (https://docs.microsoft.com/en-us/universal-print/fundamentals/universal-print-license "License Universal Print | Microsoft Docs") for details.
        o       User account must be assigned a Universal Print license. Please refer to (https://docs.microsoft.com/en-us/universal-print/fundamentals/universal-print-getting-started "Set up Universal Print | Microsoft Docs") for details.
        o       Test printer must be registered to Universal Print.
        o       Test printer is shared and granted permissions to user account.  Please refer to (https://docs.microsoft.com/en-us/universal-print/portal/share-printers "Sharing Printers using the Universal Print portal | Microsoft Docs") for details.
2.      Windows 10 client
        o       Any recent release of Windows 10 OS, such as 2004 or 20H2.
        o       The logged in user account does not matter.
        o       MacOS is not supported.

INSTRUCTIONS TO USE
1.      Open the CUPS Visual Studio Solution in Visual Studio 2019 (Enterprise or Community).
2.      Right click on the solution in the Solution Explorer, and click on Configuration Manager. Check all 10 projects under the Build column.
3.      Build the entire solution.
4.      Start a DOS command prompt in Administrator mode, and change to the output directory of the build: (e.g., ipptool-github\vcnet\x64\Debug)
5.      Run the command ipptool.exe -t https://print.print.microsoft.com/printers/<shared-device-id or cloud-device-id> ..\..\..\UniversalPrintTest\UniversalPrintPrinterAttributes.test
6.      Log into your AAD tenant account when prompted.
7.      The output is a list of the missing/incorrect attributes that failed the test. Further details for each attribute can be found at any of the following online resources:
        1.  (https://www.iana.org/assignments/ipp-registrations/ipp-registrations.xhtml "Internet Printing Protocol Registrations")
        2.  (https://ftp.pwg.org/pub/pwg/candidates/cs-ippjobprinterext10-20101030-5100.11.pdf "Internet Printing Protocol: Jobs and Extensions")
        3.  (https://datatracker.ietf.org/doc/html/rfc8011 "Internet Printing Protocol/1.1: Models and Semantics")
8.      The command to use the other test files, such as ipp-1.1.test, is provided in the comment section of their files.
9.      Besides the attribute test tools in the folder ipptool-github\UniversalPrintTest, there are some example test files provided by CUPS in the folder ipptool-github\examples, which you may also use. Some useful ones might include get-jobs.test and print-job.test.

TROUBLESHOOTING
1.      If ipptool hangs when it is run, try renewing the access token of your printer by deleting the token.txt file (located in the same folder where you're running the tool) and re-running the ipptool.
2.      Double check that in the output after ipptool finishes running, you see the following line in the output:
                status-code = successful-ok (successful-ok)
        If you see something else, for example something like the following:
                status-code = client-error-gone (client-error-gone)
                EXPECTED: status successful-ok (got client-error-gone)
        In such a case, it is likely that you mistyped the device ID of your printer.
3.      If the solution is not building correctly, try right-clicking on the solution and clicking "Restore NuGet Packages".