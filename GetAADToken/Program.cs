//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace GetAADToken
{
    internal class Constants
    {
        public const string AAD_Prod_OAuthAuthority = "https://login.microsoftonline.com/common";

        public const string Print_Prod_OAuthResourceUri = "https://print.print.microsoft.com";

        public const string Print_Prod_OAuthRedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

        // Multi-tenant app registered in universalprinttest.onmicrosoft.com tenant
        public const string Print_Prod_OAuthClientId = "2e8ebe07-1160-4287-b789-a31e5072383a";


        public const string TokenFileName = "token.txt";
    }

    internal class Program
    {
        /// <summary>
        /// Use Azure identity library to generate logged in user token and save it to a file.
        /// </summary>
        /// <param name="prompt">True to prompt for credentials.</param>
        /// <returns> 0 if successful, -1 if failed.</returns>
        private static async Task<int> GetTokenAsync()
        {
            try
            {
                // Do nothing if token is new.  User needs to delete the token.txt to force prompting.
                const int expiredMinutes = 60;
                DateTime lastWriteTime = File.GetLastWriteTimeUtc(Constants.TokenFileName);
                if (lastWriteTime.AddMinutes(expiredMinutes) > DateTime.Now.ToUniversalTime())
                {
                    return 0;
                }

                Console.WriteLine("GetAADToken: Getting new user token. Current token is either not available or old.");
                // Request for new one.
                var ac = new AuthenticationContext(Constants.AAD_Prod_OAuthAuthority, false);
                var result = await ac.AcquireTokenAsync(
                                        Constants.Print_Prod_OAuthResourceUri,
                                        Constants.Print_Prod_OAuthClientId,
                                        new Uri(Constants.Print_Prod_OAuthRedirectUri),
                                        new PlatformParameters(PromptBehavior.Always, Process.GetCurrentProcess().MainWindowHandle));

                var accessToken = result.AccessToken;
                File.WriteAllText(Constants.TokenFileName, accessToken);
                return 0;
            }
            catch (Exception exp)
            {
                Console.WriteLine("GetAADToken.exe failed: " + exp.Message);
                return -1;
            }
        }

        private static int Main(string[] args)
        {
            int i = 0;
            while (i < args.Count())
            {
                var argument = args[i++];
                switch (argument.ToLower())
                {
                    default:
                        ShowHelp();
                        return 0;
                }
            }

            var result = GetTokenAsync().Result;
            return result;
        }

        /// <summary>
        /// Available commands.
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine("Usage:  GetAADToken.exe");
            Console.WriteLine();
            Console.WriteLine("hint: Delete token.txt file to refresh the access token.");
        }
    }
}

