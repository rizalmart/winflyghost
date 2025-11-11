Public Class frmmain

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.PopulateInstalledPackages()
    End Sub

    Sub PopulateInstalledPackages()

        Dim mainKey As Microsoft.Win32.RegistryKey

        mainKey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications")

        If mainKey.SubKeyCount = 0 Then
            MsgBox("No UWP Packages Installed", vbExclamation, "")
            Exit Sub
        End If

        Dim subkeys() As String
        Dim subkey As String
        Dim newIndex As Long
        Dim testkey As Microsoft.Win32.RegistryKey
        Dim testval As Object

        subkeys = mainKey.GetSubKeyNames()

        For Each subkey In subkeys

            testkey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications\" & subkey)
            testval = testkey.GetValue("Disabled", 0)

            newIndex = Me.DataGridView1.Rows.Add
            Me.DataGridView1.Rows(newIndex).Cells(0).Value = subkey
            Me.DataGridView1.Rows(newIndex).Cells(1).Value = 1 - testval

        Next


    End Sub



    Sub ToggleSettings(RowIndex As Long)

        Dim enval As Long
        Dim testval2 As Object
        Dim regkey As Microsoft.Win32.RegistryKey
        Dim pkgname As String

        pkgname = Me.DataGridView1.Rows(RowIndex).Cells(0).Value

        regkey = My.Computer.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications\" & pkgname, True)

        If Me.DataGridView1.Rows(RowIndex).Cells(1).Value = 0 Then
            enval = 1
        Else
            enval = 0
        End If

        'MsgBox(enval)

        regkey.SetValue("Disabled", enval, Microsoft.Win32.RegistryValueKind.DWord)
        regkey.SetValue("DisabledByUser", enval, Microsoft.Win32.RegistryValueKind.DWord)
        regkey.SetValue("SleepDisabled", enval, Microsoft.Win32.RegistryValueKind.DWord)

        regkey.Close()

    End Sub


    Private Sub DataGridView1_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged
        If DataGridView1.IsCurrentCellDirty Then
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
            ToggleSettings(Me.DataGridView1.CurrentRow.Index)
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.Show()
    End Sub
End Class
