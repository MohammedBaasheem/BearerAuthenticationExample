namespace BearerAuthentication.JwtOptions
{
    public class Jwt
    {
        public string SigningKey { get; set; }
        public string Issuer { get; set; }
        public string Audiense { get; set; }
        public int Lifetime { get; set; }
    }

}
