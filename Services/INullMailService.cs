namespace DutchTreat.Services
{
    public interface INullMailService
    {
        void SendMessage(string to, string subject, string body);
    }
}