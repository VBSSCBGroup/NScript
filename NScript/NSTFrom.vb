Imports System.CodeDom.Compiler
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Net.WebRequestMethods
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports IWshRuntimeLibrary
Imports File = System.IO.File
Imports NScript.Drawing
Imports Gma
Imports System.Net
Imports System.Runtime.Serialization

Namespace NScript.Net
    <ComVisible(True), ComClass> Public NotInheritable Class Socket
        Implements IDisposable
        Private socket As System.Net.Sockets.Socket
        Private Sub New()

        End Sub
        Public Sub Send(data As String)
            socket.Send(Encoding.Unicode.GetBytes(data))
        End Sub
        Public Sub Revice(data As Byte())
            socket.Receive(data)
        End Sub
        Public Sub Init(ip As String, port As Short)
            socket = New System.Net.Sockets.Socket(System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Tcp)
            socket.Bind(New IPEndPoint(IPAddress.Parse(ip), port))
        End Sub
        Public Sub ReviceStr(ByRef data As String)
            Dim data2(65535) As Byte
            socket.Receive(data2)
            data = Encoding.Unicode.GetString(data2)
            data2 = Nothing
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            DirectCast(socket, IDisposable).Dispose()
            GC.SuppressFinalize(Me)
        End Sub

        Public Function Accept() As Socket
            Dim socket2 As New Socket With {
                .socket = socket.Accept()
            }
            Return socket2
        End Function

    End Class
End Namespace
Namespace NScript.QrCode
    <ComVisible(True), ComClass> Public Class QrCodeEncoding
        Private IQrCode As Gma.QrCodeNet.Encoding.QrEncoder
        Private Shared IsAppendEvent As Boolean
        Public Sub New()
            If Not IsAppendEvent Then
                AddHandler AppDomain.CurrentDomain.AssemblyResolve, Function(sender As Object, e As ResolveEventArgs) As Assembly
                                                                        If e.Name = "Gma.QrCodeNet.Encoding" Then
                                                                            Return Assembly.Load(My.Resources.Gma_QrCodeNet_Encoding)
                                                                        Else
                                                                            Return Nothing
                                                                        End If
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
Namespace NScript.Drawing
    <ComClass()> <ComVisible(True)> Public Class CPT
        Private _Value As New Point
        Public Sub New()

        End Sub
        Public Sub New(value As Point)
            Me._Value = value
        End Sub
        Public Property X As Int32
            Get
                Return _Value.X
            End Get
            Set(value As Int32)
                _Value.X = value
            End Set
        End Property
        Public Property Y As Int32
            Get
                Return _Value.Y
            End Get
            Set(value As Int32)
                _Value.Y = value
            End Set
        End Property
        Public Shared Widening Operator CType(point As CPT) As Point
            If point Is Nothing Then
                Return New Point()
            End If
            Return point._Value
        End Operator
        Public Shared Widening Operator CType(point As Point) As CPT
            Return New CPT() With {._Value = point}
        End Operator

        Public Function ToPoint() As Point
            Throw New NotImplementedException()
        End Function

        Public Function ToCPT() As CPT
            Throw New NotImplementedException()
        End Function
    End Class
    <ComVisible(True)> <ComClass> Public Class NPen
        Public Const NullPen = Nothing
        Public Property BasePen As Pen
        Public Sub New()
            BasePen = Pens.Black
        End Sub
        Public Property Color As Int32
            Get
                Return ColorTranslator.ToWin32(BasePen.Color)
            End Get
            Set(value As Int32)
                BasePen.Color = System.Drawing.Color.FromArgb(value)
            End Set
        End Property
        Public Property Width As Single
            Get
                Return BasePen.Width
            End Get
            Set(value As Single)
                BasePen.Width = value
            End Set
        End Property

    End Class
    <ComVisible(True)> <ComClass()> Public Class NGDI
        Friend GDI As Graphics
        Private ReadOnly Locker As New Object
        Public ReadOnly Property BaseGraphics As Graphics
            Get
                SyncLock Locker
                    If GDI Is Nothing Then
                        Throw New NotSupportedException()
                    End If
                    Return GDI
                End SyncLock
            End Get
        End Property
        Public Sub Init(BaseGraphics As Graphics)
            If BaseGraphics Is Nothing Then

            Else
                If TypeOf BaseGraphics Is Graphics Then
                Else
                    Throw New NotImplementedException()
                End If
            End If
            GDI = BaseGraphics
        End Sub
        Public Sub DrawLine(Color As Int32, width As Single, x1 As Single, y1 As Single, x2 As Single, y2 As Single)
            SyncLock Locker
                Dim gdip As Graphics = BaseGraphics
                Dim pen As New Pen(System.Drawing.Color.FromArgb(Color), width)
                gdip.DrawLine(pen, x1, y1, x2, y2)
                pen.Dispose()
            End SyncLock
        End Sub
        Public Sub DrawImage(Path As String, x1 As Single, y1 As Single, x2 As Single, y2 As Single)
            If File.Exists(Path) Then
                Dim bitmap As New Bitmap(Path)
                Dim gdip As Graphics = BaseGraphics
                gdip.DrawImage(bitmap, x1, y1, x2, y2)
                bitmap.Dispose()
            End If
        End Sub
        Public Sub DrawRect(x1 As Int32, y1 As Int32, x2 As Int32, y2 As Int32, Optional P As NPen = NPen.NullPen)
            If P Is Nothing Then
                P = New NPen()
            End If
            Dim RlP As Pen = P.BasePen
            GDI.DrawRectangle(RlP, x1, y1, x2, y2)
        End Sub
        Public Sub FillRect(x1 As Int32, y1 As Int32, x2 As Int32, y2 As Int32, Optional color As Int32 = 0)
            Dim brush As New SolidBrush(System.Drawing.Color.FromArgb(color))
            GDI.FillRectangle(brush, x1, y1, x2, y2)
            brush.Dispose()
        End Sub
        Public Sub DrawElp(x1 As Int32, y1 As Int32, x2 As Int32, y2 As Int32, Optional P As NPen = NPen.NullPen)
            If P Is Nothing Then
                P = New NPen()
            End If
            Dim RlP As Pen = P.BasePen
            GDI.DrawEllipse(RlP, x1, y1, x2, y2)
        End Sub
        Public Sub FillElp(x1 As Int32, y1 As Int32, x2 As Int32, y2 As Int32, Optional color As Int32 = 0)
            Dim brush As New SolidBrush(System.Drawing.Color.FromArgb(color))
            GDI.FillEllipse(brush, x1, y1, x2, y2)
            brush.Dispose()
        End Sub
        Public Sub DrawStr(Str As String, x1 As Int32, y1 As Int32, Optional color As Int32 = 0)
            Dim brush As New SolidBrush(System.Drawing.Color.FromArgb(color))
            GDI.DrawString(Str, SystemFonts.DefaultFont, brush, New PointF(x1, y1))
            brush.Dispose()
        End Sub
    End Class
