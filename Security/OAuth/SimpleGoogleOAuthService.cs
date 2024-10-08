using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;

namespace SimpleUtilities.Security.OAuth {
    public static class SimpleGoogleOAuthService {
        public static async Task<string> GetUserEmail(string clientId, string redirectUri = "http://localhost:5000/", int port = 5000) {
            var options = new OidcClientOptions {
                Authority = "https://accounts.google.com",
                ClientId = clientId,
                RedirectUri = redirectUri,
                Scope = "openid email profile",
                Browser = new SystemBrowser(port),
                Policy = new Policy {
                    RequireIdentityTokenSignature = false
                }
            };

            var oidcClient = new OidcClient(options);
            var loginResult = await oidcClient.LoginAsync(new LoginRequest());

            if (loginResult.IsError) throw new Exception($"Login error: {loginResult.Error}");
            
            var email = loginResult.User.FindFirst("email")?.Value;

            if(email == null) throw new Exception("Email not found");

            return email;
        }



        public class SystemBrowser : IBrowser {
            private readonly int port;

            public SystemBrowser(int port) {
                this.port = port;
            }

            public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default) {
                var listener = new HttpListener();
                listener.Prefixes.Add(options.EndUrl);
                listener.Start();

                try {
                    Process.Start(new ProcessStartInfo {
                        FileName = options.StartUrl,
                        UseShellExecute = true
                    });

                    var context = await listener.GetContextAsync();

                    var responseString = "<html><head><meta http-equiv='refresh' content='10;url=https://www.google.com'></head><body>Autenticación completada. Puedes cerrar esta ventana.</body></html>";
                    var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    var response = context.Response.OutputStream;

                    await response.WriteAsync(buffer, 0, buffer.Length);
                    response.Close();

                    var url = context.Request.Url.ToString();
                    return new BrowserResult
                    {
                        ResultType = BrowserResultType.Success,
                        Response = url
                    };
                }
                catch (Exception ex) {
                    return new BrowserResult
                    {
                        ResultType = BrowserResultType.UnknownError,
                        Error = ex.Message
                    };
                }
                finally {
                    listener.Stop();
                }
            }
        }

    }
}
