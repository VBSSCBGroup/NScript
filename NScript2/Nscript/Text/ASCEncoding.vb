Imports System.Net.WebRequestMethods
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text

Imports Enm = NScript.TextReEncoding.TextEncoding
Namespace NScript.Text
    <ComClass(), ComVisible(True)> Public Class StrEncoding
        Friend _ending As Encoding = Encoding.ASCII
        Public Function ReEncoding(Source As String, Optional SourceEncoding As Enm = Enm.UTF8, Optional TargetEncoding As Enm = Enm.ASCII)
            If String.IsNullOrEmpty(Source) Then Return ""
            Dim SceEing As Encoding = GetEncoding(SourceEncoding)
            Dim TgtEing As Encoding = GetEncoding(TargetEncoding)
            Return TgtEing.GetString(SceEing.GetBytes(Source))
        End Function
        Public Sub SetEncoding(Encoding As Enm)
            _ending = GetEncoding(Encoding)
        End Sub
        Friend Function GetEncoding(Encoding As Enm) As Encoding
            Select Case Encoding
                Case Enm.ASCII
                    Return System.Text.Encoding.ASCII
                Case Enm.UTF8
                    Return System.Text.Encoding.UTF8
                Case Enm.UTF7
                    Return System.Text.Encoding.UTF7
                Case Enm.UTF32
                    Return System.Text.Encoding.UTF32
                Case Else
                    Throw New ArgumentOutOfRangeException(NameOf(Encoding))
            End Select
        End Function
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