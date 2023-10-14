using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace AdminSite.Services;

public class FcmService
{
    public readonly FirebaseMessaging FirebaseMessaging;

    public FcmService()
    {
        var app = FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "push-notify-87a05-firebase-adminsdk.json")),
        });
        FirebaseMessaging = FirebaseMessaging.GetMessaging(app);
    }
}