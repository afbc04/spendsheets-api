public class Utils {

    public static decimal convert_to_money(long money) {
        return money / 100m;
    }

    public static decimal convert_to_money(int money) {
        return money / 100m;
    }

    public static long convert_from_money(double money) {

        decimal value = Convert.ToDecimal(money);
        return (long) Math.Round(value * 100m, MidpointRounding.AwayFromZero);

    }

    public static string convert_to_datetime(DateTime date) {
        return date.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static long? to_number(string number) {

        if (string.IsNullOrWhiteSpace(number))
            return null;

        if (long.TryParse(number, out long result))
            return result;

        return null;
        
    }


}