#include <cmath>
#include <iomanip>
#include <iostream>
#include "LKS92WGS84.h"

using namespace std;

string testCase(double *coordinates)
{
    double *converted = LKS92WGS84::convertXYToLatLon(LKS92WGS84::convertLatLonToXY(coordinates));
    string result;

    if (round(converted[0] * pow(10, 8)) / pow(10, 8) == round(coordinates[0] * pow(10, 8)) / pow(10, 8) && round(converted[1] * pow(10, 8)) / pow(10, 8) == round(coordinates[1] * pow(10, 8)) / pow(10, 8)) {
        result = "izpildās";
    } else {
        result = "neizpildās";
    }

    return result;
}

string testCase2(double *coordinates, double *lksValidate)
{
    double *converted = LKS92WGS84::convertLatLonToXY(coordinates);
    string result;

    if (round(converted[0] * pow(10, 2)) / pow(10, 2) == lksValidate[0] && round(converted[1] * pow(10, 2)) / pow(10, 2) == lksValidate[1]) {
        result = "izpildās";
    } else {
        result = "neizpildās";
    }

    return result;
}

int main()
{
    double *coordinates = new double[2] {58.079501574948, 25.189986971284};
    cout << "\"Baltās naktis\" - Latvijas tālākais ziemeļu punkts [" << setprecision(14) << coordinates[0] << ", " << setprecision(14) << coordinates[1] << "] => " << testCase(coordinates) << " => " << testCase2(coordinates, new double[2] {570181.00, 438180.00}) << endl;
    coordinates = new double[2] {56.172282784562, 28.095216442873};
    cout << "\"Austras koks\" - Latvijas tālākais austrumu punkts [" << setprecision(14) << coordinates[0] << ", " << setprecision(14) << coordinates[1] << "] => " << testCase(coordinates) << " => " << testCase2(coordinates, new double[2] {754190.00, 232806.00}) << endl;
    coordinates = new double[2] {55.675228242509, 26.580528487143};
    cout << "\"Saules puķe\" - Latvijas tālākais dienvidu punkts [" << setprecision(14) << coordinates[0] << ", " << setprecision(14) << coordinates[1] << "] => " << testCase(coordinates) << " => " << testCase2(coordinates, new double[2] {662269.00, 172953.00}) << endl;
    coordinates = new double[2] {56.377008455189, 20.979185882058};
    cout << "\"Zaļais stars\" - Latvijas tālākais rietumu punkts [" << setprecision(14) << coordinates[0] << ", " << setprecision(14) << coordinates[1] << "] => " << testCase(coordinates) << " => " << testCase2(coordinates, new double[2] {313470.00, 252137.00}) << endl;
}
