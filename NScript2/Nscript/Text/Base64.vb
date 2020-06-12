Imports System.Runtime.InteropServices
Imports System.Text
Namespace NScript
    <ComClass()> <ComVisible(True)> Public Class Base64
        Public Function ToBase64(<MarshalAs(UnmanagedType.SafeArray)> data As Byte()) As String
            Return Convert.ToBase64String(data)
        End Function
        Public Function ToRaw(data As String) As Byte()
            Return Convert.FromBase64String(data)
        End Function
        Public Function ToBase64S(data As String) As String
            Dim bytes As Byte() = Encoding.Default.GetBytes(data)
            Return Convert.ToBase64String(bytes)
        End Function
    End Class
End Namespace
