namespace EventBus
{
    public class RabbitMqConnectionOptions
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public int ConnectFailureDelay { get; set; }
        public int ConnectTimeout { get; set; }
    }
}
