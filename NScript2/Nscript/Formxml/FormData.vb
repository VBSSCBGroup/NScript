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

End Namespace
