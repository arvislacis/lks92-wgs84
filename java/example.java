import java.lang.Math;

class MainClass
{
    public static String testCase(double[] coordinates)
    {
        double[] converted = LKS92WGS84.convertXYToLatLon(LKS92WGS84.convertLatLonToXY(coordinates));
        String result;

        if (Math.round(converted[0] * Math.pow(10, 8)) / Math.pow(10, 8) == Math.round(coordinates[0] * Math.pow(10, 8)) / Math.pow(10, 8) && Math.round(converted[1] * Math.pow(10, 8)) / Math.pow(10, 8) == Math.round(coordinates[1] * Math.pow(10, 8)) / Math.pow(10, 8)) {
            result = "izpildās";
        } else {
            result = "neizpildās";
        }

        return result;
    }

    public static String testCase2(double[] coordinates, double[] lksValidate)
    {
        double[] converted = LKS92WGS84.convertLatLonToXY(coordinates);
        String result;

        if (Math.round(converted[0] * Math.pow(10, 2)) / Math.pow(10, 2) == lksValidate[0] && Math.round(converted[1] * Math.pow(10, 2)) / Math.pow(10, 2) == lksValidate[1]) {
            result = "izpildās";
        } else {
            result = "neizpildās";
        }

        return result;
    }

    public static void main(String[] args)
    {
        double[] coordinates = {58.079501574948, 25.189986971284};
        System.out.println("\"Baltās naktis\" - Latvijas tālākais ziemeļu punkts [" + coordinates[0] + ", " + coordinates[1] + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, new double[] {570181.00, 438180.00}));
        coordinates = new double[] {56.172282784562, 28.095216442873};
        System.out.println("\"Austras koks\" - Latvijas tālākais austrumu punkts [" + coordinates[0] + ", " + coordinates[1] + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, new double[] {754190.00, 232806.00}));
        coordinates = new double[] {55.675228242509, 26.580528487143};
        System.out.println("\"Saules puķe\" - Latvijas tālākais dienvidu punkts [" + coordinates[0] + ", " + coordinates[1] + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, new double[] {662269.00, 172953.00}));
        coordinates = new double[] {56.377008455189, 20.979185882058};
        System.out.println("\"Zaļais stars\" - Latvijas tālākais rietumu punkts [" + coordinates[0] + ", " + coordinates[1] + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, new double[] {313470.00, 252137.00}));
    }
}