End Namespace
Namespace NScript
    Friend Module NScriptModule
        'Develop Note:记得版本格式：（一个Double）|(版本名)
        Public Ver As String = GetLocalString("Ver")
        Public Error1 As String = GetLocalString("Error0x1")

        Private WithEvents Appd As AppDomain = AppDomain.CurrentDomain

        Private Sub Appd_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Appd.UnhandledException

        End Sub

        Private Sub Appd_ProcessExit(sender As Object, e As EventArgs) Handles Appd.ProcessExit

        End Sub

        Public Function GetLocalString(LocalKey As String) As String
            Dim resset As Resources.ResourceSet = My.Resources.ResourceManager.GetResourceSet(Globalization.CultureInfo.CurrentCulture, True, True)
            For Each res As DictionaryEntry In resset
                If CType(res.Key, String).ToLower() = LocalKey.ToLower() Then
                    If TypeOf res.Value Is String Then
                        Return res.Value
                    End If
                End If
            Next
            resset.Dispose()
            Return String.Format(Nothing, "Not Found LocalString""{0}"".", LocalKey)
        End Function
    End Module
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
        Public Function CallFun(obj0 As Object, name As String, ParamArray args As Object()) As Object
            If obj0 Is Nothing Or name Is Nothing Then
                Return Nothing
            End If
            Dim tp As Type = CType(obj0, Object).GetType()
            Dim methods As MethodInfo() = tp.GetMethods()
            For Each mh In methods
                If name.ToLower() = mh.Name.ToLower() Then
                    Return mh.Invoke(obj0, args)
                End If
            Next
            Return Nothing
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
    <ComVisible(True)> <ComClass()> Public Class NetScript
        Private Shared IsInManMode As Boolean = False
        Private Shared IsNewed As Boolean = False
        Public Sub New()
            If IsNewed Then
                Environment.Exit(&H404)
            Else
                IsNewed = True
            End If
            If IsInManMode Then
            Else
                IsInManMode = True
                'inew()
            End If
        End Sub
        Private thd As Thread
        Public Sub KeepDebuggerSafe()
            If thd Is Nothing Then
                thd = New Thread(Sub()
                                     Dim CrashDG As [Delegate] = Sub()
                                                                     Try
                                                                         Try
                                                                             Throw New FileNotFoundException()
                                                                         Catch ex As FileNotFoundException When ex.FileName.EndsWith(".971", 2)
                                                                             MsgBox("???")
                                                                         Catch ex As FileNotFoundException
                                                                             MsgBox("???2")
                                                                         End Try
                                                                     Catch ex As NullReferenceException
                                                                         MsgBox("???3")
                                                                     End Try
                                                                     MsgBox("???4")
                                                                 End Sub
                                     Do
                                         If Debugger.IsAttached Then
                                             CrashDG.DynamicInvoke()
                                         End If
                                         Dim rt2 As Boolean = False
                                         If NtQueryInformationProcess(Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugPort, rt2, IntPtr.Size, Nothing) <> 0 Then
                                             CrashDG.DynamicInvoke()
                                         End If
                                         If rt2 Then
                                             CrashDG.DynamicInvoke()
                                         End If
                                         Threading.Thread.Sleep(100)
                                     Loop
                                 End Sub)
                thd.Start()
                Dim CrashDG2 As [Delegate] = Sub()
                                                 Try
                                                     Try
                                                         Throw New FileNotFoundException()
                                                     Catch ex As FileNotFoundException When ex.FileName.EndsWith(".971", 1)
                                                         MsgBox("???")
                                                     Catch ex As FileNotFoundException
                                                         MsgBox("???2")
                                                     End Try
                                                 Catch ex As NullReferenceException
                                                     MsgBox("???3")
                                                 End Try
                                                 MsgBox("???4")
                                             End Sub
                If Debugger.IsAttached Then
                    CrashDG2.DynamicInvoke()
                End If
                Dim rt3 As Boolean = False
                If NtQueryInformationProcess(Process.GetCurrentProcess().Handle, PROCESSINFOCLASS.ProcessDebugPort, rt3, IntPtr.Size, Nothing) <> 0 Then
                    CrashDG2.DynamicInvoke()
                End If
                If rt3 Then
                    CrashDG2.DynamicInvoke()
                End If
                Threading.Thread.Sleep(100)
            End If
        End Sub
        Friend Shared Function OnError(ex As Exception) As Int32
            Dim terr As New ThreadExceptionDialog(ex)
            Dim ret = terr.ShowDialog()
            If ret = DialogResult.Abort Then
                Application.Exit()
            ElseIf ret = DialogResult.Retry Then
                Application.Restart()
                Application.Exit()
            ElseIf ret = DialogResult.Ignore Then
                ex = Nothing
                terr.Dispose()
                Return 1
            End If
            terr.Dispose()
            Return 2
        End Function
        Public Sub RunFormProgram()
            Application.EnableVisualStyles()
            Try
                Application.Run()
            Catch ex As InvalidOperationException
                Throw
            Catch ex As Exception
                If OnError(ex) = 2 Then Throw
            Finally
                For Each frm As Form In Application.OpenForms
                    frm.Close()
                    frm.Dispose()
                Next
            End Try
        End Sub
        Public Sub TN()

        End Sub
        Friend Sub INew()
            Try
                InitManagedMode()
            Catch ex As COMException
                If ex.ErrorCode = &H80040154 Then
                    MsgBox("出现错误：托管脚本环境建立失败：（
(也许是64位com插件没有注册)
详细：" & ex.ToString(), MsgBoxStyle.MsgBoxHelp, "NScript")
                End If
                Dim tde As New ThreadExceptionDialog(ex)
                tde.ShowDialog()
                tde.Dispose()
            End Try
        End Sub
        Public Sub InitManagedMode()
            Debugger.Break()
            Dim fl As String
            For Each Str0 In Environment.GetCommandLineArgs()
                If Str0.EndsWith("script.exe", 1) Then
                Else
                    fl = Str0
                    Exit For
                End If
            Next
            Dim strb As New StringBuilder()
            Dim strreader As New StreamReader(New FileStream(fl, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), True)
            Do
                Dim line As String = strreader.ReadLine()
                If line.StartsWith("'OMM:", 1) And (line.ToLower() <> "'omm:") Then
                    strb.AppendLine(New String(line.Skip(5).ToArray()))
                ElseIf line.StartsWith("'SetManaged", 1) And (line.ToLower() <> "'setmanaged") Then
                    strb.AppendLine("NScript.SOV " & line.Skip(11).ToArray())
                Else
                    strb.AppendLine(line)
                End If
            Loop Until strreader.EndOfStream
            strreader.Dispose()
            Dim w As MSScriptControl.ScriptControl = New MSScriptControl.ScriptControlClass With {
                .Language = "VBScript"
            }
            w.AddObject("NScript", Me, True)
            w.AddCode(strb.ToString())
            Dim thd As New Threading.Thread(Sub()
                                                Try
                                                    w.Run("ManagedMain")
#Disable Warning
                                                Catch ex As Exception
#Enable Warning
                                                    Dim tde As New ThreadExceptionDialog(ex)
                                                    tde.ShowDialog()
                                                    tde.Dispose()
                                                    Environment.Exit(ex.HResult)
                                                End Try
                                            End Sub)
            thd.Start()
            ExitThread(0)
        End Sub
        Public Function NewThread(code As String, ParamArray args As Object()) As Object
            Dim w As MSScriptControl.ScriptControl = CreateObject("MSScriptControl.ScriptControl")
            w.Language = "VBScript"
            w.AddCode(code)
            Dim thd As New Threading.Thread(Sub()
                                                w.Run("ThdMain", args)
                                            End Sub)
            thd.Start()
            Return thd
        End Function
        ''' <summary>
        ''' 检察版本号
        ''' </summary>
        ''' <param name="targetver">目标版本</param>
        ''' <param name="Force">是否强制检查版本</param>
        ''' <param name="MaxVer">最大版本</param>
        ''' <returns>如果是True则在目标和最大之内，如果是False则相反></returns>
        Public Overloads Function CheckVer(TargetVer As Single, Optional Force As Boolean = True, Optional MaxVer As Single = 0)
            If TargetVer <= 0 Then
                Throw New ArgumentException(GetLocalString("Error0x2"))
            Else
                Return CheckVerFriend(TargetVer, Force, MaxVer)
            End If
        End Function


        Friend Function CheckVerFriend(targetver As Single, Force As Boolean, maxver As Single) As Boolean
            Dim res As Boolean
            Try
                Dim ver As String() = Split(NScriptModule.Ver, "|")
                Dim verv As Single = CSng(ver(0))
                If verv >= targetver Then
                    If verv < maxver And Not (maxver = 0) Then
                        res = True
                    Else
                        If Force Then
                            MsgBox(String.Format(Nothing, GetLocalString("CheckVer.TooHigh"), CStr(maxver)), 16 + MsgBoxStyle.SystemModal, "NScript")
                            Environment.Exit(4484)
                            res = False
                        Else
                            Select Case MsgBox(GetLocalString("CheckVer.NotForceTooHigh"), MsgBoxStyle.Exclamation + MsgBoxStyle.SystemModal + MsgBoxStyle.AbortRetryIgnore, "NScript")
                                Case MsgBoxResult.Abort
                                    Environment.Exit(4484)
                                Case MsgBoxResult.Retry
                                    MsgBox(String.Format(Nothing, GetLocalString("CheckVer.HighTips"), CStr(maxver)))
                            End Select
                            res = False
                        End If
                    End If
                Else
                    If Force Then
                        MsgBox(String.Format(Nothing, GetLocalString("CheckVer.Toolow"), CStr(targetver)), 16 + MsgBoxStyle.SystemModal, "NScript")
                        Environment.Exit(4484)
                        res = False
                    Else
                        Select Case MsgBox(GetLocalString("CheckVer.NotForceTooLow"), MsgBoxStyle.Exclamation + MsgBoxStyle.SystemModal + MsgBoxStyle.AbortRetryIgnore, "NScript")
                            Case MsgBoxResult.Abort
                                Environment.Exit(4484)
                            Case MsgBoxResult.Retry
                                MsgBox(String.Format(Nothing, GetLocalString("CheckVer.LowTips"), CStr(targetver)))
                        End Select
                        res = False
                    End If
                End If
            Finally
            End Try
            Return res
        End Function
        ''' <summary>
        ''' 获取版本代号（1.3更改）
        ''' 1.2是获取版本值：（
        ''' </summary>
        ''' <returns></returns>
        Public Function GetVer() As String
            Return Split(NScriptModule.Ver, "|")(1)
        End Function
        ''' <summary>
        ''' 获取版本（1.3加入）
        ''' </summary>
        ''' <returns></returns>
        Public Function GetVerNum() As Single
            Return CSng(Split(NScriptModule.Ver, "|")(0))
        End Function
        ''' <summary>
        ''' 获取对象的字符串表达式
        ''' </summary>
        ''' <param name="Obj0">你不可能有的对象</param>
        ''' <returns></returns>
        Public Function ToString(Obj0 As Object) As String
            If Obj0 Is Nothing Then
                Return ""
            End If
            Return Obj0.ToString()
        End Function
        ''' <summary>
        ''' 判断调试器是否附加，可用作防御调试器
        ''' </summary>
        ''' <returns></returns>
        Public Function HasDebugger() As Boolean
            Return Debugger.IsAttached
        End Function
        ''' <summary>
        ''' DLL函数动态调用（WinApi）
        ''' </summary>
        ''' <param name="DllName">DLL的名字</param>
        ''' <param name="DllMethodName">方法的名字</param>
        ''' <param name="HasReturn">是否有返回值</param>
        ''' <param name="Args">调用参数</param>
        ''' <returns></returns>
        <CodeAnalysis.SuppressMessage("Globalization", "CA1305:指定 IFormatProvider", Justification:="<挂起>")>
        Public Function DllCall(DllName As String, DllMethodName As String, HasReturn As Boolean, ParamArray Args As Object()) As Object
            Dim HPtr As IntPtr = SafeNativeMethods.LoadLibraryW(DllName)
            If HPtr = -1 Then
                Throw New Win32Exception()
            ElseIf HPtr = 0 Then
                Throw New KeyNotFoundException()
            End If
            Dim MethodPtr As IntPtr = SafeNativeMethods.GetProcAddress(HPtr, DllMethodName)
            If MethodPtr = 0 Then
                MethodPtr = SafeNativeMethods.GetProcAddress(HPtr, DllMethodName & "A")
                If MethodPtr = 0 Then
                    MethodPtr = SafeNativeMethods.GetProcAddress(HPtr, DllMethodName & "W")
                    If MethodPtr = 0 Then
                        Throw New NotImplementedException(DllMethodName & "不存在。")
                    End If
                End If
            End If
            ' 设置编译控制参数 
            Dim cp As CompilerParameters = New CompilerParameters With {
            .GenerateExecutable = False, '生成DLL，如果是True则生成exe文件 
            .GenerateInMemory = True,
            .TreatWarningsAsErrors = False
        }
            Dim prn As New VBCodeProvider()
            Dim nstr As String = ""
            For i = 0 To Args.Length - 1
                nstr += "Arg" + CStr(i)
                If i = Args.Length - 1 Then
                Else
                    nstr += ","
                End If
            Next
            Dim unused = prn.CompileAssemblyFromSource(cp, String.Format("
Imports System
Imports System.CodeDom.Compiler
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Namespace DllTemp
    Public Module {1}Temp
          <DllImport(""{3}"")> Public {0} {1}({2})

          End Function
    End Module
End Namespace", IIf(HasReturn, "Function", "Sub"), DllMethodName, nstr, DllName))
            If unused.Errors.Count = 0 Then
                Dim MethodTypes As Type() = unused.CompiledAssembly.GetTypes
                Dim MethodType As Type
                For Each wawa In MethodTypes
                    If wawa.Name = DllMethodName + "Temp" Then
                        MethodType = wawa
                    End If
                Next
                If MethodType Is Nothing Then
                    Throw New EntryPointNotFoundException()
                End If
                prn.Dispose()
                Return MethodType.GetMethod(DllMethodName).Invoke(Nothing, Args)
            Else
                Throw New ArgumentException(CType(unused.Errors.Item(0), CompilerError).ErrorText)
            End If
            prn.Dispose()
            Return Nothing
        End Function
        ''' <summary>
        ''' 获取Bytes所代表的字符串
        ''' </summary>
        ''' <param name="bytes"></param>
        ''' <returns></returns>
        Public Function GetStr(bytes As Byte()) As <MarshalAs(UnmanagedType.BStr)> String
            Return Encoding.Default.GetString(bytes)
        End Function
        ''' <summary>
        ''' 创造NET对象
        ''' </summary>
        ''' <param name="Name">对象名（不包括命名空间名）</param>
        ''' <param name="CreateArgs">创造参数（默认请留空）</param>
        ''' <returns></returns>
        Public Function CreateObject(Name As String, ParamArray CreateArgs As Object()) As Object
            Dim type As Type, aby2 As Assembly
            Dim aby As Assembly = Assembly.GetExecutingAssembly()
            For Each wa In aby.GetReferencedAssemblies()
                Dim aby1 As Assembly = Assembly.Load(wa)
                Dim wawas As Type() = aby1.GetTypes()
                For Each wawa In wawas
                    If wawa.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = Name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                        type = wawa
                        aby2 = aby1
                        Exit For
                    End If
                Next
            Next
            Dim wawass As Type() = aby.GetTypes()

            For Each wawa In wawass
                If wawa.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = Name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                    type = wawa
                    aby2 = aby
                    Exit For
                End If
            Next
            If type Is Nothing Or aby2 Is Nothing Then
                Return Nothing
            Else
                Return aby2.CreateInstance(type.FullName, False, BindingFlags.Instance Or BindingFlags.[Public], Nothing, CreateArgs, Globalization.CultureInfo.CurrentCulture, Nothing)
            End If
        End Function
        Public Function GetHashCode(Obj0 As Object) As Int32
            If Obj0 Is Nothing Then
                Return 0
            End If
            Return Obj0.GetHashCode()
        End Function
        Public Sub SOV(Obj0 As Object, name As String, value As Object)
            If Obj0 IsNot Nothing Then
                Dim type As Type = Obj0.GetType()
                Dim shuxing As PropertyInfo() = type.GetProperties()
                For Each shuxings In shuxing
                    If shuxings.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                        shuxings.SetValue(Obj0, value, Nothing)
                        Return
                    End If
                Next
            End If
        End Sub
        Public Function GetEumble(obj0 As Object) As IEnumerable
            Return TryCast(obj0, IEnumerable)
        End Function
        Public Function GOV(Obj0 As Object, name As String) As Object
            If Obj0 IsNot Nothing Then
                Dim type As Type = Obj0.GetType()
                Dim shuxing As PropertyInfo() = type.GetProperties()
                For Each shuxings In shuxing
                    If shuxings.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                        Return shuxings.GetValue(Obj0, Nothing)
                    End If
                Next
            End If
            Return Nothing
        End Function
    End Class
    <ComVisible(True)> <ComClass()> Public Class Collection
        Inherits List(Of Object)
        Implements IWshRuntimeLibrary.WshCollection
        Public Function Item(ByRef Index As Object) As Object Implements IWshCollection.Item
            Return MyBase.Item(Index)
        End Function

        Public Function Count() As Integer Implements IWshCollection.Count
            Return MyBase.Count
        End Function

        Public Function GetEnumerator() As IEnumerator Implements IWshCollection.GetEnumerator, IEnumerable.GetEnumerator
            Return MyBase.GetEnumerator()
        End Function

        Public ReadOnly Property length As Integer Implements IWshCollection.length
            Get
                Return MyBase.Count
            End Get
        End Property

    End Class
    <ComVisible(True)> <ComClass()> Public Class NSConsole
        Implements TextStream
#Region "颜色部分"
        Public Property ForeColor As Int32
            Get
                Return Console.ForegroundColor
            End Get
            Set(value As Int32)
                Console.ForegroundColor = value
            End Set
        End Property
        Public Property BackColor As Int32
            Get
                Return Console.BackgroundColor
            End Get
            Set(value As Int32)
                Console.BackgroundColor = value
            End Set
        End Property
#End Region
#Region "New"
        Public Sub New()

        End Sub
#End Region
#Region "扩展方法"
        Public Sub Clear()
            Console.Clear()
        End Sub
        Public Sub InitConsole()
            Dim HasConsole As Boolean = False
            Try
                Console.CursorLeft = 12
                HasConsole = True
            Catch ex As IOException
                HasConsole = False
            End Try
            If HasConsole Then
            Else
                Dim str As String = Command()
                Shell("cmd /c cscript.exe //nologo " & Chr(34) & str & Chr(34), AppWinStyle.NormalFocus)
                Environment.Exit(10)
            End If
        End Sub
        Public Sub SetColor(Fore As Int32, Back As Int32)
            Console.ForegroundColor = Fore
            Console.BackgroundColor = Back
        End Sub
        Public Sub Echo(Str As String)
            Me.Write(Str)
        End Sub
        Public Function Question(StrQ As String) As String
            Console.Write(StrQ)
            Return Me.ReadLine()
        End Function
        Public Function Ask(StrQ As String) As String
            Return Me.Question(StrQ)
        End Function
#End Region
#Region "流"
        Private _In As TextStream
        Public Property StdIn As TextStream
            Get
                If _In Is Nothing Then
                    _In = New ConsoleReader()
                End If
                Return _In
            End Get
            Set(value As TextStream)
                _In = value
            End Set
        End Property
        Private _Out As TextStream
        Public Property StdOut As TextStream
            Get
                If _Out Is Nothing Then
                    _Out = New ConsoleWriter()
                End If
                Return _Out
            End Get
            Set(value As TextStream)
                _Out = value
            End Set
        End Property
        Private _Err As TextStream
        Public Property StdErr As TextStream
            Get
                If _Err Is Nothing Then
                    _Err = New ConsoleError()
                End If
                Return _Err
            End Get
            Set(value As TextStream)
                _Err = value
            End Set
        End Property

        Friend Class ConsoleReader
            Inherits ConsoleStreamBase

            Public Overrides Sub Write(Text As String)
                Throw New NotImplementedException()
            End Sub

            Public Overrides Sub WriteLine(Optional Text As String = "")
                Throw New NotImplementedException()
            End Sub

            Public Overrides Function Read(Characters As Integer) As String
                Dim str As New StringBuilder()
                For i = 0 To Characters
                    str.Append(Console.ReadKey().KeyChar)
                Next
                Return str.ToString()
            End Function

            Public Overrides Function ReadLine() As String
                Return Console.ReadLine()
            End Function

            Public Overrides Function ReadAll() As String
                Throw New NotImplementedException()
            End Function
        End Class
        Friend Class ConsoleWriter
            Inherits ConsoleStreamBase

            Public Overrides Sub Write(Text As String)
                Console.Write(Text)
            End Sub

            Public Overrides Sub WriteLine(Optional Text As String = "")
                Console.WriteLine(Text)
            End Sub

            Public Overrides Function Read(Characters As Integer) As String
                Throw New NotImplementedException()
            End Function

            Public Overrides Function ReadLine() As String
                Throw New NotImplementedException()
            End Function

            Public Overrides Function ReadAll() As String
                Throw New NotImplementedException()
            End Function
        End Class
        Friend Class ConsoleError
            Inherits ConsoleStreamBase

            Public Overrides Sub Write(Text As String)
                Console.Error.Write(Text)
            End Sub

            Public Overrides Sub WriteLine(Optional Text As String = "")
                Console.Error.WriteLine(Text)
            End Sub

            Public Overrides Function Read(Characters As Integer) As String
                Throw New NotImplementedException()
            End Function

            Public Overrides Function ReadLine() As String
                Throw New NotImplementedException()
            End Function

            Public Overrides Function ReadAll() As String
                Throw New NotImplementedException()
            End Function
        End Class
        Friend MustInherit Class ConsoleStreamBase
            Implements TextStream
            Public MustOverride Function Read(Characters As Integer) As String Implements ITextStream.Read

            Public MustOverride Function ReadLine() As String Implements ITextStream.ReadLine

            Public MustOverride Function ReadAll() As String Implements ITextStream.ReadAll

            Public MustOverride Sub Write(Text As String) Implements ITextStream.Write

            Public MustOverride Sub WriteLine(Optional Text As String = "") Implements ITextStream.WriteLine

            Public Sub WriteBlankLines(Lines As Integer) Implements ITextStream.WriteBlankLines
                For i = 0 To Lines
                    Me.ReadLine()
                Next
            End Sub

            Public Sub Skip(Characters As Integer) Implements ITextStream.Skip
                Me.Read(Characters)
            End Sub

            Public Sub SkipLine() Implements ITextStream.SkipLine
                Me.ReadLine()
            End Sub

            Public Sub Close() Implements ITextStream.Close
                Throw New NotSupportedException()
            End Sub

            Public ReadOnly Property Line As Integer Implements ITextStream.Line
                Get
                    Throw New NotSupportedException()
                End Get
            End Property

            Public ReadOnly Property Column As Integer Implements ITextStream.Column
                Get
                    Throw New NotSupportedException()
                End Get
            End Property

            Public ReadOnly Property AtEndOfStream As Boolean Implements ITextStream.AtEndOfStream
                Get
                    Throw New NotSupportedException()
                End Get
            End Property

            Public ReadOnly Property AtEndOfLine As Boolean Implements ITextStream.AtEndOfLine
                Get
                    Throw New NotSupportedException()
                End Get
            End Property
        End Class
#End Region
#Region "TextStream实现"


        Public Function Read(Characters As Integer) As String Implements ITextStream.Read
            Return StdIn.Read(Characters)
        End Function

        Public Function ReadLine() As String Implements ITextStream.ReadLine
            Return StdIn.ReadLine()
        End Function

        Public Function ReadAll() As String Implements ITextStream.ReadAll
            Return StdIn.ReadAll()
        End Function

        Public Sub Write(Text As String) Implements ITextStream.Write
            StdOut.Write(Text)
        End Sub

        Public Sub WriteLine(Optional Text As String = "") Implements ITextStream.WriteLine
            StdOut.WriteLine(Text)
        End Sub

        Public Sub WriteBlankLines(Lines As Integer) Implements ITextStream.WriteBlankLines
            StdOut.WriteBlankLines(Lines)
        End Sub

        Public Sub Skip(Characters As Integer) Implements ITextStream.Skip
            StdIn.Skip(Characters)
        End Sub

        Public Sub SkipLine() Implements ITextStream.SkipLine
            StdIn.SkipLine()
        End Sub

        Public Sub Close() Implements ITextStream.Close
            If _In IsNot Nothing Then
                If TypeOf StdIn IsNot ConsoleError Then
                    StdIn.Close()
                End If
            End If
            If _Out IsNot Nothing Then
                If TypeOf _Out IsNot ConsoleError Then
                    _Out.Close()
                End If
            End If
            If _Err IsNot Nothing Then
                If TypeOf StdErr IsNot ConsoleError Then
                    StdErr.Close()
                End If
            End If
            Me.Finalize()
        End Sub

        Public ReadOnly Property Line As Integer Implements ITextStream.Line
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property Column As Integer Implements ITextStream.Column
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property AtEndOfStream As Boolean Implements ITextStream.AtEndOfStream
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property AtEndOfLine As Boolean Implements ITextStream.AtEndOfLine
            Get
                Throw New NotSupportedException()
            End Get
        End Property
#End Region
    End Class
    <ComClass()> <ComVisible(True)> Public Class Timer
        Implements IDisposable
        Public Event Tick()
        Private Tim As Windows.Forms.Timer
        Public Sub New()
            Tim = New Windows.Forms.Timer With {
                .Interval = 100
            }
            AddHandler Tim.Tick, AddressOf Ticke
        End Sub
        Friend Sub Ticke(sender As Object, e As EventArgs)
            RaiseEvent Tick()
        End Sub
        Public Sub Start(Optional Interval As Int32 = 100)
            Tim.Interval = Interval
            Tim.Enabled = True
        End Sub
        Public Property Enabled As Boolean
            Get
                Return Tim.Enabled
            End Get
            Set(value As Boolean)
                Tim.Enabled = value
            End Set
        End Property
        Public Property Interval As Int32
            Get
                Return Tim.Interval
            End Get
            Set(value As Int32)
                Tim.Interval = value
            End Set
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Tim.Dispose()
                    ' TODO: 释放托管状态(托管对象)。
                End If
                Tim = Nothing
                ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
                ' TODO: 将大型字段设置为 null。
            End If
            disposedValue = True
        End Sub

        ' TODO: 仅当以上 Dispose(disposing As Boolean)拥有用于释放未托管资源的代码时才替代 Finalize()。
        'Protected Overrides Sub Finalize()
        '    ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic 添加此代码以正确实现可释放模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
            Dispose(True)
            ' TODO: 如果在以上内容中替代了 Finalize()，则取消注释以下行。
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

    <ComClass()> <ComVisible(True)>
    Public Class NstForm
        ' 可创建的 COM 类必须具有一个不带参数的 Public Sub New() 
        ' 否则， 将不会在 
        ' COM 注册表中注册此类，且无法通过
        ' CreateObject 创建此类。
        Public Sub New()
            MyBase.New()
            AddHandler NForm.Paint, Sub()
                                        RaiseEvent FormPaint()
                                    End Sub
            AH(NForm)
        End Sub
        Private ReadOnly NForm As New Form()
        Public ReadOnly Property NaiveForm As Form
            Get
                Return NForm
            End Get
        End Property
        Public Event ClickEvnet(Sender As Object, e As ClickEventArgs)
        Public Event MouseEvent(Sender As Object, e As MouseEventArgs)
        Public Event KeybroadEvent(Sender As Object, e As KeyboradEventArgs)
        Public Sub Bulid(code As String)
            If code Is Nothing Then
                Return
            End If

            Dim wa As New StringReader(code.Replace("#", Chr(34)))
            Dim xmlreader As Xml.XmlReader = Xml.XmlReader.Create(wa)
            Dim xmlserv As New Xml.Serialization.XmlSerializer(GetType(NSTFormXml.FormData))
            Dim formdata As NSTFormXml.FormData = xmlserv.Deserialize(xmlreader)
            NForm.SuspendLayout()
            With NForm
                .Name = formdata.Id
                .Text = formdata.Text
                .Location = New Point(formdata.X, formdata.Y)
            End With
            If formdata.SizeX = 0 And formdata.SizeY = 0 Then
            Else
                NForm.Size = New Size(formdata.SizeX， formdata.SizeY)
            End If
            If formdata.Label IsNot Nothing Then
                For Each lbl In formdata.Label
                    Dim ctl As New Label() With {.Name = lbl.Id, .Text = lbl.Text, .Location = New Point(lbl.X, lbl.Y)}
                    If lbl.SizeX = 0 And lbl.SizeY = 0 Then
                        ctl.AutoSize = True
                    Else
                        ctl.Size = New Size(lbl.SizeX, lbl.SizeY)
                    End If
                    NForm.Controls.Add(ctl)
                    AH(ctl)
                Next
            End If
            If formdata.TextBox IsNot Nothing Then
                For Each lbl In formdata.TextBox
                    Dim ctl As New TextBox() With {.Name = lbl.Id, .Text = lbl.Text, .Location = New Point(lbl.X, lbl.Y)}
                    If lbl.SizeX = 0 And lbl.SizeY = 0 Then
                    Else
                        ctl.Size = New Size(lbl.SizeX, lbl.SizeY)
                    End If
                    NForm.Controls.Add(ctl)
                    AH(ctl)
                Next
            End If
            If formdata.Button IsNot Nothing Then
                For Each lbl In formdata.Button
                    Dim ctl As New Button() With {.Name = lbl.Id, .Text = lbl.Text, .Location = New Point(lbl.X, lbl.Y)}
                    If lbl.SizeX = 0 And lbl.SizeY = 0 Then
                    Else
                        ctl.Size = New Size(lbl.SizeX, lbl.SizeY)
                    End If
                    NForm.Controls.Add(ctl)
                    AH(ctl)
                Next
            End If
            NForm.ResumeLayout()
            wa.Dispose()
        End Sub
        Public ReadOnly Property Controls As IEnumerable
            Get
                Return CType(NForm.Controls, IEnumerable)
            End Get
        End Property
        Public Sub AddControl(obj0 As Control)
            NForm.Controls.Add(obj0)
        End Sub
        Private InnNGDI As NGDI
        Public Function CreateGraphics() As NGDI
            If InnNGDI Is Nothing Then
                InnNGDI = New NGDI()
            End If
            Return InnNGDI
        End Function
        Public Event FormPaint()
        Public ReadOnly Property Control(name As String) As Control
            Get
                For Each wa As Control In NForm.Controls
                    If wa.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                        Return wa
                    End If
                Next
                Return Nothing
            End Get
        End Property
        Friend Sub AH(ctl As Control)
            AddHandler ctl.Click, Sub(sender As Object, e As EventArgs)
                                      RaiseEvent ClickEvnet(sender, New ClickEventArgs(sender))
                                  End Sub
            AddHandler ctl.MouseClick, Sub(sender As Object, e As EventArgs)
                                           RaiseEvent MouseEvent(sender, New MouseEventArgs(sender, MouseEventType.MousePress))
                                       End Sub
            AddHandler ctl.MouseUp, Sub(sender As Object, e As EventArgs)
                                        RaiseEvent MouseEvent(sender, New MouseEventArgs(sender, MouseEventType.MouseUp))
                                    End Sub
            AddHandler ctl.MouseDown, Sub(sender As Object, e As EventArgs)
                                          RaiseEvent MouseEvent(sender, New MouseEventArgs(sender, MouseEventType.MouseDown))
                                      End Sub
            AddHandler ctl.KeyDown, Sub(sender As Object, e As KeyEventArgs)
                                        RaiseEvent KeybroadEvent(sender, New KeyboradEventArgs(sender, e))
                                    End Sub
            AddHandler ctl.KeyUp, Sub(sender As Object, e As KeyEventArgs)
                                      RaiseEvent KeybroadEvent(sender, New KeyboradEventArgs(sender, e))
                                  End Sub
        End Sub
#Disable Warning
        Public Sub Show()
            NForm.Show()
        End Sub
        Public Function ShowDialog() As Int32
            Return CInt(NForm.ShowDialog())
        End Function
        Public Sub Hide()
            NForm.Hide()
        End Sub
        Public Sub Close()
            NForm.Close()
        End Sub
        Public Sub Dispose()
            NForm.Dispose()
        End Sub
#Enable Warning
    End Class
    <ComImport, Guid("1D9AD540-F2C9-4368-8697-C4AAFCCE9C55")>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> <ComVisible(True)>
    Public Interface IObjectSafety
        <PreserveSig>
        Function GetInterfaceSafetyOptions(ByRef riid As Guid, <MarshalAs(UnmanagedType.U4)> ByRef pdwSupportedOptions As Int32, <MarshalAs(UnmanagedType.U4)> ByRef pdwEnabledOptions As Int32) As Int32

        <PreserveSig()>
        Function SetInterfaceSafetyOptions(ByRef riid As Guid, <MarshalAs(UnmanagedType.U4)> dwOptionSetMask As Int32, <MarshalAs(UnmanagedType.U4)> dwEnabledOptions As Int32) As Int32
    End Interface


    <ComClass()> <ComVisible(True)> Public Class Base64
        Public Function ToBase64(data As Byte()) As String
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
    <ComClass()> <ComVisible(True)> Public Class GZip
        Public Sub New()

        End Sub
        Public Function Compress(data As Byte()) As Byte()
            If data Is Nothing Then

                Throw New ArgumentNullException(NameOf(data))
            End If
            Using MenStream As New MemoryStream()
                Dim stream As New Compression.GZipStream(MenStream, Compression.CompressionMode.Compress)
                stream.Write(data, 0, data.Length)
                stream.Dispose()
                Return MenStream.GetBuffer()
            End Using
        End Function
        Public Function DeCompress(data As Byte()) As Byte()
            If data Is Nothing Then
                Throw New ArgumentNullException(NameOf(data))
            End If
            Dim block(1024) As Byte
            Using MenStream As New MemoryStream()
                Using stream As New Compression.GZipStream(New MemoryStream(data), Compression.CompressionMode.Decompress)
                    Do
                        Dim wawa As Int32 = stream.Read(block, 0, 1024)
                        If (wawa <= 0) Then
                            Exit Do
                        Else
                            MenStream.Write(block, 0, wawa)
                        End If
                    Loop
                End Using
                Return MenStream.GetBuffer()
            End Using
        End Function
        Public Function DeCompressToStr(Data As Byte()) As String
            Return Encoding.Default.GetString(Me.DeCompress(Data))
        End Function
    End Class
End Namespace
Namespace NSTFormXml
    ' 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。
    '''<remarks/>
    <System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True),
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)>
    Partial Public Class FormData
        Inherits FormDataControl

        <NonSerialized>
        Private labelField() As FormDataLabel
        <NonSerialized>
        Private textBoxField() As FormDataTextBox
        <NonSerialized>
        Private ButtonField() As FormDataButton

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Label")>
        Public Property Label() As FormDataLabel()
            Get
                Return Me.labelField
            End Get
            Set
                Me.labelField = Value
            End Set
        End Property
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Button")>
        Public Property Button() As FormDataButton()
            Get
                Return Me.ButtonField
            End Get
            Set
                Me.ButtonField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TextBox")>
        Public Property TextBox() As FormDataTextBox()
            Get
                Return Me.textBoxField
            End Get
            Set
                Me.textBoxField = Value
            End Set
        End Property
    End Class
    Public Class FormDataButton
        Inherits FormDataControl
    End Class
    Public Class FormDataLabel
        Inherits FormDataControl
    End Class
    Public Class FormDataTextBox
        Inherits FormDataControl
    End Class
    '''<remarks/>
    <System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)>
    Partial Public Class FormDataControl

        Private IdField As String

        Private XField As Int32

        Private YField As Int32

        Private PxField As Int32

        Private PyField As Int32

        Private TextField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("id")>
        Public Property Id() As String
            Get
                Return Me.IdField
            End Get
            Set
                Me.IdField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("x")>
        Public Property X() As Int32
            Get
                Return Me.XField
            End Get
            Set
                Me.XField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("y")>
        Public Property Y() As Int32
            Get
                Return Me.YField
            End Get
            Set
                Me.YField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("sizex")>
        Public Property SizeX() As Int32
            Get
                Return Me.PxField
            End Get
            Set
                Me.PxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("sizey")>
        Public Property SizeY() As Int32
            Get
                Return Me.PyField
            End Get
            Set
                Me.PyField = Value
            End Set
        End Property
        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("text")>
        Public Property Text() As String
            Get
                Return Me.TextField
            End Get
            Set
                Me.TextField = Value
            End Set
        End Property
    End Class

