Imports System.Runtime.InteropServices
Imports System.Text
Imports Enm = NScript.TextReEncoding.TextEncoding
Namespace NScript
    <ComClass> <ComVisible(True)> Public Class TextReEncoding
        Public Enum TextEncoding
            ASCII
            UTF8
            UTF7
            UTF32
        End Enum
        Public Function ReEncoding(Source As String, Optional SourceEncoding As Enm = Enm.UTF8, Optional TargetEncoding As Enm = Enm.ASCII)
            If String.IsNullOrEmpty(Source) Then Return ""
            Dim SceEing As Encoding = GetEncoding(SourceEncoding)
            Dim TgtEing As Encoding = GetEncoding(TargetEncoding)
            Return TgtEing.GetString(SceEing.GetBytes(Source))
        End Function
        Public Function GetEncoding(Encoding As Enm) As Encoding
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
    End Class

End Namespace