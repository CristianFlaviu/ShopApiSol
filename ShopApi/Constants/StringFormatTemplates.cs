﻿namespace ShopApi.Constants
{
    public static class StringFormatTemplates
    {
        public static string EmailMessageBody = "Hello,  {0} ! \n\nPlease click on the link below for confirming your email https://localhost:5001/api/auth/confirm-email?username={1}&token={2} \n\n Best Regards, ShopOnlineApp2021";
        public static string EmailSentSuccessfully = "Register email sent successfully to {0}\n";
        public static string EmailFailedToSend = "The email could not be sent {0}\n";
    }
}
