<?php

require_once "LKS92WGS84.php";

function testCase(array $coordinates)
{
    $converted = LKS92WGS84::convertXYToLatLon(LKS92WGS84::convertLatLonToXY($coordinates));

    if (round($converted[0], 8) == round($coordinates[0], 8) and round($converted[1], 8) == round($coordinates[1], 8)) {
        $result = "izpildās";
    } else {
        $result = "neizpildās";
    }

    return $result;
}

function testCase2(array $coordinates, array $lksValidate)
{
    $converted = LKS92WGS84::convertLatLonToXY($coordinates);

    if (round($converted[0], 2) == $lksValidate[0] && round($converted[1], 2) == $lksValidate[1]) {
        $result = "izpildās";
    } else {
        $result = "neizpildās";
    }

    return $result;
}

$coordinates = [58.079501574948, 25.189986971284];
echo "\"Baltās naktis\" - Latvijas tālākais ziemeļu punkts [" . implode(", ", $coordinates) . "] => " . testCase($coordinates) . " => " . testCase2($coordinates, [570181.00, 438180.00]) . "\n";
$coordinates = [56.172282784562, 28.095216442873];
echo "\"Austras koks\" - Latvijas tālākais austrumu punkts [" . implode(", ", $coordinates) . "] => " . testCase($coordinates) . " => " . testCase2($coordinates, [754190.00, 232806.00]) . "\n";
$coordinates = [55.675228242509, 26.580528487143];
echo "\"Saules puķe\" - Latvijas tālākais dienvidu punkts [" . implode(", ", $coordinates) . "] => " . testCase($coordinates) . " => " . testCase2($coordinates, [662269.00, 172953.00]) . "\n";
$coordinates = [56.377008455189, 20.979185882058];
echo "\"Zaļais stars\" - Latvijas galējais rietumu punkts [" . implode(", ", $coordinates) . "] => " . testCase($coordinates) . " => " . testCase2($coordinates, [313470.00, 252137.00]) . "\n";
