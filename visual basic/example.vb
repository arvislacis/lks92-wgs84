Imports System

Module Program
    Function testCase(coordinates As Double()) As String
        Dim converted As Double() = LKS92WGS84.convertXYToLatLon(LKS92WGS84.convertLatLonToXY(coordinates))
        Dim result As String

        If Math.Round(converted(0), 8) = Math.Round(coordinates(0), 8) And Math.Round(converted(1), 8) = Math.Round(coordinates(1), 8) Then
            result = "izpildās"
        Else
            result = "neizpildās"
        End if

        Return result
    End Function

    Function testCase2(coordinates As Double(), lksValidate As Double()) As String
        Dim converted As Double() = LKS92WGS84.convertLatLonToXY(coordinates)
        Dim result As String

        If Math.Round(converted(0), 2) = lksValidate(0) And Math.Round(converted(1), 2) = lksValidate(1) Then
            result = "izpildās"
        Else
            result = "neizpildās"
        End If

        Return result
    End Function

    Sub Main()
        Dim coordinates = New Double() {58.079501574948, 25.189986971284}
        Console.WriteLine("""Baltās naktis"" - Latvijas tālākais ziemeļu punkts [" + String.Join(", ", coordinates) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, New Double() {570181.00, 438180.00}))
        coordinates = New Double() {56.172282784562, 28.095216442873}
        Console.WriteLine("""Austras koks"" - Latvijas tālākais austrumu punkts [" + String.Join(", ", coordinates) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, New Double() {754190.00, 232806.00}))
        coordinates = New Double() {55.675228242509, 26.580528487143}
        Console.WriteLine("""Saules puķe"" - Latvijas tālākais dienvidu punkts [" + String.Join(", ", coordinates) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, New double() {662269.00, 172953.00}))
        coordinates = New Double() {56.377008455189, 20.979185882058}
        Console.WriteLine("""Zaļais stars"" - Latvijas tālākais rietumu punkts [" + String.Join(", ", coordinates) + "] => " + testCase(coordinates) + " => " + testCase2(coordinates, New Double() {313470.00, 252137.00}))
    End Sub
End Module
