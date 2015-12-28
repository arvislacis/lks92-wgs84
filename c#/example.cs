using lks92_wgs84;
using System;
using System.IO;

class MainClass
{
    public static string testCase(double[] coordinates)
    {
        double[] converted = LKS92WGS84.convertXYToLatLon(LKS92WGS84.convertLatLonToXY(coordinates));
        string result;

        if (Math.Round(converted[0], 8) == Math.Round(coordinates[0], 8) && Math.Round(converted[1], 8) == Math.Round(coordinates[1], 8)) {
            result = "izpildās";
        } else {
            result = "neizpildās";
        }

        return result;
    }

    public static string testCase2(double[] coordinates, double[] lksValidate)
    {
        double[] converted = LKS92WGS84.convertLatLonToXY(coordinates);
        string result;

        if (Math.Round(converted[0], 2) == lksValidate[0] && Math.Round(converted[1], 2) == lksValidate[1]) {
            result = "izpildās";
        } else {
            result = "neizpildās";
        }

        return result;
    }

    public static void Main()
    {
        double[] coordinates = new double[2] {58.079501574948, 25.189986971284};
        Console.WriteLine("\"Baltās naktis\" - Latvijas tālākais ziemeļu punkts [" + String.Join(", ", coordinates) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, new double[2] {570181.00, 438180.00}));
        coordinates = new double[2] {56.172282784562, 28.095216442873};
        Console.WriteLine("\"Austras koks\" - Latvijas tālākais austrumu punkts [" + String.Join(", ", coordinates) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, new double[2] {754190.00, 232806.00}));
        coordinates = new double[2] {55.675228242509, 26.580528487143};
        Console.WriteLine("\"Saules puķe\" - Latvijas tālākais dienvidu punkts [" + String.Join(", ", coordinates) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, new double[2] {662269.00, 172953.00}));
        coordinates = new double[2] {56.377008455189, 20.979185882058};
        Console.WriteLine("\"Zaļais stars\" - Latvijas tālākais rietumu punkts [" + String.Join(", ", coordinates) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, new double[2] {313470.00, 252137.00}));
    }
}
