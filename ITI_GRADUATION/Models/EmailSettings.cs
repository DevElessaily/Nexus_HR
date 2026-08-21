namespace ITI_GRADUATION.Models
{
    // Bound from the "EmailSettings" section in appsettings.json.
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string SenderName { get; set; } = "Nexus HR";
        public string SenderEmail { get; set; } = "";
        public string SenderPassword { get; set; } = "";
        public string NotificationRecipient { get; set; } = "";
    }
}
