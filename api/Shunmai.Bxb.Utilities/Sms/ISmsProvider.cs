namespace Shunmai.Bxb.Utilities.Sms
{
    public interface ISmsProvider
    {
        string Api { get; set; }

        string Content { get; set; }

        ResponseEntity Send(string phoneNo, int count = 4);

        ResponseEntity SendCode(string phoneNo, string code);

        ResponseEntity Send(string phoneNo,string content);

    }
}
