using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LinqToTwitter;
using LinqToTwitter.OAuth;

namespace HelloTwitter
{
    class Program
    {
        static async Task Main()
        {
            TwitterContext twitterCtx = await GetTwitterContext();

            Console.WriteLine("Tweeting...");

            await twitterCtx.TweetAsync(
                "just setting up my #LinqToTwitter " +
                "by @JoeMayo: http://bit.ly/GetDotNet6" +
                "\n\n#dotnet");

            Console.WriteLine("Tweet Sent!");
        }

        static async Task<TwitterContext> GetTwitterContext()
        {
            var auth = new PinAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = "",
                    ConsumerSecret = ""
                },
                GoToTwitterAuthorization = pageLink =>
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = pageLink,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                },
                GetPin = () =>
                {
                    Console.WriteLine(
                        "\nAfter authorizing this application, Twitter " +
                        "will give you a 7-digit PIN Number.\n");
                    Console.Write("Enter the PIN number here: ");
                    return Console.ReadLine() ?? string.Empty;
                }
            };

            await auth.AuthorizeAsync();

            var twitterCtx = new TwitterContext(auth);
            return twitterCtx;
        }
    }
}
