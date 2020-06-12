Imports System.IO
Imports NScript.Update
Imports System.Net
Imports System.Runtime.InteropServices

Namespace NScript
    <ComClass(), ComVisible(True)> Public Class Updater
        Public Sub Update()
            UpdateVer()
        End Sub
    End Class
End Namespace

Namespace NScript.Update
    Module VerUpdate

        Public Sub UpdateVer()
            Dim webc As HttpWebRequest = WebRequest.Create(New Uri(
            "http://laobas.qicp.io/Site1/APIs/Virtual?File=D:\VirtualZones\NScript\Ver.xml"))
            webc.Timeout = 40000
            Dim repc As HttpWebResponse
            Try
                repc = webc.GetResponse()
            Catch ex As InvalidOperationException
                MsgBox("错误：作者可能在睡觉或者忘记开服务器,
也有可能是你家没网
详细：" & ex.ToString())
                Exit Sub
            Catch ex As NotSupportedException
                MsgBox("出乎意料的错误,
CLR可能已经崩溃
这也许是CLR的bug
详细：" & ex.ToString())
                Exit Sub
            End Try
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
            Dim NiVerH As Decimal = Join(VerXmlRoot.NiVer.Split("."c).Reverse().Skip(1).Reverse().ToArray(), "."c)
            Dim NiVerL As Int32 = VerXmlRoot.NiVer.Split("."c).Last()
            If NiVerH > My.Resources.NiVerH Then
                If MsgBox("检测到大型更新，是否更新NScript?", MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.SystemModal + vbOKCancel, "NScript") Then
                    Update(VerXmlRoot.NeedFiles)
                Else
                    Exit Sub
                End If
            ElseIf NiVerL > My.Resources.NiVerL Then
                If MsgBox("检测到小型更新，是否更新NScript?", MsgBoxStyle.MsgBoxSetForeground + MsgBoxStyle.SystemModal + vbOKCancel, "NScript") Then
                    Update(VerXmlRoot.NeedFiles)
                Else
                    Exit Sub
                End If
            End If
        End Sub
        Friend Sub Update(NeedFiles As VerRootNeedFile())
            Dim webc As New WebClient()
            Try
                Dim filepath As String = System.Reflection.Assembly.GetExecutingAssembly().CodeBase
                filepath = filepath.Substring(8, filepath.Length - 8)
                If Not File.Exists(filepath) Then
                    Exit Sub
                End If
                Dim dir As DirectoryInfo = New FileInfo(filepath).Directory
                Try
                    For Each NF In NeedFiles
                        webc.DownloadFile(New Uri(
            "http://laobas.qicp.io/Site1/APIs/Virtual?File=D:\VirtualZones\NScript\" & NF.Path), dir.FullName & "\" & NF.Path & ".Bak")
                    Next
                    MsgBox("更新完毕！
请将" & filepath & "目录下的bak后缀名去掉")
                Catch ex As WebException

                End Try
            Finally
                webc.Dispose()
            End Try

        End Sub
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