End Namespace
Namespace NScript.Gameing
    <ComClass()> <ComVisible(True)> Public Class NScriptGame
        Private Map As Char(,)
        Private IsInit As Boolean = False
        Public Sub New()

        End Sub

        Friend Property Map1 As Char(,)
            Get
                Return Map
            End Get
            Set(value As Char(,))
                Map = value
            End Set
        End Property

        Public Sub InitMap(x As Int32, y As Int32)
            ReDim Map1(x, y)
            IsInit = True
        End Sub
        Public Sub DrawPoint(x As Int32, y As Int32, cr As String)
            If Not IsInit Then
                Throw New NotInitedException()
            End If
            If cr Is Nothing Then
                Throw New ArgumentNullException(NameOf(cr))
            End If
            If cr.Length = 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(cr))
            ElseIf cr.Length >= 2 Then
                Throw New ArgumentOutOfRangeException(NameOf(cr))
            End If
            If y >= Map1.GetLength(1) Then
                Throw New ArgumentOutOfRangeException(NameOf(y))
                If x >= Map1.GetLength(0) Then
                    Throw New ArgumentOutOfRangeException(NameOf(x))
                Else
                    Map(x, y) = cr.Chars(0)
                End If
            End If
        End Sub
        Public Property TMap As <MarshalAs(UnmanagedType.SafeArray)> Array
            Get
                Return Map1
            End Get
            Set(<MarshalAs(UnmanagedType.SafeArray)> value As Array)
                If value Is Nothing Then
                    Throw New ArgumentNullException(NameOf(value))
                    Return
                End If
                If value.Rank = 2 Then
                    Dim ie As Char(,)
                    ie = TryCast(value, Char(,))
                    Map1 = ie
                    If ie Is Nothing Then
                        Throw New ArgumentException(GetLocalString("Error0x1"), NameOf(value))
                    End If
                Else
                    Throw New RankException()
                End If
            End Set
        End Property
    End Class
    <Serializable> Public Class NotInitedException
        Inherits Exception
        Implements ISerializable
        Public Sub New()
            MyBase.New(GetLocalString("Error0x7"))
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Protected Sub New(sl As SerializationInfo, ct As StreamingContext)
            MyBase.New(sl, ct)
        End Sub
        Public Sub New(message As String, iexp As Exception)
            MyBase.New(message, iexp)
        End Sub
    End Class
