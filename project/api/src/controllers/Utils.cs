public class Utils {

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