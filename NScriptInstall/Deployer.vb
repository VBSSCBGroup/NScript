Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Text
Imports Microsoft.Win32

Public Class Deployer
    Public Shared Ind As Deployer
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Ind = Me
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        BackgroundWorker1.ReportProgress(0)
        Try
            NScriptInstall.Update.UpdateVer(BackgroundWorker1)
        Catch ex As Exception
            BackgroundWorker1.ReportProgress(0)
            Me.Invoke(Sub()
                          Button1.Enabled = True
                      End Sub)
            MsgBox("Error!" & ex.ToString())
        End Try
        Environment.Exit(0)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        BackgroundWorker1.RunWorkerAsync()
        Button1.Enabled = False

    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub
End Class
Namespace Update
    Module VerUpdate

        Public Sub UpdateVer(bw As BackgroundWorker)
            bw.ReportProgress(3)
            Dim webc As HttpWebRequest = WebRequest.Create(New Uri(
            "http://laobas.qicp.io/VirtualZones/NScript/Ver.xml"))
            webc.Timeout = 40000
            Dim repc As HttpWebResponse
            Try
                repc = webc.GetResponse()
            Catch ex As InvalidOperationException
                Try
                    webc = WebRequest.Create(New Uri(
            "http://laoba.xfnet.club/Ver.xml"))
                    repc = webc.GetResponse()
                Catch ex2 As InvalidOperationException
                    MsgBox("：（你家没有网络")
                    Exit Sub
                End Try
            Catch ex As NotSupportedException
                MsgBox("出乎意料的错误,
CLR可能已经崩溃
这也许是CLR的bug
详细：" & ex.ToString())
                Exit Sub
            End Try
            bw.ReportProgress(6)
            If repc Is Nothing Then Exit Sub
            Dim xmlserli As New Xml.Serialization.XmlSerializer(GetType(VerRoot))
            Dim VerXmlRoot As VerRoot
            Dim repcreader As New StreamReader(repc.GetResponseStream())
            Try
                Dim xml = System.Xml.XmlReader.Create(repcreader)
                VerXmlRoot = xmlserli.Deserialize(xml)
                CType(xml, IDisposable).Dispose()
            Catch ex As InvalidOperationException
                MsgBox("从服务器返回的数据无效:(")
                Exit Sub
            Finally
                repcreader.Dispose()
            End Try
            bw.ReportProgress(9)
            Deployer.Ind.Invoke(Sub()
                                    Deployer.Ind.Label1.Text = "准备下载文件"
                                End Sub)
            Dim NiVerH As Decimal = Join(VerXmlRoot.NiVer.Split("."c).Reverse().Skip(1).Reverse().ToArray(), "."c)
            Dim NiVerL As Int32 = VerXmlRoot.NiVer.Split("."c).Last()
            Deployer.Ind.Invoke(Sub()
                                    Deployer.Ind.Label1.Text = "版本号:" & NiVerH & "." & NiVerL
                                End Sub)
            Dim files As List(Of String) = Update(VerXmlRoot.NeedFiles, bw)
            bw.ReportProgress(70)
            Dim cv As RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion")
            Dim pgdir As String = cv.GetValue("ProgramFilesDir")
            Dim ps As New System.Security.Permissions.FileIOPermission(System.Security.Permissions.PermissionState.Unrestricted, pgdir)
            ps.Demand()
            Dim installpath As String = Path.Combine(pgdir, "NScript")
            If Directory.Exists(installpath) Then
                Try
                    Directory.Delete(installpath, True)
                Catch ex As Exception

                End Try
            End If
            Directory.CreateDirectory(installpath)
            bw.ReportProgress(90)
            Deployer.Ind.Invoke(Sub()
                                    Deployer.Ind.Label1.Text = "写入Program Files"
                                End Sub)
            For Each fl In files
                File.Move(fl, Path.Combine(installpath, fl.Split("\"c).Last()))
            Next
            bw.ReportProgress(98)
            Deployer.Ind.Invoke(Sub()
                                    Deployer.Ind.Label1.Text = "修改注册表ing"
                                End Sub)
            Dim sw As StreamWriter = New StreamWriter(New FileStream(Path.Combine(installpath, "Install.bat"), FileMode.Create), Encoding.ASCII, 99814)
            sw.WriteLine("@echo off
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe """ & Path.Combine(installpath, "NScript.dll") & """ /codebase /regfile:%temp%\aaa.reg
reg import %temp%\aaa.reg
pause")
            sw.Dispose()
            Dim sw2 As StreamWriter = New StreamWriter(New FileStream(Path.Combine(installpath, "Update.vbs"), FileMode.Create), Encoding.ASCII, 99814)
            sw2.WriteLine("set VU=CreateObject(""NScript.Updater"")
VU.Update()")
            sw2.Dispose()
            Dim wsh As IWshRuntimeLibrary.IWshShell = CreateObject("Wscript.Shell")
            Dim shortcut As IWshRuntimeLibrary.IWshShortcut = wsh.CreateShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), "更新NScript.lnk"))
            shortcut.TargetPath = Path.Combine(installpath, "Update.vbs")
            shortcut.WorkingDirectory = installpath
            shortcut.WindowStyle = 1
            shortcut.Description = "NScript更新脚本"
            shortcut.IconLocation = "C:\Windows\System32\imagers.dll,102"
            shortcut.Save()
            Dim pcs As New ProcessStartInfo("cmd.exe", " /c """ & Path.Combine(installpath, "Install.bat") & """")
            pcs.Verb = "runas"
            Dim procs As Process = Process.Start(pcs)
            procs.WaitForExit()
            MsgBox(Environment.GetFolderPath(Environment.SpecialFolder.Programs))
            Deployer.Ind.Invoke(Sub()
                                    Deployer.Ind.Label1.Text = "Done!"
                                End Sub)
            bw.ReportProgress(100)
            Deployer.Ind.TopMost = True
            Deployer.Ind.TopMost = False
            Threading.Thread.Sleep(5000)
        End Sub
        Friend Function GetBytesString(bytes As Long) As String
            Dim dy As String() = {"Byte", "KB", "MB", "GB", "TB", "PB"}
            Const modv = 1024
            Dim s As Double = bytes
            Dim i As Int32
            For i = 0 To dy.Length - 1
                If s < modv Then
                    Exit For
                End If
                s /= modv
            Next
            Return Math.Round(s, 3).ToString() + dy(i)
        End Function
        Friend Function Update(NeedFiles As VerRootNeedFile(), bw As BackgroundWorker) As List(Of String)
            Dim ret As New List(Of String)
            Dim webc As New WebClient()
            bw.ReportProgress(11)
            Try
                Dim filepath As String = System.Reflection.Assembly.GetExecutingAssembly().CodeBase
                filepath = filepath.Substring(8, filepath.Length - 8)
                If Not File.Exists(filepath) Then
                    Exit Function
                End If
                bw.ReportProgress(12)
                Dim dir As DirectoryInfo = New FileInfo(filepath).Directory
                Dim NF As VerRootNeedFile
                Dim starttime As Date = Date.Now
                Dim right As Boolean = False
                Dim ia As Int32 = 0
                AddHandler webc.DownloadProgressChanged, Sub(sender As Object, e As DownloadProgressChangedEventArgs)
                                                             bw.ReportProgress(12 + ((e.ProgressPercentage + ia * 100) / NeedFiles.Length) * (58 / 100))

                                                             Deployer.Ind.Invoke(Sub()
                                                                                     Dim s As Double = (Date.Now - starttime).TotalSeconds
                                                                                     Dim speed As String
                                                                                     If s = 0 Then
                                                                                         speed = "?B"
                                                                                     Else
                                                                                         speed = GetBytesString(e.BytesReceived / s)
                                                                                     End If
                                                                                     Deployer.Ind.Label1.Text = "下载" &
                                                                                     NF.Type &
                                                                                     "文件" & NF.Path &
                                                                                      "速度:" &
                                                                                    speed &
                                                                                     "/s已下载:" & GetBytesString(e.BytesReceived) &
                                                                                     "共计" & GetBytesString(e.TotalBytesToReceive)
                                                                                 End Sub)
                                                         End Sub
                AddHandler webc.DownloadFileCompleted, Sub(sender, e)
                                                           right = True
                                                       End Sub
                For Each NF In NeedFiles
                    Try
                        Try
                            starttime = Date.Now
                            webc.DownloadFileAsync(New Uri(
"http://laobas.qicp.io/VirtualZones/NScript/" & NF.Path), dir.FullName & "\" & NF.Path)
                            Do Until right
                                Application.DoEvents()
                                Threading.Thread.Sleep(1)
                            Loop
                            right = False
                        Catch ex As WebException
                            Try
                                webc.DownloadFile(New Uri(
"http://laoba.xfnet.club/" & NF.Path), dir.FullName & "\" & NF.Path)
                            Catch ex2 As Exception
                                MsgBox("无法下载文件" & NF.Path)
                            End Try
                        End Try
                        ret.Add(dir.FullName & "\" & NF.Path)
                    Catch ex As Exception

                    End Try
                    ia += 1
                Next

            Finally
                webc.Dispose()
            End Try
            Return ret
        End Function
    End Module
    ' 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。
    '''<remarks/>
    <System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True),
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)>
    Partial Public Class VerRoot

        Private verField As Decimal

        Private niVerField As String

        Private needFilesField() As VerRootNeedFile

        '''<remarks/>
        Public Property Ver() As Decimal
            Get
                Return Me.verField
            End Get
            Set
                Me.verField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property NiVer() As String
            Get
                Return Me.niVerField
            End Get
            Set
                Me.niVerField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("NeedFile", IsNullable:=False)>
        Public Property NeedFiles() As VerRootNeedFile()
            Get
                Return Me.needFilesField
            End Get
            Set
                Me.needFilesField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)>
    Partial Public Class VerRootNeedFile

        Private typeField As String

        Private pathField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Path() As String
            Get
                Return Me.pathField
            End Get
            Set
                Me.pathField = Value
            End Set
        End Property
    End Class


End Namespace