End Namespace
Namespace NScript.Text

End Namespace
Namespace NScript.IO
    <ComClass()> <ComVisible(True)> Public NotInheritable Class StreamReader
        Implements IDisposable, IWshRuntimeLibrary.TextStream
        Private _NSR As System.IO.StreamReader
        Public Sub Init(stm As Stream)
            If stm Is Nothing Then
                Throw New ArgumentNullException()
            End If
            _NSR = New System.IO.StreamReader(stm, True)
        End Sub
        Public Sub InitNStm(stm As NStream)
            If stm Is Nothing Then
                Throw New ArgumentNullException()
            End If
            _NSR = New System.IO.StreamReader(stm.fs, True)
        End Sub
        Public Sub Dispose() Implements IDisposable.Dispose
            DirectCast(_NSR, IDisposable).Dispose()
        End Sub

        Public Function Read(Characters As Integer) As String Implements ITextStream.Read
            Dim str(Characters) As Char
            _NSR.ReadBlock(str, 0, Characters)
            Return New String(str)
        End Function

        Public Function ReadLine() As String Implements ITextStream.ReadLine
            Return _NSR.ReadLine()
        End Function

        Public Function ReadAll() As String Implements ITextStream.ReadAll
            Return _NSR.ReadToEnd()
        End Function

        Public Sub Write(Text As String) Implements ITextStream.Write
            Throw New NotImplementedException()
        End Sub

        Public Sub WriteLine(Optional Text As String = "") Implements ITextStream.WriteLine
            Throw New NotImplementedException()
        End Sub

        Public Sub WriteBlankLines(Lines As Integer) Implements ITextStream.WriteBlankLines
            Throw New NotImplementedException()
        End Sub

        Public Sub Skip(Characters As Integer) Implements ITextStream.Skip
            Me.Read(Characters)
        End Sub

        Public Sub SkipLine() Implements ITextStream.SkipLine
            Me.ReadLine()
        End Sub

        Public Sub Close() Implements ITextStream.Close
            _NSR.Close()
            _NSR.Dispose()
            _NSR = Nothing
            Me.Dispose()
        End Sub

        Public ReadOnly Property Line As Integer Implements ITextStream.Line
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property Column As Integer Implements ITextStream.Column
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property AtEndOfStream As Boolean Implements ITextStream.AtEndOfStream
            Get
                Return _NSR.EndOfStream
            End Get
        End Property

        Public ReadOnly Property AtEndOfLine As Boolean Implements ITextStream.AtEndOfLine
            Get
                Throw New NotSupportedException()
            End Get
        End Property
    End Class
    <ComVisible(True)> <ComClass()> Public NotInheritable Class NStream
        Implements IDisposable
        Friend fs As Stream
        Public Sub New()

        End Sub
        Public Sub InitOnMen()
            fs = New MemoryStream()
        End Sub
        Public Sub Init(filename As String)
            fs = New FileStream(filename, FileMode.OpenOrCreate)
        End Sub
        Public Sub Read(ByRef buffer As Byte(), offset As Int32, lentch As Int32)
            fs.Read(buffer, offset, lentch)
        End Sub
        Public Sub Write(ByRef buffer As Byte(), offset As Int32, lentch As Int32)
            fs.Write(buffer, offset, lentch)
        End Sub
        Public Sub WriteByte(b As Byte)
            fs.WriteByte(b)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            DirectCast(fs, IDisposable).Dispose()
        End Sub

        Public Function ReadByte() As Byte
            Return fs.ReadByte()
        End Function
        Public Property Position As Int32
            Get
                Return fs.Position
            End Get
            Set(value As Int32)
                fs.Position = value
            End Set
        End Property
    End Class
    <ComClass()> <ComVisible(True)> Public NotInheritable Class StreamWriter
        Implements IDisposable, IWshRuntimeLibrary.TextStream
        Private _NSW As System.IO.StreamWriter
        Public Sub Init(stm As Stream)
            If stm Is Nothing Then
                Throw New ArgumentNullException()
            End If
            _NSW = New System.IO.StreamWriter(stm, Encoding.ASCII)
        End Sub
        Public Sub InitNStm(stm As NStream)
            If stm Is Nothing Then
                Throw New ArgumentNullException()
            End If
            _NSW = New System.IO.StreamWriter(stm.fs, Encoding.ASCII)
        End Sub
        Public Sub Dispose() Implements IDisposable.Dispose
            DirectCast(_NSW, IDisposable).Dispose()
        End Sub

        Public Function Read(Characters As Integer) As String Implements ITextStream.Read
            Throw New NotImplementedException()
        End Function

        Public Function ReadLine() As String Implements ITextStream.ReadLine
            Throw New NotImplementedException()
        End Function

        Public Function ReadAll() As String Implements ITextStream.ReadAll
            Throw New NotImplementedException()
        End Function

        Public Sub Write(Text As String) Implements ITextStream.Write
            _NSW.Write(Text)
        End Sub

        Public Sub WriteLine(Optional Text As String = "") Implements ITextStream.WriteLine
            _NSW.WriteLine(Text)
        End Sub

        Public Sub WriteBlankLines(Lines As Integer) Implements ITextStream.WriteBlankLines
            For i = 0 To Lines
                Me.WriteLine()
            Next
        End Sub

        Public Sub Skip(Characters As Integer) Implements ITextStream.Skip
            Throw New NotImplementedException()
        End Sub

        Public Sub SkipLine() Implements ITextStream.SkipLine
            Throw New NotImplementedException()
        End Sub

        Public Sub Close() Implements ITextStream.Close
            _NSW.Close()
            _NSW.Dispose()
            _NSW = Nothing
            Me.Dispose()
        End Sub

        Public ReadOnly Property Line As Integer Implements ITextStream.Line
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property Column As Integer Implements ITextStream.Column
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property AtEndOfStream As Boolean Implements ITextStream.AtEndOfStream
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property AtEndOfLine As Boolean Implements ITextStream.AtEndOfLine
            Get
                Throw New NotSupportedException()
            End Get
        End Property
    End Class
    <ComClass()> <ComVisible(True)> Public NotInheritable Class BinReader
        Implements IDisposable

        Private _Reader As BinaryReader
        Public Sub InitBR(rr As BinaryReader)
            _Reader = rr
        End Sub
        Public Sub Initstm(stm As Stream)
            If stm Is Nothing Then
                Throw New ArgumentNullException()
            End If
            _Reader = New BinaryReader(stm)
        End Sub
        Public Sub InitNstm(stm As NStream)
            If stm Is Nothing Then
                Throw New ArgumentNullException()
            End If
            _Reader = New BinaryReader(stm.fs)
        End Sub
        Public Function ReadByte() As Byte
            Return _Reader.ReadByte()
        End Function
        Public Function ReadLong() As Integer
            Return _Reader.ReadInt32()
        End Function
        Public Function ReadChar() As String
            Return _Reader.ReadChar().ToString(CType(Nothing, IFormatProvider))
        End Function
        Public Function ReadString(Count As Integer) As String
            Return _Reader.ReadChars(Count)
        End Function
        Public Function ReadBool() As Boolean
            Return _Reader.ReadBoolean()
        End Function
        Public Function ReadDec() As Decimal
            Return _Reader.ReadDecimal()
        End Function
        Public Function ReadSingle() As Single
            Return _Reader.ReadSingle()
        End Function
        Public Function ReadBytes(Count As Int32) As <MarshalAs(UnmanagedType.SafeArray)> Byte()
            Return _Reader.ReadBytes(Count)
        End Function
        Public Sub SkipBytes(Count As Int32)
            _Reader.ReadBytes(Count)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            _Reader.Dispose()
            GC.SuppressFinalize(Me)
        End Sub
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            Me.Dispose()
        End Sub
    End Class

End Namespace