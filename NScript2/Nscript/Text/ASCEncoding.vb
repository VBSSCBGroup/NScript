Imports System.Net.WebRequestMethods
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text
Imports System.Runtime.Serialization
Namespace NScript.Text
    <ComClass(), ComVisible(True)> Public Class ASCEncoding
        Friend _ending As Encoding = New ASCIIEncoding()
        Public Function GetString(<MarshalAs(UnmanagedType.SafeArray)> bytes As Byte()) As String
            Return _ending.GetString(bytes)
        End Function
        Public Function GetBytes(Str As String) As <MarshalAs(UnmanagedType.SafeArray)> Byte()
            Return _ending.GetBytes(Str)
        End Function
        Public Function GetChars(Str As String) As String()
            Return If(Str Is Nothing, {}, Str.Cast(Of String).ToArray())
        End Function
    End Class
End Namespace