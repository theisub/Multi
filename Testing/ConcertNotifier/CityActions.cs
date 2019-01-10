namespace ConcertNotifier
{
    public class CityActions
    {
        public static string GetCity(string mess_city)
        {
            var city = "";
            switch (mess_city)
            {
                case "Москва":
                    city = "Москва";
                    break;
                case "Санкт-Петербург":
                    city = "Saint-Petersburg";
                    break;
                case "Казань":
                    city = "Kazan";
                    break;
                case "Новосибирск":
                    city = "Novosibirsk";
                    break;
                case "Екатеринбург":
                    city = "Ekaterinburg";
                    break;
                case "Moscow":
                    city = "Москва";
                    break;
                case "Saint-Petersburg":
                    city = "Санкт-Петербург";
                    break;
                case "Kazan":
                    city = "Казань";
                    break;
                case "Novosibirsk":
                    city = "Новосибирск";
                    break;
                case "Ekaterinburg":
                    city = "Екатеринбург";
                    break;
            }

            return city;
        }


    }

}