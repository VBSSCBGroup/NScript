Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports Gma
Namespace NScript.QrCode
    <ComVisible(True), ComClass> Public Class QrCodeEncoding
        Private IQrCode As Gma.QrCodeNet.Encoding.QrEncoder
        Private Shared IsAppendEvent As Boolean
        Public Sub New()
            If Not IsAppendEvent Then
                AddHandler AppDomain.CurrentDomain.AssemblyResolve, Function(sender As Object, e As ResolveEventArgs) As Assembly
                                                                        If e.Name = "Gma.QrCodeNet.Encoding" And (e.RequestingAssembly Is Nothing) Then
                                                                            If Debugger.Launch() Then
                                                                                If Debugger.IsAttached Then
                                                                                    Debugger.Break()
                                                                                Else
                                                                                    Throw New FileLoadException()
                                                                                End If
                                                                            Else
                                                                                Environment.Exit(New FileLoadException().HResult)
                                                                            End If
                                                                        End If
                                                                        Return Nothing
                                                                    End Function
                IsAppendEvent = True
            End If
            Init()
        End Sub
        Friend Sub Init()
            IQrCode = New Gma.QrCodeNet.Encoding.QrEncoder
        End Sub
        Public Enum ReturnType
            Bools
            StarString
            BlockString
        End Enum
        Public Function GetQrCodeByStr(str As String, Optional RT As ReturnType = ReturnType.BlockString) As Object
            IQrCode.ErrorCorrectionLevel = QrCodeNet.Encoding.ErrorCorrectionLevel.M
            Dim qrcode As New QrCodeNet.Encoding.QrCode()
            If Not IQrCode.TryEncode(str, qrcode) Then
                Throw New NotSupportedException()
            Else
                Dim qrbitmx As QrCodeNet.Encoding.BitMatrix = qrcode.Matrix
                Select Case RT
                    Case ReturnType.Bools
                        Return qrbitmx.InternalArray
                    Case ReturnType.BlockString
                        Dim strbulid As New StringBuilder()
                        For i = 0 To qrbitmx.Height - 1
                            For o = 0 To qrbitmx.Width - 1
                                If qrbitmx.InternalArray()(i, o) Then
                                    strbulid.Append("▉")
                                Else
                                    strbulid.Append("  ")
                                End If
                            Next
                            strbulid.Append(vbCrLf)
                        Next
                        Return strbulid.ToString()
                        ' Case ReturnType.StarString
                    Case Else
                        Throw New NotImplementedException()
                End Select
            End If

        End Function
    End Class
End Namespace
