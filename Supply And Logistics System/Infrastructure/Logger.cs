namespace Supply_And_Logistics_System.Infrastructure
{
    /// <summary>
    /// Uygulama genelinde merkezi kayıt (logging) işlemlerini yöneten sınıf.
    /// Singleton Pattern kullanılarak tüm sistemde 
    /// sadece tek bir Logger örneğinin (instance) olması garanti edilir.
    /// </summary>
    public class Logger
    {
        // Logger'ın tekil örneğini tutan statik değişken.
        private static Logger _instance;

        // Çoklu iş parçacığı (multi-threading) ortamında güvenli nesne oluşturma için kilit nesnesi.
        private static readonly object _lock = new object();

        // Log kayıtlarının yazılacağı hedef dosya adı.
        private string _logFile = "log.txt";

        // Yapıcı metodun private (özel) olması, dışarıdan "new Logger()" ile 
        // yeni örnekler oluşturulmasını engeller.
        private Logger() { }

        // Logger örneğine erişim sağlayan küresel noktadır.
        // "Double-Check Locking" yöntemi ile thread-safe bir başlatma sağlar.
        public static Logger GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                    }
                }
            }

            return _instance;
        }

        // Belirtilen mesajı zaman damgasıyla birlikte log dosyasına ekler.
        public void Log(string message)
        {
            string logEntry = $"[{DateTime.Now}] {message}";

            // Mesajı dosyaya ekler (Dosya yoksa oluşturur).
            File.AppendAllText(_logFile, logEntry + Environment.NewLine);
        }
    }
}
