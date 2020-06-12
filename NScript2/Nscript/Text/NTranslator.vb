Imports System.Runtime.InteropServices
Imports System.Text
Namespace NScript
    <ComVisible(True)> <ComClass()> Public Class NTranslator
        Private G1 As String = "咕", G2 = "唧", G3 = "嘎", G4 = "呜"
        Public Sub SetSPCLang(S1 As String, S2 As String, S3 As String, S4 As String)
            G1 = S1
            G2 = S2
            G3 = S3
            G4 = S4
        End Sub
        Public Function GetByte(str As String) As Integer
            If str Is Nothing Then
                Return 0
            End If
            Dim b1 As Byte
            Select Case str(0)
                Case G1
                    b1 = 0
                Case G2
                    b1 = 1
                Case G3
                    b1 = 2
                Case G4
                    b1 = 3
            End Select
            Return b1
        End Function
        Public Function GetSPCstr(num As Byte) As String
            Dim gu1 As Char
            Dim gu2 As Char
            Select Case num And 3
                Case 0
                    gu1 = G1
                Case 1
                    gu1 = G2
                Case 2
                    gu1 = G3
                Case 3
                    gu1 = G4
            End Select

            Select Case (num >> CByte(2)) And 3
                Case 0
                    gu2 = G1
                Case 1
                    gu2 = G2
                Case 2
                    gu2 = G3
                Case 3
                    gu2 = G4
            End Select
            Return gu1 & gu2
        End Function
        Public Function ChineseToSPC(str0 As String, Optional Pwd As String = "") As String
            If str0 Is Nothing Then
                Return ""
            Else
                If Pwd Is Nothing Then
                    Return CTS(str0)
                Else
                    Return CTS(str0, Pwd)
                End If
            End If
        End Function
        Friend Function CTS(Str0 As String, Optional Pwd As String = "") As String
            Dim strbulid As New StringBuilder
            If Pwd = "" Then
                strbulid.Append(G1 & "~")
            Else
                strbulid.Append(G1 & "~" & G1 & G1 & G1)
                Dim b As Short = Str0.GetHashCode() And Short.MaxValue
                Dim b10 As Byte = CByte(b And (2 ^ 4 - 1))
                Dim b20 As Byte = (b >> 12S) And (2 ^ 4 - 1)
                Dim b30 As Byte = (b >> &B1000S) And (2 ^ 4 - 1)
                Dim b40 As Byte = (b >> 4S) And (2 ^ 4 - 1)
                strbulid.Append(GetSPCstr(b10))
                strbulid.Append(GetSPCstr(b40))
                strbulid.Append(GetSPCstr(b30))
                strbulid.Append(GetSPCstr(b20))
            End If
            Dim p As Int32
            For Each Chr0 In Str0
                Dim a As Short = Asc(Chr0)
                If Pwd <> "" Then
                    a = a Xor (Pwd(p).GetHashCode() And Short.MaxValue)
                    p += 1
                    If Pwd.Length = p Then
                        p = 0
                    End If
                End If
                Dim b1 As Byte = CByte(a And (2 ^ 4 - 1))
                Dim b2 As Byte = (a >> 12S) And (2 ^ 4 - 1)
                Dim b3 As Byte = (a >> 8S) And (2 ^ 4 - 1)
                Dim b4 As Byte = (a >> 4S) And (2 ^ 4 - 1)
                strbulid.Append(GetSPCstr(b1))
                strbulid.Append(GetSPCstr(b4))
                strbulid.Append(GetSPCstr(b3))
                strbulid.Append(GetSPCstr(b2))
            Next
            Return strbulid.ToString()
        End Function
        Public Function SPCToChinese(str0 As String, Optional Pwd As String = "") As String
            If str0 Is Nothing Then
                Return ""
            Else
                If Pwd Is Nothing Then
                    Return STCFriend(str0)
                Else
                    Return STCFriend(str0, Pwd)
                End If
            End If
        End Function
        Friend Function STCFriend(str0 As String, Optional Pwd As String = "") As String
            If str0.StartsWith(G1 & "~", StringComparison.CurrentCulture) Then
                Dim guarray As New List(Of String)
                Dim str1 As String = str0.Substring(2)
                Dim haspwd As Boolean = False
                Dim hash As Short = 0
                If str0.StartsWith(G1 & "~" & G1 & G1 & G1, StringComparison.CurrentCulture) Then
                    str1 = str1.Substring(3)
                    Dim b0 As Integer = CInt(GetByte(str1(0)))
                    Dim b1 As Integer = CInt(GetByte(str1(1))) << 2
                    Dim b2 As Integer = CInt(GetByte(str1(2))) << 4
                    Dim b3 As Integer = CInt(GetByte(str1(3))) << 6
                    Dim b4 As Integer = CInt(GetByte(str1(4))) << 8
                    Dim b5 As Integer = CInt(GetByte(str1(5))) << 10
                    Dim b6 As Integer = CInt(GetByte(str1(6))) << 12
                    Dim b7 As Integer = CInt(GetByte(str1(7))) << 14
                    Dim sht As Int32 = b0 Or b1 Or b2 Or b3 Or b4 Or b5 Or b6 Or b7
                    hash = sht
                    str1 = str1.Substring(8)
                    haspwd = True
                End If
                If str1.Length Mod 8 = 0 Then
                    For i = 0 To str1.Length Step 8
                        If i = str1.Length Then
                            Exit For
                        End If
                        guarray.Add(str1.Substring(i, 8))
                    Next
                Else
                    MsgBox("咕咕这段话有!?¿或者是工地咕语")
                    Return "翻译失败咕"
                End If
                Dim strbulider As New StringBuilder
                Dim p As Int32
                For Each gua In guarray
                    Dim b0 As Integer = CInt(GetByte(gua(0)))
                    Dim b1 As Integer = CInt(GetByte(gua(1))) << 2
                    Dim b2 As Integer = CInt(GetByte(gua(2))) << 4
                    Dim b3 As Integer = CInt(GetByte(gua(3))) << 6
                    Dim b4 As Integer = CInt(GetByte(gua(4))) << 8
                    Dim b5 As Integer = CInt(GetByte(gua(5))) << 10
                    Dim b6 As Integer = CInt(GetByte(gua(6))) << 12
                    Dim b7 As Integer = CInt(GetByte(gua(7))) << 14
                    Dim sht As Int32 = b0 Or b1 Or b2 Or b3 Or b4 Or b5 Or b6 Or b7
                    If haspwd Then
                        strbulider.Append(Chr(sht Xor (Pwd(p).GetHashCode() And Short.MaxValue)))
                        p += 1
                        If Pwd.Length = p Then
                            p = 0
                        End If
                    Else
                        strbulider.Append(Chr(sht))
                    End If
                Next
                If ((strbulider.ToString().GetHashCode() And Short.MaxValue) = hash) Or Not (haspwd) Then
                    Return strbulider.ToString()
                Else
                    MsgBox("密码不正确")
                    Return "翻译失败咕"
                End If
            Else
                MsgBox("你这根本不是咕语吧咕咕")
                Return "翻译失败咕"
            End If
            Return "翻译失败咕"
        End Function
    End Class
End Namespace
