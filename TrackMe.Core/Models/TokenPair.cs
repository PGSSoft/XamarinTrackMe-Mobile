namespace TrackMe.Core.Models
{
    public class TokenPair
    {
        public TokenPair()
        {
            
        }
        public TokenPair(string publicToken, string privateToken)
        {
            PublicToken = publicToken;
            PrivateToken = privateToken;
        }

        public string PublicToken { get; set; }
        public string PrivateToken { get; set; }
    }
}
