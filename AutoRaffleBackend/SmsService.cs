namespace AutoRaffleBackend
{
    using Twilio;
    using Twilio.Rest.Api.V2010.Account;

    public class SmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendSms(string toPhoneNumber, string message)
        {
            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];
            var fromPhoneNumber = _configuration["Twilio:PhoneNumber"];

            TwilioClient.Init(accountSid, authToken);

            var messageResource = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(fromPhoneNumber),
                to: new Twilio.Types.PhoneNumber(toPhoneNumber)
            );

            Console.WriteLine($"SMS trimis către {toPhoneNumber}: {messageResource.Sid}");
        }
    }

}
