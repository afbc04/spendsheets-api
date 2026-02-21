public class Money {

    public static decimal Format(long money) {
        return money / 100m;
    }

    public static decimal Format(int money) {
        return money / 100m;
    }

    public static long Convert64(double money) {

        decimal value = System.Convert.ToDecimal(money);
        return (long) Math.Round(value * 100m, MidpointRounding.AwayFromZero);

    }

    public static int Convert32(double money) {

        decimal value = System.Convert.ToDecimal(money);
        return (int) Math.Round(value * 100m, MidpointRounding.AwayFromZero);

    }

    public static int Convert32(int money) {

        decimal value = System.Convert.ToDecimal(money);
        return (int) Math.Round(value * 100m, MidpointRounding.AwayFromZero);

    }

}